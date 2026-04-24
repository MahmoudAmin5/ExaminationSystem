using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.QuizEngine.Shared.Dtos;

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
        public List<AttemptAnswerDto> Answers { get; init; } = [];
        public List<QuizQuestionDto> Questions { get; init; } = [];
        public List<QuestionOptionDto> Options { get; init; } = [];
    }
}
