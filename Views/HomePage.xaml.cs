using NailBookMaui.ViewModels;

namespace NailBookMaui.Views;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _viewModel = new();

    public HomePage()
    {
        InitializeComponent();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadData();
    }

    private async void GoToBooking_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//booking");
    }

    private async void GoToExplore_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//explore");
    }
}
