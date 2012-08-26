using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace StreetFoo.Client.UI.Common
{
    public class IMappablePointConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value == null)
                return null;

            if (value is AdHocMappablePoint)
                return (AdHocMappablePoint)value;
            else if (value is IMappablePoint)
                return new AdHocMappablePoint((IMappablePoint)value);
            else
                throw new NotSupportedException(string.Format("Cannot handle '{0}'.", value.GetType()));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
