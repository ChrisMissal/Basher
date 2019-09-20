// ReSharper disable CheckNamespace

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Basher;
using Basher.Enumerations;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Jil;

public partial class CDemoPacket
{
    private VarIntData varInt;

    public Task DeserializeAsync(Action<CSVCMsg_ServerInfo> nextAction)
    {
        var buffer = this.Data.ToByteArray();

        var stream = new MemoryStream();
        stream.Read(buffer, 0, buffer.Length);

        var embed = EmbeddedTypes.FromValue(this.varInt.Kind);
        var serverInfo = embed.Read<CSVCMsg_ServerInfo>(this);

        nextAction(serverInfo);

        return Task.CompletedTask;
    }

    public void Peek(ulong message = 0, ulong size = 0, ulong kind = 0, ulong tick = 0)
    {
        this.varInt = new VarIntData { Kind = kind, Size = size, Tick = tick, Message = message };
    }

    private class VarIntData
    {
        public ulong Kind;
        public ulong Size;
        public ulong Tick;
        public ulong Message;

        public override string ToString()
        {
            return JSON.Serialize(this);
        }
    }
}