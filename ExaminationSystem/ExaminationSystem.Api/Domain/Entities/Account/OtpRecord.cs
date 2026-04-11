namespace ExaminationSystem.Api.Domain.Entities.Account
{
    public class OtpRecord : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string HashedOtp { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public int VerificationAttempts { get; set; }
        public int ResendCount { get; set; }
        public DateTime LastResentAt { get; set; } = DateTime.UtcNow;
        public bool IsUsed { get; set; }
    }
}
