using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using TkmmKaren.Dashboard.Models;
using TkmmKaren.Dashboard.ViewModels;

namespace TkmmKaren.Dashboard.Views;

public partial class LogsPageView : UserControl
{
    public LogsPageView()
    {
        InitializeComponent();
        var viewModel = new LogsPageViewModel();
        DataContext = viewModel;
        Loaded += (_, _) => viewModel.Start();
        Unloaded += (_, _) => viewModel.Stop();
    }

    private void OnContainerPrepared(object? sender, ContainerPreparedEventArgs e)
    {
        if (e.Container is not ListBoxItem item) {
            return;
        }

        item.ContextRequested -= OnItemContextRequested;
        item.ContextRequested += OnItemContextRequested;
        var copy = new MenuItem { Header = "Copy" };
        var copyMarkdown = new MenuItem { Header = "Copy Markdown" };
        copy.Click += OnCopy;
        copyMarkdown.Click += OnCopyMarkdown;
        item.ContextFlyout = new MenuFlyout {
            Items = { copy, copyMarkdown }
        };
    }

    private void OnCopy(object? sender, RoutedEventArgs e) => Copy(log => log.ToString());

    private void OnCopyMarkdown(object? sender, RoutedEventArgs e) => Copy(log => log.ToMarkdown());

    private async void Copy(Func<EventLog, string> format)
    {
        if (LogList.SelectedItem is not EventLog log
            || TopLevel.GetTopLevel(this)?.Clipboard is not { } clipboard) {
            return;
        }

        await clipboard.SetTextAsync(format(log));
    }

    private void OnItemContextRequested(object? sender, ContextRequestedEventArgs e)
    {
        if (sender is ListBoxItem { DataContext: EventLog log }) {
            LogList.SelectedItem = log;
        }
    }
}
