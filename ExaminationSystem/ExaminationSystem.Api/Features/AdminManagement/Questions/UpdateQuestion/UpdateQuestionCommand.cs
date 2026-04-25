using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.AdminManagement.Questions.UpdateQuestion
{
    public record UpdateQuestionCommand(Guid QuestionId, string Text, string? Explanation, int OrderIndex) : IRequest<Result>;

    public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, Result>
    {
        private readonly IGenericRepository<Question, Guid> _questionrepo;
        public UpdateQuestionCommandHandler(IGenericRepository<Question, Guid> repo) => _questionrepo = repo;

        public  Task<Result> Handle(UpdateQuestionCommand request, CancellationToken token)
        {
           
            var questionToUpdate = new Question
            {
                Id = request.QuestionId,
                Text = request.Text,
                Explanation = request.Explanation,
                OrderIndex = request.OrderIndex
            };

           
            _questionrepo.SaveInclude(
                questionToUpdate,
                nameof(Question.Text),
                nameof(Question.Explanation),
                nameof(Question.OrderIndex)
            );

            return Task.FromResult(Result.Success());
        }
    }
}