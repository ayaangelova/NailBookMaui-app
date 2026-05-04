using NailBookMaui.Models;
using System.Text.Json;

namespace NailBookMaui.Services;

public class AppointmentService
{
    private readonly List<Appointment> _appointments = new();
    private int _nextId = 1;

    private const string FileName = "appointments.json";

    private string FilePath =>
        Path.Combine(FileSystem.AppDataDirectory, FileName);

    public event Action? AppointmentsChanged;

    public AppointmentService()
    {
        LoadAppointments();
    }

    public List<Appointment> GetAllAppointments() =>
        _appointments.OrderBy(a => a.AppointmentDate).ToList();

    public List<Appointment> GetAppointmentsByUserId(int userId) =>
        _appointments
            .Where(a => a.UserId == userId)
            .OrderBy(a => a.AppointmentDate)
            .ToList();

    public bool IsTimeAvailable(DateTime dateTime, int salonId) =>
        !_appointments.Any(a =>
            a.Status == "Потвърден" &&
            a.AppointmentDate == dateTime &&
            a.SalonId == salonId);

    public Appointment CreateAppointment(
        int userId,
        BeautyService service,
        Salon salon,
        DateTime dateTime,
        string note,
        string imagePath)
    {
        if (service.Id <= 0)
            throw new InvalidOperationException("Невалидна услуга.");

        if (salon.Id <= 0)
            throw new InvalidOperationException("Моля, изберете салон към резервацията.");

        if (dateTime <= DateTime.Now)
            throw new InvalidOperationException("Не може да се запази час за минала дата или час.");

        if (!IsTimeAvailable(dateTime, salon.Id))
            throw new InvalidOperationException("Избраният час вече е зает в този салон.");

        if (!IsWithinWorkingHours(dateTime, service.DurationMinutes))
            throw new InvalidOperationException("Избраният час е извън работното време на салона.");

        Appointment appointment = new()
        {
            Id = _nextId++,
            UserId = userId,
            ServiceId = service.Id,
            Service = service,
            SalonId = salon.Id,
            Salon = salon,
            AppointmentDate = dateTime,
            UserNote = note,
            DesignImagePath = imagePath,
            Status = "Потвърден",
            CreatedAt = DateTime.Now
        };

        _appointments.Add(appointment);
        SaveAppointments();
        AppointmentsChanged?.Invoke();

        return appointment;
    }
    private bool IsWithinWorkingHours(DateTime appointmentDateTime, int durationMinutes)
    {
        TimeSpan start = new(9, 0, 0);
        TimeSpan end = new(18, 0, 0);

        TimeSpan appointmentStart = appointmentDateTime.TimeOfDay;
        TimeSpan appointmentEnd = appointmentStart.Add(TimeSpan.FromMinutes(durationMinutes));

        return appointmentStart >= start && appointmentEnd <= end;
    }
    public void CancelAppointment(int appointmentId)
    {
        Appointment? appointment = _appointments.FirstOrDefault(a => a.Id == appointmentId);

        if (appointment == null)
            throw new InvalidOperationException("Резервацията не е намерена.");

        _appointments.Remove(appointment);
        SaveAppointments();
        AppointmentsChanged?.Invoke();
    }

    private void LoadAppointments()
    {
        if (!File.Exists(FilePath))
            return;

        string json = File.ReadAllText(FilePath);

        List<Appointment>? data = JsonSerializer.Deserialize<List<Appointment>>(json);

        if (data == null)
            return;

        _appointments.Clear();
        _appointments.AddRange(data);

        if (_appointments.Any())
            _nextId = _appointments.Max(a => a.Id) + 1;
    }

    private void SaveAppointments()
    {
        string json = JsonSerializer.Serialize(_appointments);
        File.WriteAllText(FilePath, json);
    }
}