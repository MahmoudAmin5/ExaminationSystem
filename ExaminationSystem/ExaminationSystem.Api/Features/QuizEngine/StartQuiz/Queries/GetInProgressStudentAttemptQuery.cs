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
            var inProgressAttempt = await _unitOfWork.Repository<QuizAttempt, Guid>()
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.StudentId == request.StudentId && a.QuizId == request.QuizId && a.Status == AttemptStatus.InProgress, cancellationToken);
            
            if (inProgressAttempt == null) return Result<QuizAttemptDto>.Failure(Error.NotFound("In-ProgressAttempts.NotFound","In-progress attempt not found"));
            
            var response = new QuizAttemptDto
            {
                AttemptId = inProgressAttempt.Id,
                Status = inProgressAttempt.Status,
                StartedAt = inProgressAttempt.StartedAt,
                Deadline = inProgressAttempt.Deadline

            };

            return Result<QuizAttemptDto>.Success(response);
        }
    }
}
