using System.Collections.Concurrent;
using Basher.Observers;

namespace Basher.Collectors
{
    public class CustomDataCollector : ICollector<CDemoCustomData>
    {
        private readonly Parser parser;

        public CustomDataCollector(Parser parser)
        {
            this.parser = parser;
        }

        public void Collect(BlockingCollection<CDemoCustomData> inputCollection)
        {
            using (var monitor = new TypeMonitor<CDemoFullPacket>())
            {
                monitor.Subscribe(new CDemoCustomDataObserver(inputCollection));
                monitor.Subscribe(new FullPacketCountObserver());

                var messages = this.parser.GetMessages();

                monitor.Start(messages);
            }

            inputCollection.CompleteAdding();
        }
    }
}