using System.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TkmmKaren.Dashboard.ViewModels;

public partial class CredentialsPageViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string Username { get; set; } = "";

    [ObservableProperty]
    public partial string Password { get; set; } = "";

    [ObservableProperty]
    public partial string? Error { get; set; }

    [ObservableProperty]
    public partial bool IsBusy { get; set; }

    [RelayCommand]
    private async Task SignInAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrEmpty(Password)) {
            Error = "Enter a username and password.";
            return;
        }

        IsBusy = true;
        Error = null;
        LogAccess.SignIn(Username.Trim(), Password);

        try {
            using var response = await LogAccess.GetAsync(CancellationToken.None);
            if (response.StatusCode == HttpStatusCode.Unauthorized) {
                Error = "Those credentials were rejected.";
                return;
            }
        }
        catch (Exception ex) {
            Error = ex.Message;
            return;
        }
        finally {
            IsBusy = false;
        }

        PageRouter.ShowLogs();
    }
}
