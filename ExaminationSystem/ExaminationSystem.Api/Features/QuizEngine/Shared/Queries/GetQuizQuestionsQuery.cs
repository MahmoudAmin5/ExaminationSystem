using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.QuizEngine.Shared.Dtos;
using ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.QuizEngine.Shared.Queries
{
    public record GetQuizQuestionsQuery(Guid QuizId) : IRequest<Result<List<QuizQuestionDto>>>;
    public class GetQuizQuestionsQueryHandler : IRequestHandler<GetQuizQuestionsQuery, Result<List<QuizQuestionDto>>>
    {

        private readonly IUnitOfWork _unitOfWork;

        public GetQuizQuestionsQueryHandler(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }
        public async Task<Result<List<QuizQuestionDto>>> Handle(GetQuizQuestionsQuery request, CancellationToken cancellationToken)
        {
            var questions = await _unitOfWork.Repository<Question, Guid>()
                .AsNoTracking()
                .Where(q => q.QuizId == request.QuizId)
                .Select(q => new QuizQuestionDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    Explanation = q.Explanation
                }).ToListAsync(cancellationToken);

            return Result<List<QuizQuestionDto>>.Success(questions);
        }
    }
}
