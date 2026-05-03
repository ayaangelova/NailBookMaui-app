using System.Collections.ObjectModel;
using NailBookMaui.Models;
using NailBookMaui.Services;

namespace NailBookMaui.ViewModels;

public class HomeViewModel : BaseViewModel
{
    public ObservableCollection<BeautyService> PopularServices { get; } = new();

    private string _welcomeMessage = string.Empty;
    public string WelcomeMessage
    {
        get => _welcomeMessage;
        set => SetProperty(ref _welcomeMessage, value);
    }

    private int _loyaltyPoints;
    public int LoyaltyPoints
    {
        get => _loyaltyPoints;
        set => SetProperty(ref _loyaltyPoints, value);
    }

    private string _tipTitle = string.Empty;
    public string TipTitle
    {
        get => _tipTitle;
        set => SetProperty(ref _tipTitle, value);
    }

    private string _tipText = string.Empty;
    public string TipText
    {
        get => _tipText;
        set => SetProperty(ref _tipText, value);
    }

    private string _nextAppointmentText = "Нямате предстоящ записан час.";
    public string NextAppointmentText
    {
        get => _nextAppointmentText;
        set => SetProperty(ref _nextAppointmentText, value);
    }

    public HomeViewModel()
    {
        Title = "Начало";
        AppData.Users.CurrentUserChanged += LoadData;
        LoadData();
    }

    public void LoadData()
    {
        RefreshFooter();

        PopularServices.Clear();
        foreach (var service in AppData.BeautyServices.GetPopularServices())
            PopularServices.Add(service);

        var tip = AppData.Content.GetTipOfTheDay();
        TipTitle = tip.Title;
        TipText = tip.Text;

        if (!AppData.Users.HasCurrentUser)
        {
            WelcomeMessage = "Добре дошли! Моля, регистрирайте профил, за да запазите час.";
            LoyaltyPoints = 0;
            NextAppointmentText = "След регистрация ще виждате тук следващия си час.";
            return;
        }

        var user = AppData.Users.GetCurrentUser();
        WelcomeMessage = $"Здравей, {user.FullName}!";
        LoyaltyPoints = user.LoyaltyPoints;

        var nextAppointment = AppData.Appointments
            .GetAppointmentsByUserId(user.Id)
            .Where(a => a.AppointmentDate > DateTime.Now)
            .OrderBy(a => a.AppointmentDate)
            .FirstOrDefault();

        NextAppointmentText = nextAppointment == null
            ? "Нямате предстоящ записан час."
            : $"Следващ час: {nextAppointment.Service.Name} на {nextAppointment.DisplayDate}";
    }
}
