using Microsoft.AspNetCore.Identity;

namespace RoleBasedAuth.Models.Domain
{
	public class ApplicationUser : IdentityUser<Guid>
	{
        public string Name { get; set; }
        public string? Address1 { get; set; }
    }
}
