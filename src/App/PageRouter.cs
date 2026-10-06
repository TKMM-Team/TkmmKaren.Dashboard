using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using TkmmKaren.Dashboard.Views;

namespace TkmmKaren.Dashboard;

internal static class PageRouter
{
    public static void ShowCredentials()
    {
        Dispatcher.UIThread.Post(() => SetMainView(new CredentialsPageView()));
    }

    public static void ShowLogs()
    {
        Dispatcher.UIThread.Post(() => SetMainView(new LogsPageView()));
    }

    private static void SetMainView(Control view)
    {
        if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime singleView) {
            singleView.MainView = view;
        }
    }
}
