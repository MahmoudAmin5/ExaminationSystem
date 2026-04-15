using FluentValidation;
using static ExaminationSystem.Api.Features.Auth.PasswordReset.ForgotPassword;

namespace ExaminationSystem.Api.Features.Auth.PasswordReset
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    
    }
}
