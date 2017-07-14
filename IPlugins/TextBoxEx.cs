using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace IPlugins
{
    public class TextBoxEx : TextBox
    {
        public TextBoxEx() {
            base.BorderStyle = BorderStyle.None;

            this.GotFocus += (s, e) => {
                m_bFocus = true;
                if (m_parent != null)
                    m_parent.Invalidate(m_rect);
            };
            this.LostFocus += (s, e) => {
                m_bFocus = false;
                if (m_parent != null)
                    m_parent.Invalidate(m_rect);
            };
        }

        //public new BorderStyle BorderStyle {
        //    get { return BorderStyle.None; }
        //    set { }
        //}

        private bool m_bFocus;
        private Control m_parent;
        private Rectangle m_rect;

        protected override void OnParentChanged(EventArgs e) {
            if (this.Parent != null) {
                m_parent = this.Parent;
                this.Parent.Paint += Parent_Paint;
            }
            base.OnParentChanged(e);
        }

        protected override void OnResize(EventArgs e) {
            m_rect = new Rectangle(this.Left - 3, this.Top - 3, this.Width + 6, this.Height + 6);
            if (m_parent != null) m_parent.Invalidate(m_rect);
            base.OnResize(e);
        }

        private void Parent_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            using (LinearGradientBrush lgb = new LinearGradientBrush(
               new Point(m_rect.X, m_rect.Y + 1), new Point(m_rect.X, m_rect.Height),
               Color.Transparent, Color.FromArgb(40, 255, 255, 255))) {
                g.DrawPath(
                    new Pen(lgb),
                    RenderHelper.CreateRoundedRectanglePath(new Rectangle(m_rect.X, m_rect.Y + 1, m_rect.Width - 1, m_rect.Height - 2), 5)
                    );
            }
            using (SolidBrush sb = new SolidBrush(Color.White)) {
                g.FillPath(
                    sb,
                    RenderHelper.CreateRoundedRectanglePath(new Rectangle(m_rect.X, m_rect.Y, m_rect.Width - 1, m_rect.Height - 2), 5)
                    );
            }
            using (LinearGradientBrush lgb = new LinearGradientBrush(
               m_rect.Location, new Point(m_rect.X, m_rect.Height - 1),
               m_bFocus ? Color.FromArgb(125, 0, 125, 255) : Color.FromArgb(125, 0, 0, 0), Color.FromArgb(20, 0, 0, 0))) {
                g.DrawPath(
                    new Pen(lgb),
                    RenderHelper.CreateRoundedRectanglePath(new Rectangle(m_rect.X, m_rect.Y, m_rect.Width - 1, m_rect.Height - 2), 5)
                    );
            }
        }
    }
}
