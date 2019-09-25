using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;

namespace Basher
{
    internal class GameReplayEngine : ReplayElement
    {
        private readonly CancellationToken cancellationToken;

        private readonly BlockingCollection<CDemoPacket> cDemoPackets = new BlockingCollection<CDemoPacket>();
        private readonly BlockingCollection<CDemoFileHeader> cDemoFileHeaders = new BlockingCollection<CDemoFileHeader>();
        private readonly BlockingCollection<CSVCMsg_ServerInfo> csvServerInfo = new BlockingCollection<CSVCMsg_ServerInfo>();
        private readonly BlockingCollection<CDemoSendTables> cDemoSendTables = new BlockingCollection<CDemoSendTables>();
        private readonly BlockingCollection<CSVCMsg_PacketEntities> csvPacketEntities = new BlockingCollection<CSVCMsg_PacketEntities>();

        private readonly BlockingCollection<IMessage> messages = new BlockingCollection<IMessage>();

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

        public void Collect(IMessage message)
        {
            this.messages.Add(message, this.cancellationToken);
            this.taskFactory.StartNew(() =>
            {
                const ConsoleColor messageColor = ConsoleColor.Blue;
                switch (message)
                {
                    case CDemoFileHeader header:
                        message.Descriptor.ClrType.Dump(ConsoleColor.Cyan);
                        header.Dump(messageColor, 1);
                        break;
                    case CDemoFileInfo info:
                        message.Descriptor.ClrType.Dump(ConsoleColor.Cyan);
                        info.Dump(messageColor, 1);
                        break;
                    case CDemoStop stop:
                        message.Descriptor.ClrType.Dump(ConsoleColor.Cyan);
                        stop.Dump(messageColor, 1);
                        break;
                    case CDemoPacket packet:
                        message.Descriptor.ClrType.Dump(ConsoleColor.Cyan);
                        packet.Dump(messageColor, 1);
                        break;
                    default:
                        message.Dump(ConsoleColor.Cyan, 2);
                        break;
                }

            }, this.cancellationToken);
        }

        /*public void Collect(CDemoPacket message)
        {
            this.cDemoPackets.Add(message.Dump(ConsoleColor.Green), this.cancellationToken);
            this.taskFactory.StartNew(() => message.DeserializeAsync<IMessage>(msg =>
            {
                this.messages.Add(msg, this.cancellationToken);
            }), this.cancellationToken);
        }

        public void Collect(CDemoFileHeader message)
        {
            this.cDemoFileHeaders.Add(message.Dump(ConsoleColor.Magenta), this.cancellationToken);
        }*/

        public void AddException(Exception exception)
        {
            this.exceptions.Add(exception);
        }
    }
}