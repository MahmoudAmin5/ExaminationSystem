namespace ExaminationSystem.Api.Features.QuizEngine.ViewResult.Dtos
{
    public class AttemptWithQuizDto
    {
        public AttemptDto Attempt { get; init; } = null!;
        public QuizDto Quiz { get; init; } = null!;
    }
}
