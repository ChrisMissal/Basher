using System;
using System.IO;
using System.Linq;

namespace Basher
{
    internal static class Extensions
    {
        private static readonly object consoleLock = new object();

        private static readonly TextWriter Output = Console.Out;

        private const int MaxByteArrayLengthForDisplay = 100;
        private const int MaxMessageDepth = 32;

        private static readonly string[] DumpTabs = new string[MaxMessageDepth];

        static Extensions()
        {
            for (var i = 0; i < MaxMessageDepth; i++)
            {
                var tabs = Enumerable.Repeat('\t', i).ToArray();
                DumpTabs[i] = new string(tabs);
            }
        }

        public static ulong ReadVarInt(this Stream self)
        {
            ulong result = 0;
            var shift = 0;

            const int max = sizeof(long) * 8;
            while (shift < max)
            {
                var b = (byte)self.ReadByte();
                var temp = (ulong)(b & 0x7f);
                result |= temp << shift;

                if ((b & 0x80) != 0x80)
                {
                    return result;
                }

                shift += 7;
            }

            throw new InvalidDataException("Invalid VarInt.");
        }

        public static T Dump<T>(this T self, ConsoleColor color, int depth = 0)
        {
            lock (consoleLock)
            {
                if (color <= ConsoleColor.DarkGray)
                {
                    Console.ForegroundColor += 6;
                }
                Console.ForegroundColor = color;
                Output.Write(DumpTabs[depth]);
                Output.WriteLine(self);
                Console.ResetColor();
            }

            return self;
        }

        public static string ToFlatString(this byte[] self, int? max = 100)
        {
            max = max ?? MaxByteArrayLengthForDisplay;
            var value = self.Take(max.Value).ToArray();
            return $"[{string.Join(",", value)}] ({(value.Length >= max ? $"...\t({self.Length})]" : $"{self.Length})]")})";
        }
    }
}
