namespace ExaminationSystem.Api.Domain.Entities.Account
{
    public class PasswordResetToken : BaseEntity<int>
    {
        public Guid UserId { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;
        public virtual User User { get; set; } = null!;
    }

}
