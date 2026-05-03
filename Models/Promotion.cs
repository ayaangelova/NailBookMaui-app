namespace NailBookMaui.Models;

public class Promotion
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DiscountText { get; set; } = string.Empty;
    public string ValidUntil { get; set; } = string.Empty;

    public int BonusPoints { get; set; }
    public decimal DiscountPercent { get; set; }

    public List<string> IncludedCategories { get; set; } = new();

    public DateTime? ValidUntilDate { get; set; }

    public bool IsActive
    {
        get
        {
            if (ValidUntilDate == null)
                return true;

            return DateTime.Today <= ValidUntilDate.Value.Date;
        }
    }

    public string StatusText => IsActive ? "Активна" : "Неактивна";
}