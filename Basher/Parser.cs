using System;
using System.IO;
using System.Threading.Tasks;
using Jil;
using System.Runtime.CompilerServices;
using System.Threading;

#if DEBUG
[assembly: InternalsVisibleTo("Basher.Tests")]
#endif
namespace Basher
{
    public class Parser
    {
        private readonly TextWriter writer;

        public Parser(TextWriter writer = null)
        {
            this.writer = writer ?? TextWriter.Null;
        }

        public Task ParseFromFileAsync(string path, CancellationToken cancellationToken)
        {
            var cancellationSource = new CancellationTokenSource();
            var engine = new GameReplayEngine(this.writer, cancellationSource);

            using (var reader = new Dota2ReplayStreamReader(path, cancellationSource))
            {
                try
                {
                    reader.ReadHeader(engine);
                    reader.ReadGameReplay(engine);
                }
                catch (Exception exception)
                {
                    engine.AddException(exception);
                    return Task.FromException(exception);
                }

                this.writer.WriteLine($"{JSON.Serialize(engine)}");
            }

            return Task.CompletedTask;
        }
    }
}
