using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.QuizEngine.Shared.Queries;
using ExaminationSystem.Api.Features.QuizEngine.ViewResult.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.QuizEngine.ViewResult.Queries
{
    public record GetAttemptWithQuizQuery(Guid AttemptId) : IRequest<AttemptWithQuizDto>;
    public class GetAttemptWithQuizQueryHandler
    : IRequestHandler<GetAttemptWithQuizQuery, AttemptWithQuizDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public GetAttemptWithQuizQueryHandler(IUnitOfWork unitOfWork , IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<AttemptWithQuizDto> Handle(GetAttemptWithQuizQuery request, CancellationToken cancellationToken)
        {
            var attempt = await _mediator.Send(new GetAttemptByIdQuery(request.AttemptId), cancellationToken);

            if (attempt.IsFailure)
                return null;

            var quiz = await _mediator.Send(new GetQuizByIdQuery(attempt.Value.QuizId), cancellationToken);

            if (quiz.IsFailure)
                return null;

            return new AttemptWithQuizDto
            {
                Attempt = new AttemptDto
                {
                    Id = attempt.Value.Id,
                    QuizId = attempt.Value.QuizId,
                    StudentId = attempt.Value.StudentId,
                    Status = attempt.Value.Status.ToString(),
                    Score = attempt.Value.Score,
                    Passed = attempt.Value.Passed,
                    SubmittedAt = attempt.Value.SubmittedAt
                },
                Quiz = new QuizDto
                {
                    Id = quiz.Value.Id,
                    Title = quiz.Value.Title
                }
            };
        }
    }
}
