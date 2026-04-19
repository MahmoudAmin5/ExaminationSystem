using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.QuizEngine.AnswerQuestion.Dtos;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.QuizEngine.AnswerQuestion.Commands
{
    public record AnswerQuestionCommand(
              Guid AttemptId,
              Guid StudentId,
              Guid QuestionId,
              Guid? SelectedOptionId) : IRequest<Result<AnswerQuestionResponseDto>>;

    public class AnswerQuestionCommandHandler: IRequestHandler<AnswerQuestionCommand, Result<AnswerQuestionResponseDto>>{
        private readonly IUnitOfWork _unitOfWork;
        public AnswerQuestionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<AnswerQuestionResponseDto>> Handle(AnswerQuestionCommand request,CancellationToken cancellationToken)
        {
            var attempt = await _unitOfWork.Repository<QuizAttempt, Guid>()
                .FirstOrDefaultAsync(a => a.Id == request.AttemptId, cancellationToken);
            if (attempt == null)
                return Result<AnswerQuestionResponseDto>.Failure(
                    Error.NotFound("Attempt.NotFound", "Attempt not found"));
            var quizQuestions = await _unitOfWork.Repository<Question, Guid>()
                .FindAsync(q => q.QuizId == attempt.QuizId, cancellationToken);
            if (!quizQuestions.Any(q => q.Id == request.QuestionId))
                return Result<AnswerQuestionResponseDto>.Failure(
                    Error.Validation("Question.Invalid","This question does not belong to the current quiz"));
            if (request.SelectedOptionId.HasValue)
            {
                var option = await _unitOfWork.Repository<AnswerOption, Guid>()
                    .FirstOrDefaultAsync(o =>
                        o.Id == request.SelectedOptionId.Value &&
                        o.QuestionId == request.QuestionId,
                        cancellationToken);

                if (option == null)
                    return Result<AnswerQuestionResponseDto>.Failure(
                        Error.Validation("Option.Invalid","Selected option is not valid for this question"));
            }
            var existingAnswer = await _unitOfWork.Repository<AttemptAnswer, int>()
                .FirstOrDefaultAsync(a =>
                    a.AttemptId == request.AttemptId &&
                    a.QuestionId == request.QuestionId,
                    cancellationToken);
            if (existingAnswer != null)
            {
                existingAnswer.SelectedOptionId = request.SelectedOptionId;
                existingAnswer.AnsweredAt = DateTime.UtcNow;
                _unitOfWork.Repository<AttemptAnswer, int>().Update(existingAnswer);
            }
            else
            {
                var newAnswer = new AttemptAnswer
                {
                    AttemptId = request.AttemptId,
                    QuestionId = request.QuestionId,
                    SelectedOptionId = request.SelectedOptionId,
                    AnsweredAt = DateTime.UtcNow
                };
                _unitOfWork.Repository<AttemptAnswer, int>().Add(newAnswer);
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<AnswerQuestionResponseDto>.Success(new AnswerQuestionResponseDto
            {
                AttemptId = request.AttemptId,
                QuestionId = request.QuestionId,
                SelectedOptionId = request.SelectedOptionId,
                Saved = true,
                AnsweredAt = DateTime.UtcNow,
                Message = existingAnswer != null ? "Answer updated successfully" : "Answer saved successfully"
            });
        }
    }
}
