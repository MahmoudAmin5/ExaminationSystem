using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Queries
{
    public record GetInProgressStudentAttemptQuery(Guid StudentId, Guid QuizId) : IRequest<Result<QuizAttemptDto>>;
    public class GetInProgressStudentAttemptQueryHandler : IRequestHandler<GetInProgressStudentAttemptQuery, Result<QuizAttemptDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetInProgressStudentAttemptQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<QuizAttemptDto>> Handle(GetInProgressStudentAttemptQuery request, CancellationToken cancellationToken)
        {
            var response = await _unitOfWork.Repository<QuizAttempt, Guid>()
                .AsNoTracking()
                .Where(a => a.StudentId == request.StudentId && a.QuizId == request.QuizId && a.Status == AttemptStatus.InProgress)
                .Select(a => new QuizAttemptDto
                {
                    AttemptId = a.Id,
                    Status = a.Status,
                    StartedAt = a.StartedAt,
                    Deadline = a.Deadline
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (response == null)
            {
                return Result<QuizAttemptDto>.Failure(Error.NotFound("In-ProgressAttempts.NotFound", "In-progress attempt not found"));
            }

            return Result<QuizAttemptDto>.Success(response);
        }
    }
}
   
