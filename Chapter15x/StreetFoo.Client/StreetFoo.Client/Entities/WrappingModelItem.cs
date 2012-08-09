using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public abstract class WrappingModelItem<T> : ModelItem
        where T : ModelItem
    {
        public T InnerItem { get; private set; }

        protected WrappingModelItem(T innerItem)
        {
            this.InnerItem = innerItem;

            // subscribe...
            this.InnerItem.PropertyChanged += InnerItem_PropertyChanged;
        }

        void InnerItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // re-raise this as our own...
            this.OnPropertyChanged(e);
        }
    }
}
