using System.Globalization;

namespace CustomControls.Helpers;

public class NullToBoolConverter : IValueConverter
{
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value != null;
    }
}