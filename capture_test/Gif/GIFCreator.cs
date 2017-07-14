using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace DevCapture
{
    public class GIFCreator
    {
        public enum GIFColorDepth : byte
        {
            Depth1Bit = 1,
            Depth4Bit = 4,
            Depth8Bit = 8
        }

        private int _Width;

        public int Width {
            get { return _Width; }
        }

        private int _Height;

        public int Height {
            get { return _Height; }
        }

        private int _Sample = 10; // default sample interval for quantizer

        public int Sample {
            get { return _Sample; }
            set { _Sample = value; }
        }

        public int Size {
            get { return m_lst.Count; }
        }

        private Color _TransparentColor = Color.Empty;

        public Color TransparentColor {
            get { return _TransparentColor; }
            set { _TransparentColor = value; }
        }

        private bool m_bGet;
        private GIFColorDepth m_cd;
        private List<byte> m_lst = new List<byte>();


        private int m_nIndexTrans;                        // transparent index in color table
        private int m_nRepeat = 0;                        // 0=repeat forever

        private byte[] m_nIndexedPixels;                  // converted frame indexed to palette
        private byte[] m_byColorTabs;                     // RGB palette
        private bool[] m_bUsedEntrys = new bool[256];     // active palette entries
        private int m_nPalSize = 7;                       // color table size (bits-1)
        private int n_nDispose = -1;                      // disposal code (-1 = use default)
        private bool m_bFirstFrame = true;

        public GIFCreator(int nWidth, int nHeight, GIFColorDepth cd) {
            this._Width = nWidth;
            this._Height = nHeight;
            m_cd = cd;
            //m_nPalSize = (int)m_cd - 1;
            //if (m_nPalSize < 1) m_nPalSize = 1;
            m_lst.AddRange(Encoding.ASCII.GetBytes("GIF89a"));
            this.WriteLSD();    // logical screen descriptior
        }

        public void AddFrame(Image img, int nSleep) {
            if (m_bGet) {       //如果获取过图像数据 那么把末尾的结束标志去掉
                for (int i = 0; i < 19; i++)
                    m_lst.RemoveAt(m_lst.Count - 1);
                m_bGet = false;
            }
            this.AnalyzePixels(this.GetImagePixels(img));   // build color table & map pixels
            if (m_bFirstFrame) {
                this.WritePalette();                        // global color table
                if (m_nRepeat >= 0) {
                    this.WriteNetscapeExt();                // use NS app extension to indicate reps
                }
            }
            this.WriteGraphicCtrlExt(nSleep);               // write graphic control extension
            this.WriteImageDesc();                          // image descriptor
            if (!m_bFirstFrame) {
                this.WritePalette();                        // local color table
            }
            this.WritePixels();                             // encode and write pixel data
            m_bFirstFrame = false;
        }

        /**
         * Analyzes image colors and creates color map.
         */
        private void AnalyzePixels(byte[] byPixels) {
            m_nIndexedPixels = new byte[byPixels.Length / 3];
            NeuQuant nq = new NeuQuant();
            m_byColorTabs = nq.GetColorMap(byPixels, _Sample, 1 << (int)m_cd);// = nq.Process(); // create reduced palette
            // map image pixels to new palette
            for (int i = 0, k = 0; i < m_nIndexedPixels.Length; i++) {
                int index = nq.Map(byPixels[k++] & 0xff,
                    byPixels[k++] & 0xff,
                    byPixels[k++] & 0xff);
                m_bUsedEntrys[index] = true;
                m_nIndexedPixels[i] = (byte)index;
            }
            //m_nPalSize = 7;
            // get closest match to transparent color if specified
            if (this._TransparentColor != Color.Empty) {
                m_nIndexTrans = this.FindClosest(this._TransparentColor);
            }
        }

        /**
         * Returns index of palette color closest to c
         *
         */
        private int FindClosest(Color c) {
            if (m_byColorTabs == null) return -1;
            int r = c.R;
            int g = c.G;
            int b = c.B;
            int index = 0;
            int minpos = 0;
            int dmin = 256 * 256 * 256;
            for (int i = 0; i < m_byColorTabs.Length; i++) {
                index = i / 3;
                int dr = r - (m_byColorTabs[i++] & 0xff);
                int dg = g - (m_byColorTabs[i++] & 0xff);
                int db = b - (m_byColorTabs[i] & 0xff);
                int d = dr * dr + dg * dg + db * db;
                if (m_bUsedEntrys[index] && (d < dmin)) {
                    dmin = d;
                    minpos = index;
                }
            }
            return minpos;
        }

        /**
         * Extracts image pixels into byte array "pixels"
         */
        private byte[] GetImagePixels(Image img) {
            Bitmap bmpTemp = (Bitmap)img;
            if (img.Width != this._Width || img.Height != this._Height) {
                bmpTemp = new Bitmap(this._Width, this._Height);
                using (Graphics g = Graphics.FromImage(bmpTemp)) {
                    g.DrawImage(img, 0, 0, this._Width, this._Height);
                }
            }

            byte[] byPixels = new Byte[3 * bmpTemp.Width * bmpTemp.Height];
            int nIndex = 0;

            BitmapData bmpData = bmpTemp.LockBits(new Rectangle(Point.Empty, bmpTemp.Size), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            byte[] byClr = new byte[bmpData.Height * bmpData.Stride];
            Marshal.Copy(bmpData.Scan0, byClr, 0, byClr.Length);
            bmpTemp.UnlockBits(bmpData);

            for (int th = 0; th < img.Height; th++) {
                for (int tw = 0; tw < img.Width; tw++) {
                    byPixels[nIndex] = byClr[th * bmpData.Stride + tw * 3 + 2];
                    nIndex++;
                    byPixels[nIndex] = byClr[th * bmpData.Stride + tw * 3 + 1];
                    nIndex++;
                    byPixels[nIndex] = byClr[th * bmpData.Stride + tw * 3];
                    nIndex++;
                }
            }
            return byPixels;
        }

        /**
         * Writes Graphic Control Extension
         */
        private void WriteGraphicCtrlExt(int nSleep) {
            m_lst.Add(0x21);    // extension introducer
            m_lst.Add(0xf9);    // GCE label
            m_lst.Add(4);       // data block size

            int transp, disp;
            if (_TransparentColor == Color.Empty) {
                transp = 0;
                disp = 0;       // dispose = no action
            } else {
                transp = 1;
                disp = 2;       // force clear if using transparent color
            }
            if (n_nDispose >= 0) {
                disp = n_nDispose & 7;  // user override
            }
            disp <<= 2;

            byte byRDIT = Convert.ToByte(
                0 |             // 1:3 reserved
                disp |          // 4:6 disposal
                0 |             // 7   user input - 0 = none
                transp);

            // packed fields
            m_lst.Add(byRDIT);
            m_lst.AddRange(BitConverter.GetBytes((short)Math.Round(nSleep / 10.0f)));
            m_lst.Add(Convert.ToByte(m_nIndexTrans));
            m_lst.Add(0);
        }

        /**
         * Writes Image Descriptor
         */
        private void WriteImageDesc() {
            m_lst.Add(0x2C);    // image separator
            m_lst.AddRange(BitConverter.GetBytes((short)0));            // image position x,y = 0,0
            m_lst.AddRange(BitConverter.GetBytes((short)0));
            m_lst.AddRange(BitConverter.GetBytes((short)this._Width));  // image size
            m_lst.AddRange(BitConverter.GetBytes((short)this._Height));
            byte byMISRP = Convert.ToByte(
                 0x80 | // 1 local color table  1=yes
                    0 | // 2 interlace - 0=no
                    0 | // 3 sorted - 0=no
                    0 | // 4-5 reserved
                    m_nPalSize);
            // packed fields
            m_lst.Add(m_bFirstFrame ? (byte)0 : byMISRP);
        }

        /**
         * Writes Logical Screen Descriptor
         */
        private void WriteLSD() {
            // logical screen size
            m_lst.AddRange(BitConverter.GetBytes((short)this._Width)); // image size
            m_lst.AddRange(BitConverter.GetBytes((short)this._Height));
            // packed fields
            byte byGCSP = Convert.ToByte(
                0x80 |      // 1   : global color table flag = 1 (gct used)
                0x70 |      // 2-4 : color resolution = 7
                0x00 |      // 5   : gct sort flag = 0
                m_nPalSize);
            m_lst.Add(byGCSP);
            m_lst.Add(0);   // background color index
            m_lst.Add(0);   // pixel aspect ratio - assume 1:1
        }

        /**
         * Writes Netscape application extension to define
         * repeat count.
         */
        private void WriteNetscapeExt() {
            m_lst.Add(0x21);    // extension introducer
            m_lst.Add(0xff);    // app extension label
            m_lst.Add(11);      // block size
            m_lst.AddRange(Encoding.ASCII.GetBytes("NETSCAPE2.0"));
            m_lst.Add(3);       // sub-block size
            m_lst.Add(1);       // loop sub-block id

            m_lst.AddRange(BitConverter.GetBytes((short)m_nRepeat)); // loop count (extra iterations, 0=repeat forever);
            m_lst.Add(0);       // block terminator
        }

        /**
         * Writes color table
         */
        private void WritePalette() {
            m_lst.AddRange(m_byColorTabs);
            int n = (3 * 256) - m_byColorTabs.Length;
            for (int i = 0; i < n; i++) {
                m_lst.Add(0);
            }
        }

        /**
         * Encodes and writes pixel data
         */
        private void WritePixels() {
            GIFLZWEncoder encoder = new GIFLZWEncoder(_Width, _Height, m_nIndexedPixels, (int)m_cd);
            m_lst.AddRange(encoder.Encode());
        }

        public byte[] GetImageBytes() {
            if (m_bGet) return m_lst.ToArray();
            m_bGet = true;
            m_lst.Add(0x3B);// gif trailer
            m_lst.AddRange(Encoding.ASCII.GetBytes("\0\0\r\nBy->Crystal_lz"));
            return m_lst.ToArray();
        }

        public Image GetGifImage() {
            MemoryStream ms = new MemoryStream(this.GetImageBytes());
            return Image.FromStream(ms);
        }
    }
}
