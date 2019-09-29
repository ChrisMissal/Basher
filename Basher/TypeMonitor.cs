using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Basher
{
    internal class TypeMonitor : IObservable<PacketMessage>, IDisposable
    {
        private readonly List<IObserver<PacketMessage>> observers = new List<IObserver<PacketMessage>>();
        private readonly BlockingCollection<PacketMessage>[] collections;

        public TypeMonitor(params BlockingCollection<PacketMessage>[] collections)
        {
            this.collections = collections;
        }

        public IDisposable Subscribe(IObserver<PacketMessage> observer)
        {
            if (!this.observers.Contains(observer))
            {
                this.observers.Add(observer);
            }

            return new MessageUnsubscriber(this.observers, observer);
        }

        public void Start(IEnumerable<PacketMessage> messages)
        {
            foreach (var message in messages)
            {
                foreach (var observer in this.observers)
                {
                    observer.OnNext(message);
                }
            }
        }

        public void Dispose()
        {
            foreach (var observer in this.observers)
            {
                observer.OnCompleted();
            }

            foreach (var collection in this.collections)
            {
                collection.CompleteAdding();
            }
        }
    }
}