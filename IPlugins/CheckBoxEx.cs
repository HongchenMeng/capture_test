using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IPlugins
{
    public class CheckBoxEx : CheckBox
    {
        public CheckBoxEx() {
            this.SetStyle(ControlStyles.Opaque, false);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs pevent) {
            base.OnPaint(pevent);
            base.OnPaintBackground(pevent);
            Graphics g = pevent.Graphics;
            g.DrawImage(this.Checked ? Properties.Resources.ckbox_checked : Properties.Resources.ckbox_uncheck, 0, 1, 13, 13);
            using (SolidBrush sb = new SolidBrush(Color.White)) {
                sb.Color = this.ForeColor;
                g.DrawString(this.Text, this.Font, sb, 15, 1);
            }
        }
    }
}
