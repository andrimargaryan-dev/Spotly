using Microsoft.AspNetCore.Identity;

namespace Spotly.Core.Entities;

public class AppUser : IdentityUser<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}