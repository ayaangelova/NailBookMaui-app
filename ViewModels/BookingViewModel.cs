using Microsoft.Maui.Devices.Sensors;
using NailBookMaui.Models;
using NailBookMaui.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NailBookMaui.ViewModels;

public class BookingViewModel : BaseViewModel
{
    public ObservableCollection<BeautyService> Services { get; } = new();
    public ObservableCollection<Salon> Salons { get; } = new();

    private BeautyService? _selectedService;
    public BeautyService? SelectedService
    {
        get => _selectedService;
        set
        {
            if (SetProperty(ref _selectedService, value))
                OnPropertyChanged(nameof(SelectedServiceText));
        }
    }

    private Salon? _selectedSalon;
    public Salon? SelectedSalon
    {
        get => _selectedSalon;
        set
        {
            if (SetProperty(ref _selectedSalon, value))
            {
                OnPropertyChanged(nameof(SelectedSalonText));
                LoadServicesForSelectedSalon();
                UpdatePromotionInfo();
            }
        }
    }

    private DateTime _selectedDate = DateTime.Today.AddDays(1);
    public DateTime SelectedDate
    {
        get => _selectedDate;
        set => SetProperty(ref _selectedDate, value);
    }

    private TimeSpan _selectedTime = new(10, 0, 0);
    public TimeSpan SelectedTime
    {
        get => _selectedTime;
        set => SetProperty(ref _selectedTime, value);
    }

    private string _userNote = string.Empty;
    public string UserNote
    {
        get => _userNote;
        set => SetProperty(ref _userNote, value);
    }

    private string _designImagePath = string.Empty;
    public string DesignImagePath
    {
        get => _designImagePath;
        set => SetProperty(ref _designImagePath, value);
    }

    private string _loginWarning = string.Empty;
    public string LoginWarning
    {
        get => _loginWarning;
        set => SetProperty(ref _loginWarning, value);
    }

    private string _promotionInfo = string.Empty;
    public string PromotionInfo
    {
        get => _promotionInfo;
        set => SetProperty(ref _promotionInfo, value);
    }

    public string SelectedServiceText => SelectedService == null
        ? "Няма избрана услуга. Първо изберете салон, после услуга от неговия ценови лист."
        : $"✓ Избрана услуга: {SelectedService.Name} - {SelectedService.Price:F2} евро / {SelectedService.DurationMinutes} мин. ({SelectedService.SalonName})";

    public string SelectedSalonText => SelectedSalon == null
        ? "Няма избран салон за тази резервация."
        : $"✓ Салон към резервацията: {SelectedSalon.Name} - {SelectedSalon.Address}";

    public ICommand BookAppointmentCommand { get; }
    public ICommand PickImageCommand { get; }

    public ICommand GoToRegistrationCommand { get; }
    

    public BookingViewModel()
    {
        Title = "Запази час";

        BookAppointmentCommand = new AsyncCommand(BookAppointmentAsync);
        PickImageCommand = new AsyncCommand(PickImageAsync);

        GoToRegistrationCommand = new AsyncCommand(GoToRegistrationAsync);
        

        AppData.Users.CurrentUserChanged += UpdateLoginWarning;
        AppData.Locations.SelectedSalonChanged += RefreshSelectedSalonFromActive;

        LoadSalons();
        UpdateLoginWarning();
        UpdatePromotionInfo();
    }

    private void LoadSalons()
    {
        Salons.Clear();

        foreach (Salon salon in AppData.Locations.GetNearbySalons())
            Salons.Add(salon);

        RefreshSelectedSalonFromActive();
    }

    private void LoadServicesForSelectedSalon()
    {
        Services.Clear();
        SelectedService = null;

        if (SelectedSalon == null)
        {
            OnPropertyChanged(nameof(SelectedServiceText));
            return;
        }

        foreach (BeautyService service in AppData.BeautyServices.GetServicesBySalonId(SelectedSalon.Id))
            Services.Add(service);

        OnPropertyChanged(nameof(SelectedServiceText));
    }

    private void RefreshSelectedSalonFromActive()
    {
        SelectedSalon = AppData.Locations.SelectedSalon;
    }

    private void UpdateLoginWarning()
    {
        LoginWarning = AppData.Users.HasCurrentUser
            ? "Имате активен профил. Изберете салон, услуга, дата и час."
            : "За да запазите час, първо трябва да се регистрирате или да изберете запазен профил.";
    }

    private void UpdatePromotionInfo()
    {
        if (SelectedSalon == null)
        {
            PromotionInfo = "Изберете салон, за да видите активните промоции.";
            return;
        }

        List<BeautyService> allServices = AppData.BeautyServices.GetAllServices();

        PromotionInfo = AppData.Content.GetPromotionPriceText(
            SelectedSalon.Id,
            allServices);
    }

    private async Task GoToRegistrationAsync()
    {
        await Shell.Current.GoToAsync("//registration");
    }

    private async Task BookAppointmentAsync()
    {
        try
        {
            if (!AppData.Users.HasCurrentUser)
            {
                await AppData.Notifications.ShowErrorAsync(
                    "Не може да запазите час без регистрация. Моля, отворете секция „Регистрация“.");

                await Shell.Current.GoToAsync("//registration");
                return;
            }

            if (SelectedSalon == null)
            {
                await AppData.Notifications.ShowErrorAsync("Моля, изберете салон.");
                return;
            }

            if (SelectedService == null)
            {
                await AppData.Notifications.ShowErrorAsync("Моля, изберете услуга.");
                return;
            }

            DateTime appointmentDateTime = SelectedDate.Date + SelectedTime;

            User user = AppData.Users.GetCurrentUser();

            Appointment appointment = AppData.Appointments.CreateAppointment(
                user.Id,
                SelectedService,
                SelectedSalon,
                appointmentDateTime,
                UserNote,
                DesignImagePath);

            bool isFirstBooking = !AppData.Appointments
                .GetAppointmentsByUserId(user.Id)
                .Any(a => a.Id != appointment.Id);

            int points = isFirstBooking
                ? AppData.Content.GetFirstBookingBonusPoints()
                : 10;

            AppData.Users.AddLoyaltyPoints(points);
            AppData.Locations.SelectSalon(SelectedSalon);

            await AppData.Notifications.ShowSuccessAsync(
                $"Вашият час беше запазен в {SelectedSalon.Name}. Получавате {points} точки за лоялност.");

            await AppData.Notifications.ShowReminderAsync(
                SelectedService.Name,
                appointment.AppointmentDate);

            UserNote = string.Empty;
            DesignImagePath = string.Empty;

            await Shell.Current.GoToAsync("//appointments");
        }
        catch (Exception ex)
        {
            await AppData.Notifications.ShowErrorAsync(ex.Message);
        }
    }

    private async Task PickImageAsync()
    {
        try
        {
            FileResult? photo = await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Изберете снимка на желан дизайн"
            });

            await SavePhotoAsync(photo);
        }
        catch
        {
            await AppData.Notifications.ShowErrorAsync(
                "Неуспешно избиране на снимка. Проверете разрешенията за достъп до галерия.");
        }
    }

   

    private async Task SavePhotoAsync(FileResult? photo)
    {
        if (photo == null)
            return;

        string extension = Path.GetExtension(photo.FileName);

        if (string.IsNullOrWhiteSpace(extension))
            extension = ".jpg";

        string localFileName = $"nail_design_{DateTime.Now:yyyyMMdd_HHmmss}{extension}";
        string localPath = Path.Combine(FileSystem.AppDataDirectory, localFileName);

        await using Stream sourceStream = await photo.OpenReadAsync();
        await using FileStream localFileStream = File.OpenWrite(localPath);

        await sourceStream.CopyToAsync(localFileStream);

        DesignImagePath = localPath;

        await AppData.Notifications.ShowSuccessAsync(
            "Снимката беше добавена към резервацията.");
    }

    
}