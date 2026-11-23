using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spotly.Core.Entities;

namespace Spotly.Infrastructure.Data.Configurations;

public class EventSessionConfiguration : IEntityTypeConfiguration<EventSession>
{
    public void Configure(EntityTypeBuilder<EventSession> b)
    {
        b.HasIndex(s => new { s.EventId, s.StartsAt });
    }
}