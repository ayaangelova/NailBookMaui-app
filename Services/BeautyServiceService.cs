using NailBookMaui.Models;

namespace NailBookMaui.Services;

public class BeautyServiceService
{
    private readonly List<BeautyService> _services = new()
    {
        // Salon 1: NailBook Studio Vitosha
        new BeautyService { Id = 101, SalonId = 1, SalonName = "NailBook Studio Vitosha", Name = "Класически маникюр", Description = "Оформяне на нокти, кожички и нанасяне на лак.", Category = "Маникюр", DurationMinutes = 45, Price = 38, IsPopular = true },
        new BeautyService { Id = 102, SalonId = 1, SalonName = "NailBook Studio Vitosha", Name = "Гел лак Premium", Description = "Дълготраен гел лак с база за заздравяване и избор от premium цветове.", Category = "Маникюр", DurationMinutes = 70, Price = 58, IsPopular = true },
        new BeautyService { Id = 103, SalonId = 1, SalonName = "NailBook Studio Vitosha", Name = "Френски маникюр", Description = "Елегантен френски стил с прецизна линия и естествена визия.", Category = "Маникюр", DurationMinutes = 80, Price = 68, IsPopular = false },
        new BeautyService { Id = 104, SalonId = 1, SalonName = "NailBook Studio Vitosha", Name = "Минималистична декорация", Description = "Фини линии, точки, малки камъчета или един акцентен нокът.", Category = "Декорация", DurationMinutes = 25, Price = 20, IsPopular = false },
        new BeautyService { Id = 105, SalonId = 1, SalonName = "NailBook Studio Vitosha", Name = "SPA терапия за ръце", Description = "Хидратация, масаж и подхранваща грижа за кожата на ръцете.", Category = "Грижа", DurationMinutes = 40, Price = 25, IsPopular = true },

        // Salon 2: Ovcha Kupel 
        new BeautyService { Id = 201, SalonId = 2, SalonName = "NailBook Ovcha Kupel", Name = "Класически маникюр", Description = "Оформяне, кожички и лак в естествена визия.", Category = "Маникюр", DurationMinutes = 40, Price = 28, IsPopular = true },
        new BeautyService { Id = 202, SalonId = 2, SalonName = "NailBook Ovcha Kupel", Name = "Гел лак", Description = "Практичен и дълготраен гел лак с богата цветова палитра.", Category = "Маникюр", DurationMinutes = 60, Price = 42, IsPopular = true },
        new BeautyService { Id = 203, SalonId = 2, SalonName = "NailBook Ovcha Kupel", Name = "Сваляне на гел лак", Description = "Безопасно премахване на стар гел лак без увреждане на нокътя.", Category = "Поддръжка", DurationMinutes = 25, Price = 12, IsPopular = false },
        new BeautyService { Id = 204, SalonId = 2, SalonName = "NailBook Ovcha Kupel", Name = "Корекция на счупен нокът", Description = "Възстановяване на единичен нокът с подходящ материал.", Category = "Поддръжка", DurationMinutes = 25, Price = 10, IsPopular = false },
        new BeautyService { Id = 205, SalonId = 2, SalonName = "NailBook Ovcha Kupel", Name = "Мъжки маникюр", Description = "Оформяне, почистване и поддръжка на естествени нокти.", Category = "Маникюр", DurationMinutes = 35, Price = 25, IsPopular = false },

        // Salon 3: Bulgaria Mall 
        new BeautyService { Id = 301, SalonId = 3, SalonName = "NailBook Bulgaria Mall", Name = "Express гел лак", Description = "Бърза услуга за заети клиенти в търговски център.", Category = "Маникюр", DurationMinutes = 50, Price = 50, IsPopular = true },
        new BeautyService { Id = 302, SalonId = 3, SalonName = "NailBook Bulgaria Mall", Name = "Baby Boomer", Description = "Плавен омбре ефект в нежни нюанси.", Category = "Дизайн", DurationMinutes = 90, Price = 75, IsPopular = true },
        new BeautyService { Id = 303, SalonId = 3, SalonName = "NailBook Bulgaria Mall", Name = "Хром ефект", Description = "Перлен или огледален ефект върху готов маникюр.", Category = "Декорация", DurationMinutes = 25, Price = 12, IsPopular = true },
        new BeautyService { Id = 304, SalonId = 3, SalonName = "NailBook Bulgaria Mall", Name = "Изграждане с гел", Description = "Удължаване и оформяне на ноктите с гел.", Category = "Изграждане", DurationMinutes = 130, Price = 95, IsPopular = false },
        new BeautyService { Id = 305, SalonId = 3, SalonName = "NailBook Bulgaria Mall", Name = "Декорация с камъчета", Description = "Акцентни камъчета и луксозен завършек.", Category = "Декорация", DurationMinutes = 30, Price = 15, IsPopular = false },

        // Salon 4: Paradise Center 
        new BeautyService { Id = 401, SalonId = 4, SalonName = "NailBook Paradise Center", Name = "Luxury гел лак", Description = "Premium база, цвят и топ с лъскав завършек.", Category = "Маникюр", DurationMinutes = 75, Price = 45, IsPopular = true },
        new BeautyService { Id = 402, SalonId = 4, SalonName = "NailBook Paradise Center", Name = "Nail Art дизайн", Description = "Ръчно рисувани декорации според снимка или идея на клиента.", Category = "Декорация", DurationMinutes = 60, Price = 10, IsPopular = true },
        new BeautyService { Id = 403, SalonId = 4, SalonName = "NailBook Paradise Center", Name = "Изграждане с гел - дълга форма", Description = "Изграждане с по-дълга форма и прецизна архитектура.", Category = "Изграждане", DurationMinutes = 150, Price = 90, IsPopular = false },
        new BeautyService { Id = 404, SalonId = 4, SalonName = "NailBook Paradise Center", Name = "Френски маникюр с декорация", Description = "Френски стил с фин акцент, брокат или камъчета.", Category = "Дизайн", DurationMinutes = 100, Price = 60, IsPopular = false },
        new BeautyService { Id = 405, SalonId = 4, SalonName = "NailBook Paradise Center", Name = "Парафинова терапия", Description = "Интензивна грижа за сухи ръце и кутикули.", Category = "Грижа", DurationMinutes = 35, Price = 18, IsPopular = true },

        // Salon 5: Mladost 
        new BeautyService { Id = 501, SalonId = 5, SalonName = "NailBook Mladost", Name = "Гел лак", Description = "Устойчив гел лак за ежедневна визия.", Category = "Маникюр", DurationMinutes = 60, Price = 44, IsPopular = true },
        new BeautyService { Id = 502, SalonId = 5, SalonName = "NailBook Mladost", Name = "Поддръжка на изграждане", Description = "Попълване и оформяне на вече изградени нокти.", Category = "Поддръжка", DurationMinutes = 95, Price = 65, IsPopular = true },
        new BeautyService { Id = 503, SalonId = 5, SalonName = "NailBook Mladost", Name = "Къс френски маникюр", Description = "Нежен френски маникюр за къса естествена дължина.", Category = "Маникюр", DurationMinutes = 70, Price = 50, IsPopular = false },
        new BeautyService { Id = 504, SalonId = 5, SalonName = "NailBook Mladost", Name = "Ремонт на два нокътя", Description = "Корекция на до два счупени нокътя.", Category = "Поддръжка", DurationMinutes = 30, Price = 10, IsPopular = false },
        new BeautyService { Id = 505, SalonId = 5, SalonName = "NailBook Mladost", Name = "Матов топ ефект", Description = "Модерен матов завършек върху готов маникюр.", Category = "Декорация", DurationMinutes = 15, Price = 12, IsPopular = false }
    };

    public List<BeautyService> GetAllServices() => _services
        .OrderBy(s => s.SalonName)
        .ThenBy(s => s.Price)
        .ToList();

    public List<BeautyService> GetServicesBySalonId(int salonId) => _services
        .Where(s => s.SalonId == salonId)
        .OrderBy(s => s.Price)
        .ToList();

    public List<BeautyService> GetPopularServices() => _services
        .Where(s => s.IsPopular)
        .OrderBy(s => s.SalonName)
        .ThenBy(s => s.Price)
        .ToList();

    public List<BeautyService> SearchServices(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword)) return GetAllServices();
        keyword = keyword.Trim().ToLowerInvariant();
        return _services.Where(s =>
            s.Name.ToLowerInvariant().Contains(keyword) ||
            s.Description.ToLowerInvariant().Contains(keyword) ||
            s.Category.ToLowerInvariant().Contains(keyword) ||
            s.SalonName.ToLowerInvariant().Contains(keyword)).ToList();
    }
}
