using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace IPlugins
{
    public class ButtonEx : Button
    {
        public ButtonEx() {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.FlatStyle = FlatStyle.Flat;
            this.BackColor = Color.Transparent;
            this.ForeColor = Color.FromArgb(125, 125, 125);
            this.FlatAppearance.BorderSize = 0;
            this.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.FlatAppearance.MouseDownBackColor = Color.Transparent;
        }

        public new FlatStyle FlatStyle {
            get { return FlatStyle.Flat; }
            set { }
        }

        private bool m_bMouseOn;
        private bool m_bMouseDown;

        protected override void OnMouseHover(EventArgs e) {
            if (!m_bMouseOn) {
                m_bMouseOn = true;
                this.Invalidate();
            }
            base.OnMouseHover(e);
        }

        protected override void OnMouseLeave(EventArgs e) {
            if (m_bMouseOn) {
                m_bMouseOn = false;
                this.Invalidate();
            }
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent) {
            if (!m_bMouseDown) {
                m_bMouseDown = true;
                this.Invalidate();
            }
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent) {
            if (m_bMouseDown) {
                m_bMouseDown = false;
                this.Invalidate();
            }
            base.OnMouseUp(mevent);
        }

        //override onba
        protected override void OnAutoSizeChanged(EventArgs e) {
            this.BackColor = Color.Transparent;
            base.OnAutoSizeChanged(e);
        }

        protected override void OnPaint(PaintEventArgs pevent) {
            base.OnPaint(pevent);
            base.OnPaintBackground(pevent);
            Graphics g = pevent.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            using (LinearGradientBrush lgb = new LinearGradientBrush(
                new Point(0, 1), new Point(0, this.Height),
                Color.White, Color.FromArgb(127, 0, 0, 0))) {
                g.DrawPath(
                    new Pen(lgb),
                    RenderHelper.CreateRoundedRectanglePath(new Rectangle(0, 1, this.Width - 1, this.Height - 2), 5)
                    );
            }
            using (LinearGradientBrush lgb = new LinearGradientBrush(
                new Point(0, 1), new Point(0, this.Height),
                Color.FromArgb(255, 255, 255), Color.FromArgb(240, 240, 240))) {
                if (m_bMouseDown) lgb.LinearColors = new Color[] { Color.LightGray, Color.White };
                g.FillPath(
                    lgb,
                    RenderHelper.CreateRoundedRectanglePath(new Rectangle(0, 1, this.Width - 1, this.Height - 2), 5)
                    );
            }
            using (LinearGradientBrush lgb = new LinearGradientBrush(
               new Point(0, 0), new Point(0, this.Height - 1),
               Color.FromArgb(200, 200, 200), m_bMouseOn ? Color.DodgerBlue : Color.FromArgb(127, 127, 127))) {

                g.DrawPath(
                    new Pen(lgb),
                    RenderHelper.CreateRoundedRectanglePath(new Rectangle(0, 0, this.Width - 1, this.Height - 2), 5)
                    );
            }
            using (SolidBrush sb = new SolidBrush(this.ForeColor)) {
                g.DrawString(this.Text, this.Font, sb, this.DisplayRectangle, sf);
            }
        }
    }
}
