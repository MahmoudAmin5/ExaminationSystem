using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Queries
{
    public record GetQuizQuestionsQuery(Guid QuizId) : IRequest<Result<QuizQuestionsDto>>;
    public class GetQuizQuestionsQueryHandler : IRequestHandler<GetQuizQuestionsQuery, Result<QuizQuestionsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetQuizQuestionsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        async Task<Result<QuizQuestionsDto>> IRequestHandler<GetQuizQuestionsQuery, Result<QuizQuestionsDto>>.Handle(GetQuizQuestionsQuery request, CancellationToken cancellationToken)
        {
            var questions = await _unitOfWork.Repository<Question, Guid>()
            .FindAsync(q => q.QuizId == request.QuizId, cancellationToken);

            var questionIds = questions.Select(q => q.Id).ToList();

            var options = await _unitOfWork.Repository<AnswerOption, Guid>()
                .FindAsync(o => questionIds.Contains(o.QuestionId), cancellationToken);

            return new QuizQuestionsDto
            {
                Questions = questions,
                Options = options
            };
        }
    }
}
