namespace ExaminationSystem.Api.Features.QuizEngine.ViewResult.Dtos
{
    public record AnswerDetailDto
    {
        public Guid QuestionId { get; init; }
        public Guid? SelectedOptionId { get; init; }
        public bool IsCorrect { get; init; }
    }

    public record QuestionDetailDto
    {
        public Guid Id { get; init; }
        public string Text { get; init; } = string.Empty;
        public string? Explanation { get; init; }
    }

    public record OptionDetailDto
    {
        public Guid Id { get; init; }
        public Guid QuestionId { get; init; }
        public string Text { get; init; } = string.Empty;
        public bool IsCorrect { get; init; }
    }

    public record AttemptAnswersDetailDto
    {
        public List<AnswerDetailDto> Answers { get; init; } = [];
        public List<QuestionDetailDto> Questions { get; init; } = [];
        public List<OptionDetailDto> Options { get; init; } = [];
    }
}
