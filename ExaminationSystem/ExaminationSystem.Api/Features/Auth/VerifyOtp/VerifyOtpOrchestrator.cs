using ExaminationSystem.Api.Shared.Results;
using MediatR;
using static ExaminationSystem.Api.Features.Auth.Shared.Commands.ActivateUser;
using static ExaminationSystem.Api.Features.Auth.Shared.Commands.CheckAndConsumeOtp;

namespace ExaminationSystem.Api.Features.Auth.VerifyOtp
{
    public record VerifyOtpCommand(string Email, string Code) : IRequest<Result>;
    public class VerifyOtpOrchestratorHandler : IRequestHandler<VerifyOtpCommand, Result>
    {
        private readonly IMediator _mediator;

        public VerifyOtpOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            
            var otpCheckResult = await _mediator.Send(new CheckAndConsumeOtpCommand(request.Email, request.Code), cancellationToken);
            if (otpCheckResult.IsFailure) 
                return Result.Failure(otpCheckResult.Errors);

            
            var activationResult = await _mediator.Send(new ActivateUserCommand(request.Email), cancellationToken);
            if (activationResult.IsFailure) 
                return Result.Failure(activationResult.Errors);

            return Result.Success();
        }
    }
}
