using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;

namespace Basher
{
    /*[Obsolete("I like this idea, but not sure yet how it will play out.")]
    public class JsonFileHeaderCollector : ICollector
    {
        private readonly BlockingCollection<string> collection = new BlockingCollection<string>();

        public void Collect<T>(BlockingCollection<IMessage<T>> inputCollection) where T : IMessage<T>
        {
            var enumerable = this.collection.GetConsumingEnumerable();

            foreach (var item in enumerable)
            {
                throw new NotImplementedException("Need to write conversion code.");
            }
        }

        public Task CollectAsync<T>(BlockingCollection<IMessage<T>> inputCollection, CancellationToken cancellationToken) where T : IMessage<T>
        {
            var enumerable = this.collection.GetConsumingEnumerable(cancellationToken);

            var exception = new NotImplementedException("Need to write conversion code.");

            if (enumerable.Any())
            {
                throw exception;
            }

            return Task.FromException(exception);
        }
    }*/
}