namespace ExaminationSystem.Api.Domain.Entities.Account
{
    public class UserSession : BaseEntity
    {
        public int UserId { get; set; }
        public string RefreshTokenHash { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public bool IsRevoked { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
