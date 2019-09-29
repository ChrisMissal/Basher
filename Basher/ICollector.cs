using System.Collections.Concurrent;

namespace Basher
{
    public interface ICollector
    {
        void Collect<T>(BlockingCollection<T> collection);
    }
}