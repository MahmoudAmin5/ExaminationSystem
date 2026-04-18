using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.QuizEngine.ViewResult.Dtos;
using MediatR;

namespace ExaminationSystem.Api.Features.QuizEngine.ViewResult.Queries
{
    public record GetAttemptAnswersDetailQuery(
    Guid AttemptId,
    Guid QuizId
) : IRequest<AttemptAnswersDetailDto>;

    public class GetAttemptAnswersDetailQueryHandler
        : IRequestHandler<GetAttemptAnswersDetailQuery, AttemptAnswersDetailDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAttemptAnswersDetailQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AttemptAnswersDetailDto> Handle(
            GetAttemptAnswersDetailQuery request,
            CancellationToken cancellationToken)
        {
           
            var answers = await _unitOfWork
                .Repository<AttemptAnswer, int>()
                .FindAsync(
                    a => a.AttemptId == request.AttemptId,
                    cancellationToken);

            
            var questions = await _unitOfWork
                .Repository<Question, Guid>()
                .FindAsync(
                    q => q.QuizId == request.QuizId,
                    cancellationToken);

           
            var questionIds = questions.Select(q => q.Id).ToList();

            var options = await _unitOfWork
                .Repository<AnswerOption, Guid>()
                .FindAsync(
                    o => questionIds.Contains(o.QuestionId),
                    cancellationToken);

            return new AttemptAnswersDetailDto
            {
                Answers = answers.Select(a => new AnswerDetailDto
                {
                    QuestionId = a.QuestionId,
                    SelectedOptionId = a.SelectedOptionId,
                    IsCorrect = a.IsCorrect ?? false
                }).ToList(),

                Questions = questions.Select(q => new QuestionDetailDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    Explanation = q.Explanation
                }).ToList(),

                Options = options.Select(o => new OptionDetailDto
                {
                    Id = o.Id,
                    QuestionId = o.QuestionId,
                    Text = o.Text,
                    IsCorrect = o.IsCorrect
                }).ToList()
            };
        }
    }
}
