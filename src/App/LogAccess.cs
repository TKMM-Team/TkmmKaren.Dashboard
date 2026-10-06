using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace TkmmKaren.Dashboard;

internal static class LogAccess
{
    private static readonly HttpClient Http = new() {
        Timeout = TimeSpan.FromSeconds(15)
    };

    public const string LogUrl = "__LOG_URL__";

    public static void SignIn(string username, string password)
    {
        var token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
        Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);
    }

    public static Task<HttpResponseMessage> GetAsync(CancellationToken cancellationToken)
        => Http.GetAsync(LogUrl, cancellationToken);

    public static bool IsUnauthorized(HttpStatusCode status) => status == HttpStatusCode.Unauthorized;
}
