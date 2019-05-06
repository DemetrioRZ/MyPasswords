using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Views.Converters
{
    public class VisibilityConverterHidden : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool b))
                return DependencyProperty.UnsetValue;

            return b ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility v))
                return DependencyProperty.UnsetValue;

            return v == Visibility.Visible;
        }
    }
}