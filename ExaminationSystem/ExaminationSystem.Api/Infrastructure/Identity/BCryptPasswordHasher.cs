using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Api.Infrastructure.Identity
{
    public class BCryptPasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
    {
        public string HashPassword(TUser user, string password) => BCrypt.Net.BCrypt.HashPassword(password, 12);
        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
            => BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword)
                ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;

    }
}
