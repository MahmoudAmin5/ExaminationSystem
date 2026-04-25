using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.AdminManagement.Questions.Shared
{
    public record GetQuizStatusQuery(Guid QuizId) : IRequest<Result<string>>;
    public class GetQuizStatusQueryHandler : IRequestHandler<GetQuizStatusQuery, Result<string>>
    {
        private readonly IGenericRepository<Quiz, Guid> _quizRepo;
        public GetQuizStatusQueryHandler(IGenericRepository<Quiz, Guid> repo) => _quizRepo = repo;

        public async Task<Result<string>> Handle(GetQuizStatusQuery request, CancellationToken cancellationToken)
        {
            var quiz = await _quizRepo.GetByIdAsync(request.QuizId);
            if (quiz == null)
                return Result<string>.Failure(Error.NotFound("Quiz.NotFound", "Quiz not found."));
            return Result<string>.Success(quiz.Status.ToString());
        }
    }
}
