using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public abstract class ModelItem<T> : ModelItem
        where T : new()
    {
        public T InnerItem { get; private set; }

        protected ModelItem(T innerItem)
        {
            this.InnerItem = innerItem;
        }
    }
}
