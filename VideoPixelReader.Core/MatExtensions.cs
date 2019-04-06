using OpenCvSharp;
using System;

namespace VideoPixelReader.Core
{
    static class MatExtensions
    {
        public static Pixels ReadPixelRoi(this Mat mat, RectRatio rect)
        {
            var startX = (int)Math.Round(rect.X * mat.Width);
            var startY = (int)Math.Round(rect.Y * mat.Height);
            var w = (int)Math.Round(rect.Width * mat.Width);
            var h = (int)Math.Round(rect.Height * mat.Height);

            var endX = Math.Min(mat.Width, startX + w);
            var endY = Math.Min(mat.Height, startY + h);
            var width = endX - startX;
            var height = endY - startY;

            if (width == 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (height == 0) throw new ArgumentOutOfRangeException(nameof(height));

            ulong b = 0, g = 0, r = 0;
            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    var pix = mat.At<Vec3b>(y, x);
                    b += pix.Item0;
                    g += pix.Item1;
                    r += pix.Item2;
                }
            }

            double count = width * height;
            return new Pixels(b: b / count, g: g / count, r: r / count);
        }

    }
}
