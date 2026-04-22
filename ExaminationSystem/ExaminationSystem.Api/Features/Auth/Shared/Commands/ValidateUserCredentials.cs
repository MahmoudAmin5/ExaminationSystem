using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Api.Features.Auth.Shared.Commands
{
    public class ValidateUserCredentials
    {
        
        public record ValidateUserCredentialsCommand(string Email, string Password) : IRequest<Result<User>>;

        public class ValidateUserCredentialsCommandHandler : IRequestHandler<ValidateUserCredentialsCommand, Result<User>>
        {
            private readonly UserManager<User> _userManager;
            public ValidateUserCredentialsCommandHandler(UserManager<User> userManager) => _userManager = userManager;

            public async Task<Result<User>> Handle(ValidateUserCredentialsCommand request, CancellationToken ct)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null) 
                    return Result<User>.Failure(Error.Unauthorized("Auth.Invalid", "Invalid credentials."));

                if (await _userManager.IsLockedOutAsync(user)) 
                    return Result<User>.Failure(Error.TooManyRequests("Auth.Locked", "Account locked."));

                if (user.Status != AccountStatus.Active) 
                    return Result<User>.Failure(Error.Forbidden("Auth.NotVerified", "Account not verified."));

                if (!await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    await _userManager.AccessFailedAsync(user);
                    return Result<User>.Failure(Error.Unauthorized("Auth.Invalid", "Invalid credentials."));
                }

                await _userManager.ResetAccessFailedCountAsync(user);
                return Result<User>.Success(user);
            }
        }

    }
}
