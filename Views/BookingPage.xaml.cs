using NailBookMaui.ViewModels;

namespace NailBookMaui.Views;

public partial class BookingPage : ContentPage
{
    public BookingPage()
    {
        InitializeComponent();
        BindingContext = new BookingViewModel();
    }
}
