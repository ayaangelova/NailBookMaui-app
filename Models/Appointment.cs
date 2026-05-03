namespace NailBookMaui.Models;

public class Appointment
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public int ServiceId { get; set; }
    public BeautyService Service { get; set; } = new();

    public int SalonId { get; set; }
    public Salon Salon { get; set; } = new();

    public DateTime AppointmentDate { get; set; }

    public string Status { get; set; } = "Потвърден";

    public string UserNote { get; set; } = string.Empty;
    public string DesignImagePath { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string DisplayDate =>
        AppointmentDate.ToString("dd.MM.yyyy HH:mm");

    

    public string DisplayStatus =>
        AppointmentDate < DateTime.Now && Status == "Потвърден"
            ? "Минал"
            : Status;

    
}