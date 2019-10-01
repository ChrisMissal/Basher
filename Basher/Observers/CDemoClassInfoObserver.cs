using System.Collections.Concurrent;

namespace Basher.Observers
{
    internal class CDemoClassInfoObserver : BaseObserver
    {
        private readonly BlockingCollection<CDemoClassInfo.Types.class_t> classCollection;

        public CDemoClassInfoObserver(Context context)
        {
            this.classCollection = context.GetCollection<CDemoClassInfo.Types.class_t>();
        }

        public override void OnCompleted()
        {
            this.classCollection.CompleteAdding();
        }

        public override void OnNext(Message value)
        {
            if (value.Inner is CDemoClassInfo message)
            {
                foreach (var @class in message.Classes)
                {
                    this.classCollection.Add(@class);
                }
            }
        }
    }
}