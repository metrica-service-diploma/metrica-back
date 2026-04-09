using metrica_back.src.Domain.Models;

namespace metrica_back.src.External.Common;

public static class ClickHouseTestDataFactory
{
    private static readonly Dictionary<string, Guid> ClientIds = new()
    {
        ["client_windows"] = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
        ["client_mac"] = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
        ["client_iphone"] = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
        ["client_guest"] = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
    };

    private static readonly Dictionary<string, Guid> SessionIds = new()
    {
        // Сессии магазина
        ["session_shop_win_day1"] = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        ["session_shop_win_day2"] = Guid.Parse("22222222-2222-2222-2222-222222222222"),
        ["session_shop_mac"] = Guid.Parse("33333333-3333-3333-3333-333333333333"),
        ["session_shop_iphone"] = Guid.Parse("44444444-4444-4444-4444-444444444444"),
        ["session_shop_win_day3"] = Guid.Parse("55555555-5555-5555-5555-555555555555"),
        ["session_shop_mac_day2"] = Guid.Parse("66666666-6666-6666-6666-666666666666"),
        ["session_shop_iphone_day2"] = Guid.Parse("77777777-7777-7777-7777-777777777777"),
        ["session_shop_guest"] = Guid.Parse("88888888-8888-8888-8888-888888888888"),

        // Сессии блога
        ["session_blog_win"] = Guid.Parse("99999999-9999-9999-9999-999999999999"),
        ["session_blog_mac"] = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-000000000001"),
        ["session_blog_iphone"] = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-000000000002"),
        ["session_blog_win_day2"] = Guid.Parse("cccccccc-cccc-cccc-cccc-000000000003"),
        ["session_blog_mac_day2"] = Guid.Parse("dddddddd-dddd-dddd-dddd-000000000004"),
        ["session_blog_guest"] = Guid.Parse("eeeeeeee-eeee-eeee-eeee-000000000005"),
    };

    public static List<TrackingEvent> CreateTestData()
    {
        DateTime dateNow = DateTime.UtcNow;
        List<TrackingEvent> trackingEvents = [];

        // Магазин - Сессия 1: Windows пользователь, первый визит
        trackingEvents.AddRange(
            CreateShopEvents(
                clientId: ClientIds["client_windows"],
                sessionId: SessionIds["session_shop_win_day1"],
                startTime: dateNow.AddDays(-7).AddHours(10),
                pages: ["/", "/catalog", "/product/1"],
                titles: ["Главная", "Каталог", "Товар 1"],
                userAgent: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
                screenWidth: 1920,
                screenHeight: 1080
            )
        );

        // Магазин - Сессия 2: Windows пользователь, второй визит
        trackingEvents.AddRange(
            CreateShopEvents(
                clientId: ClientIds["client_windows"],
                sessionId: SessionIds["session_shop_win_day2"],
                startTime: dateNow.AddDays(-4).AddHours(16).AddMinutes(45),
                pages: ["/", "/blog"],
                titles: ["Главная", "Блог магазина"],
                userAgent: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
                screenWidth: 1920,
                screenHeight: 1080
            )
        );

        // Магазин - Сессия 3: Mac пользователь
        trackingEvents.AddRange(
            CreateShopEvents(
                clientId: ClientIds["client_mac"],
                sessionId: SessionIds["session_shop_mac"],
                startTime: dateNow.AddDays(-6).AddHours(14).AddMinutes(30),
                pages: ["/", "/catalog"],
                titles: ["Главная", "Каталог"],
                userAgent: "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36",
                screenWidth: 1366,
                screenHeight: 768,
                referrer: "https://yandex.ru"
            )
        );

        // Магазин - Сессия 4: iPhone пользователь
        trackingEvents.AddRange(
            CreateShopEvents(
                clientId: ClientIds["client_iphone"],
                sessionId: SessionIds["session_shop_iphone"],
                startTime: dateNow.AddDays(-5).AddHours(9).AddMinutes(15),
                pages: ["/", "/catalog", "/product/2"],
                titles: ["Главная", "Каталог", "Товар 2"],
                userAgent: "Mozilla/5.0 (iPhone; CPU iPhone OS 14_0 like Mac OS X) AppleWebKit/605.1.15",
                screenWidth: 390,
                screenHeight: 844,
                browserLanguage: "en-US"
            )
        );

        // Блог - Сессия 1: Windows пользователь
        trackingEvents.AddRange(
            CreateBlogEvents(
                clientId: ClientIds["client_windows"],
                sessionId: SessionIds["session_blog_win"],
                startTime: dateNow.AddDays(-6).AddHours(12),
                pages: ["/", "/article/1"],
                titles: ["Главная блога", "Первая статья"],
                userAgent: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
                screenWidth: 1920,
                screenHeight: 1080
            )
        );

        // Блог - Сессия 2: Mac пользователь
        trackingEvents.AddRange(
            CreateBlogEvents(
                clientId: ClientIds["client_mac"],
                sessionId: SessionIds["session_blog_mac"],
                startTime: dateNow.AddDays(-5).AddHours(10).AddMinutes(30),
                pages: ["/", "/article/2"],
                titles: ["Главная блога", "Вторая статья"],
                userAgent: "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36",
                screenWidth: 1366,
                screenHeight: 768,
                referrer: "https://yandex.ru"
            )
        );

        // Блог - Сессия 3: iPhone пользователь
        trackingEvents.AddRange(
            CreateBlogEvents(
                clientId: ClientIds["client_iphone"],
                sessionId: SessionIds["session_blog_iphone"],
                startTime: dateNow.AddDays(-4).AddHours(9).AddMinutes(15),
                pages: ["/", "/article/3"],
                titles: ["Главная блога", "Третья статья"],
                userAgent: "Mozilla/5.0 (iPhone; CPU iPhone OS 14_0 like Mac OS X) AppleWebKit/605.1.15",
                screenWidth: 390,
                screenHeight: 844,
                browserLanguage: "en-US"
            )
        );

        // Гостевые визиты (сегодня)
        trackingEvents.Add(
            CreateShopEvent(
                clientId: ClientIds["client_guest"],
                sessionId: SessionIds["session_shop_guest"],
                createdAt: dateNow.AddHours(-3),
                pageUrl: "/catalog",
                pageTitle: "Каталог",
                userAgent: "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36",
                screenWidth: 1366,
                screenHeight: 768,
                referrer: "https://google.com"
            )
        );
        trackingEvents.Add(
            CreateBlogEvent(
                clientId: ClientIds["client_guest"],
                sessionId: SessionIds["session_blog_guest"],
                createdAt: dateNow.AddHours(-5),
                pageUrl: "/",
                pageTitle: "Главная блога",
                userAgent: "Mozilla/5.0 (iPhone; CPU iPhone OS 14_0 like Mac OS X) AppleWebKit/605.1.15",
                screenWidth: 390,
                screenHeight: 844,
                browserLanguage: "en-US",
                referrer: "https://google.com"
            )
        );

        return trackingEvents;
    }

    private static List<TrackingEvent> CreateShopEvents(
        Guid clientId,
        Guid sessionId,
        DateTime startTime,
        string[] pages,
        string[] titles,
        string userAgent,
        int screenWidth,
        int screenHeight,
        string? referrer = null,
        string browserLanguage = "ru-RU"
    )
    {
        var events = new List<TrackingEvent>();

        for (int i = 0; i < pages.Length; i++)
        {
            events.Add(
                CreateShopEvent(
                    clientId,
                    sessionId,
                    startTime.AddMinutes(i * 3),
                    pages[i],
                    titles[i],
                    userAgent,
                    screenWidth,
                    screenHeight,
                    i == 0 ? referrer : $"https://shop.example.com{pages[i - 1]}",
                    browserLanguage
                )
            );
        }

        return events;
    }

    private static TrackingEvent CreateShopEvent(
        Guid clientId,
        Guid sessionId,
        DateTime createdAt,
        string pageUrl,
        string pageTitle,
        string userAgent,
        int screenWidth,
        int screenHeight,
        string? referrer = null,
        string browserLanguage = "ru-RU"
    )
    {
        return new TrackingEvent
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            SessionId = sessionId,
            TrackingCode = 1,
            CreatedAt = createdAt,
            PageUrl = $"https://shop.example.com{pageUrl}",
            PageTitle = pageTitle,
            Referrer = referrer,
            UserAgent = userAgent,
            ScreenWidth = screenWidth,
            ScreenHeight = screenHeight,
            BrowserLanguage = browserLanguage,
        };
    }

    private static List<TrackingEvent> CreateBlogEvents(
        Guid clientId,
        Guid sessionId,
        DateTime startTime,
        string[] pages,
        string[] titles,
        string userAgent,
        int screenWidth,
        int screenHeight,
        string? referrer = null,
        string browserLanguage = "ru-RU"
    )
    {
        var events = new List<TrackingEvent>();

        for (int i = 0; i < pages.Length; i++)
        {
            events.Add(
                CreateBlogEvent(
                    clientId,
                    sessionId,
                    startTime.AddMinutes(i * 3),
                    pages[i],
                    titles[i],
                    userAgent,
                    screenWidth,
                    screenHeight,
                    i == 0 ? referrer : $"https://blog.example.com{pages[i - 1]}",
                    browserLanguage
                )
            );
        }

        return events;
    }

    private static TrackingEvent CreateBlogEvent(
        Guid clientId,
        Guid sessionId,
        DateTime createdAt,
        string pageUrl,
        string pageTitle,
        string userAgent,
        int screenWidth,
        int screenHeight,
        string? referrer = null,
        string browserLanguage = "ru-RU"
    )
    {
        return new TrackingEvent
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            SessionId = sessionId,
            TrackingCode = 2,
            CreatedAt = createdAt,
            PageUrl = $"https://blog.example.com{pageUrl}",
            PageTitle = pageTitle,
            Referrer = referrer,
            UserAgent = userAgent,
            ScreenWidth = screenWidth,
            ScreenHeight = screenHeight,
            BrowserLanguage = browserLanguage,
        };
    }
}
