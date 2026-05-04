namespace NailBookMaui.Models;

public class Salon
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string WorkingHours { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public double DistanceInKm { get; set; }

    public bool ShowDistance { get; set; }

    public string FormattedDistance =>
        ShowDistance ? $"{DistanceInKm:F1} км" : string.Empty;

    public string CoordinatesText =>
        $"{Latitude:F4}, {Longitude:F4}";

   
}