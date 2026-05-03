using NailBookMaui.ViewModels;

namespace NailBookMaui.Views;

public partial class ExplorePage : ContentPage
{
    private readonly ExploreViewModel _viewModel = new();

    public ExplorePage()
    {
        InitializeComponent();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadContent();
    }
}
