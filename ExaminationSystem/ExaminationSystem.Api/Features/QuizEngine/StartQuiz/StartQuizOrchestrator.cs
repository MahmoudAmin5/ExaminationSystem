using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Commands;
using ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos;
using ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Queries;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.QuizEngine.StartQuiz
{
    public record StartQuizCommand(Guid QuizId, Guid StudentId) : IRequest<Result<StartQuizResponse>>;
    public class StartQuizOrchestrator
    : IRequestHandler<StartQuizCommand, Result<StartQuizResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public StartQuizOrchestrator(
            IMediator mediator,
            IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<StartQuizResponse>> Handle( StartQuizCommand request,CancellationToken ct)
        {

            var quiz = await _mediator.Send(new GetPublishedQuizByIdQuery(request.QuizId), ct);
            if (quiz.IsFailure) return Result<StartQuizResponse>.Failure(quiz.Errors);

            var existingAttempt = await _mediator.Send(new GetInProgressStudentAttemptQuery(request.StudentId, request.QuizId), ct);
            if (existingAttempt.IsSuccess)
                return Result<StartQuizResponse>.Failure(Error.Conflict("QuizAttempt", "An attempt is already in progress for this quiz."));

            var attemptCount = await _mediator.Send(new GetStudentAttemptCountQuery(request.StudentId, request.QuizId), ct);
            if (attemptCount.IsFailure) return Result<StartQuizResponse>.Failure(attemptCount.Errors);

            if (quiz.Value.MaxAttempts.HasValue && attemptCount.Value >= quiz.Value.MaxAttempts.Value)
                return Result<StartQuizResponse>.Failure(Error.Forbidden("Quiz.AttemptLimitReached", "Maximum attempt limit reached for this quiz."));

            var quizData = await _mediator.Send(new GetStartQuizQuestionsQuery(request.QuizId), ct);
            if (quizData.IsFailure) return Result<StartQuizResponse>.Failure(quizData.Errors);

            var attempt = await _mediator.Send(new CreateQuizAttemptCommand(request.StudentId, request.QuizId, quiz.Value.DurationMinutes), ct);
            if (attempt.IsFailure) return Result<StartQuizResponse>.Failure(attempt.Errors);

            var shuffleResult = await _mediator.Send(
                new SaveShuffledQuestionsOrderCommand(
                    attempt.Value.AttemptId,
                    quizData.Value.Questions), ct);

            if (shuffleResult.IsFailure) return Result<StartQuizResponse>.Failure(shuffleResult.Errors);

            await _unitOfWork.SaveChangesAsync(ct);

            return StartQuizResponseBuilder.Build(
                quiz,
                attempt,
                shuffleResult,
                quizData.Value.Questions);
        }
    }
}
