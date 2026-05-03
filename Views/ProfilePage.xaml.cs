using NailBookMaui.ViewModels;

namespace NailBookMaui.Views;

public partial class ProfilePage : ContentPage
{
    private readonly ProfileViewModel _viewModel = new();

    public ProfilePage()
    {
        InitializeComponent();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadProfile();
    }

}
