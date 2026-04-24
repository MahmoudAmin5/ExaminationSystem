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


            var exists = await diplomaRepo.AnyAsync(d => d.Id == webDevDiplomaId);
            if (exists) return;

            // 1. PRE-DEFINE IDs FOR RELATIONSHIPS
            {
            var csharpQuizId = Guid.NewGuid();

            // Question 1 IDs
            var q1Id = Guid.NewGuid();
            var q1CorrectOptionId = Guid.NewGuid();
            var q1WrongOptionId = Guid.NewGuid();

            // Question 2 IDs
            var q2Id = Guid.NewGuid();
            var q2CorrectOptionId = Guid.NewGuid();
            var q2WrongOptionId = Guid.NewGuid();

            // ==========================================
            // 2. BUILD THE DIPLOMA & QUIZZES
            // ==========================================
            var diploma = new Diploma
            {
                Id = webDevDiplomaId,
                Title = "Full Stack Web Development",
                Description = "Master HTML, CSS, JS, and .NET Core.",
                Status = ContentStatus.Published,
                CreatedAt = DateTime.UtcNoww,
                Quizzes = new List<Quiz>
                {
                    new Quiz
                    {
                        Id = csharpQuizId, // Using our variable!
                        Title = "C# Basics",
                        DurationMinutes = 30,
                        PassScore = 50,
                        Status = ContentStatus.Published,
                        CreatedAt = DateTime.UtcNow,
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                Id = q1Id, // Using our variable!
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
                                Id = q2Id, // Using our variable!
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
            enrollmentRepo.Add(new Enrollment { StudentId = studentId, DiplomaId = webDevDiplomaId });

            // ==========================================
            // 3. BUILD THE COMPLETED ATTEMPT
            // ==========================================
            var attemptId = Guid.NewGuid();
            var attempt = new QuizAttempt
            {
                Id = attemptId,
                QuizId = csharpQuizId,
                StudentId = studentId,
                StartedAt = DateTime.UtcNow.AddMinutes(-25), // Started 25 mins ago
                SubmittedAt = DateTime.UtcNow,               // Submitted right now
                Deadline = DateTime.UtcNow.AddMinutes(5),    // They finished 5 mins early
                Status = AttemptStatus.Submitted,            // Ensure this matches your Enum!
                Score = 50,                                  // They got 1 out of 2 right (50%)
                Passed = true,
                Answers = new List<AttemptAnswer>
                {
                    // Correct Answer
                    new AttemptAnswer
                    {
                        AttemptId = attemptId,
                        QuestionId = q1Id,
                        SelectedOptionId = q1CorrectOptionId,
                        IsCorrect = true,
                        AnsweredAt = DateTime.UtcNow.AddMinutes(-20)
                    },
                    // Wrong Answer
                    new AttemptAnswer
                    {
                        AttemptId = attemptId,
                        QuestionId = q2Id,
                        SelectedOptionId = q2WrongOptionId,
                        IsCorrect = false,
                        AnsweredAt = DateTime.UtcNow.AddMinutes(-10)
                    }
                }
            };

            attemptRepo.Add(attempt);

            // ==========================================
            // 4. SAVE EVERYTHING TO SQL SERVER
            // ==========================================
            await unitOfWork.SaveChangesAsync();
        }
    }
    }

}