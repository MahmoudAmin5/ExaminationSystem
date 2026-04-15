using ExaminationSystem.Api.BuildingBlocks.Interfaces;
using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Shared.Results;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using static ExaminationSystem.Api.Features.Auth.Login.Login;

namespace ExaminationSystem.Api.Features.Auth.PasswordReset
{
    public class ForgotPassword
    {
       
        public record ForgotPasswordCommand(string Email) : IRequest<Result>;

        public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, Result>
        {
            private readonly UserManager<User> _userManager;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IEmailService _emailService;
            private readonly IValidator<ForgotPasswordCommand> _validator;

            public ForgotPasswordHandler(UserManager<User> userManager, IUnitOfWork unitOfWork, IEmailService emailService, IValidator<ForgotPasswordCommand> validator)
            {
                _userManager = userManager;
                _unitOfWork = unitOfWork;
                _emailService = emailService;
               _validator = validator;
            }

            public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                    return Result<LoginResponse>.Failure(validationResult.Errors.Select(e => Error.Validation(e.PropertyName, e.ErrorMessage)));

                var user = await _userManager.FindByEmailAsync(request.Email);

                
                if (user == null) return Result.Success();

               
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

              
                var resetLink = $"https://examination-system.com/reset-password?token={token}&email={user.Email}";
                var emailBody = $"<h1>Reset Password</h1><p>Click <a href='{resetLink}'>here</a> to reset your password. Link expires in 15 mins.</p>";

                await _emailService.SendEmailAsync(user.Email!, "Reset Your Password", emailBody);

                return Result.Success();
            }
        }
    }
}
