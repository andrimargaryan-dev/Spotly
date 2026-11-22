using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spotly.Core.Entities;

namespace Spotly.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> b)
    {
        b.Property(o=>o.Code).HasMaxLength(20);
        b.HasIndex(o=>o.Code).IsUnique();
        b.Property(o=>o.ContactName).HasMaxLength(150);
        b.Property(o=>o.ContactEmail).HasMaxLength(256);
        b.Property(o=>o.ContactPhone).HasMaxLength(30);
        
        b.HasIndex(o=>new {o.EventSessionId, o.Status});
    }
}