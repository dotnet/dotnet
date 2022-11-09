using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace NuGet.PackageManagement.UI
{
    internal class CollectionToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var collection = value as IEnumerable<string>;
            if (collection != null)
            {
                return string.Join(", ", collection);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
