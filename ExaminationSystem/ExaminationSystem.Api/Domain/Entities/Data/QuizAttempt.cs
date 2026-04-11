using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Domain.Enums;

namespace ExaminationSystem.Api.Domain.Entities.Data
{
    public class QuizAttempt : BaseEntity
    {
        public int QuizId { get; set; }
        public int StudentId { get; set; }
        public AttemptStatus Status { get; set; } = AttemptStatus.InProgress;
        public DateTime StartedAt { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public double? Score { get; set; }
        public bool? Passed { get; set; }
        public int? ShuffleSeed { get; set; }
        public int CurrentQuestionIndex { get; set; } = 0;
        public virtual Quiz Quiz { get; set; } = null!;
        public virtual User Student { get; set; } = null!;
        public virtual ICollection<AttemptAnswer> Answers { get; set; } = new List<AttemptAnswer>();
    }
}
