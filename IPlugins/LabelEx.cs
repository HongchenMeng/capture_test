using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IPlugins
{
    public class LabelEx : Label
    {
        protected override void OnPaint(PaintEventArgs e) {
            e.Graphics.DrawLine(Pens.LightGray, 0, this.Height - 1, this.Width, this.Height - 1);
            base.OnPaint(e);
        }
    }
}
