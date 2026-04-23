using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.QuizEngine.Shared.Dtos;
using ExaminationSystem.Api.Features.QuizEngine.ViewResult.Dtos;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.QuizEngine.Shared.Queries
{
    public record GetQuestionOptionsQuery (List<Guid> QuestionIds) : IRequest<Result<List<QuestionOptionDto>>>;
    public class GetQuestionOptionsQueryHandler : IRequestHandler<GetQuestionOptionsQuery, Result<List<QuestionOptionDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetQuestionOptionsQueryHandler(IUnitOfWork unitOfWork)
        {
            
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<List<QuestionOptionDto>>> Handle(GetQuestionOptionsQuery request, CancellationToken cancellationToken)
        {
            var options = await _unitOfWork.Repository<AnswerOption, Guid>()
            .AsNoTracking()
            .Where(o => request.QuestionIds.Contains(o.QuestionId))
            .Select(o => new QuestionOptionDto
            {
                Id = o.Id,
                QuestionId = o.QuestionId,
                Text = o.Text,
                IsCorrect = o.IsCorrect
            })
            .ToListAsync(cancellationToken);

            return Result<List<QuestionOptionDto>>.Success(options);
        }
    }
}
