using System;
using System.Windows;
using System.Windows.Data;


namespace CompassPlus.Converters
{
    /// <summary>
    /// Abstract base class for value converters that convert between Boolean
    /// values and other types.
    /// </summary>
    public abstract class BooleanConverter<T> : IValueConverter
    {
        protected BooleanConverter(T trueValue, T falseValue)
        {
            this.TrueValue = trueValue;
            this.FalseValue = falseValue;
        }

        public T TrueValue { get; set; }

        public T FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Object.Equals(value, true) ? this.TrueValue : this.FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Object.Equals(value, this.TrueValue) ? true : false;
        }
    }

    /// <summary>
    /// Converts Boolean values to Visibility values. By default, true is
    /// converted to Visible and false is converted to Collapsed.
    /// </summary>
    public sealed class BooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public BooleanToVisibilityConverter()
            : base(Visibility.Visible, Visibility.Collapsed)
        {
        }
    }
}
