using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Features.QuizEngine.TimerHandling.Dtos;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.QuizEngine.TimerHandling.Queries
{
    public record GetAttemptTimerQuery(Guid AttemptId, Guid StudentId) : IRequest<Result<AttemptTimerDto>>;
    public class GetAttemptTimerQueryHandler : IRequestHandler<GetAttemptTimerQuery, Result<AttemptTimerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAttemptTimerQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<AttemptTimerDto>> Handle(GetAttemptTimerQuery request, CancellationToken cancellationToken)
        {
            var attempt = await _unitOfWork.Repository<QuizAttempt, Guid>().AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == request.AttemptId && a.StudentId == request.StudentId, cancellationToken);
            if (attempt is null) return Result<AttemptTimerDto>.Failure(Error.NotFound("QuizAttempt.NotFound", $"Quiz attempt with ID {request.AttemptId} not found for student {request.StudentId}"));
           
            if (attempt.Status != AttemptStatus.InProgress)
                return Result<AttemptTimerDto>.Failure(Error.Conflict("QuizAttempt.NotAvailable","This attempt is no longer in progress."));
           
              var secondsRemaining = Math.Max(0, (int)(attempt.Deadline - DateTime.UtcNow).TotalSeconds);
             
            return Result<AttemptTimerDto>.Success(new AttemptTimerDto
              {
                  AttemptId = attempt.Id,
                  SecondsReamning = DateTime.UtcNow.AddSeconds(secondsRemaining),
                  Deadline = attempt.Deadline,
                  IsExpired = secondsRemaining <= 0
              });

        }
    }
}
