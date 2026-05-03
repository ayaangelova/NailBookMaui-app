using NailBookMaui.ViewModels;

namespace NailBookMaui.Views;

public partial class AppointmentsPage : ContentPage
{
    private readonly AppointmentsViewModel _viewModel = new();

    public AppointmentsPage()
    {
        InitializeComponent();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadAppointments();
    }
}
