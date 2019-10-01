using System;
using System.Collections.Concurrent;
using Basher.Enumerations;

namespace Basher.Observers
{
    internal class CDemoCustomDataObserver : BaseObserver
    {
        private readonly BlockingCollection<Tuple<ulong, string>> saveCollection;

        public CDemoCustomDataObserver(Context context)
        {
            this.saveCollection = context.GetCollection<Tuple<ulong, string>>();
        }

        public override void OnCompleted()
        {
            this.saveCollection.CompleteAdding();
        }

        public override void OnNext(Message value)
        {
            if (value.Inner is CDemoCustomDataCallbacks callbacks)
            {
                foreach (var saveId in callbacks.SaveId)
                {
                    this.saveCollection.Add(new Tuple<ulong, string>(value.Kind, saveId));
                }
            }

            if (value.Inner is CDemoCustomData customData)
            {
                var data = customData.Data;
                var index = customData.CallbackIndex;


            }
        }
    }
}