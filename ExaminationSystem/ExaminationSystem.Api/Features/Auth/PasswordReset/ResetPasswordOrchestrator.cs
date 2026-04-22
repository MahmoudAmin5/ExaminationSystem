using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Shared.Results;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using static ExaminationSystem.Api.Features.Auth.Shared.Commands.ChangeUserPassword;
using static ExaminationSystem.Api.Features.Auth.Shared.Commands.RevokeUserSessions;
using static ExaminationSystem.Api.Features.Auth.Shared.Commands.ValidateAndConsumeResetToken;

namespace ExaminationSystem.Api.Features.Auth.PasswordReset
{
        public record ResetPasswordCommand(string Email, string Token, string NewPassword) : IRequest<Result>;

        public class ResetPasswordOrchestratorHandler : IRequestHandler<ResetPasswordCommand, Result>
        {
            private readonly IMediator _mediator;

            public ResetPasswordOrchestratorHandler(IMediator mediator)
            {
                _mediator = mediator;
            }

            public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
            {
              
                var tokenValidationResult = await _mediator.Send(new ValidateAndConsumeResetTokenCommand(request.Email, request.Token), cancellationToken);
                if (tokenValidationResult.IsFailure) 
                return Result.Failure(tokenValidationResult.Errors);

               
                var passwordChangeResult = await _mediator.Send(new ChangeUserPasswordCommand(request.Email, request.NewPassword), cancellationToken);
                if (passwordChangeResult.IsFailure) 
                return Result.Failure(passwordChangeResult.Errors);

                
                var revokeSessionsResult = await _mediator.Send(new RevokeUserSessionsCommand(request.Email), cancellationToken);
                if (revokeSessionsResult.IsFailure) 
                return Result.Failure(revokeSessionsResult.Errors);

                return Result.Success();
            }
        }
    }

