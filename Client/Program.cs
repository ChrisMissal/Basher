using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using lib = Basher;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                var runner = 
                    //lib.Runner.FromLocalMatchNumber(args[0]);
                    new lib.Runner(args[0]);

                runner.RunAsync(CancellationToken.None).GetAwaiter().GetResult();

                stopwatch.Stop();

                Console.WriteLine();
                Console.WriteLine($"Parsing finished in {stopwatch.Elapsed:g}");
            }
            catch (Exception exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine(exception);
            }

#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
