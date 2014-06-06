using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;


namespace CompassPlus.Converters
{
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color;
            if (value == null)
            {
                color = Color.FromArgb(0xff, 0x00, 0x00, 0x00);
            }
            else if ((string.IsNullOrEmpty(value.ToString())) || (value.ToString() == "0"))
            {
                color = Color.FromArgb(0xff, 0x00, 0x00, 0x00);
            }
            else
            {
                try
                {
                    string val = value.ToString();
                    val = val.Replace("#", "");

                    byte a = System.Convert.ToByte("ff", 16);

                    byte pos = 0;

                    if (val.Length == 8)
                    {
                        a = System.Convert.ToByte(val.Substring(pos, 2), 16);
                        pos = 2;
                    }
                    byte r = System.Convert.ToByte(val.Substring(pos, 2), 16);
                    pos += 2;
                    byte g = System.Convert.ToByte(val.Substring(pos, 2), 16);
                    pos += 2;
                    byte b = System.Convert.ToByte(val.Substring(pos, 2), 16);

                    Color col = Color.FromArgb(a, r, g, b);

                    color = col;
                }
                catch
                {
                    color = Color.FromArgb(0xff, 0x00, 0x00, 0x00);
                }
            }
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (Color)value;
            return val.ToString();
        }
    }
}