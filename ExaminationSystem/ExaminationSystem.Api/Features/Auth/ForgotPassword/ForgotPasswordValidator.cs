using FluentValidation;
using static ExaminationSystem.Api.Features.Auth.ForgotPassword.ForgotPasswordOrchestrator;

namespace ExaminationSystem.Api.Features.Auth.ForgotPassword
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordOrchestrator>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    
    }
}
