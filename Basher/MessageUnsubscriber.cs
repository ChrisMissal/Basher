using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;

namespace Basher
{
    internal class MessageUnsubscriber<T> : IDisposable where T : IMessage<T>
    {
        private readonly List<IObserver<T>> observers;
        private readonly IObserver<T> observer;

        public MessageUnsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
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