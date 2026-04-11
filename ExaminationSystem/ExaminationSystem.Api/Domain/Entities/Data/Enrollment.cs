using ExaminationSystem.Api.Domain.Entities.Account;

namespace ExaminationSystem.Api.Domain.Entities.Data
{
    public class Enrollment : BaseEntity
    {
        public int StudentId { get; set; }
        public int DiplomaId { get; set; }
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public double ProgressPercentage { get; set; }
        public virtual User Student { get; set; } = null!;
        public virtual Diploma Diploma { get; set; } = null!;
    }
}
