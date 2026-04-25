using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Features.AdminManagement.Questions.Shared;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.AdminManagement.Questions.DeleteQuestion
{
    public record DeleteQuestionOrchestrator(Guid QuestionId) : IRequest<Result>;
    public class DeleteQuestionOrchestratorHandler : IRequestHandler<DeleteQuestionOrchestrator, Result>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteQuestionOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteQuestionOrchestrator request, CancellationToken cancellationToken)
        {

           
            var quizIdResult = await _mediator.Send(new GetQuestionQuizIdQuery(request.QuestionId), cancellationToken);
            if (quizIdResult.IsFailure) return Result.Failure(quizIdResult.Errors);

            var statusResult = await _mediator.Send(new GetQuizStatusQuery (quizIdResult.Value), cancellationToken);
            if (statusResult.IsFailure)
                return Result.Failure(statusResult.Errors);

            if (statusResult.Value == "Published")
                return Result.Failure(Error.Conflict("Quiz.Published", "Cannot delete questions from a published quiz. Unpublish it first."));

            await _mediator.Send(new DeleteOptionsCommand(request.QuestionId), cancellationToken);

            await _mediator.Send(new DeleteQuestionCommand(request.QuestionId), cancellationToken);
          

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
