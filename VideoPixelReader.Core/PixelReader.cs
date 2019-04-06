using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine($"Source: {sourcePath}");
            Console.WriteLine(rectRatio.ToString() + Environment.NewLine);

#if true
            // 順次実行
            ReadFrames();
#else
            // 非同期WhenAll(家ノートではこちらの方が遅かった… 1.6倍くらい)
            ReadFramesAsync();
#endif
        }

        private void ReadFrames()
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Restart();

            int i = 0;
            using (var frameProvider = new FrameProvider(sourcePath))
            {
                Console.WriteLine($"\t{Pixels.GetHeader()}");
                var rect = rectRatio;
                foreach (var mat in frameProvider.GetNextFrame())
                {
                    var pixels = mat.ReadPixelRoi(ref rect);
                    mat.Dispose();
                    Console.WriteLine($"{i++}\t{pixels}");
                }
            }
            var span = sw.Elapsed;
            Console.WriteLine($"{span} sec, {span.TotalSeconds / (i + 1)} sec/frame");
        }

        private async void ReadFramesAsync()
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Restart();

            int i = 0;
            using (var frameProvider = new FrameProvider(sourcePath))
            {
                var mats = frameProvider.GetNextFrame();

                var pixels = await Task.WhenAll(mats.Select(async mat =>
                {
                    var p = await mat.ReadPixelRoiAsync(rectRatio);
                    mat.Dispose();
                    return p;
                }));

                var sb = new StringBuilder();
                sb.AppendLine($"\t{Pixels.GetHeader()}");
                foreach (var pixel in pixels)
                {
                    sb.AppendLine($"{i++}\t{pixel}");
                }
                Console.WriteLine(sb.ToString());
            }
            var span = sw.Elapsed;
            Console.WriteLine($"{span} sec, {span.TotalSeconds / (i + 1)} sec/frame");
        }

    }
}
