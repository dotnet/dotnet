using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.UI
{
    internal class PackageLevelGroupToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CollectionViewGroup grp = (CollectionViewGroup)value;
            if (grp.Name.ToString().Equals(PackageLevel.TopLevel.ToString(), StringComparison.OrdinalIgnoreCase)
                || grp.Name.ToString().Equals(PackageLevel.Transitive.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // no op
            Debug.Fail("Not Implemented");
            return null;
        }
    }
}
