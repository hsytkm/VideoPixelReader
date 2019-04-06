using System;

namespace VideoPixelReader.Core
{
    public class PixelReader
    {
        private readonly string sourcePath;
        private readonly RectRatio rectRatio;

        public PixelReader(string path, RectRatio rect)
        {
            sourcePath = path;
            rectRatio = rect;
        }

        public void Start()
        {
            int i = 0;
            var sw = new System.Diagnostics.Stopwatch();
            sw.Restart();

            Console.WriteLine($"Source: {sourcePath}");
            Console.WriteLine(rectRatio.ToString() + Environment.NewLine);

            using (var frameProvider = new FrameProvider(sourcePath))
            {
                Console.WriteLine($"\t{Pixels.GetHeader()}");

                foreach (var mat in frameProvider.GetNextFrame())
                {
                    var pixels = mat.ReadPixelRoi(rectRatio);
                    mat.Dispose();
                    Console.WriteLine($"{i++}\t{pixels}");
                }
            }
            var span = sw.Elapsed;
            Console.WriteLine($"{span} sec, {span.TotalSeconds / (i + 1)} sec/frame");
        }

    }
}
