using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Features.AdminManagement.Shard.Queries;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.AdminManagement.Publish_UnpublishQuiz.Command
{
    public record UnpublishQuizCommand(Guid quizId) : IRequest<Result>;
    public class UnpublishQuizCommandHandler : IRequestHandler<UnpublishQuizCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UnpublishQuizCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }
        public async Task<Result> Handle(UnpublishQuizCommand request, CancellationToken cancellationToken)
        {
            var quizRepo = _unitOfWork.Repository<Quiz, Guid>();
            var quiz = await quizRepo.GetByIdAsync(request.quizId, cancellationToken);
            if (quiz is null) return Result.Failure(Error.NotFound("Quiz.NotFound", "Quiz Not Found To Be Unpublished "));
            if (quiz.Status == ContentStatus.Draft) return Result.Failure(Error.Conflict("Quiz.AlreadyUnpublished", "Quiz Is Already Unpublished"));

            var inProgressAttemptsCount = await _mediator.Send(new GetInProgressAttemptCount(request.quizId), cancellationToken);
            if (inProgressAttemptsCount > 0) return Result.Failure(Error.Conflict("Quiz.InProgressAttempts", "Quiz Cannot Be Unpublished Because There Are In Progress Attempts"));


            quiz.Status = ContentStatus.Draft;
            quiz.UpdatedAt = DateTime.UtcNow;
            quizRepo.Update(quiz);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

    }
}
