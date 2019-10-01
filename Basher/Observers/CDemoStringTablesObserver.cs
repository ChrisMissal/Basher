using System;
using System.Collections.Concurrent;

namespace Basher.Observers
{
    internal class CDemoStringTablesObserver : IObserver<Message>
    {
        private readonly BlockingCollection<CDemoStringTables.Types.table_t> tablesCollection;
        private readonly BlockingCollection<CDemoStringTables.Types.items_t> itemsCollection;

        public CDemoStringTablesObserver(Context context)
        {
            this.tablesCollection = context.GetCollection<CDemoStringTables.Types.table_t>();
            this.itemsCollection = context.GetCollection<CDemoStringTables.Types.items_t>();
        }

        public void OnCompleted()
        {
            this.tablesCollection.CompleteAdding();
            this.itemsCollection.CompleteAdding();
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error);
        }

        public void OnNext(Message value)
        {
            if (value.Inner is CDemoStringTables message)
            {
                foreach (var table in message.Tables)
                {
                    this.tablesCollection.Add(table);

                    foreach (var item in table.Items)
                    {
                        this.itemsCollection.Add(item);
                    }
                }
            }
        }
    }
}