using System.Windows;
using System.Windows.Controls;


namespace CompassPlus.Behaviors
{
    /// <summary>
    /// Filters out unvanted characters, for example numeric text imput.
    /// </summary>
    /// <example>
    /// behaviors:TextBoxBehavior.CharFilter=".-,"
    /// </example>
    public class TextBoxBehavior
    {
        public static readonly DependencyProperty FilterProperty = DependencyProperty.RegisterAttached(
            "CharFilter",
            typeof (string),
            typeof (TextBoxBehavior),
            new PropertyMetadata(null, OnFilterPropertyChanged));

        private static void OnFilterPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var element = (TextBox) dependencyObject;
            if (element == null)
            {
                return;
            }
            element.KeyUp += (sender, args) =>
                {
                    element.Text = Filter(element.Text, (string) e.NewValue);
                    element.SelectionStart = GetSelectionStart(element.Text);
                };
        }

        public static string Filter(string text, string charFilter)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            if (string.IsNullOrEmpty(charFilter))
            {
                return null;
            }

            var chars = charFilter.ToCharArray();

            foreach (var c in chars)
            {
                int index = text.IndexOf(c);

                if (index >= 0)
                {
                    text = text.Remove(index, 1);
                }
            }
            return text;
        }

        public static int GetSelectionStart(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }
            return text.Length;
        }

        public static void SetCharFilter(DependencyObject obj, string value)
        {
            obj.SetValue(FilterProperty, value);
        }

        public static string GetCharFilter(DependencyObject obj)
        {
            return (string) obj.GetValue(FilterProperty);
        }
    }
}
