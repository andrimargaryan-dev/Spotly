using Microsoft.EntityFrameworkCore;
using Spotly.Core.Entities;

namespace Spotly.Infrastructure.Data.Seed;

public static class CategorySeeder
{
    private static readonly (string Name, string Slug, string? ParentSlug)[] Categories =
    [
        ("Music", "music", null),
        ("Jazz", "jazz", "music"),
        ("Classical", "classical", "music"),
        ("Festivals", "festivals", null),
        ("Art", "art", null),
        ("Workshops", "workshops", null),
        ("Nightlife", "nightlife", null),
        ("Techno", "techno", "nightlife"),
        ("Tech", "tech", null),
        ("Food & Drink", "food-drink", null),
        ("Sports", "sports", null),
        ("Theatre", "theatre", null),
        ("Kids & Family", "kids-family", null),
        
        ////stugel count y?
    ];

    public static async Task SeedAsync(AppDbContext db)
    {
        var existing = await db.Categories.ToDictionaryAsync(c => c.Slug);
        var order = 0;

        foreach (var (name, slug, parentSlug) in Categories)
        {
            order++;
            if (existing.ContainsKey(slug))
                continue;

            var category = new Category
            {
                Name = name,
                Slug = slug,
                SortOrder = order,
                Parent = parentSlug is null ? null : existing[parentSlug]
            };

            db.Categories.Add(category);
            existing[slug] = category;
        }

        await db.SaveChangesAsync();
    }
}