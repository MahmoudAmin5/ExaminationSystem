using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.Auth.Shared.Commands
{
    public class CheckAndConsumeOtp
    {

        public record CheckAndConsumeOtpCommand(string Email, string Code) : IRequest<Result>;

        public class CheckAndConsumeOtpCommandHandler : IRequestHandler<CheckAndConsumeOtpCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CheckAndConsumeOtpCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(CheckAndConsumeOtpCommand request, CancellationToken cancellationToken)
            {
                var otpRecord = await _unitOfWork.Repository<OtpCode, int>().FirstOrDefaultAsync(
                    x => x.Email == request.Email && !x.IsUsed, cancellationToken);

                if (otpRecord == null)
                    return Result.Failure(Error.NotFound("OTP.NotFound", "No active OTP found for this email."));

                if (otpRecord.ExpiresAt < DateTime.UtcNow)
                    return Result.Failure(Error.Validation("OTP.Expired", "OTP has expired. Please request a new one."));

                if (otpRecord.AttemptCount >= 5)
                    return Result.Failure(Error.TooManyRequests("OTP.Locked", "Too many incorrect attempts. Please resend a new OTP."));

                if (!BCrypt.Net.BCrypt.Verify(request.Code, otpRecord.CodeHash))
                {
                    otpRecord.AttemptCount++;
                    _unitOfWork.Repository<OtpCode, int>().Update(otpRecord);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    return Result.Failure(Error.Validation("OTP.Incorrect", $"Incorrect OTP. {5 - otpRecord.AttemptCount} attempts remaining."));
                }

                // OTP is correct > consume it
                otpRecord.IsUsed = true;
                _unitOfWork.Repository<OtpCode, int>().Update(otpRecord);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
        }
    }
}

