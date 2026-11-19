namespace Spotly.Core.Entities;

public class TicketType : BaseEntity
{
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int QuantityPerSession { get; set; }
    public int MaxPerOrder { get; set; } = 6;
    public DateTime? SalesEndAt { get; set; }
    public bool IsActive { get; set; } = true;
}