using OpenCvSharp;
using System;
using System.Collections.Generic;

namespace VideoPixelReader.Core
{
    class FrameProvider : IDisposable
    {
        private readonly VideoCapture _videoCapture;

        public FrameProvider(string path)
        {
            _videoCapture = new VideoCapture(path);
        }

        public IEnumerable<Mat> GetNextFrame()
        {
            while (_videoCapture.IsOpened())
            {
                var mat = new Mat();
                if (!_videoCapture.Read(mat)) yield break;
                if (!mat.IsContinuous()) yield break;
                yield return mat;
            }
        }

        public void Dispose() => _videoCapture?.Dispose();

    }
}
