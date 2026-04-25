using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.AdminManagement.Questions.DeleteQuestion
{
    public record DeleteQuestionCommand(Guid QuestionId) : IRequest<Result>;
    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Result>
    {
        private readonly IGenericRepository<Question, Guid> _questionRepo;
        public DeleteQuestionCommandHandler(IGenericRepository<Question, Guid> repo) => _questionRepo = repo;

        public async Task<Result> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _questionRepo.GetByIdAsync(request.QuestionId);
            _questionRepo.Remove(question); 
            return Result.Success();
        }
    }
}