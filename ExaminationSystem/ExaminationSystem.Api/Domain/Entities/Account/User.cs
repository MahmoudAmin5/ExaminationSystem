using ExaminationSystem.Api.BuildingBlocks.Interfaces;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Api.Domain.Entities.Account
{
    public class User : IdentityUser<Guid>, ISoftDeletable
    {
        public string FullName { get; set; } = string.Empty;
       
        public UserRole Role { get; set; } = UserRole.Student;
        public AccountStatus Status { get; set; } = AccountStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } 
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public virtual ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();
        public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
