using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Basher.Enumerations;
using Google.Protobuf;

namespace Basher.Observers
{
    internal class CDemoCustomDataObserver : TypeObserver<CDemoFullPacket>
    {
        private readonly BlockingCollection<CDemoFullPacket> collection;
        private readonly BlockingCollection<CDemoCustomData> customData;

        public CDemoCustomDataObserver(BlockingCollection<CDemoFullPacket> collection, BlockingCollection<CDemoCustomData> customData)
        {
            this.collection = collection;
            this.customData = customData;
        }

        public override void OnCompleted()
        {
            while (this.Tasks.Any(x => !x.IsCompleted))
            {
                this.collection.CompleteAdding();
            }
        }

        protected override void Handle(PacketMessage message)
        {
            var buffer = message.Inner.ToByteArray();

            this.Tasks.Add(Task.Factory.StartNew(() =>
            {
                var inputStream = new CodedInputStream(buffer);

                var tag = inputStream.PeekTag();

                var dem = DemTypes.FromValue(tag);
                var packet = dem.Descriptor.Parser.ParseFrom(inputStream);
                packet.MergeFrom(inputStream);

                ($"__________________________________________________________________{Environment.NewLine}" +
                 $"{message.Inner.GetType()} was tagged {message.Kind}->{tag} ({packet.ToByteArray().Length} bytes)").Dump(ConsoleColor.Cyan);

                this.collection.Add(packet as CDemoFullPacket);
                this.customData.Add(packet as CDemoCustomData);
            }));
        }
    }
}