using System.Collections.ObjectModel;
using NailBookMaui.Models;
using NailBookMaui.Services;

namespace NailBookMaui.ViewModels;

public class ServicesViewModel : BaseViewModel
{
    public ObservableCollection<BeautyService> Services { get; } = new();

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value)) SearchServices();
        }
    }

    public ServicesViewModel()
    {
        Title = "Услуги";
        LoadServices();
        
    }

    public void LoadServices()
    {
        Services.Clear();
        foreach (var service in AppData.BeautyServices.GetAllServices())
            Services.Add(service);
    }

    private void SearchServices()
    {
        Services.Clear();
        foreach (var service in AppData.BeautyServices.SearchServices(SearchText))
            Services.Add(service);
    }
}
