using NailBookMaui.Models;

namespace NailBookMaui.Services;

public class LocationService
{
    private readonly List<Salon> _salons = new()
    {
        new Salon { Id = 1, Name = "NailBook Studio Vitosha", Address = "гр. София, бул. Витоша 25", PhoneNumber = "0888 123 456", Latitude = 42.6940, Longitude = 23.3210, DistanceInKm = 0, WorkingHours = "Пон–Съб: 09:00–19:00", Description = "Централен салон, подходящ за бързи резервации след работа." },
        new Salon { Id = 2, Name = "NailBook Ovcha Kupel", Address = "гр. София, кв. Овча купел, ул. Монтевидео 21", PhoneNumber = "0888 555 666", Latitude = 42.6830, Longitude = 23.2550, DistanceInKm = 0, WorkingHours = "Пон–Нед: 10:00–20:00", Description = "Удобен салон за потребители от Овча купел и западна София." },
        new Salon { Id = 3, Name = "NailBook Bulgaria Mall", Address = "гр. София, бул. България 69", PhoneNumber = "0888 777 666", Latitude = 42.6649, Longitude = 23.2887, DistanceInKm = 0, WorkingHours = "Всеки ден: 10:00–21:00", Description = "Локация до търговски център с удобен достъп и паркинг." },
        new Salon { Id = 4, Name = "NailBook Paradise Center", Address = "гр. София, бул. Черни връх 100", PhoneNumber = "0888 222 333", Latitude = 42.6585, Longitude = 23.3152, DistanceInKm = 0, WorkingHours = "Всеки ден: 10:00–21:00", Description = "Подходящ салон за резервации около района на Хладилника и Лозенец." },
        new Salon { Id = 5, Name = "NailBook Mladost", Address = "гр. София, ж.к. Младост 1, бул. Йерусалим 12", PhoneNumber = "0888 444 111", Latitude = 42.6516, Longitude = 23.3797, DistanceInKm = 0, WorkingHours = "Пон–Съб: 09:30–19:30", Description = "Салон за потребители от Младост, Дружба и района около Бизнес парка." }
    };

    public Salon? SelectedSalon { get; private set; }
    public event Action? SelectedSalonChanged;

    public async Task<Location?> GetCurrentLocationAsync()
    {
        try
        {
            Location? location = await Geolocation.Default.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Best,
                Timeout = TimeSpan.FromSeconds(10)
            });

            return location;
        }
        catch
        {
            return null;
        }
    }

    public List<Salon> GetNearbySalons() => _salons.OrderBy(s => s.DistanceInKm).ToList();

    public Salon? GetSalonById(int salonId) => _salons.FirstOrDefault(s => s.Id == salonId);

    public List<Salon> SearchSalons(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return GetNearbySalons();

        string normalized = keyword.Trim().ToLowerInvariant();

        return _salons
            .Where(s => s.Name.ToLowerInvariant().Contains(normalized)
                     || s.Address.ToLowerInvariant().Contains(normalized)
                     || s.Description.ToLowerInvariant().Contains(normalized)
                     || s.WorkingHours.ToLowerInvariant().Contains(normalized))
            .OrderBy(s => s.DistanceInKm)
            .ToList();
    }

    public void SelectSalon(Salon salon)
    {
        SelectedSalon = salon;
        SelectedSalonChanged?.Invoke();
    }

    public void ClearSelectedSalon()
    {
        SelectedSalon = null;
        SelectedSalonChanged?.Invoke();
    }
}