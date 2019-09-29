namespace Basher
{
    public class Runner
    {
        private readonly MessageContext context = new MessageContext();
        private readonly Parser parser;

        public Runner(string path)
        {
            this.parser = new Parser(path);
        }

        public void Run()
        {
            using (var monitor = new TypeMonitor())
            {
                foreach (var observer in this.context.GetObservers())
                {
                    monitor.Subscribe(observer);
                }

                var messages = this.parser.GetMessages();

                monitor.Start(messages);
            }
        }
    }
}