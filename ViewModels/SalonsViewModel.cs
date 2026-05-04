using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Devices.Sensors;
using NailBookMaui.Models;
using NailBookMaui.Services;

namespace NailBookMaui.ViewModels;

public class SalonsViewModel : BaseViewModel
{
    public ObservableCollection<Salon> Salons { get; } = new();

    private Salon? _selectedSalon;
    public Salon? SelectedSalon
    {
        get => _selectedSalon;
        set
        {
            SetProperty(ref _selectedSalon, value);

        }
    }

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            SearchSalons();
        }
    }

    private string _locationStatus = "Натиснете бутона, за да използвате местоположение.";
    public string LocationStatus
    {
        get => _locationStatus;
        set => SetProperty(ref _locationStatus, value);
    }


    private string _activeSalonText = string.Empty;
    public string ActiveSalonText
    {
        get => _activeSalonText;
        set => SetProperty(ref _activeSalonText, value);
    }

    private Location? _currentLocation;

    public ICommand LoadLocationCommand { get; }
    public ICommand SelectSalonCommand { get; }
    
    public ICommand RefreshSalonsCommand { get; }
    public ICommand ChooseSalonCommand { get; }
    public ICommand ActivateSalonCommand { get; }

    public SalonsViewModel()
    {
        Title = "Близки салони";

        LoadLocationCommand = new AsyncCommand(LoadLocationAsync);
        SelectSalonCommand = new AsyncCommand(SelectSalonAsync);
        
        RefreshSalonsCommand = new Command(LoadSalons);
        ChooseSalonCommand = new Command<Salon>(ChooseSalon);
        ActivateSalonCommand = new Command<Salon>(async salon => await ActivateSalonAsync(salon));

        LoadSalons();
    }

    private void LoadSalons()
    {
        SearchText = string.Empty;
        Salons.Clear();

        foreach (var salon in AppData.Locations.GetNearbySalons())
        {
            salon.ShowDistance = false;
            salon.DistanceInKm = 0;
            Salons.Add(salon);
        }

        SelectedSalon = AppData.Locations.SelectedSalon;
        RefreshActiveSalonText();
       
    }

    private void SearchSalons()
    {
        Salons.Clear();

        foreach (var salon in AppData.Locations.SearchSalons(SearchText))
        {
            if (_currentLocation != null)
            {
                salon.DistanceInKm = Location.CalculateDistance(
                    _currentLocation.Latitude,
                    _currentLocation.Longitude,
                    salon.Latitude,
                    salon.Longitude,
                    DistanceUnits.Kilometers);

                salon.ShowDistance = true;
            }
            else
            {
                salon.ShowDistance = false;
                salon.DistanceInKm = 0;
            }

            Salons.Add(salon);
        }
    }

    private void ChooseSalon(Salon? salon)
    {
        if (salon == null)
            return;

        SelectedSalon = salon;
    }

    private async Task ActivateSalonAsync(Salon? salon)
    {
        if (salon == null)
        {
            await AppData.Notifications.ShowErrorAsync("Моля, изберете салон от списъка.");
            return;
        }

        SelectedSalon = salon;
        AppData.Locations.SelectSalon(salon);

        if (AppData.Users.HasCurrentUser)
        {
            User user = AppData.Users.GetCurrentUser();
            user.PreferredSalonId = salon.Id;
            AppData.Users.UpdateUser(user);
        }

        RefreshActiveSalonText();
       


        await AppData.Notifications.ShowSuccessAsync($"{salon.Name} е избран като салон по подразбиране.");
    }


    private void RefreshActiveSalonText()
    {
        Salon? active = AppData.Locations.SelectedSalon;

        ActiveSalonText = active == null
            ? "Няма избран салон по подразбиране."
            : $"✓ Салон по подразбиране: {active.Name}\n{active.Address}\nТелефон: {active.PhoneNumber}";
    }

    private async Task LoadLocationAsync()
    {
        LocationStatus = "Зареждане на местоположение...";

        Location? location = await AppData.Locations.GetCurrentLocationAsync();

        if (location == null)
        {
            LocationStatus = "Не беше получено местоположение.";
            return;
        }

        _currentLocation = location;

        LocationStatus = $"Текущо местоположение: {location.Latitude:F4}, {location.Longitude:F4}";

        Salons.Clear();

        foreach (var salon in AppData.Locations.GetNearbySalons())
        {
            salon.DistanceInKm = Location.CalculateDistance(
                location.Latitude,
                location.Longitude,
                salon.Latitude,
                salon.Longitude,
                DistanceUnits.Kilometers);

            salon.ShowDistance = true;
            Salons.Add(salon);
        }
    }

    private async Task SelectSalonAsync()
    {
        if (SelectedSalon == null)
        {
            await AppData.Notifications.ShowErrorAsync("Моля, изберете салон от списъка.");
            return;
        }

        await ActivateSalonAsync(SelectedSalon);
    }

    
}