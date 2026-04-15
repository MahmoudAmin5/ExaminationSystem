using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Domain.Enums;
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

        public RegisterUserHandler(UserManager<User> userManager, IValidator<RegisterUserCommand> validator)
        {
            _userManager = userManager;
            _validator = validator;
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

            return Result<Guid>.Success(user.Id);
        }
    }
}
