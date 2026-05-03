using NailBookMaui.Views;

namespace NailBookMaui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new LoginPage();
    }
}
