using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.AdminManagement.Questions.Shared
{
    public record GetQuestionQuizIdQuery(Guid QuestionId) : IRequest<Result<Guid>>;
    public class GetQuestionQuizIdQueryHandler : IRequestHandler<GetQuestionQuizIdQuery, Result<Guid>>
    {
        private readonly IGenericRepository<Question, Guid> _questionRepo;
        public GetQuestionQuizIdQueryHandler(IGenericRepository<Question, Guid> repo) 
            => _questionRepo = repo;

        public async Task<Result<Guid>> Handle(GetQuestionQuizIdQuery request, CancellationToken cancellationToken)
        {
            var question = await _questionRepo.GetByIdAsync(request.QuestionId);
            if (question == null)
                return Result<Guid>.Failure(Error.NotFound("Question.NotFound", "Question not found."));
            return Result<Guid>.Success(question.QuizId);
        }
    }
}
