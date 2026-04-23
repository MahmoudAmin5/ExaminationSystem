using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.QuizEngine.Shared.Dtos;
using ExaminationSystem.Api.Features.QuizEngine.ViewResult.Queries;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.QuizEngine.Shared.Queries
{
    public record GetAttemptAnswerQuery(Guid AttemptId) : IRequest<Result<List<AttemptAnswerDto>>>;
    public class GetAttemptAnswerHandler : IRequestHandler<GetAttemptAnswerQuery, Result<List<AttemptAnswerDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAttemptAnswerHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<List<AttemptAnswerDto>>> Handle(GetAttemptAnswerQuery request, CancellationToken cancellationToken)
        {
            var attemptAnswers = await _unitOfWork.Repository<AttemptAnswer,int>()
                .AsNoTracking()
                .Where(a => a.AttemptId == request.AttemptId)
                .Select(a => new AttemptAnswerDto
                {
                    AttemptId = a.AttemptId,
                    QuestionId = a.QuestionId,
                    SelectedOptionId = a.SelectedOptionId,
                    AnsweredAt = a.AnsweredAt,
                    IsCorrect = a.IsCorrect ?? false
                })
                .ToListAsync(cancellationToken);

            return Result<List<AttemptAnswerDto>>.Success(attemptAnswers);
        }
    }
}
