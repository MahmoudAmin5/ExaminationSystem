using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.Auth.Common.Commands
{
    public class LogActivity
    {
        public record LogUserActivityCommand(Guid UserId, ActivityType Type, string IpAddress) : IRequest<Result>;
        public class LogUserActivityCommandHandler : IRequestHandler<LogUserActivityCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            public LogUserActivityCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
            public async Task<Result> Handle(LogUserActivityCommand request, CancellationToken ct)
            {

                _unitOfWork.Repository<UserActivityLog, int>()
                    .Add(new UserActivityLog 
                {
                    UserId = request.UserId,
                    ActivityType = request.Type ,
                    IpAddress = request.IpAddress  
                });

                await _unitOfWork.SaveChangesAsync(ct);
                return Result.Success();
            }
        }
    }
}
