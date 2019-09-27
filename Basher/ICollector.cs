using System.Collections.Concurrent;
using Google.Protobuf;

namespace Basher
{
    public interface ICollector<T> where T : IMessage<T>
    {
        void Collect(BlockingCollection<T> inputCollection);
    }
}