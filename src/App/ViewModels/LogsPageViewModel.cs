using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using TkmmKaren.Dashboard.Models;

namespace TkmmKaren.Dashboard.ViewModels;

public partial class LogsPageViewModel : ObservableObject
{
    private static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(3);

    private string? _lastRaw;
    private CancellationTokenSource? _cts;

    public ObservableCollection<EventLog> Logs { get; } = [];

    [ObservableProperty]
    public partial EventLog? Selected { get; set; }

    public void Start()
    {
        if (_cts is not null) {
            return;
        }

        _cts = new CancellationTokenSource();
        _ = RunAsync(_cts.Token);
    }

    public void Stop()
    {
        if (_cts is null) {
            return;
        }

        _cts.Cancel();
        _cts.Dispose();
        _cts = null;
    }

    private async Task RunAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested) {
            try {
                if (!await RefreshAsync(cancellationToken)) {
                    return;
                }
            }
            catch (OperationCanceledException) {
                return;
            }
            catch (Exception ex) {
                SetLogs([
                    new EventLog(
                        LogLevel.Error,
                        0,
                        "Dashboard",
                        DateTime.UtcNow,
                        $"[Dashboard] Failed to load {LogAccess.LogUrl}: {ex.Message}",
                        ex.ToString())
                ]);
            }

            try {
                await Task.Delay(RefreshInterval, cancellationToken);
            }
            catch (OperationCanceledException) {
                return;
            }
        }
    }

    private async Task<bool> RefreshAsync(CancellationToken cancellationToken)
    {
        using var response = await LogAccess.GetAsync(cancellationToken);
        if (LogAccess.IsUnauthorized(response.StatusCode)) {
            if (LogAccess.HasCredentials) {
                return true;
            }

            PageRouter.ShowCredentials();
            return false;
        }

        response.EnsureSuccessStatusCode();
        var raw = await response.Content.ReadAsStringAsync(cancellationToken);
        if (raw != _lastRaw) {
            _lastRaw = raw;
            SetLogs(LogParser.Parse(raw));
        }

        return true;
    }

    private void SetLogs(IReadOnlyList<EventLog> entries)
    {
        Logs.Clear();
        foreach (var entry in entries) {
            Logs.Add(entry);
        }

        Selected = Logs.Count == 0 ? null : Logs[^1];
    }
}
