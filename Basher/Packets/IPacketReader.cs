using System;
using System.IO;
using Basher.Enumerations;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Basher.Packets
{
    internal interface IPacketReader
    {
        IMessage Read(ReplayElement element, Stream stream);
    }

    /*internal class EntityReader : IPacketReader
    {
        private readonly MessageDescriptor descriptor;

        public EntityReader(MessageDescriptor descriptor)
        {
            this.descriptor = descriptor;
        }

        public IMessage Read(ReplayElement element, Stream stream)
        {
            var value = this.descriptor.Parser.ParseFrom()
        }
    }*/

    internal class PacketReader : IPacketReader
    {
        private readonly MessageDescriptor descriptor;

        public PacketReader(MessageDescriptor descriptor)
        {
            this.descriptor = descriptor;
        }

        public IMessage Read(ReplayElement element, Stream stream)
        {
            var value = this.descriptor.Parser.ParseDelimitedFrom(stream);
            value.MergeDelimitedFrom(stream);
            return value;
        }
    }

    internal class EmbeddedPacketReader : IPacketReader
    {
        private readonly MessageDescriptor descriptor;

        public EmbeddedPacketReader(MessageDescriptor descriptor)
        {
            this.descriptor = descriptor;
        }

        public IMessage Read(ReplayElement element, Stream stream)
        {
            var buffer = new byte[element.Size];
            stream.Read(buffer, 0, buffer.Length);

            var value = this.descriptor.Parser.ParseFrom(buffer);

            return value;
        }
    }
}