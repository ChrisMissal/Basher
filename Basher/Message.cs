using Basher.Enumerations;
using Google.Protobuf;

namespace Basher
{
    internal class Message
    {
        public Message(IMessage message, ulong kind)
        {
            this.Inner = message;
            this.Kind = kind;
        }

        public ulong Kind { get; }

        public IMessage Inner { get; }

        public bool HasEmbeddedData()
        {
            DemTypes.TryParse(this.Kind, out var result);
            return result?.HasEmbeddedData ?? false;
        }
    }
}