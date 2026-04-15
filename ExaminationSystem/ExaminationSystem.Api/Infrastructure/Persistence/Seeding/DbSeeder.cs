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
                new IdentityRole<Guid>
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "D1111111-1111-1111-1111-111111111111"
                },
                new IdentityRole<Guid>
                {
                    Id = studentRoleId,
                    Name = "Student",
                    NormalizedName = "STUDENT",
                    ConcurrencyStamp = "E2222222-2222-2222-2222-222222222222"
                }
            );

            
            var admin = new User
            {
                Id = adminUserId,
                FullName = "Ahmed Ali",
                UserName = "ahmedali660",
                NormalizedUserName = "AHMEDALI660",
                Email = "ahmedali660@gmail.com",
                NormalizedEmail = "AHMEDALI660@GMAIL.COM",
                EmailConfirmed = true,
                Status = AccountStatus.Active,
                Role = UserRole.Admin,
                SecurityStamp = "F3333333-3333-3333-3333-333333333333",
                ConcurrencyStamp = "G4444444-4444-4444-4444-444444444444",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                PasswordHash = "AQAAAAIAAYagAAAAEE6mfs5nlUwXENNWDXGrILwjOLAVq/UkeXlSCmM8qnKQVk6qO9H3AX8m7TgsHIa7Ag=="
            };

            modelBuilder.Entity<User>().HasData(admin);

          
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid> { UserId = adminUserId, RoleId = adminRoleId }
            );
        }

       
        
    }
}
