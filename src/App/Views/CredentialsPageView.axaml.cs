using Avalonia.Controls;

namespace TkmmKaren.Dashboard.Views;

public partial class CredentialsPageView : UserControl
{
    public CredentialsPageView()
    {
        InitializeComponent();
        DataContext = new ViewModels.CredentialsPageViewModel();
    }
}
