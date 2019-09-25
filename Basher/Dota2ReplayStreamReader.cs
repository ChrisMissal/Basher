using System;
using System.IO;
using System.Threading;
using Basher.Enumerations;
using Google.Protobuf;
using Snappy;

namespace Basher
{
    internal class Dota2ReplayStreamReader : IDisposable
    {
        private readonly CancellationTokenSource cancellationSource;
        private readonly FileStream stream;

        public Dota2ReplayStreamReader(string path, CancellationTokenSource cancellationSource)
        {
            this.cancellationSource = cancellationSource;
            this.stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        }

        public void ReadHeader(GameReplayEngine engine)
        {
            using (new UnhandledExceptionContext(this.cancellationSource, engine))
            {
                var reader = new HeaderReader(this.stream);
                reader.Read(engine);

                engine.Kind = this.stream.ReadVarInt();
                engine.Tick = this.stream.ReadVarInt();
                engine.Size = this.stream.ReadVarInt();
                engine.Message = this.stream.ReadVarInt();

                var dem = DemTypes.FromValue(engine.Message);
                var header = dem.Descriptor.Parser.ParseDelimitedFrom(this.stream);
                header.MergeDelimitedFrom(this.stream);

                engine.Collect(header);
            }
        }

        public void ReadGameReplay(GameReplayEngine engine)
        {
            using (new UnhandledExceptionContext(this.cancellationSource, engine))
            {
                while (!engine.IsComplete)
                {
                    engine.Kind = this.stream.ReadVarInt();
                    engine.Tick = this.stream.ReadVarInt();
                    engine.Size = this.stream.ReadVarInt();

                    var buffer = new byte[engine.Size];
                    this.stream.Read(buffer, 0, buffer.Length);

                    if (engine.IsCompressed)
                    {
                        engine.ResetCompression();

                        buffer = SnappyCodec.Uncompress(buffer);

                        var message = DemTypes.FromValue(engine.Kind).Descriptor.Parser.ParseFrom(buffer);

                        message.MergeFrom(buffer);
                    }
                    else
                    {
                        var message = DemTypes.FromValue(engine.Kind).Descriptor.Parser.ParseFrom(buffer);

                        engine.Collect(message);
                    }

                    if (this.stream.Position == this.stream.Length)
                    {
                        engine.Complete();
                    }
                }
            }
        }

        private class UnhandledExceptionContext : IDisposable
        {
            private readonly CancellationTokenSource cancellationSource;
            private readonly GameReplayEngine engine;

            public UnhandledExceptionContext(CancellationTokenSource cancellationSource, GameReplayEngine engine)
            {
                this.cancellationSource = cancellationSource;
                this.engine = engine;
            }

            public void Dispose()
            {
                if (!engine.Successful)
                {
                    cancellationSource.Cancel();
                }
            }
        }

        public void Dispose()
        {
            stream?.Dispose();
        }
    }
}