using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SpyTool
{
    public partial class FrmShowRect : Form
    {
        public FrmShowRect() {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.BackColor = Color.Green;
            this.TransparencyKey = this.BackColor;
            //this.TopMost = true;

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        private int m_xc;
        private int m_yc;
        private int m_rc;
        private int m_bc;
        private Rectangle m_rectTemp;
        private bool m_bAnimateRunning;
        private object m_obj_sync = new object();

        private void FrmShowRect_Load(object sender, EventArgs e) {
            Rectangle rect = Win32.GetDesktopRect();
            this.Location = rect.Location;
            this.Size = rect.Size;
        }

        protected override void OnPaint(PaintEventArgs e) {
            Graphics g = e.Graphics;
            using (Pen p = new Pen(Brushes.Cyan, 3)) {
                g.DrawRectangle(p, m_rectTemp);
            }
            base.OnPaint(e);
        }

        public void AnimateSelect(Rectangle rect) {
            if (this.Left < 0) rect.X -= this.Left;
            if (this.Top < 0) rect.Y -= this.Top;
            m_xc = (int)Math.Ceiling(Math.Abs(rect.X - m_rectTemp.X) / (double)20);
            if (m_xc == 0) m_xc = 1;
            m_yc = (int)Math.Ceiling(Math.Abs(rect.Y - m_rectTemp.Y) / (double)20);
            if (m_yc == 0) m_yc = 1;
            m_rc = (int)Math.Ceiling(Math.Abs(rect.Right - m_rectTemp.Right) / (double)20);
            if (m_rc == 0) m_rc = 1;
            m_bc = (int)Math.Ceiling(Math.Abs(rect.Bottom - m_rectTemp.Bottom) / (double)20);
            if (m_bc == 0) m_bc = 1;
            lock (m_obj_sync) {
                if (m_bAnimateRunning || (rect == m_rectTemp)) {
                    this.Invalidate();
                    return;
                }
                m_bAnimateRunning = true;
            }
            System.Threading.ThreadPool.QueueUserWorkItem((o) => {
                int increament = 0;
                while (m_rectTemp != rect) {
                    if (m_rectTemp.X != rect.X) {
                        increament = rect.X < m_rectTemp.X ? -m_xc : m_xc;
                        m_rectTemp.X += increament;
                        m_rectTemp.Width -= increament;
                        if (Math.Abs(rect.X - m_rectTemp.X) <= Math.Abs(increament)) m_rectTemp.X = rect.X;
                    }
                    if (m_rectTemp.Y != rect.Y) {
                        increament = rect.Y < m_rectTemp.Y ? -m_yc : m_yc;
                        m_rectTemp.Y += increament;
                        m_rectTemp.Height -= increament;
                        if (Math.Abs(m_rectTemp.Y - rect.Y) <= Math.Abs(increament)) m_rectTemp.Y = rect.Y;
                    }
                    if (m_rectTemp.Right != rect.Right) {
                        m_rectTemp.Width += rect.Right < m_rectTemp.Right ? -m_rc : m_rc;
                        if (Math.Abs(m_rectTemp.Right - rect.Right) < Math.Abs(m_rc)) m_rectTemp.Width = rect.Right - m_rectTemp.X;
                    }
                    if (m_rectTemp.Bottom != rect.Bottom) {
                        m_rectTemp.Height += rect.Bottom < m_rectTemp.Bottom ? -m_bc : m_bc;
                        if (Math.Abs(m_rectTemp.Bottom - rect.Bottom) < Math.Abs(m_bc)) m_rectTemp.Height = rect.Bottom - m_rectTemp.Y;
                    }
                    this.Invalidate();
                    System.Threading.Thread.Sleep(5);
                }
                m_rectTemp = rect;
                lock (m_obj_sync) m_bAnimateRunning = false;
                this.Invalidate();
            });
        }
    }
}
