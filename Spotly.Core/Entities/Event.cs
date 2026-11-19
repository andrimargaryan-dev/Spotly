using Spotly.Core.Enums;

namespace Spotly.Core.Entities;

public class Event : BaseEntity
{
    public int OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;
    
    public int CategoryId   { get; set; }
    public Category Category { get; set; } = null!;
    
    public int VenueId { get; set; }
    public Venue Venue { get; set; } = null!;
    
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? CoverImageUrl { get; set; } = null!;
    public string? MinAge { get; set; }
    public string? Language { get; set; }
    
    public EventStatus Status { get; set; } = EventStatus.Draft;
    public DateTime? PublishedAt { get; set; }

    public ICollection<EventSession> Sessions { get; set; } = new List<EventSession>();
    public ICollection<TicketType> TicketTypes { get; set; } = new List<TicketType>();
}