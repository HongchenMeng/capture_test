using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IPlugins
{
    public partial class FrmBase : Form
    {
        public FrmBase() {
            InitializeComponent();

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.Padding = new Padding(2, 35, 2, 2);
            base.FormBorderStyle = FormBorderStyle.None;
            this.Icon = Properties.Resources.Icon;
            this.BackColor = Color.Black;
        }

        public new bool MaximizeBox {
            get { return base.MaximizeBox; }
            set {
                if (value == base.MaximizeBox) return;
                base.MaximizeBox = value;
                this.CheckTitleBarSize();
            }
        }

        public new bool MinimizeBox {
            get { return base.MinimizeBox; }
            set {
                if (value == base.MinimizeBox) return;
                base.MinimizeBox = value;
                this.CheckTitleBarSize();
            }
        }

        public new FormBorderStyle FormBorderStyle {
            get { return FormBorderStyle.None; }
            set { }
        }

        public override string Text {
            get { return base.Text; }
            set {
                if (value == base.Text) return;
                base.Text = value;
                this.Invalidate(m_rectTitle);
            }
        }
        
        private bool _Sizeable = true;

        public bool Sizeable {
            get { return _Sizeable; }
            set { _Sizeable = value; }
        }

        private Rectangle m_rectClose;
        private Rectangle m_rectMax;
        private Rectangle m_rectMin;
        private Rectangle m_rectTitle;

        private bool m_bMouseOnClose;
        private bool m_bMouseOnMax;
        private bool m_bMouseOnMin;

        protected override void OnSizeChanged(EventArgs e) {
            this.CheckTitleBarSize();
            base.OnSizeChanged(e);
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            if (m_rectTitle.Contains(e.Location)) {
                Win32.ReleaseCapture();
                Win32.SendMessage(this.Handle, Win32.WM_SYSCOMMAND, Win32.SC_MOVE + (int)Win32.HTCAPTION, 0);
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            this.CheckRectToReDraw(m_rectClose, e.Location, ref m_bMouseOnClose);
            this.CheckRectToReDraw(m_rectMax, e.Location, ref m_bMouseOnMax);
            this.CheckRectToReDraw(m_rectMin, e.Location, ref m_bMouseOnMin);
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e) {
            m_bMouseOnClose = m_bMouseOnMax = m_bMouseOnMin = false;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnClick(EventArgs e) {
            if (m_bMouseOnClose) this.Close();
            if (m_bMouseOnMax) this.WindowState = this.WindowState != FormWindowState.Maximized ? FormWindowState.Maximized : FormWindowState.Normal;
            if (m_bMouseOnMin) this.WindowState = FormWindowState.Minimized;
            base.OnClick(e);
        }

        protected override void OnPaint(PaintEventArgs e) {
            Graphics g = e.Graphics;
            this.OnDrawTitle(g);
            g.DrawRectangle(Pens.Gray, 0, 0, this.Width - 1, this.Height - 1);
            base.OnPaint(e);
        }

        protected virtual void OnDrawTitle(Graphics g) {
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            g.FillRectangle(Brushes.White, 0, 0, this.Width, 35);
            g.DrawIcon(this.Icon, new Rectangle(6, 6, 23, 23));
            g.DrawString(this.Text, this.Font, Brushes.Gray, m_rectTitle, sf);
            if (m_rectClose != Rectangle.Empty) {
                g.DrawLine(Pens.Gray, m_rectClose.Right - 1, 0, m_rectClose.Right - 1, 34);
                if (m_bMouseOnClose) g.FillRectangle(Brushes.Red, m_rectClose);
                g.DrawLine(Pens.Gray, m_rectClose.X + 14, 14, m_rectClose.X + 21, 21);
                g.DrawLine(Pens.Gray, m_rectClose.X + 14, 21, m_rectClose.X + 21, 14);
            }
            if (m_rectMax != Rectangle.Empty) {
                g.DrawLine(Pens.Gray, m_rectMax.Right - 1, 0, m_rectMax.Right - 1, 34);
                if (m_bMouseOnMax) g.FillRectangle(Brushes.LightGray, m_rectMax);
                g.DrawRectangle(Pens.Gray, m_rectMax.X + 14, 14, 7, 7);
            }
            if (m_rectMin != Rectangle.Empty) {
                g.DrawLine(Pens.Gray, m_rectMin.Right - 1, 0, m_rectMin.Right- 1, 34);
                if (m_bMouseOnMin) g.FillRectangle(Brushes.LightGray, m_rectMin);
                g.DrawLine(Pens.Gray, m_rectMin.X + 14, 21, m_rectMin.X + 21, 21);
            }
        }

        protected override void WndProc(ref Message m) {
            switch ((uint)m.Msg) {
                case Win32.WM_NCHITTEST:
                    m.Result = this.OnHitTest(m);
                    return;
                case Win32.WM_GETMINMAXINFO:        //在无边窗体时确定窗体最大、最小、最大化尺寸
                    Rectangle rectArea = Screen.GetWorkingArea(MousePosition);
                    Rectangle rectBounds = Screen.GetBounds(MousePosition);
                    Win32.MINMAXINFO stMinMaxInfo = (Win32.MINMAXINFO)m.GetLParam(typeof(Win32.MINMAXINFO));
                    stMinMaxInfo.ptMinTrackSize.X = 160;
                    stMinMaxInfo.ptMinTrackSize.Y = 60;
                    //窗体最大化坐标及宽高
                    stMinMaxInfo.ptMaxPosition.X = rectArea.X - rectBounds.X;
                    stMinMaxInfo.ptMaxPosition.Y = rectArea.Y - rectBounds.Y;
                    stMinMaxInfo.ptMaxSize.X = rectArea.Width;
                    stMinMaxInfo.ptMaxSize.Y = rectArea.Height;
                    System.Runtime.InteropServices.Marshal.StructureToPtr(stMinMaxInfo, m.LParam, true);
                    return;
            }
            base.WndProc(ref m);
        }

        protected virtual IntPtr OnHitTest(Message msg) {
            Point pt = new Point((int)msg.LParam);
            pt.Offset(-this.Left, -this.Top);
            if (this._Sizeable) {
                if (pt.X < 5 && pt.Y < 5)
                    return (IntPtr)Win32.HTTOPLEFT;
                if (pt.X > this.Width - 5 && pt.Y < 5)
                    return (IntPtr)Win32.HTTOPRIGHT;
                if (pt.X < 5 && pt.Y > this.Height - 5)
                    return (IntPtr)Win32.HTBOTTOMLEFT;
                if (pt.X > this.Width - 5 && pt.Y > this.Height - 5)
                    return (IntPtr)Win32.HTBOTTOMRIGHT;

                if (pt.X < 5) return (IntPtr)Win32.HTLEFT;
                if (pt.Y < 5) return (IntPtr)Win32.HTTOP;
                if (pt.X > this.Width - 5) return (IntPtr)Win32.HTRIGHT;
                if (pt.Y > this.Height - 5) return (IntPtr)Win32.HTBOTTOM;
            }
            if (this.MaximizeBox && m_rectTitle.Contains(pt)) return (IntPtr)Win32.HTCAPTION;
            return (IntPtr)Win32.HTCLIENT;
        }

        private void CheckTitleBarSize() {
            m_rectClose = new Rectangle(this.Width - 35, 0, 35, 35);
            if (this.MaximizeBox)
                m_rectMax = new Rectangle(this.Width - 70, 0, 35, 35);
            else m_rectMax = Rectangle.Empty;
            if (this.MinimizeBox)
                m_rectMin = new Rectangle(this.Width - 70 - m_rectMax.Width, 0, 35, 35);
            else m_rectMin = Rectangle.Empty;
            m_rectTitle = new Rectangle(35, 0, this.Width - 70 - m_rectMax.Width - m_rectMin.Width, 35);
        }

        private void CheckRectToReDraw(Rectangle rect, Point pt, ref bool bFlag) {
            if (rect.Contains(pt)) {
                if (!bFlag) {
                    bFlag = true;
                    this.Invalidate(rect);
                }
            } else {
                if (bFlag) {
                    bFlag = false;
                    this.Invalidate(rect);
                }
            }
        }
    }
}
