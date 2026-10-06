using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using TkmmKaren.Dashboard;

internal sealed partial class Program
{
    private static Task Main(string[] args)
    {
        LogAccess.LogUrl = Environment.GetEnvironmentVariable("LOG_URL") ?? "";

        return BuildAvaloniaApp()
            .StartBrowserAppAsync("out");
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .WithInterFont();
}
