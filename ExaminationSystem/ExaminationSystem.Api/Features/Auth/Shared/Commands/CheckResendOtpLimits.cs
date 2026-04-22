using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.Auth.Shared.Commands
{
    public class CheckResendOtpLimits
    {
       
        public record CheckResendLimitAndInvalidateOldCommand(string Email) : IRequest<Result>;
        public class CheckResendLimitAndInvalidateOldCommandHandler : IRequestHandler<CheckResendLimitAndInvalidateOldCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            public CheckResendLimitAndInvalidateOldCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

            public async Task<Result> Handle(CheckResendLimitAndInvalidateOldCommand request, CancellationToken ct)
            {
                var otpRepo = _unitOfWork.Repository<OtpCode, int>();
                var oneHourAgo = DateTime.UtcNow.AddHours(-1);

                var resendCount = await otpRepo.CountAsync(x => x.Email == request.Email && x.CreatedAt > oneHourAgo, ct);
                if (resendCount >= 3) 
                    return Result.Failure(Error.TooManyRequests("OTP.Limit", "Max 3 OTPs per hour allowed."));

                var oldOtps = await otpRepo.FindAsync(x => x.Email == request.Email && !x.IsUsed, ct);
                foreach (var otp in oldOtps) otp.IsUsed = true;

                if (oldOtps.Any())
                { 
                    otpRepo.UpdateRange(oldOtps); 
                    await _unitOfWork.SaveChangesAsync(ct);
                }
                return Result.Success();
            }
        }
    }
}
