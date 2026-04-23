namespace ExaminationSystem.Api.Features.QuizEngine.Shared.Dtos
{
    public class QuizQuestionDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
         public string? Explanation { get; set; }
    }
}
