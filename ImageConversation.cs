using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FWManager
{
    public class ImageConversation
    {
        public static Bitmap ByteToImage(int w, int h, byte[] pixels)
        {
            var bmp = new Bitmap(w, h, PixelFormat.Format16bppRgb565);
            byte bpp = 2;
            var BoundsRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(BoundsRect,
                                            ImageLockMode.WriteOnly,
                                            bmp.PixelFormat);
            // copy line by line:
            for (int y = 0; y < h; y++)
                Marshal.Copy(pixels, y * w * bpp, bmpData.Scan0 + bmpData.Stride * y, w * bpp);
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        public static byte[] ImageToByte(Image img)
        {

            Bitmap src = (Bitmap)img;

            byte[] pixels = new byte[(src.Width * src.Height) * 2];

            byte bpp = 2;
            var BoundsRect = new Rectangle(0, 0, src.Width, src.Height);
            BitmapData bmpData = src.LockBits(BoundsRect,
                                            ImageLockMode.ReadOnly,
                                            PixelFormat.Format16bppRgb565);
            // copy line by line:
            for (int y = 0; y < src.Height; y++)
                Marshal.Copy(bmpData.Scan0 + bmpData.Stride * y, pixels, y * src.Width * bpp, src.Width * bpp);
            src.UnlockBits(bmpData);

            return pixels;

            //int srcW = src.Width;
            //int srcH = src.Height;

            //double scale = Math.Min(176.0 / (double)srcH, 320 / (double)srcW);

            //int artW = srcW;
            //int artH = srcH;

            //if (scale < 1)
            //{
            //    artW = (int)(srcW * scale);
            //    artH = (int)(srcH * scale);
            //}




            //byte[] buffer = new byte[4 + (artW * artH) * 2];

            //using (Bitmap art = new Bitmap(artW, artH, System.Drawing.Imaging.PixelFormat.Format16bppRgb565))
            //{

            //    using (Graphics gr = Graphics.FromImage(art))
            //    {
            //        gr.DrawImage(src, new Rectangle(0, 0, art.Width, art.Height));
            //    }

            //    byte[] pixels = new byte[(artW * artH) * 2];

            //    byte bpp = 2;
            //    var BoundsRect = new Rectangle(0, 0, art.Width, art.Height);
            //    BitmapData bmpData = art.LockBits(BoundsRect,
            //                                    ImageLockMode.ReadOnly,
            //                                    art.PixelFormat);
            //    // copy line by line:
            //    for (int y = 0; y < artH; y++)
            //        Marshal.Copy(bmpData.Scan0 + bmpData.Stride * y, pixels, y * artW * bpp, artW * bpp);
            //    art.UnlockBits(bmpData);

            //    System.Array.Copy(pixels, 0, buffer, 4, pixels.Length);
            //    buffer[0] = (byte)(art.Width & 0xff);
            //    buffer[1] = (byte)((art.Width >> 8) & 0xff);
            //    buffer[2] = (byte)(art.Height & 0xff);
            //    buffer[3] = (byte)((art.Height >> 8) & 0xff);

            //}
            //return buffer;

        }

        public static Image ReseizeImage(int width, int height, Image src)
        {
            int srcW = src.Width;
            int srcH = src.Height;

            Bitmap art = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);

            using (Graphics gr = Graphics.FromImage(art))
            {
                gr.DrawImage(src, new Rectangle(0, 0, width, height));
            }

            src.Dispose();
            return art;
        }

    }

}
