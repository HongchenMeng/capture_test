using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace Gray
{
    public class PluginGray : IPlugins.IFilter
    {
        public string GetPluginName() {
            return "黑白处理";
        }

        public System.Drawing.Image GetPluginIcon() {
            return Properties.Resources.icon_gray;
        }

        public void InitPlugin(string strStarPath) {

        }

        public IPlugins.ResultInfo ExecFilter(System.Drawing.Image imgSrc) {
            Bitmap bmp = imgSrc as Bitmap;
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte[] byColorInfo = new byte[bmp.Height * bmpData.Stride];
            Marshal.Copy(bmpData.Scan0, byColorInfo, 0, byColorInfo.Length);
            byte byR, byG, byB;
            for (int x = 0, xLen = bmp.Width; x < xLen; x++) {
                for (int y = 0, yLen = bmp.Height; y < yLen; y++) {
                    byB = byColorInfo[y * bmpData.Stride + x * 3];
                    byG = byColorInfo[y * bmpData.Stride + x * 3 + 1];
                    byR = byColorInfo[y * bmpData.Stride + x * 3 + 2];
                    byte byV = (byte)((byR + byG + byB) / 3);
                    byColorInfo[y * bmpData.Stride + x * 3] =
                        byColorInfo[y * bmpData.Stride + x * 3 + 1] =
                        byColorInfo[y * bmpData.Stride + x * 3 + 2] = byV;
                }
            }
            Marshal.Copy(byColorInfo, 0, bmpData.Scan0, byColorInfo.Length);
            bmp.UnlockBits(bmpData);
            return new IPlugins.ResultInfo(bmp, true, false);
        }
    }
}
