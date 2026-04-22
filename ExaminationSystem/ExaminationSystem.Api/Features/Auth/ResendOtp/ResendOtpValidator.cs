using FluentValidation;

namespace ExaminationSystem.Api.Features.Auth.ResendOtp
{
    public class ResendOtpValidator : AbstractValidator<ResendOtpOrchestrator>
    {
        public ResendOtpValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }

}
