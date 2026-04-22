using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Api.Features.Auth.Shared.Commands
{
    public class ActivateUser
    {
        public record ActivateUserCommand(string Email) : IRequest<Result>;

        public class ActivateUserCommandHandler : IRequestHandler<ActivateUserCommand, Result>
        {
            private readonly UserManager<User> _userManager;

            public ActivateUserCommandHandler(UserManager<User> userManager)
            {
                _userManager = userManager;
            }

            public async Task<Result> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                    return Result.Failure(Error.NotFound("User.NotFound", "User not found"));

                user.Status = AccountStatus.Active;
                user.EmailConfirmed = true;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    return Result.Failure(Error.Unexpected("User.UpdateFailed", "Failed to activate user account."));

                return Result.Success();
            }
        }
    }
}
