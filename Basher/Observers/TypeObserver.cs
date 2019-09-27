using System;
using Google.Protobuf;

namespace Basher.Observers
{
    public abstract class TypeObserver<T> : IObserver<IMessage<T>> where T : IMessage<T>
    {
        public abstract void OnCompleted();

        public void OnError(Exception error)
        {
            Console.WriteLine(error);
        }

        public abstract void OnNext(IMessage<T> value);
    }
}