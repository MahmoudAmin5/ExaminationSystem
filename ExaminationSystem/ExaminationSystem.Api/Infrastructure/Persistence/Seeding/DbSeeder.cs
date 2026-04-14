using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Infrastructure.Persistence.Seeding
{
    public static class DbSeeder
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            var adminRoleId = Guid.Parse("A1111111-1111-1111-1111-111111111111");
            var studentRoleId = Guid.Parse("B2222222-2222-2222-2222-222222222222");
            var adminUserId = Guid.Parse("C3333333-3333-3333-3333-333333333333");

           
            modelBuilder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid> { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole<Guid> { Id = studentRoleId, Name = "Student", NormalizedName = "STUDENT" }
            );

           
            var admin = new User
            {
                Id = adminUserId,
                FullName = "Sara Maged",
                UserName = "saramaged660",
                Email = "saramaged660@gmail.com",
                EmailConfirmed = true,
                Status = AccountStatus.Active,
                Role = UserRole.Admin,
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow
            };

           
            var hasher = new PasswordHasher<User>();
            admin.PasswordHash = hasher.HashPassword(admin, "Admin@Secure671");

            modelBuilder.Entity<User>().HasData(admin);

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid> { UserId = adminUserId, RoleId = adminRoleId }
            );
        }
    }
}
