using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace NuGet.PackageManagement.UI
{
    internal class EnumDescriptionValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetEnumDescription((Enum)value, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private static string GetEnumDescription(Enum enumValue, CultureInfo culture)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var attribArray = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
            if (attribArray.Length == 0)
            {
                return enumValue.ToString();
            }

            var attrib = attribArray[0] as DescriptionAttribute;
            if (string.IsNullOrEmpty(attrib.Description))
            {
                return enumValue.ToString();
            }

            var resourceString = Resources.ResourceManager.GetString(attrib.Description, culture);
            return !string.IsNullOrEmpty(resourceString) ? resourceString : attrib.Description;
        }
    }
}
