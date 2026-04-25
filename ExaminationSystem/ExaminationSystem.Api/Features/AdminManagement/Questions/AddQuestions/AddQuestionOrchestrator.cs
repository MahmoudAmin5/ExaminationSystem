using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.AdminManagement.Questions.AddQuestions.Commands;
using ExaminationSystem.Api.Features.AdminManagement.Questions.Shared;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.AdminManagement.Questions.AddQuestions
{
   
    public record AddQuestionOrchestrator(
    Guid QuizId, string Text,
    string? Explanation,
    int OrderIndex,
    List<CreateOptionDto> Options) : IRequest<Result<Guid>>;
    public class AddQuestionOrchestratorHandler : IRequestHandler<AddQuestionOrchestrator, Result<Guid>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _uow;

        public AddQuestionOrchestratorHandler(IMediator mediator, IUnitOfWork uow) { _mediator = mediator; _uow = uow; }

        public async Task<Result<Guid>> Handle(AddQuestionOrchestrator request, CancellationToken token)
        {
            var status = await _mediator.Send(new GetQuizStatusQuery(request.QuizId), token);
            if (status.IsFailure) 
                return Result<Guid>.Failure(status.Errors);
            if (status.Value == "Published")
                return Result<Guid>.Failure(Error.Conflict("Quiz.Published", "Cannot add questions to published quiz."));

            var questionRes = await _mediator.Send(new CreateQuestionCommand(request.QuizId, request.Text, request.Explanation, request.OrderIndex), token);
            await _mediator.Send(new CreateOptionsCommand(questionRes.Value.Id, request.Options), token);

            await _uow.SaveChangesAsync(token);
            return Result<Guid>.Success(questionRes.Value.Id);
        }
    }
}
