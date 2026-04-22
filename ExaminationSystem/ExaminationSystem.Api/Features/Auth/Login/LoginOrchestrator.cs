using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Features.Auth.Register;
using ExaminationSystem.Api.Infrastructure.Identity;
using ExaminationSystem.Api.Shared.Results;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using static ExaminationSystem.Api.Features.Auth.Common.Commands.LogActivity;
using static ExaminationSystem.Api.Features.Auth.Shared.Commands.GenerateTokens;
using static ExaminationSystem.Api.Features.Auth.Shared.Commands.ValidateUserCredentials;

namespace ExaminationSystem.Api.Features.Auth.Login
{
    public record LoginCommand(string Email, string Password, string IpAddress) : IRequest<Result<LoginResponseDto>>;
    public class LoginOrchestratorHandler : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
    {
        private readonly IMediator _mediator;
        public LoginOrchestratorHandler(IMediator mediator) 
            => _mediator = mediator;

        public async Task<Result<LoginResponseDto>> Handle(LoginCommand request, CancellationToken ct)
        {
            var userResult = await _mediator.Send(new ValidateUserCredentialsCommand(request.Email, request.Password), ct);
            if (userResult.IsFailure) return Result<LoginResponseDto>.Failure(userResult.Errors);

            var tokenResult = await _mediator.Send(new GenerateTokensCommand(userResult.Value), ct);

            await _mediator.Send(new LogUserActivityCommand(userResult.Value.Id, ActivityType.Login, request.IpAddress), ct);
            return Result<LoginResponseDto>.Success(tokenResult.Value);
        }

    }
}
