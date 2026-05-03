namespace NailBookMaui.WinUI;

public partial class App : MauiWinUIApplication
{
    public App()
    {
        InitializeComponent();
    }

    protected override MauiApp CreateMauiApp() => NailBookMaui.MauiProgram.CreateMauiApp();
}
