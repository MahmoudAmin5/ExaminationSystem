namespace ExaminationSystem.Api.Domain.Entities.Data
{
    public class AnswerOption : BaseEntity
    {
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
        public int OrderIndex { get; set; }
        public virtual Question Question { get; set; } = null!;
    }
}
