using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Features.Auth.Register;
using ExaminationSystem.Api.Infrastructure.Identity;
using ExaminationSystem.Api.Shared.Results;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Api.Features.Auth.Login
{
    public class Login
    {
       
        public record LoginResponse(string AccessToken, string RefreshToken);
        public record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;

        public class LoginHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
        {
            private readonly UserManager<User> _userManager;
            private readonly IValidator<LoginCommand> _validator;
            private readonly JwtProvider _jwtProvider;
            private readonly IUnitOfWork _unitOfWork;

            public LoginHandler(UserManager<User> userManager, JwtProvider jwtProvider, IUnitOfWork unitOfWork, IValidator<LoginCommand> validator)
            {
                _userManager = userManager;
                _jwtProvider = jwtProvider;
                _unitOfWork = unitOfWork;
                _validator = validator;
            }
         
            public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
            {

                
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                    return Result<LoginResponse>.Failure(validationResult.Errors.Select(e => Error.Validation(e.PropertyName, e.ErrorMessage)));

                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                    return Result<LoginResponse>.Failure(Error.Unauthorized("Auth.InvalidCredentials", "Invalid email or password."));

               
                if (await _userManager.IsLockedOutAsync(user))
                    return Result<LoginResponse>.Failure(Error.TooManyRequests("Auth.Locked", "Account is locked for 15 minutes due to 5 failed attempts."));

               
                if (user.Status != AccountStatus.Active)
                    return Result<LoginResponse>.Failure(Error.Forbidden("Auth.NotVerified", "Account not verified. Please verify your email first."));

               
                var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
                if (!passwordValid)
                {
                    await _userManager.AccessFailedAsync(user); // Increment failed attempts
                    return Result<LoginResponse>.Failure(Error.Unauthorized("Auth.InvalidCredentials", "Invalid email or password."));
                }

                // Reset failed attempts on success
                await _userManager.ResetAccessFailedCountAsync(user);

                var accessToken = _jwtProvider.GenerateToken(user);
                var refreshTokenValue = Guid.NewGuid().ToString();

               
                var refreshToken = new RefreshToken
                {
                    UserId = user.Id,
                    TokenHash = BCrypt.Net.BCrypt.HashPassword(refreshTokenValue),
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    IsUsed = false,
                    IsRevoked = false
                };

                _unitOfWork.Repository<RefreshToken, int>().Add(refreshToken);

                
                var log = new UserActivityLog { UserId = user.Id, ActivityType = ActivityType.Login };
                _unitOfWork.Repository<UserActivityLog, int>().Add(log);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<LoginResponse>.Success(new LoginResponse(accessToken, refreshTokenValue));
            }
        }
    }
}
