using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using TkmmKaren.Dashboard.Models;

namespace TkmmKaren.Dashboard;

internal static class LogParser
{
    private static readonly Regex AnsiRegex = new(@"\x1B(?:[@-Z\\-_]|\[[0-?]*[ -/]*[@-~])", RegexOptions.Compiled);
    private static readonly Regex HeaderRegex = new(
        @"^(?:(?<date>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2})\s+)?(?<level>trce|dbug|info|warn|fail|crit|trace|debug|information|warning|error|critical):\s+(?<category>.+?)(?:\[(?<eventId>\d+)\])?\s*$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static List<EventLog> Parse(string raw)
    {
        var lines = AnsiRegex.Replace(raw, string.Empty)
            .Replace("\r\n", "\n", StringComparison.Ordinal)
            .Split('\n');

        var results = new List<EventLog>();
        var contentLines = new List<string>();
        var level = LogLevel.Information;
        var eventId = 0;
        var category = "log";
        var date = DateTime.UtcNow;

        void Flush()
        {
            if (contentLines.Count == 0) {
                return;
            }

            results.Add(new EventLog(
                level,
                eventId,
                category,
                date,
                string.Join('\n', contentLines).TrimEnd(),
                null));
            contentLines.Clear();
        }

        foreach (var line in lines) {
            if (string.IsNullOrWhiteSpace(line)
                || line.Contains("Using launch settings", StringComparison.Ordinal)
                || line.Trim() is "Building...") {
                continue;
            }

            var header = HeaderRegex.Match(line);
            if (header.Success) {
                Flush();
                level = ParseLevel(header.Groups["level"].Value);
                category = header.Groups["category"].Value.Trim();
                eventId = header.Groups["eventId"].Success
                    ? int.Parse(header.Groups["eventId"].Value)
                    : 0;
                date = header.Groups["date"].Success
                    && DateTime.TryParse(header.Groups["date"].Value, out var parsed)
                        ? DateTime.SpecifyKind(parsed, DateTimeKind.Utc)
                        : DateTime.UtcNow;
                continue;
            }

            var body = line.StartsWith("      ", StringComparison.Ordinal) ? line.TrimStart() : line;
            if (!string.IsNullOrWhiteSpace(body)) {
                contentLines.Add(body);
            }
        }

        Flush();
        return results;
    }

    private static LogLevel ParseLevel(string value) => value.ToLowerInvariant() switch {
        "trce" or "trace" => LogLevel.Trace,
        "dbug" or "debug" => LogLevel.Debug,
        "info" or "information" => LogLevel.Information,
        "warn" or "warning" => LogLevel.Warning,
        "fail" or "error" => LogLevel.Error,
        "crit" or "critical" => LogLevel.Critical,
        _ => LogLevel.Information
    };
}
