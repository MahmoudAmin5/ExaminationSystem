namespace ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos
{
    public record ShuffleResult
    {
        public List<ShuffledQuestionDto> Questions { get; init; } = [];
    }
}
