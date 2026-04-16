using ExaminationSystem.Api.BuildingBlocks.Interfaces;
using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

public record ResendOtpCommand(string Email) : IRequest<Result>;

public class ResendOtpHandler : IRequestHandler<ResendOtpCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public ResendOtpHandler(IUnitOfWork unitOfWork, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task<Result> Handle(ResendOtpCommand request, CancellationToken cancellationToken)
    {
        var otpRepo = _unitOfWork.Repository<OtpCode, int>();

       
        var oneHourAgo = DateTime.UtcNow.AddHours(-1);
        var resendCount = await otpRepo.CountAsync(
            x => x.Email == request.Email && x.CreatedAt > oneHourAgo,
            cancellationToken);

        if (resendCount >= 3)
        {
            return Result.Failure(Error.TooManyRequests("OTP.LimitExceeded", "Maximum 3 OTPs per hour allowed."));
        }

        
        var oldOtps = await otpRepo.FindAsync(
            x => x.Email == request.Email && !x.IsUsed,
            cancellationToken);

        if (oldOtps.Any())
        {
            foreach (var otp in oldOtps)
            {
                otp.IsUsed = true;
            }
            otpRepo.UpdateRange(oldOtps); 
        }

        
        var newCode = new Random().Next(100000, 999999).ToString();
        var hashedCode = BCrypt.Net.BCrypt.HashPassword(newCode, 12);

        var otpEntry = new OtpCode
        {
            Email = request.Email,
            CodeHash = hashedCode,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            IsUsed = false
        };

        
        otpRepo.Add(otpEntry);

       
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        
        var emailBody = $"<h1>New Code</h1><p>Your new verification code is: <b>{newCode}</b></p>";
        await _emailService.SendEmailAsync(request.Email, "Your New Verification Code", emailBody);


        return Result.Success();
    }
}