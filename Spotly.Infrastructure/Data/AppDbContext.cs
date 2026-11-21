using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Spotly.Core.Entities;

namespace Spotly.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Venue> Venues => Set<Venue>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventSession> EventSessions => Set<EventSession>();
    public DbSet<TicketType> TicketTypes => Set<TicketType>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Payment> Payments => Set<Payment>();
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
       configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
       configurationBuilder.Properties<Enum>().HaveConversion<string>().HaveMaxLength(32);
    }
    
     protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    
            var domainForeignKeys = builder.Model.GetEntityTypes()
                .Where(e => e.ClrType.Namespace == typeof(BaseEntity).Namespace)
                .SelectMany(e => e.GetForeignKeys());
    
            foreach (var fk in domainForeignKeys)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
        }
        
     public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
         {
             var now = DateTime.UtcNow;
             foreach (var entry in ChangeTracker.Entries<BaseEntity>())
             {
                 if (entry.State == EntityState.Added) entry.Entity.CreatedAt = now;
                 else if (entry.State == EntityState.Modified) entry.Entity.UpdatedAt = now;
             }
             return base.SaveChangesAsync(cancellationToken);
         }
}