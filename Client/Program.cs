using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Basher;

namespace Client
{
    class Program
    {
        static readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(2));

        static void Main(string[] args)
        {
            var path = args.Any() ? args[0] : @"C:\Program Files (x86)\Steam\steamapps\common\dota 2 beta\game\dota\replays\" +
                                              "4757318890.dem";
                                              //"4984038549.dem";

            var cancellationToken = cancellationTokenSource.Token;

            var cancelTask = Task.Factory.StartNew(() => {
                if (Console.ReadKey().Key == ConsoleKey.Escape) cancellationTokenSource.Cancel(true);
            }, cancellationToken);

            var shortFileName = path.Split('\\').Last();
            Console.WriteLine($"Beginning to parse \"{shortFileName}\"");

            var parser = new Parser();
            var parseTask = parser.ParseFromFileAsync(path, cancellationToken);

            Task.WhenAny(cancelTask, parseTask).ConfigureAwait(true).GetAwaiter().GetResult();

            if (cancelTask.IsCanceled)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Parse cancelled'");
                Console.ResetColor();
            }
            else if (parseTask.IsFaulted)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(parseTask.Exception.Flatten());
                Console.ResetColor();
            }
            else if (parseTask.IsCompleted && !parseTask.IsCompletedSuccessfully)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unknown error? :(");
                Console.WriteLine("==============");
                Console.WriteLine(parseTask.Exception.Flatten());
                Console.ResetColor();
            }
            else if (parseTask.IsCompletedSuccessfully)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Complete");
                Console.ResetColor();
            }
            
#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
