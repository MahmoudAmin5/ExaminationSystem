using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Infrastructure.Persistence.Seeding
{
    public static class IdentitySeeder
    {

        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            var studentId = Guid.Parse("C3333333-3333-3333-3333-333333333333");

            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole<Guid> { Name = "Admin" });

            if (!await roleManager.RoleExistsAsync("Student"))
                await roleManager.CreateAsync(new IdentityRole<Guid> { Name = "Student" });


            var adminEmail = "saramaged660@gmail.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new User
                {
                    Id = Guid.NewGuid(),
                    FullName = "Sara Maged",
                    UserName = "saramaged660",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Status = AccountStatus.Active,
                    Role = UserRole.Admin
                };

                await userManager.CreateAsync(admin, "Admin@Secure671");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            var studentEmail = "student.demo@examination.local";
            if (await userManager.FindByEmailAsync(studentEmail) == null)
            {
                var student = new User
                {
                    Id = studentId,
                    FullName = "Demo Student",
                    UserName = "student.demo",
                    Email = studentEmail,
                    EmailConfirmed = true,
                    Status = AccountStatus.Active,
                    Role = UserRole.Student
                };

                await userManager.CreateAsync(student, "Student@Secure671");
                await userManager.AddToRoleAsync(student, "Student");
            }

        }
    }
}
            
