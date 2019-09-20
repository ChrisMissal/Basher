using System;
using System.Threading;
using System.Threading.Tasks;

namespace Basher
{
    public class Parser
    {
        private readonly CancellationToken cancellationToken;

        public Parser(CancellationToken cancellationToken)
        {
            this.cancellationToken = cancellationToken;
        }

        public Task ParseFromFileAsync(string fileName)
        {
            return Task.Delay(10000, cancellationToken);
        }
    }
}
