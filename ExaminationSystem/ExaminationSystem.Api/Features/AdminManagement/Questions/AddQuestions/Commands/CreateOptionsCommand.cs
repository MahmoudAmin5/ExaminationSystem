using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.AdminManagement.Questions.AddQuestions.Commands
{
    public record CreateOptionsCommand(Guid QuestionId, List<CreateOptionDto> Options) : IRequest<Result>;
    public class CreateOptionsCommandHandler : IRequestHandler<CreateOptionsCommand, Result>
    {
        private readonly IGenericRepository<AnswerOption, Guid> _repo;
        public CreateOptionsCommandHandler(IGenericRepository<AnswerOption, Guid> repo) => _repo = repo;

        public  Task<Result> Handle(CreateOptionsCommand request, CancellationToken token)
        {
            var options = request.Options.Select(o => new AnswerOption { QuestionId = request.QuestionId, Text = o.Text, IsCorrect = o.IsCorrect }).ToList();
             _repo.AddRange(options);
            return  Task.FromResult(Result.Success());
        }
    }
}
