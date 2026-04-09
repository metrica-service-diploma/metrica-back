using metrica_back.src.Business.Helpers;
using metrica_back.src.Domain.Models;

namespace metrica_back.src.External.Common;

public static class PostgreSqlTestDataFactory
{
    private static readonly Dictionary<string, Guid> UserIds = new()
    {
        ["john_doe"] = Guid.Parse("11111111-1111-1111-1111-111111111111"),
    };

    private static readonly Dictionary<string, Guid> WebsiteIds = new()
    {
        ["shop"] = Guid.Parse("22222222-2222-2222-2222-222222222222"),
        ["blog"] = Guid.Parse("33333333-3333-3333-3333-333333333333"),
    };

    public static (User[], Website[]) CreateTestData()
    {
        DateTime dateNow = DateTime.UtcNow;

        User[] users =
        [
            new()
            {
                Id = UserIds["john_doe"],
                UserName = "john_doe",
                Email = "john.doe@example.com",
                CreatedAt = dateNow.AddDays(-30),
                PasswordHash = PasswordHasher.Hash("SecurePass123!"),
            },
        ];

        Website[] websites =
        [
            new()
            {
                Id = WebsiteIds["shop"],
                Name = "Интернет-магазин",
                Domain = "shop.example.com",
                TrackingCode = 1,
                CreatedAt = dateNow.AddDays(-5),
                UserId = users[0].Id,
            },
            new()
            {
                Id = WebsiteIds["blog"],
                Name = "Блог",
                Domain = "blog.example.com",
                TrackingCode = 2,
                CreatedAt = dateNow.AddDays(-4),
                UserId = users[0].Id,
            },
        ];

        return (users, websites);
    }
}
