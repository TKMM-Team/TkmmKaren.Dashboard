using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace TkmmKaren.Dashboard.Models;

public sealed partial class EventLog : ObservableObject
{
    public EventLog(LogLevel logLevel, int eventId, string eventName, DateTime date, string content, string? exception)
    {
        LogLevel = logLevel;
        EventId = eventId;
        EventName = eventName;
        Date = date;
        Content = content;
        Exception = exception;
    }

    [ObservableProperty]
    private LogLevel _logLevel;

    [ObservableProperty]
    private int _eventId;

    [ObservableProperty]
    private string _eventName = string.Empty;

    [ObservableProperty]
    private DateTime _date;

    [ObservableProperty]
    private string _content = string.Empty;

    [ObservableProperty]
    private string? _exception;

    public string ToMarkdown()
    {
        return $"""
            `[{EventId}: {LogLevel}]`
                 {Content}
            """;
    }

    public override string ToString()
    {
        return $"""
            [{EventId,2}: {LogLevel,-12}]
                 {Content}
            """;
    }
}
