using System.Globalization;

namespace CustomControls.Helpers;

public class InputTypeToKeyboardConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (Constants.EntryInputType)value! switch
        {
            Constants.EntryInputType.Numeric      => Keyboard.Numeric,
            Constants.EntryInputType.Email        => Keyboard.Email,
            _                           => Keyboard.Text
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}