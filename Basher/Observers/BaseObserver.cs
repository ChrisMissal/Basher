using System;

namespace Basher.Observers
{
    internal abstract class BaseObserver : IObserver<Message>
    {
        public abstract void OnCompleted();

        public void OnError(Exception error)
        {
            throw new InvalidOperationException($"OnError in {this.GetType()}", error);
        }

        public abstract void OnNext(Message value);
    }
}