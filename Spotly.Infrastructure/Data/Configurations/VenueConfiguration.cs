using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spotly.Core.Entities;

namespace Spotly.Infrastructure.Data.Configurations;

public class VenueConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> b)
    {
        b.Property(v => v.Name).HasMaxLength(150);
        b.Property(v => v.Address).HasMaxLength(300);
        b.Property(v => v.Latitude).HasPrecision(9, 6);
        b.Property(v => v.Longitude).HasPrecision(9, 6);
    }
}