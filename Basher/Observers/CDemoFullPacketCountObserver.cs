using System;

namespace Basher.Observers
{
    internal class CDemoFullPacketCountObserver : TypeObserver<CDemoFullPacket>
    {
        private int count;

        public override void OnCompleted()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Found {this.count} {nameof(CDemoFullPacket)}");
            Console.ResetColor();
        }

        protected override void Handle(PacketMessage value)
        {
            this.count++;
        }
    }
}