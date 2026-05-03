using System.Collections.ObjectModel;
using System.Windows.Input;
using NailBookMaui.Models;
using NailBookMaui.Services;

namespace NailBookMaui.ViewModels;

public class AppointmentsViewModel : BaseViewModel
{
    public ObservableCollection<Appointment> Appointments { get; } = new();

    private Appointment? _selectedAppointment;
    public Appointment? SelectedAppointment
    {
        get => _selectedAppointment;
        set => SetProperty(ref _selectedAppointment, value);
    }

    private string _statusMessage = string.Empty;
    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    
    public ICommand CancelSpecificAppointmentCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand GoToRegistrationCommand { get; }

    public AppointmentsViewModel()
    {
        Title = "Моите часове";

       
        CancelSpecificAppointmentCommand = new Command<Appointment>(async appointment => await CancelSpecificAppointmentAsync(appointment));

        RefreshCommand = new AsyncCommand(RefreshAsync);
        GoToRegistrationCommand = new AsyncCommand(GoToRegistrationAsync);

        AppData.Users.CurrentUserChanged += LoadAppointments;
        AppData.Appointments.AppointmentsChanged += LoadAppointments;

        LoadAppointments();
    }
    private async Task RefreshAsync()
    {
        LoadAppointments();

        await AppData.Notifications.ShowSuccessAsync("Обновено");
    }
    public void LoadAppointments()
    {
        RefreshFooter();
        Appointments.Clear();

        if (!AppData.Users.HasCurrentUser)
        {
            StatusMessage = "Няма активен профил. Регистрирайте се, за да виждате запазените си часове.";
            return;
        }

        var user = AppData.Users.GetCurrentUser();

        foreach (var appointment in AppData.Appointments.GetAppointmentsByUserId(user.Id))
            Appointments.Add(appointment);

        StatusMessage = Appointments.Count == 0
            ? "Все още нямате запазени часове."
            : $"Показани са {Appointments.Count} запазени часа за {user.FullName}.";
    }

    private async Task GoToRegistrationAsync()
    {
        await Shell.Current.GoToAsync("//registration");
    }

    private async Task CancelSpecificAppointmentAsync(Appointment appointment)
    {
        if (appointment == null)
            return;

        try
        {
            var user = AppData.Users.GetCurrentUser();

            
            var userAppointments = AppData.Appointments
                .GetAppointmentsByUserId(user.Id)
                .OrderBy(a => a.CreatedAt)
                .ToList();

           
            var firstAppointment = userAppointments.FirstOrDefault();

            bool isFirstBooking = firstAppointment != null && firstAppointment.Id == appointment.Id;

            
            AppData.Appointments.CancelAppointment(appointment.Id);

            int pointsToRemove = isFirstBooking
                ? AppData.Content.GetFirstBookingBonusPoints() 
                : 10;

            AppData.Users.RemoveLoyaltyPoints(pointsToRemove);

            await AppData.Notifications.ShowSuccessAsync(
                $"Резервацията беше отменена. Премахнати са {pointsToRemove} точки.");
        }
        catch (Exception ex)
        {
            await AppData.Notifications.ShowErrorAsync(ex.Message);
        }
    }



}