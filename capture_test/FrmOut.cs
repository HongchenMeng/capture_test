using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Drawing.Imaging;

namespace DevCapture
{
    public partial class FrmOut : Form
    {
        public FrmOut(Image img) {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            m_img = img;

            this.KeyPreview = true;
            this.FormClosing += (s, e) => m_img.Dispose();

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        private Image m_img;
        public Image Image {
            get { return m_img; }
        }

        private Point m_ptLastDown;
        private bool m_bTwist;
        private int m_nAlpha;
        private Size m_szForm;
        private float m_fScale;


        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            this.Width = m_img.Width + 2;
            this.Height = m_img.Height + 2;
            m_szForm = m_img.Size;
            m_fScale = 1;
            this.Twist();
        }

        protected override void OnMouseClick(MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) this.Close();
            base.OnMouseClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            m_ptLastDown = e.Location;
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle)
                this.Location = (Point)((Size)MousePosition - (Size)m_ptLastDown);
            base.OnMouseMove(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e) {
            if (this.Size != this.MinimumSize)
                this.Size = this.MinimumSize;
            else
                this.Size = new Size(m_img.Width + 2, m_img.Height + 2);
            base.OnMouseDoubleClick(e);
        }

        protected override void OnMouseEnter(EventArgs e) {
            this.Twist();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e) {
            int x = this.Left;
            int y = this.Top;
            int w = this.Width;
            int h = this.Height;
            float fTemp = m_fScale;
            float nIncrement = e.Delta > 0 ? 0.1F : -0.1F;
            fTemp += nIncrement;
            x = (int)(MousePosition.X - (int)(e.X / (fTemp - nIncrement)) * fTemp);
            y = (int)(MousePosition.Y - (int)(e.Y / (fTemp - nIncrement)) * fTemp);
            w = (int)(m_szForm.Width * fTemp + 2);
            h = (int)(m_szForm.Height * fTemp + 2);
            if ((w <= this.MinimumSize.Width || h <= this.MinimumSize.Height) && nIncrement < 0) return;
            m_fScale = fTemp;
            Win32.MoveWindow(this.Handle, x, y, w, h, true);
            base.OnMouseWheel(e);
        }
        
        protected override void OnPaint(PaintEventArgs e) {
            if (m_img == null) {
                MessageBox.Show("Bitmap cannot be null!");
                this.Close();
            }
            Graphics g = e.Graphics;
            g.DrawImage(m_fScale > 1 ? ImageHelper.ZoomImage(m_img, m_fScale) : m_img, 1, 1, this.Width - 2, this.Height - 2);
            g.DrawRectangle(Pens.Black, 0, 0, this.Width - 1, this.Height - 1);
            if (m_nAlpha > 0) {
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(m_nAlpha, 0, 0, 0))) {
                    g.FillRectangle(sb, 1, 1, this.Width - 2, this.Height - 2);
                    sb.Color = Color.FromArgb(m_nAlpha, 0, 0, 0);

                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;

                    string strDraw = "原图:" + ("[" + m_img.Width + "," + m_img.Height + "]").PadRight(15, ' ')
                        + "倍数:" + ((double)(this.Width - 2) / m_img.Width).ToString("F2").PadRight(6, ' ') + "[宽]"
                        + "\r\n当前:" + ("[" + (this.Width - 2) + "," + (this.Height - 2) + "]").PadRight(15, ' ')
                        + "倍数:" + ((double)(this.Height - 2) / m_img.Height).ToString("F2").PadRight(6, ' ') + "[高]";
                    Rectangle rectString = new Rectangle(new Point(1, 1), g.MeasureString(strDraw, this.Font, this.Width, sf).ToSize());
                    rectString.Width += 5; rectString.Height += 5;
                    g.FillRectangle(sb, rectString);
                    g.DrawString(strDraw, this.Font, Brushes.White, rectString, sf);

                    rectString = new Rectangle(1, this.Height - 2 * this.Font.Height - 6,
                        this.Width - 2, this.Font.Height * 2 + 5);
                    sf.Alignment = StringAlignment.Far;
                    g.FillRectangle(sb, rectString);
                    g.DrawString("缩放 [中间] 退出 [右键]\r\n移动 [W,S,A,D]", this.Font, Brushes.White, rectString, sf);
                }
            }
            base.OnPaint(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e) {
            switch (e.KeyChar) {
                case 'w': this.Top--; break;
                case 's': this.Top++; break;
                case 'a': this.Left--; break;
                case 'd': this.Left++; break;
            }
           
            base.OnKeyPress(e);
        }

        private void Twist() {
            lock (this) if (m_bTwist) return;
            m_bTwist = true;
            Thread threadShow = new Thread(new ThreadStart(() => {
                try {
                    for (int i = 0; i < 4; i++) {
                        m_nAlpha = i % 2 == 0? 150 : 0;
                        this.Invoke(new MethodInvoker(() => this.Invalidate()));
                        Thread.Sleep(200);
                    }
                    m_nAlpha = 150;
                    while (m_nAlpha > 0) {
                        this.Invoke(new MethodInvoker(() => this.Invalidate()));
                        m_nAlpha -= 5;
                        Thread.Sleep(50);
                    }
                    this.Invoke(new MethodInvoker(() => this.Invalidate()));
                } catch { }
                lock (this) m_bTwist = false;
            }));
            threadShow.IsBackground = true;
            threadShow.Start();
        }

        private void FrmOut_Load(object sender, EventArgs e) {
            this.Icon = Properties.Resources.frm_out;
        }
    }
}
