using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Basher.Enumerations;
using Google.Protobuf;
using Snappy;

namespace Basher
{
    internal class ReplayFileEnumerator : IEnumerator<PacketMessage>, IEnumerable<PacketMessage>
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

                this.Current = new PacketMessage(header, this.kind);
                return this.Current != null;
            }

            this.kind = this.stream.ReadVarInt();
            this.tick = this.stream.ReadVarInt();
            this.size = this.stream.ReadVarInt();

            var buffer = new byte[size];
            this.stream.Read(buffer, 0, buffer.Length);

            var summaryComplete = this.stream.Position >= this.stream.Length;

            this.Current = !summaryComplete ? GetMessage(buffer) : null;

            return !summaryComplete;
        }

        private PacketMessage GetMessage(byte[] buffer)
        {
            if (this.IsCompressed)
            {
                this.ResetCompression();

                buffer = SnappyCodec.Uncompress(buffer);

                var message = DemTypes.FromValue(kind).Descriptor.Parser.ParseFrom(buffer);

                message.MergeFrom(buffer);

                return new PacketMessage(message, this.kind);
            }
            else
            {
                var message = DemTypes.FromValue(kind).Descriptor.Parser.ParseFrom(buffer);

                return new PacketMessage(message, this.kind);
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

        public PacketMessage Current { get; private set; }

        object IEnumerator.Current => this.Current;

        public void Dispose()
        {
            this.stream?.Dispose();
        }

        public IEnumerator<PacketMessage> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}