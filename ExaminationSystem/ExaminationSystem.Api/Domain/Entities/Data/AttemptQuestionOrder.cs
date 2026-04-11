namespace ExaminationSystem.Api.Domain.Entities.Data
{
    public class AttemptQuestionOrder : BaseEntity
    {
        public int AttemptId { get; set; }
        public int QuestionId { get; set; }
        public int DisplayOrder { get; set; }
        public virtual QuizAttempt Attempt { get; set; } = null!;
        public virtual Question Question { get; set; } = null!;
    }
}
