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
            string[] roles = { "Admin", "Student" };
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole<Guid> 
                    { 
                        Name = roleName,
                    } );
            }

            var adminEmail = "saramaged660@gmail.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new User
                {
                    FullName = "Sara Maged",
                    UserName = "saramaged660",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Status = AccountStatus.Active,
                    Role = UserRole.Admin,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                await userManager.CreateAsync(admin, "Admin@Secure671");
                await userManager.AddToRoleAsync(admin, "Admin");
                var studentId = Guid.Parse("C3333333-3333-3333-3333-333333333333");

                // 2. Check if the student already exists
                if (await userManager.FindByIdAsync(studentId.ToString()) == null)
                {
                    // 3. Create the student
                    var testStudent = new User
                    {
                        Id = studentId,
                        FullName = "Test Student",
                        UserName = "teststudent",
                        Email = "student@test.com",
                        EmailConfirmed = true,
                        Status = AccountStatus.Active,
                        Role = UserRole.Student
                    };

                    // 4. Save to database and assign the Student role
                    await userManager.CreateAsync(testStudent, "Student@Secure123");
                    await userManager.AddToRoleAsync(testStudent, "Student");
                }
            }

    

        }
    }
}
            
