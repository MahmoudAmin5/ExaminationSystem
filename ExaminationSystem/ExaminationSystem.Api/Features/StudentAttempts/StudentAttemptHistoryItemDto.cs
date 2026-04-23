namespace ExaminationSystem.Api.Features.StudentAttempts
{
    public class StudentAttemptHistoryItemDto
    {
        public Guid AttemptId { get; init; }
        public Guid QuizId { get; init; }
        public string QuizTitle { get; init; } = string.Empty;
        public Guid DiplomaId { get; init; }
        public string DiplomaTitle { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public decimal? Score { get; init; }
        public bool? Passed { get; init; }
        public int TotalQuestions { get; init; }
        public DateTime StartedAt { get; init; }
        public DateTime? SubmittedAt { get; init; }
    }
}
