using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Queries
{
    public record GetStartQuizQuestionsQuery(Guid QuizId) : IRequest<Result<StartQuizQuestionsWithOptionsDto>>;

    public class GetStartQuizQuestionsQueryHandler : IRequestHandler<GetStartQuizQuestionsQuery, Result<StartQuizQuestionsWithOptionsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStartQuizQuestionsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<StartQuizQuestionsWithOptionsDto>> Handle(GetStartQuizQuestionsQuery request, CancellationToken cancellationToken)
        {
            var questions = await _unitOfWork.Repository<Question, Guid>()
                .AsNoTracking()
                .Where(q => q.QuizId == request.QuizId)
                .Select(q => new StartQuizQuestionDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    Options = q.Options.Select(o => new StartQuizOptionDto
                    {
                        Id = o.Id,
                        Text = o.Text
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

 
            if (!questions.Any())
            {
                return Result<StartQuizQuestionsWithOptionsDto>.Failure(Error.NotFound("Questions.NotFound", "No questions found for this quiz."));
            }

            var response = new StartQuizQuestionsWithOptionsDto
            {
                Questions = questions
            };

            return Result<StartQuizQuestionsWithOptionsDto>.Success(response);
        }
    }
}
