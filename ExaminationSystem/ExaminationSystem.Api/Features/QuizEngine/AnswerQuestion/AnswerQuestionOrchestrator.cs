using ExaminationSystem.Api.Features.QuizEngine.AnswerQuestion.Commands;
using ExaminationSystem.Api.Features.QuizEngine.AnswerQuestion.Dtos;
using ExaminationSystem.Api.Features.QuizEngine.AnswerQuestion.Queries;
using ExaminationSystem.Api.Shared.Results;
using FluentValidation;
using MediatR;

namespace ExaminationSystem.Api.Features.QuizEngine.AnswerQuestion
{
    public record AnswerQuestionOrchestratorCommand(
           Guid AttemptId,
           Guid StudentId,
           Guid QuestionId,
           Guid SelectedOptionId) : IRequest<Result<AnswerQuestionResponseDto>>;
    public class AnswerQuestionOrchestrator
        : IRequestHandler<AnswerQuestionOrchestratorCommand, Result<AnswerQuestionResponseDto>>{
        private readonly IMediator _mediator;
        private readonly IValidator<AnswerQuestionCommand> _validator;
        public AnswerQuestionOrchestrator(IMediator mediator, IValidator<AnswerQuestionCommand> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }
        public async Task<Result<AnswerQuestionResponseDto>> Handle(AnswerQuestionOrchestratorCommand request,CancellationToken cancellationToken){
            var validationResult = await _validator.ValidateAsync(
                new AnswerQuestionCommand(
                    request.AttemptId,
                    request.StudentId,
                    request.QuestionId,
                    request.SelectedOptionId),
                cancellationToken);
            if (!validationResult.IsValid){
                var errors = validationResult.Errors
                    .Select(e => Error.Validation(e.PropertyName, e.ErrorMessage))
                    .ToList();
                return Result<AnswerQuestionResponseDto>.Failure(errors); }
            var attemptValidation = await _mediator.Send(
                new GetAttemptWithValidationQuery(request.AttemptId, request.StudentId),cancellationToken);
            if (attemptValidation.IsFailure)
            {
                var firstError = attemptValidation.Errors.FirstOrDefault();
                //if (firstError?.Code == "Attempt.Expired")
                //{
                //    await _mediator.Send(
                //        new AutoSubmitExpiredAttemptCommand(request.AttemptId),cancellationToken);
                //    return Result<AnswerQuestionResponseDto>.Failure(Error.Gone("Attempt.AutoSubmitted","Quiz time has expired. Your attempt has been auto-submitted."));
                //}
                return Result<AnswerQuestionResponseDto>.Failure(attemptValidation.Errors);
            }
            var result = await _mediator.Send(
                new AnswerQuestionCommand(
                    request.AttemptId,
                    request.StudentId,
                    request.QuestionId,
                    request.SelectedOptionId),
                cancellationToken);
            return result;
        }
    }
}
