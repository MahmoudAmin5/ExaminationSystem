using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Features.QuizEngine.TimerHandling.Dtos;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.QuizEngine.TimerHandling.Commands
{
    public record AutoSubmitExpiredAttemptCommand(Guid AttemptId) : IRequest<Result<AutoSubmitDto>>;

    public class AutoSubmitExpiredAttemptCommandHandler : IRequestHandler<AutoSubmitExpiredAttemptCommand, Result<AutoSubmitDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AutoSubmitExpiredAttemptCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<AutoSubmitDto>> Handle(AutoSubmitExpiredAttemptCommand request, CancellationToken cancellationToken)
        {
            var attempt = await _unitOfWork.Repository<QuizAttempt, Guid>()
                .GetByIdAsync(request.AttemptId, cancellationToken);

            if (attempt == null)
            {
                return Result<AutoSubmitDto>.Failure(
                    Error.NotFound("AttemptNotFound", $"No quiz attempt found with ID {request.AttemptId}"));
            }

            if (DateTime.UtcNow < attempt.Deadline)
            {
                return Result<AutoSubmitDto>.Failure(
                    Error.BadRequest("AttemptNotExpired", $"Quiz attempt with ID {request.AttemptId} has not expired yet"));
            }
            var answers = await _unitOfWork.Repository<AttemptAnswer, int>()
                .FindAsync(a => a.AttemptId == attempt.Id, cancellationToken);
            var quiz = await _unitOfWork.Repository<Quiz, Guid>()
                .GetByIdAsync(attempt.QuizId, cancellationToken);

            var questions = await _unitOfWork.Repository<Question, Guid>()
                .FindAsync(q => q.QuizId == attempt.QuizId, cancellationToken);

            var questionIds = questions.Select(q => q.Id).ToList();

            var correctOptions = await _unitOfWork.Repository<AnswerOption, Guid>()
                .FindAsync(o => questionIds.Contains(o.QuestionId) && o.IsCorrect, cancellationToken);

            var questionCount = questions.Count();
            var correctCount = 0;

            foreach (var answer in answers)
            {
                var isCorrect = correctOptions.Any(co => co.Id == answer.SelectedOptionId);
                answer.IsCorrect = isCorrect;
                if (isCorrect) correctCount++;
            }

            var score = questionCount > 0
                ? Math.Round((decimal)correctCount / questionCount * 100, 2)
                : 0;

            var passed = score >= quiz.PassScore;

            attempt.Status = AttemptStatus.TimedOut;
            attempt.SubmittedAt = attempt.Deadline;
            attempt.Score = score;
            attempt.Passed = passed;

            _unitOfWork.Repository<QuizAttempt, Guid>().Update(attempt);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var response = new AutoSubmitDto
            {
                AttemptId = attempt.Id,
                SubmittedAt = attempt.SubmittedAt.Value,
                WasAutoSubmitted = true,
                Score = score,           
                IsPassed = passed
            };

            return Result<AutoSubmitDto>.Success(response);
        }
    }
}
