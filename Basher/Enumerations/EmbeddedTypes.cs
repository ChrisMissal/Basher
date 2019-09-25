using Google.Protobuf.Reflection;
using Headspring;

namespace Basher.Enumerations
{
    internal class EmbeddedTypes : Enumeration<EmbeddedTypes, ulong>
    {
        public static EmbeddedTypes CNETMsg_Tick = new EmbeddedTypes(4, global::CNETMsg_Tick.Descriptor);
        public static EmbeddedTypes CNETMsg_SignonState = new EmbeddedTypes(7, global::CNETMsg_SignonState.Descriptor);
        public static EmbeddedTypes CSVCMsg_ServerInfo = new EmbeddedTypes(8, global::CSVCMsg_ServerInfo.Descriptor);
        public static EmbeddedTypes CSVCMsg_SendTable = new EmbeddedTypes(9, global::CSVCMsg_SendTable.Descriptor);
        public static EmbeddedTypes CSVCMsg_ClassInfo = new EmbeddedTypes(10, global::CSVCMsg_ClassInfo.Descriptor);
        public static EmbeddedTypes CSVCMsg_CreateStringTable = new EmbeddedTypes(12, global::CSVCMsg_CreateStringTable.Descriptor);
        public static EmbeddedTypes CSVCMsg_UpdateStringTable = new EmbeddedTypes(13, global::CSVCMsg_UpdateStringTable.Descriptor);
        public static EmbeddedTypes CSVCMsg_VoiceInit = new EmbeddedTypes(14, global::CSVCMsg_VoiceInit.Descriptor);
        public static EmbeddedTypes CSVCMsg_VoiceData = new EmbeddedTypes(15, global::CSVCMsg_VoiceData.Descriptor);
        //public static EmbeddedTypes CSVCMsg_Sounds = new EmbeddedTypes(17, global::CSVCMsg_Sounds.Descriptor);
        public static EmbeddedTypes CSVCMsg_SetView = new EmbeddedTypes(18, global::CSVCMsg_SetView.Descriptor);
        public static EmbeddedTypes CSVCMsg_UserMessage = new EmbeddedTypes(23, global::CSVCMsg_UserMessage.Descriptor);
        //public static EmbeddedTypes EDotaEntityMessages = new EmbeddedTypes(24, global::EDotaEntityMessages.Descriptor);
        public static EmbeddedTypes CSVCMsg_GameEvent = new EmbeddedTypes(25, global::CSVCMsg_GameEvent.Descriptor);
        public static EmbeddedTypes CSVCMsg_PacketEntities = new EmbeddedTypes(26, global::CSVCMsg_PacketEntities.Descriptor);
        public static EmbeddedTypes CSVCMsg_TempEntities = new EmbeddedTypes(27, global::CSVCMsg_TempEntities.Descriptor);
        public static EmbeddedTypes CSVCMsg_GameEventList = new EmbeddedTypes(30, global::CSVCMsg_GameEventList.Descriptor);

        private EmbeddedTypes(ulong value, MessageDescriptor descriptor) : base(value, descriptor.Name)
        {
            this.Descriptor = descriptor;
        }

        public MessageDescriptor Descriptor { get; }
    }
}