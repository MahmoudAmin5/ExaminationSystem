namespace ExaminationSystem.Api.Domain.Entities.Data
{
    public class AttemptOptionOrder
    {
        public Guid AttemptId { get; set; }
        public Guid OptionId { get; set; }
        public int DisplayOrder { get; set; } 
        public virtual QuizAttempt Attempt { get; set; } = null!;
        public virtual AnswerOption Option { get; set; } = null!;
    }
}
