using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Basher.Observers;

namespace Basher
{
    internal class MessageContext
    {
        private readonly BlockingCollection<CDemoFullPacket> fullPackets = new BlockingCollection<CDemoFullPacket>();
        private readonly BlockingCollection<CDemoCustomData> customData = new BlockingCollection<CDemoCustomData>();

        public IEnumerable<IObserver<PacketMessage>> GetObservers()
        {
            yield return new CDemoCustomDataObserver(this.fullPackets, this.customData);
            yield return new CDemoFullPacketCountObserver();
        }
    }
}