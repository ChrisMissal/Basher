using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Basher.Observers;
using Google.Protobuf;

namespace Basher
{
    public class Runner
    {
        private const string DefaultSteamReplayFolder = @"Steam\steamapps\common\dota 2 beta\game\dota\replays";

        private readonly Parser parser;
        private readonly Context context;

        public static Runner FromLocalMatchNumber(string matchNumber)
        {
            var path = matchNumber.All(char.IsDigit) ? $"{matchNumber}.dem" : matchNumber;
            return new Runner(path);
        }

        public Runner(string path)
        {
            if (!File.Exists(path))
            {
                path = this.GetPath(path);
            }

            this.parser = new Parser(path);
            this.context = new Context();
        }

        public string SteamReplayFolder { get; set; } = DefaultSteamReplayFolder;

        public Task RunAsync(CancellationToken cancellationToken)
        {
            using (var monitor = new TypeMonitor())
            {
                //monitor.Subscribe(new ExperimentalObserver(context));
                monitor.Subscribe(new CDemoClassInfoObserver(this.context));
                monitor.Subscribe(new CDemoCustomDataObserver(this.context));
                monitor.Subscribe(new CDemoStringTablesObserver(this.context));

                var messages = this.parser.GetMessages();

                monitor.Start(messages, cancellationToken);
            }

            return Task.CompletedTask;
        }

        private string GetPath(string path)
        {
            try
            {
                var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                path = Path.Combine($"{programFiles}", this.SteamReplayFolder, $"{path}");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }

            return path;
        }
    }

    internal class ExperimentalObserver : IObserver<Message>
    {
        private readonly Context context;

        public ExperimentalObserver(Context context)
        {
            this.context = context;
        }

        public void OnCompleted()
        {
            Console.WriteLine();
            Console.WriteLine($"{nameof(ExperimentalObserver)}'s watch has ended.");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error);
        }

        public void OnNext(Message value)
        {
            if (value.Kind != 6UL)
            {
                return;
            }

            ($"__________________________________________________________________{Environment.NewLine}" +
             $"{value.Inner.GetType()} was tagged {value.Kind} ({value.Inner.ToByteArray().Length} bytes)").Dump(ConsoleColor.Cyan);

            if (value.HasEmbeddedData())
            {
                var stream = new MemoryStream(value.Inner.ToByteArray());

                while (stream.Position < stream.Length)
                {
                    var kind = stream.ReadVarInt();
                    var size = stream.ReadVarInt();
                    var buffer = new byte[size];

                    stream.Read(buffer, 0, buffer.Length);

                    kind.Dump(ConsoleColor.Magenta);

                    if (kind != 26UL)
                    {
                        $"{kind} ???".Dump(ConsoleColor.Yellow);
                        System.Diagnostics.Debugger.Break();
                    }
                }
            }
            else
            {
                //System.Diagnostics.Debugger.Break();
            }
        }
    }
}