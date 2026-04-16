namespace ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos
{
    public record ShuffledOptionDto
    {
        public Guid OptionId { get; init; }
        public int DisplayOrder { get; init; }
    }
}
