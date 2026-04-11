namespace ExaminationSystem.Api.Domain.Entities.Account
{
    public class RevokedToken : BaseEntity
    {
        public string TokenHash { get; set; } = string.Empty;
        public DateTime RevokedAt { get; set; } = DateTime.UtcNow;
        public string Reason { get; set; } = string.Empty;
    }
}
