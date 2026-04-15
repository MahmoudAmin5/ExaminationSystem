using ExaminationSystem.Api.BuildingBlocks.Interfaces;
using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Infrastructure.Persistence;
using ExaminationSystem.Api.Shared.Results;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Api.Features.Auth.Register
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IValidator<RegisterUserCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public RegisterUserHandler(UserManager<User> userManager, IValidator<RegisterUserCommand> validator,
        IUnitOfWork unitOfWork , IEmailService emailService)
        {
            _userManager = userManager;
            _validator = validator;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => Error.Validation(e.PropertyName, e.ErrorMessage));

                return Result<Guid>.Failure(errors);
            }

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return Result<Guid>.Failure(Error.Conflict("Auth.EmailExists", "Email already registered."));
            }

            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                Status = AccountStatus.Pending,
                Role = UserRole.Student
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => Error.Validation(e.Code, e.Description));
                return Result<Guid>.Failure(errors);
            }

            await _userManager.AddToRoleAsync(user, "Student");

          
            var otpCode = new Random().Next(100000, 999999).ToString();

          
            var hashedOtp = BCrypt.Net.BCrypt.HashPassword(otpCode, 12);

           
            var otpEntry = new OtpCode
            {
                Email = user.Email!,
                CodeHash = hashedOtp,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10), 
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWork.Repository<OtpCode, int>().Add(otpEntry);

            await _unitOfWork.SaveChangesAsync(cancellationToken);


            var emailBody = $"<h1>Welcome!</h1><p>Your verification code is: <b>{otpCode}</b></p>";
            await _emailService.SendEmailAsync(user.Email!, "Verify Your Account", emailBody);


            return Result<Guid>.Success(user.Id);

           
        }
    }
}
