namespace ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos
{
    public record ShuffledQuestionDto
    {
        public Guid QuestionId { get; init; }
        public int DisplayOrder { get; init; }
        public List<ShuffledOptionDto> Options { get; init; } = [];
    }
}
