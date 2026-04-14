namespace ExaminationSystem.Api.Domain.Entities.Data
{
    public class AttemptAnswer : BaseEntity<int>
    {
        public Guid AttemptId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid? SelectedOptionId { get; set; }
        public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;
        public bool? IsCorrect { get; set; }

        public virtual QuizAttempt Attempt { get; set; } = null!;
        public virtual Question Question { get; set; } = null!;
        public virtual AnswerOption? SelectedOption { get; set; } = null!;
    }
}
