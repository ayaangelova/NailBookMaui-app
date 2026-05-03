namespace NailBookMaui.Services;

public class NotificationService
{
    public async Task ShowSuccessAsync(string message)
    {
        if (Application.Current?.MainPage != null)
            await Application.Current.MainPage.DisplayAlert("Успешно", message, "OK");
    }

    public async Task ShowErrorAsync(string message)
    {
        if (Application.Current?.MainPage != null)
            await Application.Current.MainPage.DisplayAlert("Грешка", message, "OK");
    }

    public async Task ShowReminderAsync(string serviceName, DateTime appointmentDate)
    {
        if (Application.Current?.MainPage != null)
            await Application.Current.MainPage.DisplayAlert("Напомняне", $"Имате запазен час за {serviceName} на {appointmentDate:dd.MM.yyyy HH:mm}.", "OK");
    }
}
