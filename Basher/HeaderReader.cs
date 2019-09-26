using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Basher
{
    internal class HeaderReader
    {
        public void Find(Stream stream, out bool headerFound, out ulong summaryOffset)
        {
            var buffer = new byte[8];

            stream.Read(buffer, 0, 8);

            var value = Encoding.UTF8.GetString(buffer);

            headerFound = value == "PBDEMS2\0";

            buffer = new byte[4];
            stream.Read(buffer, 0, 4);
            if (BitConverter.IsLittleEndian != true)
            {
                Array.Reverse(buffer);
            }

            var summary = Enumerable.Range(0, 8).Select(x => buffer.Length > x ? buffer[x] : byte.MinValue).ToArray();
            summaryOffset = BitConverter.ToUInt64(summary, 0);
        }
    }
}