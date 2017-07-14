﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DevCapture
{
    public class GIFLZWEncoder
    {
        private static readonly int EOF = -1;

        public static readonly int BITS = 12;
        public static readonly int HSIZE = 5003; // 80% occupancy

        private int imgW, imgH;
        private byte[] m_arrPixels;
        private int initCodeSize;
        private int remaining;
        private int m_nIndexPixel;

        // GIFCOMPR.C       - GIF Image compression routines
        //
        // Lempel-Ziv compression based on 'compress'.  GIF modifications by
        // David Rowley (mgardi@watdcsu.waterloo.edu)

        // General DEFINEs

        // GIF Image compression - modified 'compress'
        //
        // Based on: compress.c - File compression ala IEEE Computer, June 1984.
        //
        // By Authors:  Spencer W. Thomas      (decvax!harpo!utah-cs!utah-gr!thomas)
        //              Jim McKie              (decvax!mcvax!jim)
        //              Steve Davies           (decvax!vax135!petsd!peora!srd)
        //              Ken Turkowski          (decvax!decwrl!turtlevax!ken)
        //              James A. Woods         (decvax!ihnp4!ames!jaw)
        //              Joe Orost              (decvax!vax135!petsd!joe)

        int n_bits; // number of bits/code
        int maxbits = BITS; // user settable max # bits/code
        int maxcode; // maximum code, given n_bits
        int maxmaxcode = 1 << BITS; // should NEVER generate this code

        int[] htab = new int[HSIZE];
        int[] codetab = new int[HSIZE];

        int hsize = HSIZE; // for dynamic table sizing

        int free_ent = 0; // first unused entry

        // block compression parameters -- after all codes are used up,
        // and compression rate changes, start over.
        bool clear_flg = false;

        // Algorithm:  use open addressing double hashing (no chaining) on the
        // prefix code / next character combination.  We do a variant of Knuth's
        // algorithm D (vol. 3, sec. 6.4) along with G. Knott's relatively-prime
        // secondary probe.  Here, the modular division first probe is gives way
        // to a faster exclusive-or manipulation.  Also do block compression with
        // an adaptive reset, whereby the code table is cleared when the compression
        // ratio decreases, but after the table fills.  The variable-length output
        // codes are re-sized at this point, and a special CLEAR code is generated
        // for the decompressor.  Late addition:  construct the table according to
        // file size for noticeable speed improvement on small files.  Please direct
        // questions about this implementation to ames!jaw.

        int g_init_bits;

        int ClearCode;
        int EOFCode;

        // output
        //
        // Output the given code.
        // Inputs:
        //      code:   A n_bits-bit integer.  If == -1, then EOF.  This assumes
        //              that n_bits =< wordsize - 1.
        // Outputs:
        //      Outputs code to the file.
        // Assumptions:
        //      Chars are 8 bits long.
        // Algorithm:
        //      Maintain a BITS character long buffer (so that 8 codes will
        // fit in it exactly).  Use the VAX insv instruction to insert each
        // code in turn.  When the buffer fills up empty it and start over.

        int cur_accum = 0;
        int cur_bits = 0;

        private static int[] masks =
		{
			0x0000,
			0x0001,
			0x0003,
			0x0007,
			0x000F,
			0x001F,
			0x003F,
			0x007F,
			0x00FF,
			0x01FF,
			0x03FF,
			0x07FF,
			0x0FFF,
			0x1FFF,
			0x3FFF,
			0x7FFF,
			0xFFFF };

        // Number of characters so far in this 'packet'
        int a_count;

        // Define the storage for the packet accumulator
        byte[] accum = new byte[255];

        //----------------------------------------------------------------------------
        public GIFLZWEncoder(int width, int height, byte[] pixels, int color_depth) {
            imgW = width;
            imgH = height;
            m_arrPixels = pixels;
            initCodeSize = Math.Max(2, color_depth);
        }

        // Add a character to the end of the current packet, and if it is 254
        // characters, flush the packet to disk.
        void Add(byte c, List<byte> lst) {
            accum[a_count++] = c;
            if (a_count == accum.Length)
                Flush(lst);
        }

        // Clear out the hash table

        // table clear for block compress
        void ClearTable(List<byte> lst) {
            ResetCodeTable(hsize);
            free_ent = ClearCode + 2;
            clear_flg = true;

            Output(ClearCode, lst);
        }

        // reset code table
        void ResetCodeTable(int hsize) {
            for (int i = 0; i < hsize; ++i)
                htab[i] = -1;
        }

        void Compress(int init_bits, List<byte> lst) {
            int fcode;
            int i /* = 0 */;
            int c;
            int ent;
            int disp;
            int hsize_reg;
            int hshift;

            // Set up the globals:  g_init_bits - initial number of bits
            g_init_bits = init_bits;

            // Set up the necessary values
            clear_flg = false;
            n_bits = g_init_bits;
            maxcode = MaxCode(n_bits);

            ClearCode = 1 << (init_bits - 1);
            EOFCode = ClearCode + 1;
            free_ent = ClearCode + 2;

            a_count = 0; // clear packet

            ent = NextPixel();

            hshift = 0;
            for (fcode = hsize; fcode < 65536; fcode *= 2)
                ++hshift;
            hshift = 8 - hshift; // set hash code range bound

            hsize_reg = hsize;
            ResetCodeTable(hsize_reg); // clear hash table

            Output(ClearCode, lst);

        outer_loop:
            while ((c = NextPixel()) != EOF) {
                fcode = (c << maxbits) + ent;
                i = (c << hshift) ^ ent; // xor hashing

                if (htab[i] == fcode) {
                    ent = codetab[i];
                    continue;
                } else if (htab[i] >= 0) { // non-empty slot
                    disp = hsize_reg - i; // secondary hash (after G. Knott)
                    if (i == 0) disp = 1;
                    do {
                        if ((i -= disp) < 0)
                            i += hsize_reg;

                        if (htab[i] == fcode) {
                            ent = codetab[i];
                            goto outer_loop;
                        }
                    } while (htab[i] >= 0);
                }
                Output(ent, lst);
                ent = c;
                if (free_ent < maxmaxcode) {
                    codetab[i] = free_ent++; // code -> hashtable
                    htab[i] = fcode;
                } else
                    ClearTable(lst);
            }
            // Put out the final code.
            Output(ent, lst);
            Output(EOFCode, lst);
        }

        //----------------------------------------------------------------------------
        public byte[] Encode() {
            List<byte> lst = new List<byte>();
            lst.Add(Convert.ToByte(initCodeSize));// write "initial code size" byte

            remaining = imgW * imgH; // reset navigation variables
            m_nIndexPixel = 0;

            Compress(initCodeSize + 1, lst); // compress and write the pixel data

            lst.Add(0);// write block terminator
            return lst.ToArray();
        }

        // Flush the packet to disk, and reset the accumulator
        void Flush(List<byte> lst) {
            if (a_count > 0) {
                lst.Add(Convert.ToByte(a_count));
                byte[] byData = accum;//
                if (a_count != accum.Length) {
                    byData = new byte[a_count];
                    Array.Copy(accum, byData, a_count);
                }
                lst.AddRange(byData);

                a_count = 0;
            }
        }

        int MaxCode(int n_bits) {
            return (1 << n_bits) - 1;
        }

        //----------------------------------------------------------------------------
        // Return the next pixel from the image
        //----------------------------------------------------------------------------
        private int NextPixel() {
            if (remaining == 0)
                return EOF;

            --remaining;

            int temp = m_nIndexPixel + 1;
            if (temp < m_arrPixels.GetUpperBound(0)) {
                byte pix = m_arrPixels[m_nIndexPixel++];

                return pix & 0xff;
            }
            return 0xff;
        }

        void Output(int code, List<byte> lst) {
            cur_accum &= masks[cur_bits];

            if (cur_bits > 0)
                cur_accum |= (code << cur_bits);
            else
                cur_accum = code;

            cur_bits += n_bits;

            while (cur_bits >= 8) {
                Add((byte)(cur_accum & 0xff), lst);
                cur_accum >>= 8;
                cur_bits -= 8;
            }

            // If the next entry is going to be too big for the code size,
            // then increase it, if possible.
            if (free_ent > maxcode || clear_flg) {
                if (clear_flg) {
                    maxcode = MaxCode(n_bits = g_init_bits);
                    clear_flg = false;
                } else {
                    ++n_bits;
                    if (n_bits == maxbits)
                        maxcode = maxmaxcode;
                    else
                        maxcode = MaxCode(n_bits);
                }
            }

            if (code == EOFCode) {
                // At EOF, write the rest of the buffer.
                while (cur_bits > 0) {
                    Add((byte)(cur_accum & 0xff), lst);
                    cur_accum >>= 8;
                    cur_bits -= 8;
                }

                Flush(lst);
            }
        }
    }
}
