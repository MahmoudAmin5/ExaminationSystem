using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.AdminManagement.Questions.UpdateQuestion
{
    public record UpdateOptionsCommand(Guid QuestionId, List<UpdateOptionDto> OptionsData) : IRequest<Result>;

    public class UpdateOptionsCommandHandler : IRequestHandler<UpdateOptionsCommand, Result>
    {
        private readonly IGenericRepository<AnswerOption, Guid> _repo;
        public UpdateOptionsCommandHandler(IGenericRepository<AnswerOption, Guid> repo) => _repo = repo;

        public async Task<Result> Handle(UpdateOptionsCommand request, CancellationToken token)
        { 
           
            foreach (var dto in request.OptionsData)
            {
                if (dto.Id.HasValue)
                {
                   
                    var optionToUpdate = new AnswerOption
                    {
                        Id = dto.Id.Value,
                        Text = dto.Text,
                        IsCorrect = dto.IsCorrect
                    };

                    _repo.SaveInclude(
                        optionToUpdate,
                        nameof(AnswerOption.Text),
                        nameof(AnswerOption.IsCorrect)
                    );
                }
                else
                {
                   
                    var newOption = new AnswerOption
                    {
                        QuestionId = request.QuestionId,
                        Text = dto.Text,
                        IsCorrect = dto.IsCorrect
                    };
                    _repo.Add(newOption);
                }
            }

            return Result.Success();
        }
    }
}
