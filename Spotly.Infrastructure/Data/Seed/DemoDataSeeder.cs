using Microsoft.EntityFrameworkCore;
using Spotly.Core.Entities;
using Spotly.Core.Enums;

namespace Spotly.Infrastructure.Data.Seed;

public static class DemoDataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Events.AnyAsync())
            return;

        var categories = await db.Categories.ToDictionaryAsync(c => c.Slug);

        var poplavok = new Venue
        {
            Name = "Poplavok Jazz Café", Address = "Isahakyan St", District = District.Kentron, Latitude = 40.1858m,
            Longitude = 44.5152m, IsAccessible = true
        };
        var kond = new Venue
        {
            Name = "Kond Ceramics Studio", Address = "Kond", District = District.Kentron, Latitude = 40.1795m,
            Longitude = 44.5040m
        };
        var stage21 = new Venue
        {
            Name = "Stage 21", Address = "Kentron", District = District.Kentron, Latitude = 40.1810m,
            Longitude = 44.5135m
        };
        var botanical = new Venue
        {
            Name = "Yerevan Botanical Garden", Address = "Acharyan St", District = District.Avan, Latitude = 40.2150m,
            Longitude = 44.5690m, IsAccessible = true
        };

        var saturday = NextDay(DayOfWeek.Saturday);
        var friday = saturday.AddDays(-1);
        var sunday = saturday.AddDays(1);

        db.Events.AddRange(
            CreateEvent("Jazz on the Lake: Vahagn Hayrapetyan Trio", "jazz-on-the-lake",
                "An intimate autumn set at Poplavok, the café on the pond in the heart of Kentron.",
                categories["jazz"], poplavok, 16,
                YerevanToUtc(saturday, 20), YerevanToUtc(saturday, 22, 30),
                ("Standard", 6000m, 100), ("Student", 3500m, 20), ("Table for 4", 28000m, 5)),
            CreateEvent("Clay & Pomegranates: Pottery Workshop", "clay-and-pomegranates",
                "A hands-on pottery class in Kond. All materials included.",
                categories["workshops"], kond, null,
                YerevanToUtc(saturday, 12), YerevanToUtc(saturday, 14),
                ("Standard", 7500m, 12)),
            CreateEvent("Tuff Bass: Techno Night", "tuff-bass-techno-night",
                "All-night techno with local and guest DJs.",
                categories["techno"], stage21, 18,
                YerevanToUtc(friday, 23), YerevanToUtc(saturday, 4),
                ("General", 8000m, 300)),
            CreateEvent("Bubble Day at the Botanical Garden", "bubble-day-botanical-garden",
                "Giant bubbles, games and picnics for the whole family.",
                categories["kids-family"], botanical, null,
                YerevanToUtc(sunday, 11), YerevanToUtc(sunday, 15),
                ("Child", 2000m, 200), ("Adult", 3000m, 200)));

        await db.SaveChangesAsync();
    }

    private static Event CreateEvent(
        string title, string slug, string description, Category category, Venue venue, int? minAge,
        DateTime startsAt, DateTime endsAt, params (string Name, decimal Price, int Quantity)[] tickets)
    {
        var ev = new Event
        {
            Title = title,
            Slug = slug,
            Description = description,
            Category = category,
            Venue = venue,
            MinAge = minAge,
            Status = EventStatus.Published,
            PublishedAt = DateTime.UtcNow
        };

        ev.Sessions.Add(new EventSession
            { StartsAt = startsAt, EndsAt = endsAt, DoorsOpenAt = startsAt.AddMinutes(-30) });

        foreach (var (name, price, quantity) in tickets)
            ev.TicketTypes.Add(new TicketType { Name = name, Price = price, QuantityPerSession = quantity });

        return ev;
    }

    private static DateTime NextDay(DayOfWeek day)
    {
        var today = DateTime.UtcNow.Date;
        var daysAhead = ((int)day - (int)today.DayOfWeek + 7) % 7;
        return today.AddDays(daysAhead == 0 ? 7 : daysAhead);
    }

    private static DateTime YerevanToUtc(DateTime date, int hour, int minute = 0) =>
        TimeZoneInfo.ConvertTimeToUtc(date.Date.AddHours(hour).AddMinutes(minute),
            TimeZoneInfo.FindSystemTimeZoneById("Asia/Yerevan"));
}