using Spotly.Core.Enums;

namespace Spotly.Core.Entities;

public class OrganizationMember
{
    public int OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;

    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;

    public OrganizationRole Role { get; set; }
    public DateTime JoinedAt { get; set; }
}