using System;

namespace VideoPixelReader.Core
{
    /// <summary>
    /// Rect of Interest
    /// </summary>
    public readonly struct RectRatio
    {
        public double X { get; }
        public double Y { get; }
        public double Width { get; }
        public double Height { get; }

        public RectRatio(double x, double y, double w, double h)
        {
            double clip(double v) => Math.Max(0.0, Math.Min(v, 1.0));

            var stx = clip(x);
            var sty = clip(y);
            var edx = clip(stx + w);
            var edy = clip(sty + h);

            X = stx;
            Y = sty;
            Width = edx - stx;
            Height = edy - sty;
        }

        public override string ToString() =>
            $"{nameof(X)}={X:f2}, {nameof(Y)}={Y:f2}, {nameof(Width)}={Width:f2}, {nameof(Height)}={Height:f2}";
    }

}
