using System;
using System.IO;
using System.Threading;
using Basher.Enumerations;
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

                var header = DemTypes.FromValue(engine.Message).Read<CDemoFileHeader>(engine, this.stream);
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

                    if (!engine.IsCompressed)
                    {
                        var dem = DemTypes.FromValue(engine.Kind);

                        var packet = dem.Read<CDemoPacket>(engine, this.stream);
                        packet.Peek(kind: engine.Kind, size: engine.Size, message: engine.Message);
                        engine.Collect(packet);

                        continue;
                    }

                    engine.ResetCompression();

                    var buffer = new byte[engine.Size];
                    this.stream.Read(buffer, 0, buffer.Length);

                    buffer = SnappyCodec.Uncompress(buffer);

                    var stream = new MemoryStream(buffer);

                    while (stream.Position < stream.Length)
                    {
                        var kind = stream.ReadVarInt();
                        var size = stream.ReadVarInt();

                        var buf = new byte[size];

                        stream.Read(buf, 0, buf.Length);

                        var embed = EmbeddedTypes.FromValue(kind);

                        buf.Dump(ConsoleColor.Yellow);
                        embed.Dump(ConsoleColor.DarkYellow);

                        var packetEntities = embed.Descriptor.Parser.ParseFrom(buf);

                        engine.Collect((CSVCMsg_PacketEntities)packetEntities);
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