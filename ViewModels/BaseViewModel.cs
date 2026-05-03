using System.ComponentModel;
using System.Runtime.CompilerServices;
using NailBookMaui.Services;

namespace NailBookMaui.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    private string _title = string.Empty;
    private bool _isBusy;

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    
    public string FooterSalonAddress => "email: salonNonStop@gmail.com";
    public string FooterSalonPhone => "Phone: 0882540765";

    public string FooterSalonLocation => string.Empty;

    public void RefreshFooter()
    {
        
        OnPropertyChanged(nameof(FooterSalonAddress));
        OnPropertyChanged(nameof(FooterSalonPhone));
        OnPropertyChanged(nameof(FooterSalonLocation));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value)) return false;
        backingStore = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
