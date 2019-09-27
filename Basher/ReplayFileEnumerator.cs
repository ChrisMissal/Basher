using System.Collections;
using System.Collections.Generic;
using System.IO;
using Dota;
using Google.Protobuf;
using Snappy;

namespace Basher
{
    internal class ReplayFileEnumerator : IEnumerator<IMessage>, IEnumerable<IMessage>
    {
        private readonly Stream stream;
        private readonly HeaderReader headerReader = new HeaderReader();

        private bool headerFound;
        private ulong summaryOffset;

        private ulong kind;
        private ulong tick;
        private ulong size;
        private ulong msg;

        public ReplayFileEnumerator(Stream stream)
        {
            this.stream = stream;
        }

        public bool MoveNext()
        {
            if (!headerFound)
            {
                this.headerReader.Find(this.stream, out this.headerFound, out this.summaryOffset);

                this.kind = this.stream.ReadVarInt();
                this.tick = this.stream.ReadVarInt();
                this.size = this.stream.ReadVarInt();
                this.msg = this.stream.ReadVarInt();

                var header = DemTypes.FromValue(msg).Descriptor.Parser.ParseDelimitedFrom(this.stream);
                header.MergeDelimitedFrom(this.stream);

                this.Current = header;
                return this.Current != null;
            }

            this.kind = this.stream.ReadVarInt();
            this.tick = this.stream.ReadVarInt();
            this.size = this.stream.ReadVarInt();

            var buffer = new byte[size];
            this.stream.Read(buffer, 0, buffer.Length);

            var summaryComplete = this.stream.Position >= (long) this.summaryOffset;

            this.Current = !summaryComplete ? GetMessage(buffer) : null;

            return !summaryComplete;
        }

        private IMessage GetMessage(byte[] buffer)
        {
            if (this.IsCompressed)
            {
                this.ResetCompression();

                buffer = SnappyCodec.Uncompress(buffer);

                var message = DemTypes.FromValue(kind).Descriptor.Parser.ParseFrom(buffer);

                message.MergeFrom(buffer);

                return message;
            }
            else
            {
                var message = DemTypes.FromValue(kind).Descriptor.Parser.ParseFrom(buffer);

                return message;
            }
        }

        private bool IsCompressed => ((int) this.kind & (int) EDemoCommands.DemIsCompressed) != 0;

        private void ResetCompression()
        {
            this.kind -= (ulong)EDemoCommands.DemIsCompressed;
        }

        public void Reset()
        {
            this.headerFound = false;
            this.summaryOffset = 0UL;
            this.stream.Position = 0L;
        }

        public IMessage Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            this.stream?.Dispose();
        }

        public IEnumerator<IMessage> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}