namespace ExaminationSystem.Api.Features.QuizEngine.ViewResult.Dtos
{
    public class AttemptDto
    {
        public Guid Id { get; init; }
        public Guid QuizId { get; init; }
        public Guid StudentId { get; init; }
        public string Status { get; init; } = string.Empty;
        public decimal? Score { get; init; }
        public bool? Passed { get; init; }
        public int? TotalQuestions { get; init; }
        public DateTime? SubmittedAt { get; init; }
    }
}
