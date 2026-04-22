using ExaminationSystem.Api.BuildingBlocks.Interfaces;
using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using static ExaminationSystem.Api.Features.Auth.Common.Commands.CreateOtp;
using static ExaminationSystem.Api.Features.Auth.Shared.Commands.CheckResendOtpLimits;

public record ResendOtpCommand(string Email) : IRequest<Result>;

public class ResendOtpOrchestratorHandler : IRequestHandler<ResendOtpCommand, Result>
{
    private readonly IMediator _mediator; private readonly IEmailService _emailService;
    public ResendOtpOrchestratorHandler(IMediator mediator, IEmailService emailService)
    { 
        _mediator = mediator;
        _emailService = emailService;
    }
    public async Task<Result> Handle(ResendOtpCommand request, CancellationToken ct)
    {
        var checkLimit = await _mediator.Send(new CheckResendLimitAndInvalidateOldCommand(request.Email), ct);

        if (checkLimit.IsFailure) 
            return Result.Failure(checkLimit.Errors);

        var newOtp = await _mediator.Send(new CreateAndSaveOtpCommand(request.Email), ct);

        await _emailService.SendEmailAsync(request.Email, "New OTP", $"Your new code is: {newOtp.Value}");
        return Result.Success();
    }
}
