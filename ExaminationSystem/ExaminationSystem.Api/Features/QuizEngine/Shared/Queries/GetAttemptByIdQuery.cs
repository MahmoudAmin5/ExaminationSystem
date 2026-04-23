using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.QuizEngine.Shared.Dtos;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.QuizEngine.Shared.Queries
{
    public record GetAttemptByIdQuery(Guid AttemptId) : IRequest<Result<QuizAttemptDto>>;
    public class GetAttemptByIdQueryHandler : IRequestHandler<GetAttemptByIdQuery, Result<QuizAttemptDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAttemptByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<QuizAttemptDto>> Handle(GetAttemptByIdQuery request, CancellationToken cancellationToken)
        {
            var quizAttemptRepo = _unitOfWork.Repository<QuizAttempt, Guid>();
            var response = await quizAttemptRepo
                .AsNoTracking()
                .Where(x => x.Id == request.AttemptId)
                .Select(x => new QuizAttemptDto
                {
                    Id = x.Id,
                    QuizId = x.QuizId,
                    StudentId = x.StudentId,
                    StartedAt = x.StartedAt,
                    Deadline = x.Deadline,
                    Status = x.Status,
                    Score = x.Score,
                    Passed = x.Passed,
                    SubmittedAt = x.SubmittedAt
                })
                .FirstOrDefaultAsync(cancellationToken);
            if (response is null)
            {
                return Result<QuizAttemptDto>.Failure(Error.NotFound("Attempt.NotFound", "Attempt not found"));
            }
            return Result<QuizAttemptDto>.Success(response);

        }
    }

}
