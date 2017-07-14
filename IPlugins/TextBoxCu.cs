using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace IPlugins
{
    public class TextBoxCu : Control
    {
        public TextBoxCu() {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            m_tbx = new TextBox();
            m_tbx.Multiline = true;
            m_tbx.BorderStyle = BorderStyle.None;
            this.BackColor = Color.Transparent;
            m_tbx.GotFocus += (s, e) => { m_bFocus = true; this.Invalidate(); };
            m_tbx.LostFocus += (s, e) => { m_bFocus = false; this.Invalidate(); };
            m_tbx.KeyDown += (s, e) => this.OnKeyDown(e);
            m_tbx.KeyPress += (s, e) => this.OnKeyPress(e);
            m_tbx.KeyUp += (s, e) => this.OnKeyUp(e);
            m_tbx.TextChanged += (s, e) => this.OnTextChanged(e);
            m_tbx.BackColorChanged += (s, e) => m_tbx.BackColor = Color.White;
            this.Controls.Add(m_tbx);
        }

        public override string Text {
            get { return m_tbx.Text; }
            set {
                if (m_tbx.Text == value) return;
                m_tbx.Text = value;
            }
        }

        public TextBox TextBox {
            get { return m_tbx; }
        }

        public override Color BackColor {
            get { return Color.Transparent; }
            set { }
        }

        public override Color ForeColor {
            get { return base.ForeColor; }
            set {
                if (base.ForeColor == value) return;
                base.ForeColor = value;
                m_tbx.ForeColor = value;
            }
        }

        private bool _ReadOnly;

        public bool ReadOnly {
            get { return m_tbx.ReadOnly; }
            set { m_tbx.ReadOnly = value; m_tbx.BackColor = Color.White; }
        }

        private TextBox m_tbx;
        private bool m_bFocus;

        public event EventHandler TextChanged;

        protected virtual void OnTextChanged(EventArgs e) {
            if (this.TextChanged != null) this.TextChanged(this, e);
        }

        protected override void OnResize(EventArgs e) {
            m_tbx.Left = m_tbx.Top = 3;
            m_tbx.Width = this.Width - 6;
            m_tbx.Height = this.Height - 6;
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            using (SolidBrush sb = new SolidBrush(Color.White)) {
                g.FillPath(
                    sb,
                    RenderHelper.CreateRoundedRectanglePath(new Rectangle(0, 0, this.Width - 1, this.Height - 1), 5)
                    );
            }
            using (LinearGradientBrush lgb = new LinearGradientBrush(
               new Point(0, 1), new Point(0, this.Height),
               Color.LightGray, Color.White)) {
                g.DrawPath(
                    new Pen(lgb),
                    RenderHelper.CreateRoundedRectanglePath(new Rectangle(0, 1, this.Width - 1, this.Height - 2), 5)
                    );
            }
            using (LinearGradientBrush lgb = new LinearGradientBrush(
               new Point(0, 0), new Point(0, this.Height - 1),
               m_bFocus ? Color.DodgerBlue : Color.Gray, Color.FromArgb(20, 0, 0, 0))) {
                g.DrawPath(
                    new Pen(lgb),
                    RenderHelper.CreateRoundedRectanglePath(new Rectangle(0, 0, this.Width - 1, this.Height - 2), 5)
                    );
            }
            base.OnPaint(e);
        }
    }
}
