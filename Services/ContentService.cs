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
            DiscountPercent = 10,
            IncludedCategories = new List<string> { "Маникюр", "Декорация" },
            ValidUntil = "валидно до края на месеца",
            ValidUntilDate = new DateTime(2026, 5, 31)
        },

        new Promotion
        {
            Id = 2,
            Title = "Първа резервация",
            Description = "Новите потребители получават бонус точки при първо успешно записване.",
            DiscountText = "+20 точки",
            BonusPoints = 20,
            ValidUntil = "еднократно",
            ValidUntilDate = null
        },

        new Promotion
        {
            Id = 3,
            Title = "Сезонен френски маникюр",
            Description = "Нежна визия за официални поводи с възможност за минималистична декорация.",
            DiscountText = "популярно",
            ValidUntil = "тази седмица",
            ValidUntilDate = DateTime.Today.AddDays(7)
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
        new BeautyTip
        {
            Id = 1,
            Title = "Поддръжка",
            Text = "Използвайте масло за кожички, за да запазите маникюра свеж по-дълго."
        },

        new BeautyTip
        {
            Id = 2,
            Title = "Преди час",
            Text = "При качване на снимка на желания дизайн специалистът по-лесно подготвя цветовете и материалите."
        },

        new BeautyTip
        {
            Id = 3,
            Title = "След процедура",
            Text = "Избягвайте агресивни препарати без ръкавици, за да предпазите покритието."
        },

        new BeautyTip
        {
            Id = 4,
            Title = "Избор на дизайн",
            Text = "За ежедневна визия най-практични са неутрални цветове, френски стил или минимална декорация."
        }
    };

    public List<Promotion> GetPromotions()
    {
        return _promotions.ToList();
    }

    public List<Promotion> GetActivePromotions()
    {
        return _promotions
            .Where(p => p.IsActive)
            .ToList();
    }

    public List<NailDesign> GetDesigns()
    {
        return _designs.ToList();
    }

    public List<BeautyTip> GetTips()
    {
        return _tips.ToList();
    }

    public int GetFirstBookingBonusPoints()
    {
        Promotion? promotion = _promotions.FirstOrDefault(p => p.Id == 2 && p.IsActive);

        return promotion?.BonusPoints ?? 10;
    }

    public Promotion? GetPackagePromotion()
    {
        return _promotions.FirstOrDefault(p =>
            p.Id == 1 &&
            p.IsActive &&
            p.DiscountPercent > 0 &&
            p.IncludedCategories.Count > 0);
    }

    public decimal CalculateDiscountedPrice(decimal totalPrice, Promotion promotion)
    {
        if (promotion.DiscountPercent <= 0)
            return totalPrice;

        return totalPrice - (totalPrice * promotion.DiscountPercent / 100);
    }

    public string GetPromotionPriceText(int salonId, List<BeautyService> services)
    {
        Promotion? promotion = GetPackagePromotion();

        if (promotion == null)
            return "В момента няма активна пакетна промоция.";

        List<BeautyService> selectedServices = new();

        foreach (string category in promotion.IncludedCategories)
        {
            BeautyService? service = services
                .Where(s => s.SalonId == salonId)
                .Where(s => s.Category == category)
                .OrderBy(s => s.Price)
                .FirstOrDefault();

            if (service != null)
                selectedServices.Add(service);
        }

        if (selectedServices.Count != promotion.IncludedCategories.Count)
            return "Промоцията не е налична за избрания салон.";

        decimal originalPrice = selectedServices.Sum(s => s.Price);
        decimal discountedPrice = CalculateDiscountedPrice(originalPrice, promotion);

        string serviceNames = string.Join(" + ", selectedServices.Select(s => s.Name));

        return $"{promotion.Title}: {serviceNames} — {originalPrice:F2} € → {discountedPrice:F2} €";
    }

    public BeautyTip GetTipOfTheDay()
    {
        if (_tips.Count == 0)
            return new BeautyTip
            {
                Title = "Съвет",
                Text = "Няма наличен съвет за деня."
            };

        int index = DateTime.Today.DayOfYear % _tips.Count;
        return _tips[index];
    }
}