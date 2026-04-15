using MediatR;
using Microsoft.AspNetCore.Identity;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Domain.Contracts.Repository.Contract; 
using ExaminationSystem.Api.Shared.Results;
using FluentValidation;

namespace ExaminationSystem.Api.Features.Auth.VerifyOtp;

public class VerifyOtpHandler : IRequestHandler<VerifyOtpCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly IValidator<VerifyOtpCommand> _validator;

    public VerifyOtpHandler(IUnitOfWork unitOfWork, UserManager<User> userManager, IValidator<VerifyOtpCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _validator = validator;
    }

    public async Task<Result> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Failure(validationResult.Errors.Select(e => Error.Validation(e.PropertyName, e.ErrorMessage)));

       
        var otpRecord = await _unitOfWork.Repository<OtpCode, int>().FirstOrDefaultAsync(
            x => x.Email == request.Email && !x.IsUsed,
            cancellationToken);

        if (otpRecord == null)
            return Result.Failure(Error.NotFound("OTP.NotFound", "No active OTP found for this email"));

        
        if (otpRecord.ExpiresAt < DateTime.UtcNow)
            return Result.Failure(Error.Validation("OTP.Expired", "OTP has expired. Please request a new one"));

       
        if (otpRecord.AttemptCount >= 5)
            return Result.Failure(Error.TooManyRequests("OTP.Locked", "Too many incorrect attempts. Please resend a new OTP"));

        
        if (!BCrypt.Net.BCrypt.Verify(request.Code, otpRecord.CodeHash))
        {
            otpRecord.AttemptCount++;
            _unitOfWork.Repository<OtpCode, int>().Update(otpRecord); 
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Failure(Error.Validation("OTP.Incorrect", $"Incorrect OTP. {5 - otpRecord.AttemptCount} attempts remaining."));
        }

       
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return Result.Failure(Error.NotFound("User.NotFound", "User not found"));

        
        user.Status = AccountStatus.Active;
        user.EmailConfirmed = true;

       
        otpRecord.IsUsed = true;
        _unitOfWork.Repository<OtpCode, int>().Update(otpRecord);

      
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}