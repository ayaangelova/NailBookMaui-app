using System.Collections.ObjectModel;
using System.Windows.Input;
using NailBookMaui.Models;
using NailBookMaui.Services;

namespace NailBookMaui.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private string _loginEmail = string.Empty;
    public string LoginEmail
    {
        get => _loginEmail;
        set => SetProperty(ref _loginEmail, value);
    }

    private string _loginPassword = string.Empty;
    public string LoginPassword
    {
        get => _loginPassword;
        set => SetProperty(ref _loginPassword, value);
    }

    private string _savedUserPassword = string.Empty;
    public string SavedUserPassword
    {
        get => _savedUserPassword;
        set => SetProperty(ref _savedUserPassword, value);
    }

    private string _fullName = string.Empty;
    public string FullName
    {
        get => _fullName;
        set => SetProperty(ref _fullName, value);
    }

    private string _phoneNumber = string.Empty;
    public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetProperty(ref _phoneNumber, value);
    }

    private string _registerEmail = string.Empty;
    public string RegisterEmail
    {
        get => _registerEmail;
        set => SetProperty(ref _registerEmail, value);
    }

    private string _registerPassword = string.Empty;
    public string RegisterPassword
    {
        get => _registerPassword;
        set => SetProperty(ref _registerPassword, value);
    }

    private bool _isRegistrationVisible;
    public bool IsRegistrationVisible
    {
        get => _isRegistrationVisible;
        set => SetProperty(ref _isRegistrationVisible, value);
    }

    private User? _selectedSavedUser;
    public User? SelectedSavedUser
    {
        get => _selectedSavedUser;
        set
        {
            if (SetProperty(ref _selectedSavedUser, value))
            {
                OnPropertyChanged(nameof(IsSavedUserSelected));
                OnPropertyChanged(nameof(SelectedSavedUserText));
            }
        }
    }

    public bool IsSavedUserSelected => SelectedSavedUser != null;

    public string SelectedSavedUserText =>
        SelectedSavedUser == null
            ? string.Empty
            : $"Влизане като: {SelectedSavedUser.FullName}";

    public ObservableCollection<User> SavedUsers { get; } = new();

    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }
    public ICommand ToggleRegistrationCommand { get; }
    public ICommand SelectSavedUserCommand { get; }
    public ICommand LoginSavedUserCommand { get; }

    public LoginViewModel()
    {
        Title = "Вход";

        LoginCommand = new AsyncCommand(LoginAsync);
        RegisterCommand = new AsyncCommand(RegisterAsync);
        ToggleRegistrationCommand = new Command(() => IsRegistrationVisible = !IsRegistrationVisible);

        SelectSavedUserCommand = new Command<User>(SelectSavedUser);
        LoginSavedUserCommand = new AsyncCommand(LoginSavedUserAsync);

        LoadSavedUsers();
    }

    private void SelectSavedUser(User user)
    {
        if (user == null)
            return;

        SelectedSavedUser = user;
        SavedUserPassword = string.Empty;
    }

    private void LoadSavedUsers()
    {
        SavedUsers.Clear();

        foreach (User user in AppData.Users.GetRegisteredUsers())
            SavedUsers.Add(user);
    }

    private async Task LoginAsync()
    {
        try
        {
            User user = AppData.Users.Login(LoginEmail, LoginPassword);
            SelectPreferredSalon(user);
            OpenMainApp();
        }
        catch (Exception ex)
        {
            await AppData.Notifications.ShowErrorAsync(ex.Message);
        }
    }

    private async Task LoginSavedUserAsync()
    {
        try
        {
            if (SelectedSavedUser == null)
            {
                await AppData.Notifications.ShowErrorAsync("Моля, изберете профил.");
                return;
            }

            User user = AppData.Users.Login(SelectedSavedUser.Email, SavedUserPassword);
            SelectPreferredSalon(user);
            OpenMainApp();
        }
        catch (Exception ex)
        {
            await AppData.Notifications.ShowErrorAsync(ex.Message);
        }
    }

    private async Task RegisterAsync()
    {
        try
        {
            User user = AppData.Users.RegisterUser(FullName, PhoneNumber, RegisterEmail, RegisterPassword);

            await AppData.Notifications.ShowSuccessAsync($"Добре дошли, {user.FullName}!");

            LoadSavedUsers();

            SelectPreferredSalon(user);
            OpenMainApp();
        }
        catch (Exception ex)
        {
            await AppData.Notifications.ShowErrorAsync(ex.Message);
        }
    }

    private static void SelectPreferredSalon(User user)
    {
        Salon? preferredSalon = AppData.Locations.GetSalonById(user.PreferredSalonId);

        if (preferredSalon != null)
            AppData.Locations.SelectSalon(preferredSalon);
        else
            AppData.Locations.ClearSelectedSalon();
    }

    private static void OpenMainApp()
    {
        Application.Current!.MainPage = new AppShell();
    }
}
