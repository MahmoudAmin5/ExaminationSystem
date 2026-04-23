using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.QuizEngine.Shared.Dtos;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.QuizEngine.Shared.Queries
{
    public record GetQuizByIdQuery(Guid QuizId) : IRequest<Result<QuizDto>>;
    public class GetQuizByIdHandler : IRequestHandler<GetQuizByIdQuery, Result<QuizDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetQuizByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<QuizDto>> Handle(GetQuizByIdQuery request, CancellationToken cancellationToken)
        {
            var quizRepo = _unitOfWork.Repository<Quiz, Guid>();
            var response = await quizRepo
                .AsNoTracking()
                .Where(x => x.Id == request.QuizId)
                .Select(x => new QuizDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    DiplomaId = x.DiplomaId,
                    DurationMinutes = x.DurationMinutes,
                    Instructions = x.Instructions,
                    MaxAttempts = x.MaxAttempts,
                    PassScore = x.PassScore,
                    Status = x.Status
                })
               .FirstOrDefaultAsync(cancellationToken);
            if (response is null)
            {
                return Result<QuizDto>.Failure(Error.NotFound("Quiz.NotFound", "Quiz not found"));
            }


            return Result<QuizDto>.Success(response);
        }
    }

}
