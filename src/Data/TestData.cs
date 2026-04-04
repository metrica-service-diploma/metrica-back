using metrica_back.src.Helpers;
using metrica_back.src.Models;

// TODO: Для метрик добавить реалистичные значения полей ClientId и SessionId.
// Желательно так, чтобы они ссылались на реальные таблицы в БД.

namespace metrica_back.src.Data;

public static class TestData
{
    public static readonly List<User> Users =
    [
        new()
        {
            Id = Guid.NewGuid(),
            UserName = "john_doe",
            Email = "john.doe@example.com",
            CreatedAt = DateTime.UtcNow.AddDays(-30),
            PasswordHash = PasswordHasher.HashPassword("SecurePass123!"),
        },
    ];

    public static readonly List<Website> Websites =
    [
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Интернет-магазин",
            Domain = "shop.example.com",
            TrackingCode = 1,
            CreatedAt = DateTime.UtcNow.AddDays(-5),
            UserId = Users[0].Id,
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Блог",
            Domain = "blog.example.com",
            TrackingCode = 2,
            CreatedAt = DateTime.UtcNow.AddDays(-4),
            UserId = Users[0].Id,
        },
    ];

    private static readonly List<Guid> ClientIds =
    [
        Guid.NewGuid(), // ClientId[0] - Постоянный посетитель Windows
        Guid.NewGuid(), // ClientId[1] - Постоянный посетитель Mac
        Guid.NewGuid(), // ClientId[2] - Постоянный посетитель iPhone
        Guid.NewGuid(), // ClientId[3] - Гостевой посетитель
    ];

    // Список SessionId (разные сессии)
    private static readonly List<Guid> SessionIds =
    [
        Guid.NewGuid(), // SessionIds[0] - Win, магазин, день1
        Guid.NewGuid(), // SessionIds[1] - Win, магазин, день2
        Guid.NewGuid(), // SessionIds[2] - Mac, магазин
        Guid.NewGuid(), // SessionIds[3] - iPhone, магазин
        Guid.NewGuid(), // SessionIds[4] - Win, магазин, день3
        Guid.NewGuid(), // SessionIds[5] - Mac, магазин, день2
        Guid.NewGuid(), // SessionIds[6] - iPhone, магазин, день2
        Guid.NewGuid(), // SessionIds[7] - Win, блог
        Guid.NewGuid(), // SessionIds[8] - Mac, блог
        Guid.NewGuid(), // SessionIds[9] - iPhone, блог
        Guid.NewGuid(), // SessionIds[10] - Win, блог, день2
        Guid.NewGuid(), // SessionIds[11] - Mac, блог, день2
        Guid.NewGuid(), // SessionIds[12] - Гость, магазин, сегодня
    ];
    public static readonly List<TrackingEvent> TrackingEvents =
    [
        // ========== ИНТЕРНЕТ-МАГАЗИН ==========

        // Сессия 0: Windows пользователь (ClientIds[0]), первый визит
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[0],
            SessionId = SessionIds[0],
            TrackingCode = Websites[0].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-7).AddHours(10),
            PageUrl = "https://shop.example.com/",
            PageTitle = "Главная",
            Referrer = "https://google.com",
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            ScreenWidth = 1920,
            ScreenHeight = 1080,
            BrowserLanguage = "ru-RU",
        },
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[0],
            SessionId = SessionIds[0],
            TrackingCode = Websites[0].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-7).AddHours(10).AddMinutes(3),
            PageUrl = "https://shop.example.com/catalog",
            PageTitle = "Каталог",
            Referrer = "https://shop.example.com/",
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            ScreenWidth = 1920,
            ScreenHeight = 1080,
            BrowserLanguage = "ru-RU",
        },
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[0],
            SessionId = SessionIds[0],
            TrackingCode = Websites[0].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-7).AddHours(10).AddMinutes(7),
            PageUrl = "https://shop.example.com/product/1",
            PageTitle = "Товар 1",
            Referrer = "https://shop.example.com/catalog",
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            ScreenWidth = 1920,
            ScreenHeight = 1080,
            BrowserLanguage = "ru-RU",
        },
        // Сессия 1: Windows пользователь (ClientIds[0]), второй визит
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[0],
            SessionId = SessionIds[1],
            TrackingCode = Websites[0].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-4).AddHours(16).AddMinutes(45),
            PageUrl = "https://shop.example.com/",
            PageTitle = "Главная",
            Referrer = "https://google.com",
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            ScreenWidth = 1920,
            ScreenHeight = 1080,
            BrowserLanguage = "ru-RU",
        },
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[0],
            SessionId = SessionIds[1],
            TrackingCode = Websites[0].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-4).AddHours(16).AddMinutes(48),
            PageUrl = "https://shop.example.com/blog",
            PageTitle = "Блог магазина",
            Referrer = "https://shop.example.com/",
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            ScreenWidth = 1920,
            ScreenHeight = 1080,
            BrowserLanguage = "ru-RU",
        },
        // Сессия 2: Mac пользователь (ClientIds[1])
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[1],
            SessionId = SessionIds[2],
            TrackingCode = Websites[0].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-6).AddHours(14).AddMinutes(30),
            PageUrl = "https://shop.example.com/",
            PageTitle = "Главная",
            Referrer = "https://yandex.ru",
            UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36",
            ScreenWidth = 1366,
            ScreenHeight = 768,
            BrowserLanguage = "ru-RU",
        },
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[1],
            SessionId = SessionIds[2],
            TrackingCode = Websites[0].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-6).AddHours(14).AddMinutes(33),
            PageUrl = "https://shop.example.com/catalog",
            PageTitle = "Каталог",
            Referrer = "https://shop.example.com/",
            UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36",
            ScreenWidth = 1366,
            ScreenHeight = 768,
            BrowserLanguage = "ru-RU",
        },
        // Сессия 3: iPhone пользователь (ClientIds[2])
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[2],
            SessionId = SessionIds[3],
            TrackingCode = Websites[0].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-5).AddHours(9).AddMinutes(15),
            PageUrl = "https://shop.example.com/",
            PageTitle = "Главная",
            Referrer = null,
            UserAgent =
                "Mozilla/5.0 (iPhone; CPU iPhone OS 14_0 like Mac OS X) AppleWebKit/605.1.15",
            ScreenWidth = 390,
            ScreenHeight = 844,
            BrowserLanguage = "en-US",
        },
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[2],
            SessionId = SessionIds[3],
            TrackingCode = Websites[0].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-5).AddHours(9).AddMinutes(18),
            PageUrl = "https://shop.example.com/catalog",
            PageTitle = "Каталог",
            Referrer = "https://shop.example.com/",
            UserAgent =
                "Mozilla/5.0 (iPhone; CPU iPhone OS 14_0 like Mac OS X) AppleWebKit/605.1.15",
            ScreenWidth = 390,
            ScreenHeight = 844,
            BrowserLanguage = "en-US",
        },
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[2],
            SessionId = SessionIds[3],
            TrackingCode = Websites[0].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-5).AddHours(9).AddMinutes(22),
            PageUrl = "https://shop.example.com/product/2",
            PageTitle = "Товар 2",
            Referrer = "https://shop.example.com/catalog",
            UserAgent =
                "Mozilla/5.0 (iPhone; CPU iPhone OS 14_0 like Mac OS X) AppleWebKit/605.1.15",
            ScreenWidth = 390,
            ScreenHeight = 844,
            BrowserLanguage = "en-US",
        },
        // Сессия 4: Windows пользователь (ClientIds[0]), третий визит
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[0],
            SessionId = SessionIds[4],
            TrackingCode = Websites[0].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-1).AddHours(15).AddMinutes(40),
            PageUrl = "https://shop.example.com/",
            PageTitle = "Главная",
            Referrer = null,
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            ScreenWidth = 1920,
            ScreenHeight = 1080,
            BrowserLanguage = "ru-RU",
        },
        // Сессия 5: Mac пользователь (ClientIds[1]), второй визит
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[1],
            SessionId = SessionIds[5],
            TrackingCode = Websites[0].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-3).AddHours(11).AddMinutes(20),
            PageUrl = "https://shop.example.com/",
            PageTitle = "Главная",
            Referrer = "https://yandex.ru",
            UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36",
            ScreenWidth = 1366,
            ScreenHeight = 768,
            BrowserLanguage = "ru-RU",
        },
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[1],
            SessionId = SessionIds[5],
            TrackingCode = Websites[0].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-3).AddHours(11).AddMinutes(24),
            PageUrl = "https://shop.example.com/cart",
            PageTitle = "Корзина",
            Referrer = "https://shop.example.com/",
            UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36",
            ScreenWidth = 1366,
            ScreenHeight = 768,
            BrowserLanguage = "ru-RU",
        },
        // Сессия 6: iPhone пользователь (ClientIds[2]), второй визит
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[2],
            SessionId = SessionIds[6],
            TrackingCode = Websites[0].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-2).AddHours(13).AddMinutes(10),
            PageUrl = "https://shop.example.com/",
            PageTitle = "Главная",
            Referrer = "https://google.com",
            UserAgent =
                "Mozilla/5.0 (iPhone; CPU iPhone OS 14_0 like Mac OS X) AppleWebKit/605.1.15",
            ScreenWidth = 390,
            ScreenHeight = 844,
            BrowserLanguage = "en-US",
        },
        // Сессия 12: Гостевой посетитель (ClientIds[3]), сегодня
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[3],
            SessionId = SessionIds[12],
            TrackingCode = Websites[0].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddHours(-3),
            PageUrl = "https://shop.example.com/catalog",
            PageTitle = "Каталог",
            Referrer = "https://google.com",
            UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36",
            ScreenWidth = 1366,
            ScreenHeight = 768,
            BrowserLanguage = "ru-RU",
        },
        // ========== БЛОГ ==========

        // Сессия 7: Windows пользователь (ClientIds[0]), блог
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[0],
            SessionId = SessionIds[7],
            TrackingCode = Websites[1].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-6).AddHours(12).AddMinutes(0),
            PageUrl = "https://blog.example.com/",
            PageTitle = "Главная блога",
            Referrer = "https://google.com",
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            ScreenWidth = 1920,
            ScreenHeight = 1080,
            BrowserLanguage = "ru-RU",
        },
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[0],
            SessionId = SessionIds[7],
            TrackingCode = Websites[1].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-6).AddHours(12).AddMinutes(4),
            PageUrl = "https://blog.example.com/article/1",
            PageTitle = "Первая статья",
            Referrer = "https://blog.example.com/",
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            ScreenWidth = 1920,
            ScreenHeight = 1080,
            BrowserLanguage = "ru-RU",
        },
        // Сессия 8: Mac пользователь (ClientIds[1]), блог
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[1],
            SessionId = SessionIds[8],
            TrackingCode = Websites[1].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-5).AddHours(10).AddMinutes(30),
            PageUrl = "https://blog.example.com/",
            PageTitle = "Главная блога",
            Referrer = "https://yandex.ru",
            UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36",
            ScreenWidth = 1366,
            ScreenHeight = 768,
            BrowserLanguage = "ru-RU",
        },
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[1],
            SessionId = SessionIds[8],
            TrackingCode = Websites[1].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-5).AddHours(10).AddMinutes(33),
            PageUrl = "https://blog.example.com/article/2",
            PageTitle = "Вторая статья",
            Referrer = "https://blog.example.com/",
            UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36",
            ScreenWidth = 1366,
            ScreenHeight = 768,
            BrowserLanguage = "ru-RU",
        },
        // Сессия 9: iPhone пользователь (ClientIds[2]), блог
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[2],
            SessionId = SessionIds[9],
            TrackingCode = Websites[1].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-4).AddHours(9).AddMinutes(15),
            PageUrl = "https://blog.example.com/",
            PageTitle = "Главная блога",
            Referrer = null,
            UserAgent =
                "Mozilla/5.0 (iPhone; CPU iPhone OS 14_0 like Mac OS X) AppleWebKit/605.1.15",
            ScreenWidth = 390,
            ScreenHeight = 844,
            BrowserLanguage = "en-US",
        },
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[2],
            SessionId = SessionIds[9],
            TrackingCode = Websites[1].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-4).AddHours(9).AddMinutes(18),
            PageUrl = "https://blog.example.com/article/3",
            PageTitle = "Третья статья",
            Referrer = "https://blog.example.com/",
            UserAgent =
                "Mozilla/5.0 (iPhone; CPU iPhone OS 14_0 like Mac OS X) AppleWebKit/605.1.15",
            ScreenWidth = 390,
            ScreenHeight = 844,
            BrowserLanguage = "en-US",
        },
        // Сессия 10: Windows пользователь (ClientIds[0]), блог второй визит
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[0],
            SessionId = SessionIds[10],
            TrackingCode = Websites[1].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-3).AddHours(16).AddMinutes(45),
            PageUrl = "https://blog.example.com/",
            PageTitle = "Главная блога",
            Referrer = "https://google.com",
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            ScreenWidth = 1920,
            ScreenHeight = 1080,
            BrowserLanguage = "ru-RU",
        },
        // Сессия 11: Mac пользователь (ClientIds[1]), блог второй визит
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[1],
            SessionId = SessionIds[11],
            TrackingCode = Websites[1].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-2).AddHours(14).AddMinutes(20),
            PageUrl = "https://blog.example.com/",
            PageTitle = "Главная блога",
            Referrer = "https://yandex.ru",
            UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36",
            ScreenWidth = 1366,
            ScreenHeight = 768,
            BrowserLanguage = "ru-RU",
        },
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[1],
            SessionId = SessionIds[11],
            TrackingCode = Websites[1].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddDays(-2).AddHours(14).AddMinutes(24),
            PageUrl = "https://blog.example.com/about",
            PageTitle = "Об авторе",
            Referrer = "https://blog.example.com/",
            UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36",
            ScreenWidth = 1366,
            ScreenHeight = 768,
            BrowserLanguage = "ru-RU",
        },
        // Сессия 13: Гость (ClientIds[3]), блог сегодня
        new()
        {
            Id = Guid.NewGuid(),
            ClientId = ClientIds[3],
            SessionId = Guid.NewGuid(), // Новая сессия для гостя в блоге
            TrackingCode = Websites[1].TrackingCode,
            EventType = "page_view",
            Timestamp = DateTime.UtcNow.AddHours(-5),
            PageUrl = "https://blog.example.com/",
            PageTitle = "Главная блога",
            Referrer = "https://google.com",
            UserAgent =
                "Mozilla/5.0 (iPhone; CPU iPhone OS 14_0 like Mac OS X) AppleWebKit/605.1.15",
            ScreenWidth = 390,
            ScreenHeight = 844,
            BrowserLanguage = "en-US",
        },
    ];
}
