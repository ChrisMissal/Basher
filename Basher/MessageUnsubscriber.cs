using System;
using System.Collections.Generic;
using System.Linq;

namespace Basher
{
    internal class MessageUnsubscriber : IDisposable
    {
        private readonly List<IObserver<PacketMessage>> observers;
        private readonly IObserver<PacketMessage> observer;

        public MessageUnsubscriber(List<IObserver<PacketMessage>> observers, IObserver<PacketMessage> observer)
        {
            this.observers = observers.ToList();
            this.observer = observer;
        }

        public void Dispose()
        {
            if (this.observers.Contains(this.observer))
            {
                this.observers.Remove(this.observer);
            }
        }
    }
}