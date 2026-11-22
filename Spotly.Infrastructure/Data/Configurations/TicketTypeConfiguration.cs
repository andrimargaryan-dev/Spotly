using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spotly.Core.Entities;

namespace Spotly.Infrastructure.Data.Configurations;

public class TicketTypeConfiguration : IEntityTypeConfiguration<TicketType>
{
    public void Configure(EntityTypeBuilder<TicketType> b)
    {
        b.Property(t=>t.Name).HasMaxLength(100);
        b.Property(t=>t.Description).HasMaxLength(300);

        b.ToTable(t =>
        {
            t.HasCheckConstraint("CK_TicketType_Price", "[Price] >= 0");
            t.HasCheckConstraint("CK_TicketType_Quantity", "[QuantityPerSession] >= 0");
            t.HasCheckConstraint("CK_TicketType_MaxPerOrder", "[MaxPerOrder] > 0");
        });
    }
}