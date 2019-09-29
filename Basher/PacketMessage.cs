using Google.Protobuf;

namespace Basher
{
    internal class PacketMessage
    {
        public PacketMessage(IMessage message, ulong kind)
        {
            this.Inner = message;
            this.Kind = kind;
        }

        public ulong Kind { get; }

        public IMessage Inner { get; }

        public bool IsA<T>()
        {
            return this.Inner is T;
        }
    }
}