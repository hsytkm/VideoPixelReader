using OpenCvSharp;
using System;
using System.Collections.Generic;

namespace VideoPixelReader.Core
{
    class FrameProvider : IDisposable
    {
        private readonly VideoCapture videoCapture;

        public FrameProvider(string path)
        {
            videoCapture = new VideoCapture(path);
        }

        public IEnumerable<Mat> GetNextFrame()
        {
            while (videoCapture.IsOpened())
            {
                var mat = new Mat();
                if (!videoCapture.Read(mat)) yield break;
                if (!mat.IsContinuous()) yield break;
                yield return mat;
            }
        }

        public void Dispose() => videoCapture?.Dispose();

    }
}
