using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.AdminManagement.Questions.DeleteQuestion
{
    public record DeleteOptionsCommand(Guid QuestionId) : IRequest<Result>;

    public class DeleteOptionsCommandHandler : IRequestHandler<DeleteOptionsCommand, Result>
    {
        private readonly IGenericRepository<AnswerOption, Guid> _optionRepo;

        public DeleteOptionsCommandHandler(IGenericRepository<AnswerOption, Guid> optionRepo)
        {
            _optionRepo = optionRepo;
        }

        public async Task<Result> Handle(DeleteOptionsCommand request, CancellationToken ct)
        {
           
            var options = await _optionRepo.FindAsync(o => o.QuestionId == request.QuestionId, ct);

            if (options.Any())
            {
                _optionRepo.RemoveRange(options);
            }

            return Result.Success();
        }
    }
}
