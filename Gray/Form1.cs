using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Gray
{
    public partial class Form1 : IPlugins.FrmBase
    {
        public Form1(Image img) {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Sizeable = false;
            this.TopMost = true;        //很重要 不然会被截图窗体挡住
            //this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
            btn_cancel.Click += (s, e) => this.Close();
            btn_ok.Click += (s, e) => this.DialogResult = DialogResult.OK;
            m_imgSrc = img;
            if (m_imgSrc.Width > pictureBox1.Width || m_imgSrc.Height > pictureBox1.Height)
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            else
                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            this.Text = "Powered by Crystal_lz";
        }

        private Image m_imgSrc;

        private Image _ResultImage;

        public Image ResultImage {
            get { return _ResultImage; }
        }

        private void Form1_Load(object sender, EventArgs e) {
            this._ResultImage = this.GetBinaryImage(m_imgSrc, 127);
            pictureBox1.Image = this._ResultImage;
        }

        private void trackBox1_ValueChanged(object sender, EventArgs e) {
            this.Text = trackBox1.Value.ToString();
            if (this._ResultImage != null) this._ResultImage.Dispose();
            this._ResultImage = this.GetBinaryImage(m_imgSrc, (byte)trackBox1.Value);
            pictureBox1.Image = this._ResultImage;
        }

        public Image GetBinaryImage(Image imgSrc, byte byValue) {
            Bitmap b = new Bitmap(imgSrc);
            Bitmap bmp = b.Clone(new Rectangle(0, 0, imgSrc.Width, imgSrc.Height), PixelFormat.Format24bppRgb);
            b.Dispose();
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            byte[] byColorInfo = new byte[bmp.Height * bmpData.Stride];
            Marshal.Copy(bmpData.Scan0, byColorInfo, 0, byColorInfo.Length);
            byte byR, byG, byB;
            for (int x = 0, xLen = bmp.Width; x < xLen; x++) {
                for (int y = 0, yLen = bmp.Height; y < yLen; y++) {
                    byB = byColorInfo[y * bmpData.Stride + x * 3];
                    byG = byColorInfo[y * bmpData.Stride + x * 3 + 1];
                    byR = byColorInfo[y * bmpData.Stride + x * 3 + 2];
                    byte byV = (byte)((byR + byG + byB) / 3);
                    byV = (byte)(byV > byValue ? 255 : 0);
                    byColorInfo[y * bmpData.Stride + x * 3] =
                        byColorInfo[y * bmpData.Stride + x * 3 + 1] =
                        byColorInfo[y * bmpData.Stride + x * 3 + 2] = byV;
                }
            }
            Marshal.Copy(byColorInfo, 0, bmpData.Scan0, byColorInfo.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}
