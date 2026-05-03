namespace NailBookMaui.Models;

public class BeautyService
{
    public int Id { get; set; }
    public int SalonId { get; set; }
    public string SalonName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
    
    public bool IsPopular { get; set; }

    public string FormattedPrice => $"{Price:F2} euro";
    public string FormattedDuration => $"{DurationMinutes} мин.";
    public string SalonDisplayText => string.IsNullOrWhiteSpace(SalonName) ? "Без посочен салон" : SalonName;
    public string PickerDisplayText => $"{Name} - {Price:F2} euro ({DurationMinutes} мин.)";

}
