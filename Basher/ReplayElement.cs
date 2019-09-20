using System.IO;

namespace Basher
{
    internal abstract class ReplayElement
    {
        protected readonly TextWriter writer;

        protected ReplayElement(TextWriter writer)
        {
            this.writer = writer;
        }

        public HeaderElement Header { get; } = new HeaderElement();

        public ulong Kind { get; set; }

        public ulong Tick { get; set; }

        public ulong Size { get; set; }

        public ulong Message { get; set; }

        public bool IsCompressed => ((int) this.Kind & (int) EDemoCommands.DemIsCompressed) != 0;

        public void ResetCompression()
        {
            this.Kind -= (ulong)EDemoCommands.DemIsCompressed;
        }

        public class HeaderElement
        {
            public bool Found { get; set; }

            public ulong SummaryOffset { get; set; }
        }
    }
}