using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Basher
{
    internal class Context
    {
        private readonly Dictionary<Type, object> data = new Dictionary<Type, object>();

        public BlockingCollection<T> GetCollection<T>()
        {
            var type = typeof(T);
            if (!this.data.ContainsKey(type))
            {
                this.data[type] = new BlockingCollection<T>();
            }

            var collection = (BlockingCollection<T>) this.data[type];

            return collection;
        }
    }
}