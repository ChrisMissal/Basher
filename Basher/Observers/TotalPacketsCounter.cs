using System;

namespace Basher.Observers
{
    public class TotalPacketsCounter : IObserver<CDemoPacket>
    {
        private int count;

        public void OnCompleted()
        {
            Console.WriteLine();
            Console.WriteLine($"Parsed {this.count} packets.");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error);
        }

        public void OnNext(CDemoPacket value)
        {
            count++;
        }
    }
}