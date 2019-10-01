using System;
using System.Collections.Generic;
using System.Linq;

namespace Basher
{
    internal class MessageUnsubscriber : IDisposable
    {
        private readonly List<IObserver<Message>> observers;
        private readonly IObserver<Message> observer;

        public MessageUnsubscriber(List<IObserver<Message>> observers, IObserver<Message> observer)
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