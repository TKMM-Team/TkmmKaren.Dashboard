using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace TkmmKaren.Dashboard;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is ISingleViewApplicationLifetime singleView) {
            _ = ChoosePageAsync(singleView);
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static async Task ChoosePageAsync(ISingleViewApplicationLifetime singleView)
    {
        var locked = false;
        try {
            using var response = await LogAccess.GetAsync(CancellationToken.None);
            locked = LogAccess.IsUnauthorized(response.StatusCode);
        }
        catch {
            locked = false;
        }

        await Dispatcher.UIThread.InvokeAsync(() => {
            singleView.MainView = locked
                ? new Views.CredentialsPageView()
                : new Views.LogsPageView();
        });
    }
}
