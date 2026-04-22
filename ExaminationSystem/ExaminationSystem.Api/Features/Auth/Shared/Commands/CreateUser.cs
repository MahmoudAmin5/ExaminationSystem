using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Api.Features.Auth.Common.Commands
{
    public class CreateUser
    {
      
        public record CreateUserCommand(string Email, string Password, string FullName) : IRequest<Result<User>>;
        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<User>>
        {
            private readonly UserManager<User> _userManager;
            public CreateUserCommandHandler(UserManager<User> userManager) => _userManager = userManager;

            public async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken ct)
            {
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null) 
                    return Result<User>.Failure(Error.Conflict("Auth.EmailExists", "Email already registered."));

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
                    return Result<User>.Failure(Error.Unexpected("Auth.CreateFailed", "Failed to create user."));

                await _userManager.AddToRoleAsync(user, "Student");
                return Result<User>.Success(user);
            }
        }


    }
}
