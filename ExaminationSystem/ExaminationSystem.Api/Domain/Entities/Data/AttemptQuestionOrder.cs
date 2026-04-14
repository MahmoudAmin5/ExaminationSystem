namespace ExaminationSystem.Api.Domain.Entities.Data
{
    public class AttemptQuestionOrder 
    {
        public Guid AttemptId { get; set; }
        public Guid QuestionId { get; set; }
        public int DisplayOrder { get; set; }
        public virtual QuizAttempt Attempt { get; set; } = null!;
        public virtual Question Question { get; set; } = null!;
    }
}
