using ExaminationSystem.Api.Domain.Enums;

namespace ExaminationSystem.Api.Domain.Entities.Account
{
    public class UserActivityLog : BaseEntity<int>
    {
        public Guid UserId { get; set; }
        public ActivityType ActivityType { get; set; } 
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; } 

        public virtual User User { get; set; } = null!;
    }
}
