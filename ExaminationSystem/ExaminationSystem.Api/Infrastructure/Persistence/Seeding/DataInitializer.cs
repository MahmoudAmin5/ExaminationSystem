using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;

namespace ExaminationSystem.Api.Infrastructure.Persistence.Seeding
{
    public class DataInitializer
    {
        public static async Task SeedDiplomasAsync(IUnitOfWork unitOfWork)
        {
            var diplomaRepo = unitOfWork.Repository<Diploma, Guid>();
            var webDevDiplomaId = Guid.Parse("D1111111-1111-1111-1111-111111111111");

            var quizRepo = unitOfWork.Repository<Quiz, Guid>();


            var draftQuizId = Guid.Parse("B1111111-1111-1111-1111-111111111111");

            if (await diplomaRepo.AnyAsync(d => d.Id == webDevDiplomaId))
            {
                // لو الدبلومة موجودة، نتأكد بس إن الكويز الـ Draft موجود
                if (!await quizRepo.AnyAsync(q => q.Id == draftQuizId))
                {
                    quizRepo.Add(new Quiz
                    {
                        Id = draftQuizId,
                        DiplomaId = webDevDiplomaId,
                        Title = "Advanced C# Patterns (Draft)",
                        DurationMinutes = 60,
                        PassScore = 75m,
                        Status = ContentStatus.Draft,
                        CreatedAt = DateTime.UtcNow
                    });
                    await unitOfWork.SaveChangesAsync();
                }
                return;
            }

            var exists = await diplomaRepo.AnyAsync(d => d.Id == webDevDiplomaId);
            if (exists) return;

            {
                var csharpQuizId = Guid.NewGuid();
           
                var q1Id = Guid.NewGuid();
                var q1CorrectOptionId = Guid.NewGuid();
                var q1WrongOptionId = Guid.NewGuid();


                var q2Id = Guid.NewGuid();
                var q2CorrectOptionId = Guid.NewGuid();
                var q2WrongOptionId = Guid.NewGuid();

                var diploma = new Diploma
                {
                    Id = webDevDiplomaId,
                    Title = "Full Stack Web Development",
                    Description = "Master HTML, CSS, JS, and .NET Core.",
                    Status = ContentStatus.Published,
                    CreatedAt = DateTime.UtcNow,
                    Quizzes = new List<Quiz>
                {
                    new Quiz
                    {
                        Id = csharpQuizId,
                        Title = "C# Basics",
                        DurationMinutes = 30,
                        PassScore = 50,
                        Status = ContentStatus.Published,
                        CreatedAt = DateTime.UtcNow,
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                Id = q1Id,
                                Text = "Which of the following is a Value Type in C#?",
                                Explanation = "Integers are structs in C#, making them Value Types.",
                                Options = new List<AnswerOption>
                                {
                                    new AnswerOption { Id = q1WrongOptionId, Text = "string", IsCorrect = false },
                                    new AnswerOption { Id = q1CorrectOptionId, Text = "int", IsCorrect = true }
                                }
                            },
                            new Question
                            {
                                Id = q2Id,
                                Text = "What is the default access modifier for a class in C#?",
                                Explanation = "If no access modifier is specified, a class defaults to internal.",
                                Options = new List<AnswerOption>
                                {
                                    new AnswerOption { Id = q2WrongOptionId, Text = "public", IsCorrect = false },
                                    new AnswerOption { Id = q2CorrectOptionId, Text = "internal", IsCorrect = true }
                                }
                            }
                        }
                    }
                }
                };



             
        
        
                diplomaRepo.Add(diploma);



                await unitOfWork.SaveChangesAsync();
        }
    }
    }

}