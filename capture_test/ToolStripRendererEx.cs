using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace DevCapture
{
    public class ToolStripRendererEx : ToolStripRenderer
    {
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e) {
            using (SolidBrush sb = new SolidBrush(Color.FromArgb(30, 30, 30))) {
                e.Graphics.FillRectangle(sb, e.AffectedBounds);
            }
            base.OnRenderToolStripBackground(e);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) {
            e.Graphics.DrawRectangle(Pens.Black, e.AffectedBounds.X, e.AffectedBounds.Y, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1);
            base.OnRenderToolStripBorder(e);
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e) {
            using (SolidBrush sb = new SolidBrush(Color.FromArgb(50, 50, 50))) {
                e.Graphics.FillRectangle(sb, e.AffectedBounds);
            }
            base.OnRenderImageMargin(e);
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e) {
            e.TextColor = e.Item.Selected ? Color.Cyan : Color.DarkCyan;
            base.OnRenderItemText(e);
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e) {
            e.ArrowColor = e.Item.Selected ? Color.Cyan : Color.DarkCyan;
            base.OnRenderArrow(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e) {
            Point ptEnd = new Point(e.Item.ContentRectangle.X + e.Item.Width / 2, e.Item.ContentRectangle.Y);
            using (LinearGradientBrush lgb = new LinearGradientBrush(e.Item.ContentRectangle.Location, ptEnd, Color.Transparent, Color.DarkCyan)) {
                lgb.WrapMode = WrapMode.TileFlipX;
                using (Pen p = new Pen(lgb)) {
                    e.Graphics.DrawLine(p, e.Item.ContentRectangle.Location, new Point(e.Item.ContentRectangle.Right, ptEnd.Y));
                }
            }
            base.OnRenderSeparator(e);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e) {
            if (e.Item.Selected)
                e.Graphics.FillRectangle(Brushes.Gray, e.Item.ContentRectangle);
            base.OnRenderMenuItemBackground(e);
        }
    }
}
