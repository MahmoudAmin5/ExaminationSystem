using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.Auth.VerifyOtp
{
    public record VerifyOtpCommand(string Email, string Code) : IRequest<Result>;
    
}
