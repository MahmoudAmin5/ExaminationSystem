using ExaminationSystem.Api.Domain.Entities.Data;

namespace ExaminationSystem.Api.Features.QuizEngine.ViewResult.Dtos
{
    public class QuestionResultDto
    {
        public Guid QuestionId { get; init; }
        public string Text { get; init; } = string.Empty;
        public string? Explanation { get; init; }
        public Guid StudentAnswerId { get; init; }
        public string StudentAnswerText { get; init; } = string.Empty;
        public Guid CorrectAnswerId { get; init; }
        public string CorrectAnswerText { get; init; } = string.Empty;
        public bool IsCorrect { get; init; }
    }
}
