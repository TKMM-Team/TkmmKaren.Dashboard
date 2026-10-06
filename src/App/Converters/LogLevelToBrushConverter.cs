using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Microsoft.Extensions.Logging;

namespace TkmmKaren.Dashboard.Converters;

public sealed class LogLevelToBrushConverter : IValueConverter
{
    private static readonly IBrush DefaultBrush = Brushes.Transparent;
    private static readonly IBrush Info = Brush.Parse("#2E5FC9");
    private static readonly IBrush Debug = Brush.Parse("#9477ED");
    private static readonly IBrush Warning = Brush.Parse("#C9962E");
    private static readonly IBrush Error = Brush.Parse("#C9402E");

    public static LogLevelToBrushConverter Shared { get; } = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value switch {
            LogLevel.Debug => Debug,
            LogLevel.Information => Info,
            LogLevel.Warning => Warning,
            LogLevel.Error => Error,
            _ => DefaultBrush
        };

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => LogLevel.None;
}
