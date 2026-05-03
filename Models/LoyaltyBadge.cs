namespace NailBookMaui.Models;

public class LoyaltyBadge
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int RequiredPoints { get; set; }
    public bool IsUnlocked { get; set; }
    

    public void CheckUnlock(int userPoints)
    {
        IsUnlocked = userPoints >= RequiredPoints;
    }
}
