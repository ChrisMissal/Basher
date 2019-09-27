using System;
using Google.Protobuf;

namespace Basher.Observers
{
    internal class FullPacketCountObserver : TypeObserver<CDemoFullPacket>
    {
        private int count;

        public override void OnCompleted()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Found {this.count} {nameof(CDemoFullPacket)}");
            Console.ResetColor();
        }

        public override void OnNext(IMessage<CDemoFullPacket> value)
        {
            this.count++;
        }
    }
}