using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.QuizEngine.Shared.Dtos;
using ExaminationSystem.Api.Features.QuizEngine.Shared.Queries;
using ExaminationSystem.Api.Features.QuizEngine.ViewResult.Dtos;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ExaminationSystem.Api.Features.QuizEngine.ViewResult.Queries
{
    public record GetAttemptAnswersDetailQuery(
    Guid AttemptId,
    Guid QuizId
) : IRequest<Result<AttemptAnswersDetailDto>>;

    public class GetAttemptAnswersDetailQueryHandler
        : IRequestHandler<GetAttemptAnswersDetailQuery, Result<AttemptAnswersDetailDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public GetAttemptAnswersDetailQueryHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Result<AttemptAnswersDetailDto>> Handle(
            GetAttemptAnswersDetailQuery request,
            CancellationToken cancellationToken)
        {
                var response = await _unitOfWork.Repository<QuizAttempt, Guid>()
                    .AsNoTracking() 
                    .Where(a => a.Id == request.AttemptId && a.QuizId == request.QuizId)
                    .Select(a => new AttemptAnswersDetailDto
                    {
                        AttemptId = a.Id,

                        ReviewedQuestions = a.Quiz.Questions.Select(q => new ReviewedQuestionDto
                        {
                            QuestionId = q.Id,
                            Text = q.Text,
                            Explanation = q.Explanation,

                            Options = q.Options.Select(o => new ReviewedOptionDto
                            {
                                OptionId = o.Id,
                                Text = o.Text,
                                IsCorrect = o.IsCorrect
                            }).ToList(),

                          
                            StudentAnswer = a.Answers
                                .Where(ans => ans.QuestionId == q.Id)
                                .Select(ans => new StudentAnswerDto
                                {
                                    SelectedOptionId = ans.SelectedOptionId,
                                    IsCorrect = ans.IsCorrect 
                                })
                                .FirstOrDefault()
                        }).ToList()
                    })
                    .FirstOrDefaultAsync(cancellationToken);

               
                if (response is null)
                {
                    return Result<AttemptAnswersDetailDto>.Failure(
                        Error.NotFound("Attempt.NotFound", "The specified quiz attempt could not be found."));
                }

                return Result<AttemptAnswersDetailDto>.Success(response);
            }
        }
    }
