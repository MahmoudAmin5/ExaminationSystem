using ExaminationSystem.Api.Domain.Enums;

namespace ExaminationSystem.Api.Features.QuizEngine.Shared.Dtos
{
    public class QuizAttemptDto
    {
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        public Guid StudentId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime Deadline { get; set; }
        public AttemptStatus Status { get; set; }
        public decimal? Score { get; set; }
        public bool? Passed { get; set; }
        public DateTime? SubmittedAt { get; set; }
    }

}
