using System.Collections.ObjectModel;
using NailBookMaui.Models;
using NailBookMaui.Services;

namespace NailBookMaui.ViewModels;

public class ExploreViewModel : BaseViewModel
{
    public ObservableCollection<Promotion> Promotions { get; } = new();
    public ObservableCollection<NailDesign> Designs { get; } = new();
    public ObservableCollection<BeautyTip> Tips { get; } = new();

    public ExploreViewModel()
    {
        Title = "Идеи";
        LoadContent();
    }

    public void LoadContent()
    {
        RefreshFooter();
        Promotions.Clear();
        Designs.Clear();
        Tips.Clear();

        foreach (var promotion in AppData.Content.GetPromotions())
            Promotions.Add(promotion);

        foreach (var design in AppData.Content.GetDesigns())
            Designs.Add(design);

    }
}
