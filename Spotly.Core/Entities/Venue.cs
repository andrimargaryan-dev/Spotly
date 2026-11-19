using Spotly.Core.Enums;

namespace Spotly.Core.Entities;

public class Venue : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public District District { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    
    public ICollection<Event> Events { get; set; } = new List<Event>();
}