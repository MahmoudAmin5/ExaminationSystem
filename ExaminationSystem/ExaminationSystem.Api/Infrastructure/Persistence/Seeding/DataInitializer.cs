using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;

namespace ExaminationSystem.Api.Infrastructure.Persistence.Seeding
{
    public class DataInitializer
    {
        public static async Task SeedDiplomsAsync(IUnitOfWork unitOfWork)
        {
            var diplomaRepo = unitOfWork.Repository<Diploma, Guid>();
            var webDevDiplomaId = Guid.Parse("D1111111-1111-1111-1111-111111111111");

           
            var exists = await diplomaRepo.AnyAsync(d => d.Id == webDevDiplomaId);
            if (exists) return;

            
            var diploma = new Diploma
            {
                Id = webDevDiplomaId,
                Title = "Full Stack Web Development",
                Description = "Master HTML, CSS, JS, and .NET Core.",
                Status = ContentStatus.Published,
                CreatedAt = DateTime.UtcNow
            };

            diploma.Quizzes.Add(new Quiz
            {
                Id = Guid.NewGuid(),
                Title = "C# Basics",
                DurationMinutes = 30,
                PassScore = 60,
                Status = ContentStatus.Published,
                CreatedAt = DateTime.UtcNow
            });

            diploma.Quizzes.Add(new Quiz
            {
                Id = Guid.NewGuid(),
                Title = "EF Core Intermediate",
                DurationMinutes = 45,
                PassScore = 70,
                Status = ContentStatus.Published,
                CreatedAt = DateTime.UtcNow
            });

            
            diplomaRepo.Add(diploma);
            await unitOfWork.SaveChangesAsync();
        }
    }
}