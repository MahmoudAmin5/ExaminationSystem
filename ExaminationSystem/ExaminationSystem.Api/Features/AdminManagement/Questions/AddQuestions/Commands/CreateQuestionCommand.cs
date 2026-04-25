using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.AdminManagement.Questions.AddQuestions.Commands
{
    public record CreateQuestionCommand(Guid QuizId, string Text, string? Explanation, int OrderIndex) : IRequest<Result<Question>>;
    public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, Result<Question>>
    {
        private readonly IGenericRepository<Question, Guid> _repo;
        public CreateQuestionCommandHandler(IGenericRepository<Question, Guid> repo) => _repo = repo;

        public Task<Result<Question>> Handle(CreateQuestionCommand request, CancellationToken token)
        {
            var question = new Question { QuizId = request.QuizId, Text = request.Text, Explanation = request.Explanation, OrderIndex = request.OrderIndex };
             _repo.Add(question);
            return  Task.FromResult (Result<Question>.Success(question));
        }
    }
}
