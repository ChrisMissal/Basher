using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Basher.Observers
{
    internal abstract class TypeObserver<T> : IObserver<PacketMessage> where T : class
    {
        protected List<Task> Tasks { get; } = new List<Task>();

        public abstract void OnCompleted();

        public void OnError(Exception error)
        {
            Console.WriteLine(error);
        }

        public void OnNext(PacketMessage value)
        {
            if (!value.IsA<T>())
            {
                return;
            }

            this.Handle(value);
        }

        protected abstract void Handle(PacketMessage value);
    }
}