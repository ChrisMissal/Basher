using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Basher.Enumerations;
using Google.Protobuf;

namespace Basher.Observers
{
    internal class CDemoCustomDataObserver : TypeObserver<CDemoFullPacket>
    {
        private Task decodeTask;

        private readonly BlockingCollection<CDemoCustomData> collection;

        public CDemoCustomDataObserver(BlockingCollection<CDemoCustomData> collection)
        {
            this.collection = collection;
        }

        public override void OnCompleted()
        {
            while (decodeTask == null || !decodeTask.IsCompleted)
            {
                collection.CompleteAdding();
            }
        }

        public override void OnNext(IMessage<CDemoFullPacket> value)
        {
            if (!(value is CDemoFullPacket fullPacket))
            {
                throw new InvalidOperationException($"''{nameof(fullPacket)}' is null.");
            }

            this.decodeTask = Task.Factory.StartNew(() =>
            {
                var buffer = fullPacket.ToByteArray();
                var inputStream = new CodedInputStream(buffer);

                var tag = inputStream.PeekTag();
                var dem = DemTypes.FromValue(tag);
                var customData = dem.Read<CDemoCustomData>(inputStream);

                this.collection.Add(customData);
            });
        }
    }
}