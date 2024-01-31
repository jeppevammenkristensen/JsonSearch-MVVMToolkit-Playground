using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Searcher.Converters;

public class CustomBooleanToVisibiltyConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool b = false;
        if (value is bool bValue)
        {
            b = bValue;
        }

        if (value is Nullable<bool>)
        {
            var nValue = (bool?) value;
            b = nValue.HasValue ? nValue.Value : false;
        }

        if (parameter != null)
        {
            b = !b;
        }

        return b ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}