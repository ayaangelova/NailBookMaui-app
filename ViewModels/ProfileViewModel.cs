using System.Collections.ObjectModel;
using System.Windows.Input;
using NailBookMaui.Models;
using NailBookMaui.Services;
using NailBookMaui.Views;

namespace NailBookMaui.ViewModels;

public class ProfileViewModel : BaseViewModel
{
    private User _currentUser = new();
    public User CurrentUser
    {
        get => _currentUser;
        set => SetProperty(ref _currentUser, value);
    }

    private string _profileStatus = string.Empty;
    public string ProfileStatus
    {
        get => _profileStatus;
        set => SetProperty(ref _profileStatus, value);
    }

    private string _userType = "Нов клиент";
    public string UserType
    {
        get => _userType;
        set => SetProperty(ref _userType, value);
    }

    public ObservableCollection<LoyaltyBadge> Badges { get; } = new();
    public ObservableCollection<User> RegisteredUsers { get; } = new();

    public ICommand SaveProfileCommand { get; }
    public ICommand RefreshProfilesCommand { get; }
    public ICommand LogoutCommand { get; }
    public ICommand OpenSalonsCommand { get; }

    public ProfileViewModel()
    {
        Title = "Профил";

        SaveProfileCommand = new AsyncCommand(SaveProfileAsync);
        RefreshProfilesCommand = new Command(LoadProfile);
        LogoutCommand = new AsyncCommand(LogoutAsync);
        OpenSalonsCommand = new AsyncCommand(OpenSalonsAsync);

        AppData.Users.CurrentUserChanged += LoadProfile;
        LoadProfile();
    }

    public void LoadProfile()
    {
        if (!AppData.Users.HasCurrentUser)
        {
            CurrentUser = new User();
            ProfileStatus = "Няма активен профил. Секцията „Профил“ се показва след регистрация.";
        }
        else
        {
            CurrentUser = AppData.Users.GetCurrentUser();
            ProfileStatus = $"Активен профил: {CurrentUser.FullName}";
        }

        LoadBadges();
        LoadRegisteredUsers();
        UpdateUserType();
        RefreshFooter();
    }

    private void UpdateUserType()
    {
        int points = CurrentUser.LoyaltyPoints;

        UserType = points switch
        {
            >= 100 => "VIP клиент",
            >= 50 => "Лоялен клиент",
            >= 20 => "Редовен клиент",
            _ => "Нов клиент"
        };
    }

    private void LoadBadges()
    {
        Badges.Clear();
        foreach (var badge in AppData.Users.GetUserBadges())
            Badges.Add(badge);
    }

    private void LoadRegisteredUsers()
    {
        RegisteredUsers.Clear();
        foreach (var user in AppData.Users.GetRegisteredUsers())
            RegisteredUsers.Add(user);
    }

    private async Task SaveProfileAsync()
    {
        try
        {
            AppData.Users.UpdateUser(CurrentUser);
            await AppData.Notifications.ShowSuccessAsync("Профилът е запазен успешно.");
            LoadProfile();
        }
        catch (Exception ex)
        {
            await AppData.Notifications.ShowErrorAsync(ex.Message);
        }
    }

    private async Task LogoutAsync()
    {
        AppData.Users.Logout();
        await AppData.Notifications.ShowSuccessAsync("Излязохте от активния профил. За нова резервация изберете или регистрирайте профил.");
        Application.Current!.MainPage = new LoginPage();
    }

    private async Task OpenSalonsAsync()
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(SalonsPage));
        }
        catch
        {
            await AppData.Notifications.ShowErrorAsync("Не може да се отвори страницата със салони.");
        }
    }
}