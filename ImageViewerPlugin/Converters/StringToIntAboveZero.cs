using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ImageViewerPlugin.Converters
{
    public class StringToIntAboveZero : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? value.ToString() : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && int.TryParse(value.ToString(), out var result) && result >= 0)
                return result;

            return DependencyProperty.UnsetValue;
        }
    }
}
