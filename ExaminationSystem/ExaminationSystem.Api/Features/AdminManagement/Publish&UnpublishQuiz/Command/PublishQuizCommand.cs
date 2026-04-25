using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Features.AdminManagement.Shard.Queries;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.AdminManagement.Publish_UnpublishQuiz.Command
{
    public record PublishQuizCommand(Guid QuizId) : IRequest<Result>;
    public class PublishQuizCommandHandler : IRequestHandler<PublishQuizCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public PublishQuizCommandHandler(IUnitOfWork unitOfWork , IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }
        public async Task<Result> Handle(PublishQuizCommand request, CancellationToken cancellationToken)
        {
            var quizRepo = _unitOfWork.Repository<Quiz, Guid>();

            var quiz = await quizRepo.GetByIdAsync(request.QuizId, cancellationToken);

            if (quiz is null) return Result.Failure(Error.NotFound("Quiz.NotFound", "Quiz Not Found To Be Published "));

            if(quiz.Status == ContentStatus.Published) return Result.Failure(Error.Conflict("Quiz.AlreadyPublished", "Quiz Is Already Published"));

            var questionsCount = await _mediator.Send(new GetQuestionCountForQuiz(request.QuizId), cancellationToken);

            if(questionsCount == 0) return Result.Failure(Error.Validation("Quiz.NoQuestions", "Quiz Must Have At Least One Question To Be Published"));

            quiz.Status = ContentStatus.Published;
            quiz.PublishedAt = DateTime.UtcNow;
            quiz.UpdatedAt = DateTime.UtcNow;

            quizRepo.Update(quiz);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
    }

}
