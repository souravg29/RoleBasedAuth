using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoleBasedAuth.Models.Domain;
using System.Security.Cryptography.Xml;

namespace RoleBasedAuth.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
		{

		}


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);


			var superAdminRoleId = Guid.NewGuid();
			var adminRoleId = Guid.NewGuid();
			var memberRoleId = Guid.NewGuid();

			var roles = new List<ApplicationRole>()
			{
				new ApplicationRole()
				{
					Id = superAdminRoleId,
					Name = "SuperAdmin"
				},
				new ApplicationRole()
				{
					Id = adminRoleId,
					Name = "Admin"
				},
				new ApplicationRole()
				{
					Id = memberRoleId,
					Name = "Member"
				}
			};

			builder.Entity<ApplicationRole>().HasData(roles);

			var superAdminId = Guid.NewGuid();
			var superAdminUser = new ApplicationUser()
			{
				Id = superAdminId,
				UserName = "superadmin@admin.com",
				Email = "superadmin@admin.com",
				Name = "Sourav Ganguly"
				
			};

			superAdminUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(superAdminUser, "Super@Admin.123#");

			builder.Entity<ApplicationUser>().HasData(superAdminUser);

			var superAdminRoles = new List<IdentityUserRole<Guid>>
			{
				new IdentityUserRole<Guid>
				{
					RoleId = superAdminRoleId,
					UserId = superAdminId
				},
				new IdentityUserRole<Guid>
				{
					RoleId = adminRoleId,
					UserId = superAdminId
				},
				new IdentityUserRole<Guid>
				{
					RoleId = memberRoleId,
					UserId = superAdminId
				}
			};
			builder.Entity<IdentityUserRole<Guid>>().HasData(superAdminRoles);

		}
	}
}
