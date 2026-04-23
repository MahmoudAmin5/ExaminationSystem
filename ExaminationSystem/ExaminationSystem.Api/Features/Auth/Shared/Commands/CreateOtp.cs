using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.Auth.Common.Commands
{
    public class CreateOtp
    {

        public record CreateAndSaveOtpCommand(string Email) : IRequest<Result<string>>;
        public class CreateAndSaveOtpCommandHandler : IRequestHandler<CreateAndSaveOtpCommand, Result<string>>
        {
            private readonly IUnitOfWork _unitOfWork;
            public CreateAndSaveOtpCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

            public async Task<Result<string>> Handle(CreateAndSaveOtpCommand request, CancellationToken ct)
            {
                var otpCode = new Random().Next(100000, 999999).ToString();
                var hashedOtp = BCrypt.Net.BCrypt.HashPassword(otpCode, 12);

                _unitOfWork.Repository<OtpCode, int>().Add(new OtpCode
                {
                    Email = request.Email, CodeHash = hashedOtp,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                    IsUsed = false, CreatedAt = DateTime.UtcNow 
                });
                await _unitOfWork.SaveChangesAsync(ct);

                return Result<string>.Success(otpCode);
            }
        }
    }
}
