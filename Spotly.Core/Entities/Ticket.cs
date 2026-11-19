using Spotly.Core.Enums;

namespace Spotly.Core.Entities;

public class Ticket : BaseEntity
{
    public int OrderItemId { get; set; }
    public OrderItem OrderItem { get; set; } = null!;

    public string? HolderName { get; set; }
    public string QrToken { get; set; } = string.Empty;
    public TicketStatus Status { get; set; } = TicketStatus.Valid;
    public DateTime? CheckedInAt { get; set; }
}