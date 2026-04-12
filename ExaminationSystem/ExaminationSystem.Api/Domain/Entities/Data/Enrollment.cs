using ExaminationSystem.Api.Domain.Entities.Account;

namespace ExaminationSystem.Api.Domain.Entities.Data
{
    public class Enrollment : BaseEntity<int>
    {
        public int StudentId { get; set; }
        public int DiplomaId { get; set; }
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
      
        public virtual User Student { get; set; } = null!;
        public virtual Diploma Diploma { get; set; } = null!;
    }
}
