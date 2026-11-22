using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spotly.Core.Entities;

namespace Spotly.Infrastructure.Data.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> b)
    {
        b.Property(t => t.HolderName).HasMaxLength(150);
        b.Property(t => t.QrToken).HasMaxLength(64);
        b.HasIndex(t => t.QrToken).IsUnique();
    }
}