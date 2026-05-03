using NailBookMaui.ViewModels;

namespace NailBookMaui.Views;

public partial class ServicesPage : ContentPage
{
    public ServicesPage()
    {
        InitializeComponent();
        BindingContext = new ServicesViewModel();
    }
}
