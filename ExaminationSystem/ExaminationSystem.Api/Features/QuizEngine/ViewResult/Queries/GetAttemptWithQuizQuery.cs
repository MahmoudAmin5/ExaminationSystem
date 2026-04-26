using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.QuizEngine.Shared.Queries;
using ExaminationSystem.Api.Features.QuizEngine.ViewResult.Dtos;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.QuizEngine.ViewResult.Queries
{
    public record GetAttemptWithQuizQuery(Guid AttemptId) : IRequest<Result<AttemptWithQuizDto>>;
    public class GetAttemptWithQuizQueryHandler
    : IRequestHandler<GetAttemptWithQuizQuery, Result<AttemptWithQuizDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public GetAttemptWithQuizQueryHandler(IUnitOfWork unitOfWork , IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Result<AttemptWithQuizDto>> Handle(GetAttemptWithQuizQuery request, CancellationToken cancellationToken)
        {
            var response = await _unitOfWork.Repository<QuizAttempt, Guid>()
                  .AsNoTracking()
                  .Where(a => a.Id == request.AttemptId)
                  .Select(a => new AttemptWithQuizDto
                  {
                      Attempt = new AttemptDto
                      {
                          Id = a.Id,
                          QuizId = a.QuizId,
                          StudentId = a.StudentId,
                          Status = a.Status.ToString(),
                          Score = a.Score,
                          Passed = a.Passed,
                          SubmittedAt = a.SubmittedAt
                      },
                      Quiz = new QuizDto
                      {
                          Id = a.Quiz.Id,
                          Title = a.Quiz.Title
                      }
                  })
                  .FirstOrDefaultAsync(cancellationToken);
            if (response is null)
            {
                return Result<AttemptWithQuizDto>.Failure(
                    Error.NotFound("QuizAttempt.NotFound", $"Quiz Attempt {request.AttemptId} not found."));
            }

            return Result<AttemptWithQuizDto>.Success(response);
        }
    }
}