using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Features.Auth.Login;
using ExaminationSystem.Api.Infrastructure.Identity;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.Auth.Shared.Commands
{
    public class GenerateTokens
    {
        public record GenerateTokensCommand(User User) : IRequest<Result<LoginResponseDto>>;
        public class GenerateTokensCommandHandler : IRequestHandler<GenerateTokensCommand, Result<LoginResponseDto>>
        {
            private readonly JwtProvider _jwtProvider;
            private readonly IUnitOfWork _unitOfWork;
            public GenerateTokensCommandHandler(JwtProvider jwtProvider, IUnitOfWork unitOfWork)
            { 
                _jwtProvider = jwtProvider;
                _unitOfWork = unitOfWork; 
            }

            public async Task<Result<LoginResponseDto>> Handle(GenerateTokensCommand request, CancellationToken ct)
            {
                var accessToken = _jwtProvider.GenerateToken(request.User);
                var refreshTokenValue = Guid.NewGuid().ToString();

                _unitOfWork.Repository<RefreshToken, int>()
                    .Add(new RefreshToken 
                    { 
                        UserId = request.User.Id,
                        TokenHash = BCrypt.Net.BCrypt.HashPassword(refreshTokenValue),
                        ExpiresAt = DateTime.UtcNow.AddDays(7) 
                    });

                await _unitOfWork.SaveChangesAsync(ct);

                return Result<LoginResponseDto>.Success(new LoginResponseDto 
                { 
                    AccessToken = accessToken,
                    RefreshToken = refreshTokenValue 
                });
            }

        }
    }
}
