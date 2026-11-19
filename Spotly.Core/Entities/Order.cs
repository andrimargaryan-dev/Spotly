using Spotly.Core.Enums;

namespace Spotly.Core.Entities;

public class Order : BaseEntity
{
    public string Code { get; set; } = string.Empty;

    public int EventSessionId { get; set; }
    public EventSession EventSession { get; set; } = null!;

    public int? UserId { get; set; }
    public AppUser? User { get; set; }

    public string? ContactName { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal Subtotal { get; set; }
    public decimal ServiceFee { get; set; }
    public decimal Total { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? PaidAt { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}