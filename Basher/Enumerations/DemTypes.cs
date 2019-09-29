using Google.Protobuf.Reflection;
using Headspring;

namespace Basher.Enumerations
{
    internal class DemTypes : Enumeration<DemTypes, ulong>
    {
        public static DemTypes CDemoStop = new DemTypes(EDemoCommands.DemStop, global::CDemoStop.Descriptor);
        public static DemTypes CDemoFileHeader = new DemTypes(EDemoCommands.DemFileHeader, global::CDemoFileHeader.Descriptor);
        public static DemTypes CDemoFileInfo = new DemTypes(EDemoCommands.DemFileInfo, global::CDemoFileInfo.Descriptor);
        public static DemTypes CDemoSyncTick = new DemTypes(EDemoCommands.DemSyncTick, global::CDemoSyncTick.Descriptor);
        public static DemTypes CDemoSendTables = new DemTypes(EDemoCommands.DemSendTables, global::CDemoSendTables.Descriptor);
        public static DemTypes CDemoClassInfo = new DemTypes(EDemoCommands.DemClassInfo, global::CDemoClassInfo.Descriptor);
        public static DemTypes CDemoStringTables = new DemTypes(EDemoCommands.DemStringTables, global::CDemoStringTables.Descriptor);
        public static DemTypes CDemoPacket = new DemTypes(EDemoCommands.DemPacket, global::CDemoPacket.Descriptor);
        public static DemTypes CDemoSignonPacket = new DemTypes(EDemoCommands.DemSignonPacket, global::CDemoPacket.Descriptor);
        public static DemTypes CDemoConsoleCmd = new DemTypes(EDemoCommands.DemConsoleCmd, global::CDemoConsoleCmd.Descriptor);
        public static DemTypes CDemoCustomData = new DemTypes(EDemoCommands.DemCustomData, global::CDemoCustomData.Descriptor);
        public static DemTypes CDemoCustomDataCallbacks = new DemTypes(EDemoCommands.DemCustomDataCallbacks, global::CDemoCustomDataCallbacks.Descriptor);
        public static DemTypes CDemoUserCmd = new DemTypes(EDemoCommands.DemUserCmd, global::CDemoUserCmd.Descriptor);
        public static DemTypes CDemoFullPacket = new DemTypes(EDemoCommands.DemFullPacket, global::CDemoFullPacket.Descriptor);
        public static DemTypes CDemoSaveGame = new DemTypes(EDemoCommands.DemSaveGame, global::CDemoSaveGame.Descriptor);
        public static DemTypes CDemoSpawnGroups = new DemTypes(EDemoCommands.DemSpawnGroups, global::CDemoSaveGame.Descriptor);

        private DemTypes(EDemoCommands value, MessageDescriptor descriptor) : base((ulong) value, descriptor.Name)
        {
            this.Descriptor = descriptor;
        }

        public MessageDescriptor Descriptor { get; }
    }
}