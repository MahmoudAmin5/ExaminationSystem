using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Infrastructure.Persistence.Seeding
{
    public static class DbSeeder
    {

        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            
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

         
            var webDevDiplomaId = Guid.Parse("D1111111-1111-1111-1111-111111111111");

            if (await context.Diplomas.AnyAsync(d => d.Id == webDevDiplomaId) == false)
            {
                // 2. زرع الدبلومة (Diploma)
                var diploma = new Diploma
                {
                    Id = webDevDiplomaId,
                    Title = "Full Stack Web Development",
                    Description = "Master HTML, CSS, JS, and .NET Core.",
                    Status = ContentStatus.Published,
                    CreatedAt = DateTime.UtcNow
                };
                context.Diplomas.Add(diploma);

                // 3. زرع كويزات تابعة للدبلومة (Quizzes)
                context.Quizzes.AddRange(
                    new Quiz
                    {
                        Id = Guid.NewGuid(),
                        DiplomaId = webDevDiplomaId,
                        Title = "C# Basics",
                        DurationMinutes = 30,
                        PassScore = 60,
                        Status = ContentStatus.Published
                    },
                    new Quiz
                    {
                        Id = Guid.NewGuid(),
                        DiplomaId = webDevDiplomaId,
                        Title = "EF Core Intermediate",
                        DurationMinutes = 45,
                        PassScore = 70,
                        Status = ContentStatus.Published
                    }
                );

                await context.SaveChangesAsync();
            }
    }
}
