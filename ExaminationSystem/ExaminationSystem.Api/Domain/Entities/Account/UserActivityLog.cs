using ExaminationSystem.Api.Domain.Enums;

namespace ExaminationSystem.Api.Domain.Entities.Account
{
    public class UserActivityLog : BaseEntity
    {
        public int UserId { get; set; }
        public ActivityType ActivityType { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string? Details { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
        public virtual User User { get; set; } = null!;
    }
}
