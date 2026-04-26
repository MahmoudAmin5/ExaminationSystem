using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.QuizEngine.Shared.Dtos;

namespace ExaminationSystem.Api.Features.QuizEngine.ViewResult.Dtos
{
    public record AttemptAnswersDetailDto
    {
        public Guid AttemptId { get; init; }
        public decimal TotalScore { get; init; } 
        public IReadOnlyList<ReviewedQuestionDto> ReviewedQuestions { get; init; } = [];
    }

   
    public record ReviewedQuestionDto
    {
        public Guid QuestionId { get; init; }
        public string Text { get; init; } = string.Empty;
        public string? Explanation { get; init; }

     
        public IReadOnlyList<ReviewedOptionDto> Options { get; init; } = [];

       
        public StudentAnswerDto? StudentAnswer { get; init; }
    }

   
    public record ReviewedOptionDto
    {
        public Guid OptionId { get; init; }
        public string Text { get; init; } = string.Empty;

        
        public bool IsCorrect { get; init; }
    }

    
    public record StudentAnswerDto
    {
        public Guid? SelectedOptionId { get; init; }
        public bool? IsCorrect { get; init; }
    }
}

