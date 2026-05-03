using NailBookMaui.ViewModels;

namespace NailBookMaui.Views;

public partial class SalonsPage : ContentPage
{
    public SalonsPage()
    {
        InitializeComponent();
        BindingContext = new SalonsViewModel();
    }
}
