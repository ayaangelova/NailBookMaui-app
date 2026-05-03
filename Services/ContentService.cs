using NailBookMaui.Models;

namespace NailBookMaui.Services;

public class ContentService
{
    private readonly List<Promotion> _promotions = new()
    {
        new Promotion
        {
            Id = 1,
            Title = "Пакет гел лак + декорация",
            Description = "Подходящо предложение за потребители, които искат завършена визия с акцентен нокът.",
            DiscountText = "-10%",
            ValidUntil = "валидно до края на месеца"
        },
        new Promotion
        {
            Id = 2,
            Title = "Първа резервация",
            Description = "Новите потребители получават бонус точки при първо успешно записване.",
            DiscountText = "+20 точки",
            ValidUntil = "еднократно"
        },
        new Promotion
        {
            Id = 3,
            Title = "Сезонен френски маникюр",
            Description = "Нежна визия за официални поводи с възможност за минималистична декорация.",
            DiscountText = "популярно",
            ValidUntil = "тази седмица"
        }
    };

    private readonly List<NailDesign> _designs = new()
    {
        new NailDesign
        {
            Id = 1,
            Name = "Milky French",
            Style = "Елегантен",
            Description = "Млечна основа с тънка френска линия. Подходящ за ежедневие и официални поводи.",
            Difficulty = "Средна"
        },
        new NailDesign
        {
            Id = 2,
            Name = "Chrome Pearl",
            Style = "Модерен",
            Description = "Перлен хром ефект върху светла основа. Изглежда чисто, луксозно и актуално.",
            Difficulty = "Висока"
        },
        new NailDesign
        {
            Id = 3,
            Name = "Soft Pink Minimal",
            Style = "Минималистичен",
            Description = "Нежно розово покритие с малък декоративен елемент на един или два нокътя.",
            Difficulty = "Ниска"
        },
        new NailDesign
        {
            Id = 4,
            Name = "Red Classic",
            Style = "Класически",
            Description = "Класически червен маникюр с гланцов завършек. Подходящ за силна и изчистена визия.",
            Difficulty = "Ниска"
        }
    };

    private readonly List<BeautyTip> _tips = new()
    {
        new BeautyTip { Id = 1, Title = "Поддръжка", Text = "Използвайте масло за кожички, за да запазите маникюра свеж по-дълго." },
        new BeautyTip { Id = 2, Title = "Преди час", Text = "При качване на снимка на желания дизайн специалистът по-лесно подготвя цветовете и материалите." },
        new BeautyTip { Id = 3, Title = "След процедура", Text = "Избягвайте агресивни препарати без ръкавици, за да предпазите покритието." },
        new BeautyTip { Id = 4, Title = "Избор на дизайн", Text = "За ежедневна визия най-практични са неутрални цветове, френски стил или минимална декорация." }
    };

    public List<Promotion> GetPromotions() => _promotions.ToList();
    public List<NailDesign> GetDesigns() => _designs.ToList();
    public List<BeautyTip> GetTips() => _tips.ToList();

    public BeautyTip GetTipOfTheDay()
    {
        int index = DateTime.Today.DayOfYear % _tips.Count;
        return _tips[index];
    }
}
