using OpenCvSharp;
using System;
using System.Threading.Tasks;

namespace VideoPixelReader.Core
{
    static class MatExtensions
    {
        public static async Task<Pixels> ReadPixelRoiAsync(this Mat mat, RectRatio rect) =>
            await Task.Run(() => mat.ReadPixelRoi(rect));

        private static Pixels ReadPixelRoi(this Mat mat, in RectRatio rect)
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
#if true
            unsafe
            {
                int ch = mat.Channels();
                int stride = mat.Width * ch;
                byte* pyst = mat.DataPointer + (startY * stride) + (startX * ch);
                byte* pyed = pyst + (height * stride);

                for (var py = pyst; py < pyed; py += stride)
                {
                    for (var px = py; px < py + (width * ch); px += ch)
                    {
                        b += px[0];
                        g += px[1];
                        r += px[2];
                    }
                }
            }
#else
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
#endif

            double count = width * height;
            return new Pixels(b: b / count, g: g / count, r: r / count);
        }

    }
}
