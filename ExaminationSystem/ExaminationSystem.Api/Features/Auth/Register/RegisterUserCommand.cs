using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Shared.Results;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Api.Features.Auth.Register
{
      
        public record RegisterUserCommand(string Email, string Password, string FullName) : IRequest<Result<Guid>>;
    
}
