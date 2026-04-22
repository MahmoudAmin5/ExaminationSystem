using ExaminationSystem.Api.BuildingBlocks.Interfaces;
using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Shared.Results;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using static ExaminationSystem.Api.Features.Auth.Shared.Commands.GeneratePasswordResetToken;

namespace ExaminationSystem.Api.Features.Auth.ForgotPassword
{
    public record ForgotPasswordCommand(string Email) : IRequest<Result>;

    public class ForgotPasswordOrchestratorHandler : IRequestHandler<ForgotPasswordCommand, Result>
    {
        private readonly IMediator _mediator;
        private readonly IEmailService _emailService;

        public ForgotPasswordOrchestratorHandler(IMediator mediator, IEmailService emailService)
        {
            _mediator = mediator;
            _emailService = emailService;
        }

        public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            
            var tokenResult = await _mediator.Send(new GeneratePasswordResetTokenCommand(request.Email), cancellationToken);
            if (tokenResult.IsFailure) return Result.Failure(tokenResult.Errors);

            //  If token is empty string, user was not found > return success to prevent email enumeration
            if (string.IsNullOrEmpty(tokenResult.Value))
                return Result.Success();

            
            var resetLink = $"https://examination-system.com/reset-password?token={tokenResult.Value}&email={request.Email}";
            var emailBody = $"<h1>Reset Password</h1><p>Click <a href='{resetLink}'>here</a> to reset your password. Link expires in 15 mins.</p>";

            await _emailService.SendEmailAsync(request.Email, "Reset Your Password", emailBody);

            return Result.Success();
        }
    }
}
