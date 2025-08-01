using System.Globalization;

namespace AdvGenCalculator.Converters
{
    public class BoolToModeNameConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is bool isScientific && isScientific ? "Scientific" : "Basic";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value?.ToString() == "Scientific";
        }
    }
}