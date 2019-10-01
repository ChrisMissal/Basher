using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

#if DEBUG
[assembly: InternalsVisibleTo("Basher.Tests")]
#endif
namespace Basher
{
    internal class Parser
    {
        private readonly ReplayFileEnumerator fileEnumerator;

        public Parser(string path)
        {
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            this.fileEnumerator = new ReplayFileEnumerator(fileStream);
        }

        internal IEnumerable<Message> GetMessages()
        {
            foreach (var message in this.fileEnumerator)
            {
                yield return message;
            }
        }
    }
}
