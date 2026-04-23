using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Api.Features.Auth.Shared.Commands
{
    public class ChangeUserPassword
    {
        public record ChangeUserPasswordCommand(string Email, string NewPassword) : IRequest<Result>;

        public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, Result>
        {
            private readonly UserManager<User> _userManager;

            public ChangeUserPasswordCommandHandler(UserManager<User> userManager)
            {
                _userManager = userManager;
            }

            public async Task<Result> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null) 
                    return Result.Failure(Error.NotFound("User.NotFound", "User not found"));

                var removeResult = await _userManager.RemovePasswordAsync(user);
                if (!removeResult.Succeeded)
                    return Result.Failure(Error.Unexpected("Reset.Error", "Could not remove old password."));

                var addResult = await _userManager.AddPasswordAsync(user, request.NewPassword);
                if (!addResult.Succeeded)
                    return Result.Failure(Error.Unexpected("Reset.Error", "Could not set new password."));

                return Result.Success();
            }
        }
    }
}
