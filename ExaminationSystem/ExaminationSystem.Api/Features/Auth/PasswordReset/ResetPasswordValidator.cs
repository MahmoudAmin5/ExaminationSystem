using FluentValidation;
namespace ExaminationSystem.Api.Features.Auth.PasswordReset
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordOrchestrator>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(8).Matches(@"[A-Z]").Matches(@"[^a-zA-Z0-9]");
        }
    
    }
}
