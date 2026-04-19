using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.QuizEngine.AnswerQuestion.Queries
{
    public record GetAttemptWithValidationQuery(Guid AttemptId, Guid StudentId): IRequest<Result<QuizAttempt>>;
    public class GetAttemptWithValidationQueryHandler: IRequestHandler<GetAttemptWithValidationQuery, Result<QuizAttempt>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAttemptWithValidationQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<QuizAttempt>> Handle(
            GetAttemptWithValidationQuery request,
            CancellationToken cancellationToken)
        {
            var attempt = await _unitOfWork.Repository<QuizAttempt, Guid>()
                .FirstOrDefaultAsync(a =>
                    a.Id == request.AttemptId &&
                    a.StudentId == request.StudentId,
                    cancellationToken);

            if (attempt == null)
                return Result<QuizAttempt>.Failure(
                    Error.NotFound("Attempt.NotFound",$"Attempt with ID {request.AttemptId} not found or does not belong to student"));

            if (attempt.Status != AttemptStatus.InProgress)
                return Result<QuizAttempt>.Failure(
                    Error.Conflict("Attempt.NotInProgress", "This attempt is no longer in progress and cannot accept answers"));
            if (DateTime.UtcNow > attempt.Deadline)
                return Result<QuizAttempt>.Failure(Error.Gone("Attempt.Expired","The time limit for this quiz has expired"));
            return Result<QuizAttempt>.Success(attempt);
        }
    }
}
