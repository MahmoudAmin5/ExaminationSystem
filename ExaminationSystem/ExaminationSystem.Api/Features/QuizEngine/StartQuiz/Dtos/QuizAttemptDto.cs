using ExaminationSystem.Api.Domain.Enums;

namespace ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos
{
    public class QuizAttemptDto
    {
        public Guid AttemptId { get; set; }
        public Guid StudentId { get; set; }
        public Guid QuizId { get; set; }
        public AttemptStatus Status { get; set; }
        public DateTime StartedAt {  get; set; } 
        public DateTime Deadline { get; set; }

    }
}
