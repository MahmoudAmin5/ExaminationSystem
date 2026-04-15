using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Shared.Results;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Api.Features.Auth.PasswordReset
{
    public class ResetPasswordpublic
    {

        public record ResetPasswordCommand(string Email, string Token, string NewPassword) : IRequest<Result>;

        public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, Result>
        {
            private readonly UserManager<User> _userManager;
            private readonly IUnitOfWork _unitOfWork;

            private readonly IValidator<ResetPasswordCommand> _validator;

            public ResetPasswordHandler(UserManager<User> userManager, IUnitOfWork unitOfWork, IValidator<ResetPasswordCommand> validator)
            {
                _userManager = userManager;
                _unitOfWork = unitOfWork;
                _validator = validator;
            }

         public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
            {
                
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                    return Result.Failure(validationResult.Errors.Select(e => Error.Validation(e.PropertyName, e.ErrorMessage)));

                
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                    return Result.Failure(Error.BadRequest("Reset.Invalid", "Invalid request parameters."));

              
                var tokenRepo = _unitOfWork.Repository<PasswordResetToken, int>();

                
                var tokens = await tokenRepo.FindAsync(x => x.UserId == user.Id && !x.IsUsed, cancellationToken);
                var latestToken = tokens.OrderByDescending(x => x.CreatedAt).FirstOrDefault();

                
                if (latestToken == null || latestToken.ExpiresAt < DateTime.UtcNow)
                    return Result.Failure(Error.BadRequest("Token.Invalid", "Token is invalid or has expired."));

                
                if (!BCrypt.Net.BCrypt.Verify(request.Token, latestToken.TokenHash))
                    return Result.Failure(Error.BadRequest("Token.Invalid", "Token is incorrect."));

               
                var removeResult = await _userManager.RemovePasswordAsync(user);
                if (!removeResult.Succeeded)
                    return Result.Failure(Error.Unexpected("Reset.Error", "Could not reset password."));

                var addResult = await _userManager.AddPasswordAsync(user, request.NewPassword);
                if (!addResult.Succeeded)
                    return Result.Failure(Error.Unexpected("Reset.Error", "Could not set new password."));

                var refreshTokenRepo = _unitOfWork.Repository<RefreshToken, int>();
                var activeSessions = await refreshTokenRepo.FindAsync(x => x.UserId == user.Id && !x.IsRevoked, cancellationToken);

                foreach (var session in activeSessions)
                {
                    session.IsRevoked = true;
                }
                refreshTokenRepo.UpdateRange(activeSessions);

                
                latestToken.IsUsed = true;
                tokenRepo.Update(latestToken);

               
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();

            }
        }
    }
}
