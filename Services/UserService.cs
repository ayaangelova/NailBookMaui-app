using System.Text.Json;
using Microsoft.Maui.Storage;
using NailBookMaui.Models;

namespace NailBookMaui.Services;

public class UserService
{
    private readonly List<User> _registeredUsers = new();
    private User? _currentUser;
    private readonly string _filePath;

    public event Action? CurrentUserChanged;

    public UserService()
    {
        _filePath = Path.Combine(FileSystem.AppDataDirectory, "registered-users.json");
        LoadUsersFromFile();
    }

    public bool HasCurrentUser => _currentUser != null;
    public User? CurrentUserOrNull => _currentUser;

    public User GetCurrentUser()
    {
        if (_currentUser == null)
            throw new InvalidOperationException("Първо трябва да влезете в профила си.");

        return _currentUser;
    }
    public void SetCurrentUser(int userId)
    {
        User? user = _registeredUsers.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            throw new InvalidOperationException("Профилът не е намерен.");

        _currentUser = user;
        CurrentUserChanged?.Invoke();
    }

    public List<User> GetRegisteredUsers()
    {
        return _registeredUsers.OrderBy(u => u.Id).ToList();
    }

    public User RegisterUser(string fullName, string phoneNumber, string email, string password)
    {
        ValidateRegistrationData(fullName, phoneNumber, email, password);

        fullName = fullName.Trim();
        phoneNumber = phoneNumber.Trim();
        email = email.Trim().ToLower();

        bool emailExists = _registeredUsers.Any(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        if (emailExists)
            throw new InvalidOperationException("Вече има регистриран профил с този имейл.");

        int nextId = _registeredUsers.Count == 0
            ? 1
            : _registeredUsers.Max(u => u.Id) + 1;

        User user = new()
        {
            Id = nextId,
            FullName = fullName,
            PhoneNumber = phoneNumber,
            Email = email,
            Password = password,
            LoyaltyPoints = 0
        };

        _registeredUsers.Add(user);
        _currentUser = user;

        SaveUsersToFile();
        CurrentUserChanged?.Invoke();

        return user;
    }

    public User Login(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new InvalidOperationException("Въведете имейл.");

        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException("Въведете парола.");

        email = email.Trim().ToLower();

        User? user = _registeredUsers.FirstOrDefault(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        if (user == null || user.Password != password)
            throw new InvalidOperationException("Грешен имейл или парола.");

        _currentUser = user;
        CurrentUserChanged?.Invoke();

        return user;
    }

    public User LoginSavedUser(User selectedUser, string password)
    {
        if (selectedUser == null)
            throw new InvalidOperationException("Моля, изберете профил.");

        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException("Въведете парола.");

        User? user = _registeredUsers.FirstOrDefault(u => u.Id == selectedUser.Id);

        if (user == null || user.Password != password)
            throw new InvalidOperationException("Грешна парола.");

        _currentUser = user;
        CurrentUserChanged?.Invoke();

        return user;
    }

    public void Logout()
    {
        _currentUser = null;
        CurrentUserChanged?.Invoke();
    }

    public void AddLoyaltyPoints(int points)
    {
        if (_currentUser == null)
            throw new InvalidOperationException("Първо трябва да имате активен профил.");

        if (points <= 0)
            return;

        _currentUser.AddLoyaltyPoints(points);

        CurrentUserChanged?.Invoke();
        SaveUsersToFile();
    }

    public void RemoveLoyaltyPoints(int points)
    {
        if (_currentUser == null)
            throw new InvalidOperationException("Първо трябва да имате активен профил.");

        if (points <= 0)
            return;

        _currentUser.LoyaltyPoints -= points;

        if (_currentUser.LoyaltyPoints < 0)
            _currentUser.LoyaltyPoints = 0;

        CurrentUserChanged?.Invoke();
        SaveUsersToFile();
    }

    public void UpdateUser(User user)
    {
        if (_currentUser == null)
            throw new InvalidOperationException("Няма активен профил за редакция.");

        if (string.IsNullOrWhiteSpace(user.FullName))
            throw new InvalidOperationException("Името не може да бъде празно.");

        if (string.IsNullOrWhiteSpace(user.PhoneNumber))
            throw new InvalidOperationException("Телефонът не може да бъде празен.");

        User? existing = _registeredUsers.FirstOrDefault(u => u.Id == user.Id);

        if (existing == null)
            throw new InvalidOperationException("Профилът не е намерен.");

        existing.FullName = user.FullName.Trim();
        existing.PhoneNumber = user.PhoneNumber.Trim();
        existing.ProfileImagePath = user.ProfileImagePath;
        existing.PreferredSalonId = user.PreferredSalonId;
        existing.PreferredSalonName = user.PreferredSalonName;
        existing.PreferredSalonAddress = user.PreferredSalonAddress;
        existing.PreferredSalonPhone = user.PreferredSalonPhone;

        _currentUser = existing;

        SaveUsersToFile();
        CurrentUserChanged?.Invoke();
    }

    public List<LoyaltyBadge> GetUserBadges()
    {
        int points = _currentUser?.LoyaltyPoints ?? 0;

        List<LoyaltyBadge> badges = new()
        {
            new LoyaltyBadge { Id = 1, Title = "Нов клиент", Description = "Направена първа резервация.", RequiredPoints = 20 },
            new LoyaltyBadge { Id = 2, Title = "Редовен клиент", Description = "Направени няколко успешни резервации.", RequiredPoints = 30 },
            new LoyaltyBadge { Id = 3, Title = "Beauty Lover", Description = "Редовен потребител на приложението.", RequiredPoints = 70 },
            new LoyaltyBadge { Id = 4, Title = "VIP клиент", Description = "Потребител с висока активност.", RequiredPoints = 150 }
        };

        foreach (LoyaltyBadge badge in badges)
            badge.CheckUnlock(points);

        return badges;
    }

    private static void ValidateRegistrationData(string fullName, string phoneNumber, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new InvalidOperationException("Името не може да бъде празно.");

        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new InvalidOperationException("Телефонът не може да бъде празен.");

        phoneNumber = phoneNumber.Trim();

        if (phoneNumber.Length != 10 || !phoneNumber.All(char.IsDigit))
            throw new InvalidOperationException("Телефонният номер трябва да съдържа точно 10 цифри.");

        if (string.IsNullOrWhiteSpace(email))
            throw new InvalidOperationException("Имейлът не може да бъде празен.");

        email = email.Trim().ToLower();

        bool validEmail =
            email.EndsWith("@gmail.com") ||
            email.EndsWith("@tu-sofia.bg") ||
            email.EndsWith("@abv.bg");

        if (!validEmail)
            throw new InvalidOperationException("Имейлът трябва да завършва на @gmail.com, @tu-sofia.bg или @abv.bg.");

        if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
            throw new InvalidOperationException("Паролата трябва да бъде поне 4 символа.");
    }

    private void LoadUsersFromFile()
    {
        try
        {
            if (!File.Exists(_filePath))
                return;

            string json = File.ReadAllText(_filePath);
            List<User>? users = JsonSerializer.Deserialize<List<User>>(json);

            if (users != null)
                _registeredUsers.AddRange(users);
        }
        catch
        {
            _registeredUsers.Clear();
        }
    }

    private void SaveUsersToFile()
    {
        try
        {
            string json = JsonSerializer.Serialize(
                _registeredUsers,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(_filePath, json);
        }
        catch
        {
        }
    }
}