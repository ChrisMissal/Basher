using System;
using System.Collections.Generic;
using Google.Protobuf;

namespace Basher.Observers
{
    internal class TypeMonitor<T> : IObservable<T>, IDisposable where T : class, IMessage<T>
    {
        private readonly List<IObserver<T>> observers = new List<IObserver<T>>();

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!this.observers.Contains(observer))
            {
                this.observers.Add(observer);
            }

            return new MessageUnsubscriber<T>(this.observers, observer);
        }

        public void Start(IEnumerable<IMessage> messages)
        {
            foreach (var message in messages)
            {
                foreach (var observer in this.observers)
                {
                    if (message is T msg)
                    {
                        observer.OnNext(msg);
                    }
                }
            }
        }

        public void Dispose()
        {
            foreach (var observer in this.observers)
            {
                observer.OnCompleted();
            }
        }
    }
}