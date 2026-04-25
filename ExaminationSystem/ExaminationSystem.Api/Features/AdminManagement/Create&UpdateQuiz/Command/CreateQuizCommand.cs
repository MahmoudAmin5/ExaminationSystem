using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Features.AdminManagement.Shard.Queries;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.AdminManagement.CreateQuiz.Command
{
    public record CreateQuizCommand(
    string Title,
    Guid DiplomaId,
    int DurationMinutes,
    decimal PassScore,
    int MaxAttempts,
    string Instructions) : IRequest<Result<Guid>>;

    public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public CreateQuizCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }
        public async Task<Result<Guid>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
        {
            var _quizRepo = _unitOfWork.Repository<Quiz,Guid>();
            var isDiplomaExist = await _mediator.Send(new IsDiplomaExistQuery(request.DiplomaId), cancellationToken);
            if (!isDiplomaExist)
            {
                return Result<Guid>.Failure(Error.NotFound("Diploma.NotFound", "The specified diploma does not exist."));
            }
            var quiz = new Quiz
            {
                Id = Guid.CreateVersion7(),
                Title = request.Title,
                DiplomaId = request.DiplomaId,
                DurationMinutes = request.DurationMinutes,
                PassScore = request.PassScore,
                MaxAttempts = request.MaxAttempts,
                Instructions = request.Instructions,
                Status = ContentStatus.Draft,
                CreatedAt = DateTime.UtcNow
            };
           _quizRepo.Add(quiz);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(quiz.Id);
        }
    }

}
