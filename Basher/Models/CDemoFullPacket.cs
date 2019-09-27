namespace Basher.Models
{
    internal class CDemoFullPacket
    {
        private readonly global::CDemoFullPacket fullPacket;
        private readonly CDemoPacket innerPacket;

        public CDemoFullPacket(global::CDemoFullPacket fullPacket, CDemoPacket innerPacket, CDemoCustomData customData)
        {
            this.fullPacket = fullPacket;
            this.innerPacket = innerPacket;
        }
    }
}