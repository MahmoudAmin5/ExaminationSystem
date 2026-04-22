using ExaminationSystem.Api.BuildingBlocks.Interfaces;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Shared.Results;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using static ExaminationSystem.Api.Features.Auth.Common.Commands.CreateOtp;
using static ExaminationSystem.Api.Features.Auth.Common.Commands.CreateUser;

namespace ExaminationSystem.Api.Features.Auth.Register
{
      
        public record RegisterUserOrchestrator(string Email, string Password, string FullName) : IRequest<Result<Guid>>;

        public class RegisterUserOrchestratorHandler : IRequestHandler<RegisterUserOrchestrator, Result<Guid>>
        {

        private readonly IMediator _mediator; private readonly IEmailService _emailService;
        public RegisterUserOrchestratorHandler(IMediator mediator, IEmailService emailService) { _mediator = mediator; _emailService = emailService; } 
        public async Task<Result<Guid>> Handle(RegisterUserOrchestrator request, CancellationToken ct)
        {
            var userResult = await _mediator.Send(new CreateUserCommand(request.Email, request.Password, request.FullName), ct);
            if (userResult.IsFailure)
                return Result<Guid>.Failure(userResult.Errors);

            var otpResult = await _mediator.Send(new CreateAndSaveOtpCommand(request.Email), ct);

            await _emailService.SendEmailAsync(request.Email, "Verify Account", $"Your code is: {otpResult.Value}");
            return Result<Guid>.Success(userResult.Value.Id);
        }

        }


}
