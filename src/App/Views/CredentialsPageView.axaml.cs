using Avalonia.Controls;
using TkmmKaren.Dashboard.ViewModels;

namespace TkmmKaren.Dashboard.Views;

public partial class CredentialsPageView : UserControl
{
    public CredentialsPageView()
    {
        InitializeComponent();

        var viewModel = new CredentialsPageViewModel();
        DataContext = viewModel;

        Form.Submitted += () => {
            if (viewModel.IsBusy) {
                return;
            }

            viewModel.Username = Form.Username;
            viewModel.Password = Form.Password;
            if (viewModel.SignInCommand.CanExecute(null)) {
                viewModel.SignInCommand.Execute(null);
            }
        };

        viewModel.PropertyChanged += (_, e) => {
            if (e.PropertyName == nameof(CredentialsPageViewModel.Error)) {
                Form.ShowError(viewModel.Error);
            }

            if (e.PropertyName == nameof(CredentialsPageViewModel.IsBusy)) {
                Form.ShowBusy(viewModel.IsBusy);
            }
        };
    }
}
