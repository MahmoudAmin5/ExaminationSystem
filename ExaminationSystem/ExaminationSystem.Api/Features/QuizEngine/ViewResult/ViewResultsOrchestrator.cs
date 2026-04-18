using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Features.QuizEngine.ViewResult.Dtos;
using ExaminationSystem.Api.Features.QuizEngine.ViewResult.Queries;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.QuizEngine.ViewResult
{
    public record ViewResultsQuery(Guid AttemptId, Guid RequesterId, string RequesterRole) : IRequest<Result<AttemptResultDto>>;
    public class ViewResultsOrchestrator : IRequestHandler<ViewResultsQuery, Result<AttemptResultDto>>
    {
        private readonly IMediator _mediator;

        public ViewResultsOrchestrator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<AttemptResultDto>> Handle(ViewResultsQuery request,CancellationToken ct)
        {

            var attemptWithQuiz = await _mediator.Send(
                new GetAttemptWithQuizQuery(request.AttemptId), ct);

            if (attemptWithQuiz is null) return Result<AttemptResultDto>.Failure(Error.NotFound("QuizAttempt", $"QuizAttempt {request.AttemptId} not found."));

            var isOwner = attemptWithQuiz.Attempt.StudentId == request.RequesterId;
            var isAdmin = request.RequesterRole == "Admin";

            if (!isOwner && !isAdmin) return Result<AttemptResultDto>.Failure(Error.Forbidden("QuizAttempt.ForbiddenAccess", "You do not have permission to view these results."));

            if (attemptWithQuiz.Attempt.Status == AttemptStatus.InProgress.ToString()) return Result<AttemptResultDto>.Failure(Error.Forbidden(
                        "QuizAttempt.InProgress", "Results are not available until the attempt is submitted."));

            var answersDetail = await _mediator.Send(new GetAttemptAnswersDetailQuery(request.AttemptId, attemptWithQuiz.Attempt.QuizId), ct);

            return ViewResultsResponseBuilder.Build(attemptWithQuiz, answersDetail);
        }
    }
}
