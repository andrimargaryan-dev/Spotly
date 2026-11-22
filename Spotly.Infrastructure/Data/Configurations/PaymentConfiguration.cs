using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spotly.Core.Entities;

namespace Spotly.Infrastructure.Data.Configurations;

public class PaymentConfiguration: IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> b)
    {
        b.Property(p => p.ProviderReference).HasMaxLength(100);
        b.Property(p => p.CardLast4).HasMaxLength(4);
        b.Property(p => p.FailureReason).HasMaxLength(300);
    }
}