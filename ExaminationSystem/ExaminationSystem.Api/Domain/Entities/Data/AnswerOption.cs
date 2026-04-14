namespace ExaminationSystem.Api.Domain.Entities.Data
{
    public class AnswerOption : BaseEntity<Guid>
    {
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } = false;
        public Guid QuestionId { get; set; }
    
        public virtual Question Question { get; set; } = null!;
    }
}
