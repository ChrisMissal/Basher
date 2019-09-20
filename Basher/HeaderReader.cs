using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Basher
{
    internal class HeaderReader
    {
        private readonly Stream stream;

        public HeaderReader(Stream stream)
        {
            this.stream = stream;
        }

        public void Read(ReplayElement element)
        {
            var buffer = new byte[8];

            this.stream.Read(buffer, 0, 8);

            var value = Encoding.UTF8.GetString(buffer);

            element.Header.Found = value == "PBDEMS2\0";

            buffer = new byte[4];
            this.stream.Read(buffer, 0, 4);
            if (BitConverter.IsLittleEndian != true)
            {
                Array.Reverse(buffer);
            }

            var summary = Enumerable.Range(0, 8).Select(x => buffer.Length > x ? buffer[x] : byte.MinValue).ToArray();
            element.Header.SummaryOffset = BitConverter.ToUInt64(summary, 0);
        }
    }
}