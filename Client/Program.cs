using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using Basher;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = args.Any() ? args[0] : @"C:\Program Files (x86)\Steam\steamapps\common\dota 2 beta\game\dota\replays\" +
                                              "4757318890.dem";
                                              //"4984038549.dem";

            var shortFileName = path.Split('\\').Last();
            Console.WriteLine($"Beginning to parse \"{shortFileName}\"");

            try
            {
                var stopwatch = Stopwatch.StartNew();

                var runner = new Runner(path);
                runner.Run();

                stopwatch.Stop();

                Console.WriteLine();
                Console.WriteLine($"Parsing finished in {stopwatch.Elapsed:g}");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
