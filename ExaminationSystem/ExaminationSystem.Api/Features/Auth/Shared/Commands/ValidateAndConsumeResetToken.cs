using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Api.Features.Auth.Shared.Commands
{
    public class ValidateAndConsumeResetToken
    {
        public record ValidateAndConsumeResetTokenCommand(string Email, string Token) : IRequest<Result>;

        public class ValidateAndConsumeResetTokenCommandHandler : IRequestHandler<ValidateAndConsumeResetTokenCommand, Result>
        {
            private readonly UserManager<User> _userManager;
            private readonly IUnitOfWork _unitOfWork;

            public ValidateAndConsumeResetTokenCommandHandler(UserManager<User> userManager, IUnitOfWork unitOfWork)
            {
                _userManager = userManager;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(ValidateAndConsumeResetTokenCommand request, CancellationToken cancellationToken)
            {
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

                // Consume the token
                latestToken.IsUsed = true;
                tokenRepo.Update(latestToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
        }
    }
}
