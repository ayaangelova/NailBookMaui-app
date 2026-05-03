namespace NailBookMaui.Models;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int LoyaltyPoints { get; set; }
    public string ProfileImagePath { get; set; } = string.Empty;

    public int PreferredSalonId { get; set; }
    public string PreferredSalonName { get; set; } = string.Empty;
    public string PreferredSalonAddress { get; set; } = string.Empty;
    public string PreferredSalonPhone { get; set; } = string.Empty;

    public string PreferredSalonText => PreferredSalonId <= 0
        ? "Няма избран салон."
        : $"{PreferredSalonName} - {PreferredSalonAddress}";

    public void AddLoyaltyPoints(int points)
    {
        if (points > 0)
            LoyaltyPoints += points;
    }
}
