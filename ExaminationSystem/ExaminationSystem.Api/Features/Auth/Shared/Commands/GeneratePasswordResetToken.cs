using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Api.Features.Auth.Shared.Commands
{
    public class GeneratePasswordResetToken
    {

        public record GeneratePasswordResetTokenCommand(string Email) : IRequest<Result<string>>; // Returns the raw token string

        public class GeneratePasswordResetTokenCommandHandler : IRequestHandler<GeneratePasswordResetTokenCommand, Result<string>>
        {
            private readonly UserManager<User> _userManager;
            private readonly IUnitOfWork _unitOfWork;

            public GeneratePasswordResetTokenCommandHandler(UserManager<User> userManager, IUnitOfWork unitOfWork)
            {
                _userManager = userManager;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<string>> Handle(GeneratePasswordResetTokenCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                // Prevent enumeration. If user not found return empty string "Success"
                if (user == null)
                    return Result<string>.Success(string.Empty);

                var token = Guid.NewGuid().ToString();
                var resetToken = new PasswordResetToken
                {
                    UserId = user.Id,
                    TokenHash = BCrypt.Net.BCrypt.HashPassword(token),
                    ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                    IsUsed = false
                };

                _unitOfWork.Repository<PasswordResetToken, int>().Add(resetToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<string>.Success(token);
            }
        }
    }
}
