using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DevCapture
{
    public partial class FrmRectAlert : Form
    {
        public FrmRectAlert(Rectangle rect, string strShow) {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = rect.Location;
            this.Size = rect.Size;
            this.TopMost = true;
            this.BackColor = Color.Black;
            m_strShow = strShow;
        }

        private string m_strShow;

        private void FrmRectAlert_Load(object sender, EventArgs e) {
            new System.Threading.Thread(() => {
                int i = 0;
                while (i++ < 5) {
                    this.Invoke(new MethodInvoker(() => this.Opacity = i % 2 == 0 ? 0 : 1));
                    System.Threading.Thread.Sleep(200);
                }
                this.Invoke(new MethodInvoker(() => this.Close()));
            }) { IsBackground = true }.Start();
        }

        protected override void OnPaint(PaintEventArgs e) {
            Graphics g = e.Graphics;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            g.DrawRectangle(Pens.White, 0, 0, this.Width - 1, this.Height - 1);
            using (Font ft = new Font("simsun", 12, FontStyle.Bold)) {
                g.DrawString(m_strShow, ft, Brushes.White, this.DisplayRectangle, sf);
            }
            base.OnPaint(e);
        }
    }
}
