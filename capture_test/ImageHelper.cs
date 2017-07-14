using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace DevCapture
{
    public class ImageHelper
    {
        public static Image ZoomImage(Image image, float scale) {
            if (image == null)
                throw new ArgumentNullException("image cannot be null");
            if (scale <= 0)
                throw new ArgumentException("scale must be more than zero");
            Bitmap bmp = new Bitmap((int)Math.Ceiling(image.Width * scale), (int)Math.Ceiling(image.Height * scale));
            Bitmap bmpOld = image.Clone() as Bitmap;
            BitmapData bmpDataNew = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            BitmapData bmpDataOld = bmpOld.LockBits(new Rectangle(Point.Empty, image.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            byte[] byColorNew = new byte[bmpDataNew.Height * bmpDataNew.Stride];
            byte[] byColorOld = new byte[bmpDataOld.Height * bmpDataOld.Stride];
            Marshal.Copy(bmpDataOld.Scan0, byColorOld, 0, byColorOld.Length);
            for (int x = 0, lenX = bmpDataNew.Width; x < lenX; x++) {
                int srcX = (int)(x / scale) << 2;
                for (int y = 0, lenY = bmpDataNew.Height; y < lenY; y++) {
                    int offsetOld = (int)(y / scale) * bmpDataOld.Stride + srcX;
                    int offsetNew = y * bmpDataNew.Stride + (x << 2);
                    if (offsetOld < 0) offsetOld = 0;
                    else if (offsetOld >= byColorOld.Length) offsetOld = byColorOld.Length - 1;
                    if (offsetNew < 0) offsetNew = 0;
                    else if (offsetNew >= byColorNew.Length) offsetNew = byColorNew.Length - 1;
                    byColorNew[offsetNew] = byColorOld[offsetOld];
                    byColorNew[offsetNew + 1] = byColorOld[offsetOld + 1];
                    byColorNew[offsetNew + 2] = byColorOld[offsetOld + 2];
                    byColorNew[offsetNew + 3] = byColorOld[offsetOld + 3];
                }
            }
            bmpOld.UnlockBits(bmpDataOld);
            Marshal.Copy(byColorNew, 0, bmpDataNew.Scan0, byColorNew.Length);
            bmp.UnlockBits(bmpDataNew);
            bmpOld.Dispose();
            return bmp;
        }

        public static Image Mosaic(Image imgSrc, int width) {
            Bitmap bmp = ((Bitmap)imgSrc).Clone(new Rectangle(Point.Empty, imgSrc.Size), imgSrc.PixelFormat);
            BitmapData bmpData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte[] byColorInfoSrc = new byte[bmpData.Height * bmpData.Stride];
            Marshal.Copy(bmpData.Scan0, byColorInfoSrc, 0, byColorInfoSrc.Length);
            int indexB = 0;
            int indexG = 0;
            int indexR = 0;
            int rCount = 0;
            for (int x = 0, lenx = bmp.Width; x < lenx; x += width) {
                for (int y = 0, leny = bmp.Height; y < leny; y += width) {
                    int r = 0, g = 0, b = 0; rCount = 0;
                    for (int tempx = x, lentx = x + width <= lenx ? x + width : lenx; tempx < lentx; tempx++) {
                        for (int tempy = y, lenty = y + width <= leny ? y + width : leny; tempy < lenty; tempy++) {
                            indexB = tempy * bmpData.Stride + tempx * 3;
                            indexG = indexB + 1;
                            indexR = indexB + 2;

                            b += byColorInfoSrc[indexB];
                            g += byColorInfoSrc[indexG];
                            r += byColorInfoSrc[indexR];
                            rCount++;
                        }
                    }
                    for (int tempx = x, lentx = x + width <= lenx ? x + width : lenx; tempx < lentx; tempx++) {
                        for (int tempy = y, lenty = y + width <= leny ? y + width : leny; tempy < lenty; tempy++) {
                            indexB = tempy * bmpData.Stride + tempx * 3;
                            indexG = indexB + 1;
                            indexR = indexB + 2;

                            byColorInfoSrc[indexB] = (byte)(b / (rCount));
                            byColorInfoSrc[indexG] = (byte)(g / (rCount));
                            byColorInfoSrc[indexR] = (byte)(r / (rCount));
                        }
                    }
                }
            }
            Marshal.Copy(byColorInfoSrc, 0, bmpData.Scan0, byColorInfoSrc.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}
