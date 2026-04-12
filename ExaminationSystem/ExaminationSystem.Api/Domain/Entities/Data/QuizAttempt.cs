using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Domain.Enums;

namespace ExaminationSystem.Api.Domain.Entities.Data
{
    public class QuizAttempt : BaseEntity<Guid>
    {
        public Guid QuizId { get; set; }
        public Guid StudentId { get; set; }
        public AttemptStatus Status { get; set; } = AttemptStatus.InProgress;
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime Deadline { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public decimal? Score { get; set; }
        public bool? Passed { get; set; }
       
        public virtual Quiz Quiz { get; set; } = null!;
        public virtual User Student { get; set; } = null!;
        public virtual ICollection<AttemptAnswer> Answers { get; set; } = new List<AttemptAnswer>();
    }
}
