namespace ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos
{
    public record StartQuizResponse
    {
        public Guid AttemptId { get; init; }
        public Guid QuizId { get; init; }
        public string Title { get; init; } = string.Empty;
        public string? Instructions { get; init; }
        public int DurationMinutes { get; init; }
        public DateTime StartedAt { get; init; }
        public DateTime Deadline { get; init; }
        public int TotalQuestions { get; init; }
        public List<QuestionDto> Questions { get; init; } = [];
    }

    public record QuestionDto
    {
        public Guid QuestionId { get; init; }
        public string Text { get; init; } = string.Empty;
        public int DisplayOrder { get; init; }
        public List<OptionDto> Options { get; init; } = [];
    }

    public record OptionDto
    {
        public Guid OptionId { get; init; }
        public string Text { get; init; } = string.Empty;
        public int DisplayOrder { get; init; }
    }
}
