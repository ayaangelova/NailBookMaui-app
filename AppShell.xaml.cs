using NailBookMaui.Services;
using NailBookMaui.Views;

namespace NailBookMaui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(SalonsPage), typeof(SalonsPage));

        Dispatcher.Dispatch(() =>
        {
            if (!AppData.Users.HasCurrentUser)
                Application.Current!.MainPage = new LoginPage();
        });
    }
}