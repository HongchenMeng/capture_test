using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace DevCapture
{
    internal class RenderHelper
    {
        public static void RenderBackground(Graphics g, Rectangle rectDest, Image imgSrc) {
            g.DrawImage(imgSrc, new Rectangle(0, 0, 5, 5), new Rectangle(0, 0, 5, 5), GraphicsUnit.Pixel);
            g.DrawImage(imgSrc, new Rectangle(rectDest.Width - 5, 0, 5, 5), new Rectangle(imgSrc.Width - 5, 0, 5, 5), GraphicsUnit.Pixel);
            g.DrawImage(imgSrc, new Rectangle(0, rectDest.Height - 5, 5, 5), new Rectangle(0, imgSrc.Height - 5, 5, 5), GraphicsUnit.Pixel);
            g.DrawImage(imgSrc, new Rectangle(rectDest.Width - 5, rectDest.Height - 5, 5, 5), new Rectangle(imgSrc.Width - 5, imgSrc.Height - 5, 5, 5), GraphicsUnit.Pixel);

            g.DrawImage(imgSrc, new Rectangle(0, 5, 5, rectDest.Height - 10), new Rectangle(0, 5, 5, imgSrc.Height - 10), GraphicsUnit.Pixel);
            g.DrawImage(imgSrc, new Rectangle(5, 0, rectDest.Width - 10, 5), new Rectangle(5, 0, imgSrc.Width - 10, 5), GraphicsUnit.Pixel);
            g.DrawImage(imgSrc, new Rectangle(rectDest.Width - 5, 5, 5, rectDest.Height - 10), new Rectangle(imgSrc.Width - 5, 5, 5, imgSrc.Height - 10), GraphicsUnit.Pixel);
            g.DrawImage(imgSrc, new Rectangle(5, rectDest.Height - 5, rectDest.Width - 10, 5), new Rectangle(5, imgSrc.Height - 5, imgSrc.Width - 10, 5), GraphicsUnit.Pixel);

            g.DrawImage(imgSrc, new Rectangle(5, 5, rectDest.Width - 10, rectDest.Height - 10), new Rectangle(5, 5, imgSrc.Width - 10, imgSrc.Height - 10), GraphicsUnit.Pixel);
        }

    }
}
