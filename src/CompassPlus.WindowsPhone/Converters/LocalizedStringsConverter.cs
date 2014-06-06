using System;
using System.Globalization;
using System.Windows.Data;

using CompassPlus.Globalization;


namespace CompassPlus.Converters
{
    public class LocalizedStringsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
               throw new ArgumentNullException("value");
            }
            var key = (string) value;
            return new LocalizedStrings()[key];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
