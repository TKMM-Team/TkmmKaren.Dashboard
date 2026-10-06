using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace TkmmKaren.Dashboard;

internal static class LogAccess
{
    private static readonly HttpClient Http = new() {
        Timeout = TimeSpan.FromSeconds(15)
    };

    public const string LogUrl = "https://repo.tkmm.org/.karen-logs/latest.log";

    private static string? _username;
    private static string? _password;

    public static bool HasCredentials => _username is not null;

    public static void SignIn(string username, string password)
    {
        _username = username;
        _password = password;
        ApplyAuthorization();
    }

    public static Task<HttpResponseMessage> GetAsync(CancellationToken cancellationToken)
    {
        ApplyAuthorization();
        return Http.GetAsync(LogUrl, cancellationToken);
    }

    private static void ApplyAuthorization()
    {
        if (_username is null || _password is null) {
            return;
        }

        var token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_username}:{_password}"));
        Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);
    }

    public static bool IsUnauthorized(HttpStatusCode status) => status == HttpStatusCode.Unauthorized;
}
