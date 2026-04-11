namespace ExaminationSystem.Api.Domain.Entities.Data
{
    public class AttemptAnswer : BaseEntity
    {
        public int AttemptId { get; set; }
        public int QuestionId { get; set; }
        public int SelectedOptionId { get; set; }
        public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;
        public bool IsModified { get; set; }
        public bool IsCorrect { get; set; }
        public virtual QuizAttempt Attempt { get; set; } = null!;
        public virtual Question Question { get; set; } = null!;
        public virtual AnswerOption SelectedOption { get; set; } = null!;
    }
}
