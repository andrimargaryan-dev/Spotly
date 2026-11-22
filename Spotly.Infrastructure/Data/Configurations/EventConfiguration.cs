using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spotly.Core.Entities;

namespace Spotly.Infrastructure.Data.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> b)
    {
        b.Property(e => e.Title).HasMaxLength(200);
        b.Property(e => e.Slug).HasMaxLength(220);
        b.HasIndex(e => e.Slug).IsUnique();
        b.Property(e => e.Description).HasMaxLength(2000);
        b.Property(e => e.CoverImageUrl).HasMaxLength(500);
        b.Property(e => e.Language).HasMaxLength(50);

        b.HasIndex(e => e.Status);
    }
}