namespace ExaminationSystem.Api.Features.QuizEngine.AnswerQuestion.Dtos
{
    public class AnswerQuestionRequestDto
    {
        public Guid QuestionId { get; set; }
        public Guid SelectedOptionId { get; set; }
    }
}
