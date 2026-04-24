namespace ExaminationSystem.Api.Features.QuizEngine.Shared.Dtos
{
    public class QuestionOptionDto
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
