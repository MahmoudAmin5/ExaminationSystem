using FluentValidation;

namespace ExaminationSystem.Api.Features.Auth.VerifyOtp
{
    public class VerifyOtpValidator : AbstractValidator<VerifyOtpOrchestrator>
    {
        public VerifyOtpValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Code).NotEmpty().Length(6).Matches(@"^\d+$").WithMessage("OTP must be 6 digits.");
        }
    }
}
