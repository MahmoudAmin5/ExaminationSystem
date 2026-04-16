using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Commands;
using ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos;
using ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Queries;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using System.Net.Quic;

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
           
            var quiz = await _mediator.Send(
                new GetPublishedQuizByIdQuery(request.QuizId), ct);

            if (quiz is null)
                return Result<StartQuizResponse>.Failure(
                Error.NotFound("Quiz.NotFound", $"Quiz with ID {request.QuizId} not found"));

            var existingAttempt = await _mediator.Send(
                new GetInProgressStudentAttemptQuery(request.StudentId, request.QuizId), ct);

            if (existingAttempt is not null)
                return Result<StartQuizResponse>.Failure(
               Error.Conflict("QuizAttempt", "An attempt is already in progress for this quiz."));

            var attemptCount = await _mediator.Send(
                new GetStudentAttemptCountQuery(request.StudentId, request.QuizId), ct);

            if (attemptCount.Value >= quiz.Value.MaxAttempts)
                return Result<StartQuizResponse>.Failure(
               Error.Forbidden("Quiz.AttemptLimitReached", "Maximum attempt limit reached for this quiz."));

            var quizQuestions = await _mediator.Send(
                new GetQuizQuestionsQuery(request.QuizId), ct);


            var attempt = await _mediator.Send(
                new CreateQuizAttemptCommand(
                    request.QuizId,
                    request.StudentId,
                    quiz.Value.DurationMinutes
                    ), ct);

            var shuffleResult = await _mediator.Send(
                new SaveShuffledQuestionsOrderCommand(
                    attempt.Value.AttemptId,
                    quizQuestions.Value.Questions,
                    quizQuestions.Value.Options), ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return StartQuizResponseBuilder.Build(
                quiz,
                attempt,
                shuffleResult,
                quizQuestions.Value.Questions,
                quizQuestions.Value.Options);
        }
    }
}
