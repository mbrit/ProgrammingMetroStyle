using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace StreetFoo.Client
{
    public abstract class ViewModelSingleton<T> : ViewModel, IViewModelSingleton<T>
        where T : ModelItem
    {
        // holds the base item that we're mapped to...
        private T _item;

        protected ViewModelSingleton(IViewModelHost host)
            : base(host)
        {
        }

        public T Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
                this.OnPropertyChanged();

                // reload...
                this.ItemChanged();
            }
        }

        protected virtual void ItemChanged()
        {
            // no-op...
        }

        public override void Activated(object args)
        {
            base.Activated(args);

            // check...
            if (args == null)
                throw new InvalidOperationException("An item was not supplied.");
            if(!(typeof(T).GetTypeInfo().IsAssignableFrom(args.GetType().GetTypeInfo())))
            {
                throw new InvalidOperationException(string.Format("An item of type '{0}' was supplied, but an item of type '{1}' was required.",
                    args.GetType(), typeof(T)));
            }

            // are our arguments initializing our item?
            this.Item = (T)args;
        }
    }
}
