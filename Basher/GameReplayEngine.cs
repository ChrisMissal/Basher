using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Basher
{
    internal class GameReplayEngine : ReplayElement
    {
        private readonly CancellationToken cancellationToken;

        private readonly BlockingCollection<CDemoPacket> cDemoPackets = new BlockingCollection<CDemoPacket>();
        private readonly BlockingCollection<CDemoFileHeader> cDemoFileHeaders = new BlockingCollection<CDemoFileHeader>();
        private readonly BlockingCollection<CSVCMsg_ServerInfo> csvServerInfo = new BlockingCollection<CSVCMsg_ServerInfo>();
        private readonly BlockingCollection<CSVCMsg_PacketEntities> csvPacketEntities = new BlockingCollection<CSVCMsg_PacketEntities>();

        private readonly TaskFactory taskFactory;
        private List<Exception> exceptions = new List<Exception>();

        internal GameReplayEngine() : base(TextWriter.Null)
        {
        }

        internal GameReplayEngine(TextWriter writer) : base(writer)
        {
        }

        public GameReplayEngine(TextWriter writer, CancellationTokenSource cancellationTokenSource) : base(writer)
        {
            this.cancellationToken = cancellationTokenSource.Token;
            this.taskFactory = new TaskFactory(this.cancellationToken);
        }

        public bool IsComplete { get; private set; }

        public bool IsHeaderComplete => this.Header.Found;

        public bool Successful => !this.exceptions.Any();

        public void Complete()
        {
            this.IsComplete = true;

            this.cDemoPackets.CompleteAdding();
            this.cDemoFileHeaders.CompleteAdding();
            this.csvServerInfo.CompleteAdding();
            this.csvPacketEntities.CompleteAdding();
        }

        public void Collect(CDemoPacket message)
        {
            this.cDemoPackets.Add(message.Dump(ConsoleColor.Green), this.cancellationToken);
            this.taskFactory.StartNew(() => message.DeserializeAsync(serverInfo =>
            {
                //serverInfo.Dump(ConsoleColor.Cyan, 2);
                this.csvServerInfo.Add(serverInfo, this.cancellationToken);
            }), this.cancellationToken);
        }

        public void Collect(CDemoFileHeader message)
        {
            this.cDemoFileHeaders.Add(message.Dump(ConsoleColor.Green), this.cancellationToken);
        }

        public void Collect(CSVCMsg_PacketEntities message)
        {
            this.csvPacketEntities.Add(message.Dump(ConsoleColor.Green), this.cancellationToken);
        }

        public void AddException(Exception exception)
        {
            this.exceptions.Add(exception);
        }
    }
}