namespace ExaminationSystem.Api.Features.QuizEngine.Shared.Dtos
{
    public class AttemptAnswerDto
    {
        public Guid AttemptId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid? SelectedOptionId { get; set; }
        public DateTime AnsweredAt { get; set; }
        public bool? IsCorrect { get; set; }
    }
}
