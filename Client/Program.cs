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

        static int Main(string[] args)
        {
            var fileName = args.Any() ? args[0] : @"C:\Program Files (x86)\Steam\steamapps\common\dota 2 beta\game\dota\replays\4984038549.dem";

            var cancellationToken = cancellationTokenSource.Token;

            var cancelTask = Task.Factory.StartNew(() =>
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    cancellationTokenSource.Cancel(true);
                }
            }, CancellationToken.None);

            var shortFileName = fileName.Split('/').Reverse().First();
            Console.WriteLine($"Beginning to parse {shortFileName}");

            var parser = new Parser(cancellationToken);
            var parseTask = parser.ParseFromFileAsync(fileName);

            Task.WhenAny(cancelTask, parseTask).ConfigureAwait(false).GetAwaiter().GetResult();

            var exitCode = cancelTask.IsCompletedSuccessfully
                ? 100 - (int) parseTask.Status // error code, sure
                : TaskStatus.RanToCompletion - parseTask.Status; // success = 0

            Console.WriteLine($"Exited with {exitCode}");
#if DEBUG
            Console.ReadLine();
#endif
            return exitCode;
        }
    }
}
