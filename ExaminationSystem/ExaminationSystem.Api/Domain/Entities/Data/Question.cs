namespace ExaminationSystem.Api.Domain.Entities.Data
{
    public class Question : BaseEntity<Guid>
    {
        public string Text { get; set; } = string.Empty;
        public Guid QuizId { get; set; }
        public string? Explanation { get; set; }
        public int OrderIndex { get; set; } =1;

        public virtual Quiz Quiz { get; set; } = null!;
        public virtual ICollection<AnswerOption> Options { get; set; } = new List<AnswerOption>();
    }
}
