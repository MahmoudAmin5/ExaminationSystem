using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Features.AdminManagement.Questions.Shared;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.AdminManagement.Questions.UpdateQuestion
{
    public record UpdateQuestionOrchestrator(Guid QuestionId, string Text, string? Explanation, int OrderIndex, List<UpdateOptionDto> Options) : IRequest<Result>;
    public class UpdateQuestionOrchestratorHandler : IRequestHandler<UpdateQuestionOrchestrator, Result>
    {
        private readonly IMediator _mediator; private readonly IUnitOfWork _uow;
        public UpdateQuestionOrchestratorHandler(IMediator mediator, IUnitOfWork uow) 
        {
            _mediator = mediator; 
            _uow = uow;
        }

        public async Task<Result> Handle(UpdateQuestionOrchestrator request, CancellationToken token)
        {
            var quizIdResult = await _mediator.Send(new GetQuestionQuizIdQuery(request.QuestionId), token);
            if (quizIdResult.IsFailure)
                return Result.Failure(quizIdResult.Errors);

            var status = await _mediator.Send(new GetQuizStatusQuery(quizIdResult.Value), token);
            if (status.Value == "Published") 
                return Result.Failure(Error.Conflict("Quiz.Published", "Cannot update published quiz."));

            await _mediator.Send(new UpdateQuestionCommand(request.QuestionId, request.Text, request.Explanation, request.OrderIndex), token);
            await _mediator.Send(new UpdateOptionsCommand(request.QuestionId, request.Options), token);

            await _uow.SaveChangesAsync(token);
            return Result.Success();
        }
    }
}
