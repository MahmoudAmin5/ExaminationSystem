namespace ExaminationSystem.Api.Features.QuizEngine.ViewResult.Dtos
{
    public class AttemptResultDto
    {
        public Guid AttemptId { get; init; }
        public Guid QuizId { get; init; }
        public string QuizTitle { get; init; } = string.Empty;
        public decimal Score { get; init; }
        public bool Passed { get; init; }
        public int TotalQuestions { get; init; }
        public int CorrectCount { get; init; }
        public string Status { get; init; } = string.Empty;
        public DateTime? SubmittedAt { get; init; }
        public List<QuestionResultDto> PerQuestion { get; init; } = [];
    }
}
