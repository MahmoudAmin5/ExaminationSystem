using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos;
using ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Queries;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Commands
{
    public record CreateQuizAttemptCommand(Guid studentId, Guid QuizId, int durationMinutes) : IRequest<Result<QuizAttemptDto>>;

    public class CreateQuizAttemptCommandHandler : IRequestHandler<CreateQuizAttemptCommand, Result<QuizAttemptDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public CreateQuizAttemptCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }
        public async Task<Result<QuizAttemptDto>> Handle(CreateQuizAttemptCommand request, CancellationToken cancellationToken)
        {

            var attempt = new QuizAttempt
            {
                Id = Guid.NewGuid(),
                StudentId = request.studentId,
                QuizId = request.QuizId,
                StartedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow.AddMinutes(request.durationMinutes),
                Status = AttemptStatus.InProgress
            };
            _unitOfWork.Repository<QuizAttempt, Guid>().Add(attempt);
            return Result<QuizAttemptDto>.Success(new QuizAttemptDto
            {
                AttemptId = attempt.Id,
                StudentId = attempt.StudentId,
                QuizId = attempt.QuizId,
                StartedAt = attempt.StartedAt,
                Deadline = attempt.Deadline,
                Status = attempt.Status
            });


        }
    }
}
