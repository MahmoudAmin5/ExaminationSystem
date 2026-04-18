using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Infrastructure.Persistence;
using ExaminationSystem.Api.Infrastructure.Persistence.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task InitializeDatabaseAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

                 //Update-Database
                await context.Database.MigrateAsync();

                
                await DbSeeder.SeedAsync(userManager, roleManager);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred during database initialization.");
            }
        }
    }
}
