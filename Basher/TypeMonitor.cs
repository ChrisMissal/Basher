using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Basher
{
    internal class TypeMonitor : IObservable<Message>, IDisposable
    {
        private readonly List<IObserver<Message>> observers = new List<IObserver<Message>>();
        private readonly BlockingCollection<Message>[] collections;

        public TypeMonitor(params BlockingCollection<Message>[] collections)
        {
            this.collections = collections;
        }

        public IDisposable Subscribe(IObserver<Message> observer)
        {
            if (!this.observers.Contains(observer))
            {
                this.observers.Add(observer);
            }

            return new MessageUnsubscriber(this.observers, observer);
        }

        public void Start(IEnumerable<Message> messages, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var message in messages)
            {
                cancellationToken.ThrowIfCancellationRequested();

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