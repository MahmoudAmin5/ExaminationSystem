using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Features.AdminManagement.Shard.Queries;
using ExaminationSystem.Api.Infrastructure.Repositories;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.AdminManagement.Create_UpdateQuiz.Command
{
    public record UpdateQuizCommand(
    Guid Id,
    string Title,
    Guid DiplomaId,
    int DurationMinutes,
    decimal PassScore,
    int MaxAttempts,
    string Instructions,
    ContentStatus Status) : IRequest<Result>;

    public class UpdateQuizCommandHandler : IRequestHandler<UpdateQuizCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UpdateQuizCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }
        public async Task<Result> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
        {
            var quizRepo = _unitOfWork.Repository<Quiz, Guid>();

            var quiz = await quizRepo.GetByIdAsync(request.Id, cancellationToken);

            if (quiz is null) return Result.Failure(Error.NotFound("Quiz.NotFound", "Quiz Not Found"));

            var isDiplomaExist = await _mediator.Send(new IsDiplomaExistQuery(request.DiplomaId), cancellationToken);
            if (!isDiplomaExist)
            {
                return Result<Guid>.Failure(Error.NotFound("Diploma.NotFound", "The specified diploma does not exist."));
            }
            quiz.Title = request.Title;
            quiz.DiplomaId = request.DiplomaId;
            quiz.DurationMinutes = request.DurationMinutes;
            quiz.PassScore = request.PassScore;
            quiz.MaxAttempts = request.MaxAttempts;
            quiz.Instructions = request.Instructions;
            quiz.Status = request.Status;
            quiz.UpdatedAt = DateTime.UtcNow;

            quizRepo.Update(quiz);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

    }
}
