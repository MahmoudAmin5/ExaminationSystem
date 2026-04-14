using ExaminationSystem.Api.Domain.Entities.Account;

namespace ExaminationSystem.Api.Domain.Entities.Data
{
    public class Enrollment : BaseEntity<int>
    {
        public Guid StudentId { get; set; }
        public Guid DiplomaId { get; set; }
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
      
        public virtual User Student { get; set; } = null!;
        public virtual Diploma Diploma { get; set; } = null!;
    }
}
