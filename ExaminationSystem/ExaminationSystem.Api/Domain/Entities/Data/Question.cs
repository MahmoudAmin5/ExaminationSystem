namespace ExaminationSystem.Api.Domain.Entities.Data
{
    public class Question : BaseEntity
    {
        public string Text { get; set; } = string.Empty;
        public int QuizId { get; set; }
        public string? Explanation { get; set; }
        public int OrderIndex { get; set; }

        public virtual Quiz Quiz { get; set; } = null!;
        public virtual ICollection<AnswerOption> Options { get; set; } = new List<AnswerOption>();
    }
}
