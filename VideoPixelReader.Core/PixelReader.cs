using System;
using System.Threading.Tasks;

namespace VideoPixelReader.Core
{
    public class PixelReader
    {
        private readonly string _sourcePath;
        private readonly RectRatio _rectRatio;

        public PixelReader(string path, RectRatio rect)
        {
            _sourcePath = path;
            _rectRatio = rect;
        }

        public async Task Start()
        {
            Console.WriteLine($"Source: {_sourcePath}");
            Console.WriteLine(_rectRatio.ToString() + Environment.NewLine);

            await ReadFrames();
        }

        private async Task ReadFrames()
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Restart();
            int i = 0;

            using (var frameProvider = new FrameProvider(_sourcePath))
            {
                Console.WriteLine($"\t{Pixels.GetHeader()}");
                var rect = _rectRatio;
                foreach (var mat in frameProvider.GetNextFrame())
                {
                    var pixels = await mat.ReadPixelRoiAsync(rect);
                    mat.Dispose();
                    Console.WriteLine($"{i++}\t{pixels}");
                }
            }

            var span = sw.Elapsed;
            Console.WriteLine($"{span} sec, {span.TotalSeconds / (i + 1)} sec/frame");
        }

    }
}
