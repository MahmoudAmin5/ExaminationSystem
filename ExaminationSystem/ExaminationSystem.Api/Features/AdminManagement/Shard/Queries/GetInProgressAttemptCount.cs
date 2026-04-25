using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.AdminManagement.Shard.Queries
{
    public record GetInProgressAttemptCount(Guid quizId) : IRequest<int>;
    public class GetInProgressAttemptCountHandler : IRequestHandler<GetInProgressAttemptCount, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetInProgressAttemptCountHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(GetInProgressAttemptCount request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<QuizAttempt, Guid>()
                .AsNoTracking()
                .Where(x => x.QuizId == request.quizId && x.Status == AttemptStatus.InProgress)
                .CountAsync(cancellationToken);
        }
    }
}
