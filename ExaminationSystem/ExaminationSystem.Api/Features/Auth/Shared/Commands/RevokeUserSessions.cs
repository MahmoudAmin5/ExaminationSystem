using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Api.Features.Auth.Shared.Commands
{
    public class RevokeUserSessions
    {
        public record RevokeUserSessionsCommand(string Email) : IRequest<Result>;

        public class RevokeUserSessionsCommandHandler : IRequestHandler<RevokeUserSessionsCommand, Result>
        {
            private readonly UserManager<User> _userManager;
            private readonly IUnitOfWork _unitOfWork;

            public RevokeUserSessionsCommandHandler(UserManager<User> userManager, IUnitOfWork unitOfWork)
            {
                _userManager = userManager;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(RevokeUserSessionsCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null) return Result.Failure(Error.NotFound("User.NotFound", "User not found"));

                var refreshTokenRepo = _unitOfWork.Repository<RefreshToken, int>();
                var activeSessions = await refreshTokenRepo.FindAsync(x => x.UserId == user.Id && !x.IsRevoked, cancellationToken);

                foreach (var session in activeSessions)
                {
                    session.IsRevoked = true;
                }

                if (activeSessions.Any())
                {
                    refreshTokenRepo.UpdateRange(activeSessions);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }

                return Result.Success();
            }
        }
    }
}
