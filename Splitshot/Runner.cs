using System;
using System.Collections.Concurrent;
using System.Text;
using Basher;
using Basher.Collectors;

namespace Splitshot
{
    public class Runner
    {
        private readonly string filePath;

        public Runner(string filePath)
        {
            this.filePath = filePath;
        }

        public void Run()
        {
            var inputCollection = new BlockingCollection<CDemoCustomData>();
            var parser = new Parser(this.filePath);
            var collector = new CustomDataCollector(parser);

            collector.Collect(inputCollection);

            var enumerable = inputCollection.GetConsumingEnumerable();

            foreach (var item in enumerable)
            {
                Console.WriteLine(Encoding.UTF8.GetString(item.Data.ToByteArray()));
            }
        }
    }
}
