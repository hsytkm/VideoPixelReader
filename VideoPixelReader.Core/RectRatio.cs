using System;

namespace VideoPixelReader.Core
{
    // Rect of Interest
    public struct RectRatio
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Width;
        public readonly double Height;

        public RectRatio(double _x, double _y, double _w, double _h)
        {
            double clip(double x) => Math.Max(0.0, Math.Min(x, 1.0));

            var stx = clip(_x);
            var sty = clip(_y);
            var edx = clip(stx + _w);
            var edy = clip(sty + _h);

            X = stx;
            Y = sty;
            Width = edx - stx;
            Height = edy - sty;
        }

        public override string ToString() => $"X={X:f2}, Y={Y:f2}, Width={Width:f2}, Height={Height:f2}";
    }

}
