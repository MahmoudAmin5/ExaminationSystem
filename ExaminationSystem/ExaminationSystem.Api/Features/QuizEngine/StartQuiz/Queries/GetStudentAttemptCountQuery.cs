using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Queries
{
    public record GetStudentAttemptCountQuery (Guid StudentId, Guid QuizId) : IRequest<Result<int>>;

        public class GetStudentAttemptCountQueryHandler : IRequestHandler<GetStudentAttemptCountQuery, Result<int>>
        {
            private readonly IUnitOfWork _unitOfWork;
    
            public GetStudentAttemptCountQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            public async Task<Result<int>> Handle(GetStudentAttemptCountQuery request, CancellationToken cancellationToken)
            {
                var attemptCount = await _unitOfWork.Repository<QuizAttempt, Guid>()
                    .CountAsync(a => a.StudentId == request.StudentId && a.QuizId == request.QuizId, cancellationToken);
    
                return Result<int>.Success(attemptCount);
            }
    }

}
