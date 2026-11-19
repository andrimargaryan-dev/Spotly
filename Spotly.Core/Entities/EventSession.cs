namespace Spotly.Core.Entities;

public class EventSession : BaseEntity
{
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;

    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public DateTime? DoorsOpenAt { get; set; }
    public bool IsCanceled { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}