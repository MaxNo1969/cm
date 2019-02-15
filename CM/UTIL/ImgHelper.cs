using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace CM
{
    public class ImgHelper
    {
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        /// <summary>
        /// Записываем массив байтов в память Bitmap
        /// </summary>
        /// <param name="_bitmap">Bitmap для записи</param>
        /// <param name="_array">массив записываемых байтов</param>
        public static void setBitmapData(ref Bitmap _bitmap, ref byte[] _array)
        {

            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, _bitmap.Width, _bitmap.Height);
            BitmapData bmpData = _bitmap.LockBits(rect, ImageLockMode.WriteOnly, _bitmap.PixelFormat);
            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;
            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * _bitmap.Height;
            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(_array, 0, ptr, bytes);
            // Unlock the bits.
            _bitmap.UnlockBits(bmpData);
        }

    }
}
