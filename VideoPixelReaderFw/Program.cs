using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPixelReader.Core;

namespace VideoPixelReaderFw
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            if ((args?.Length ?? 0) == 0)
            {
                args = new[] {
                    @"C:\data\sample.mp4",
                    "0.47",
                    "0.47",
                    "0.06",
                    "0.06",
                };
                Console.WriteLine("*** This is Debug Arguments. ***" + Environment.NewLine);
            }
#endif

            if (args.Length < 5)
            {
                Console.WriteLine("Argument Error.  VideoPixelReader.exe [video_path] [x] [y] [width] [height]");
                return;
            }

            try
            {
                var path = args[0];
                var x = double.Parse(args[1]);
                var y = double.Parse(args[2]);
                var width = double.Parse(args[3]);
                var height = double.Parse(args[4]);

                if (File.Exists(path))
                {
                    var reader = new PixelReader(path, new RectRatio(x, y, width, height));
                    reader.Start();
                }

                Console.WriteLine(Environment.NewLine + "End!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
