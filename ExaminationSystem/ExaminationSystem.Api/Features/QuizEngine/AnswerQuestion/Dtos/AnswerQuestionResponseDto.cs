namespace ExaminationSystem.Api.Features.QuizEngine.AnswerQuestion.Dtos
{
    public class AnswerQuestionResponseDto
    {
        public Guid AttemptId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid? SelectedOptionId { get; set; }
        public bool Saved { get; set; }
        public DateTime AnsweredAt { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
