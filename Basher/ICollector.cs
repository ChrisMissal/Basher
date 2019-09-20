using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;

namespace Basher
{
    public interface ICollector
    {
        void Collect<T>(BlockingCollection<IMessage<T>> inputCollection) where T : IMessage<T>;

        Task CollectAsync<T>(BlockingCollection<IMessage<T>> inputCollection, CancellationToken cancellationToken) where T : IMessage<T>;

        //IEnumerable<IMessage<T>> GetEnumerable<T>() where T : IMessage<T>;
    }
}