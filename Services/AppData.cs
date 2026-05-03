namespace NailBookMaui.Services;

public static class AppData
{
    public static BeautyServiceService BeautyServices { get; } = new();
    public static AppointmentService Appointments { get; } = new();
    public static UserService Users { get; } = new();
    public static LocationService Locations { get; } = new();
    public static NotificationService Notifications { get; } = new();
    public static ContentService Content { get; } = new();
}
