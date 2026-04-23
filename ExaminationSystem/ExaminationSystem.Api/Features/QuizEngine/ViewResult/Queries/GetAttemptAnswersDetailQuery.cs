using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.QuizEngine.Shared.Queries;
using ExaminationSystem.Api.Features.QuizEngine.ViewResult.Dtos;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

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

            var answersResult = await _mediator.Send(new GetAttemptAnswerQuery(request.AttemptId), cancellationToken);
            if (answersResult.IsFailure) return Result<AttemptAnswersDetailDto>.Failure(answersResult.Errors);

           
            var questionsResult = await _mediator.Send(new GetQuizQuestionsQuery(request.QuizId), cancellationToken);
            if (questionsResult.IsFailure) return Result<AttemptAnswersDetailDto>.Failure(questionsResult.Errors);
            
            var questionIds = questionsResult.Value.Select(q => q.Id).ToList();

            var optionsResult = await _mediator.Send(new GetQuestionOptionsQuery(questionIds), cancellationToken);
            if (optionsResult.IsFailure) return Result<AttemptAnswersDetailDto>.Failure(optionsResult.Errors);
            
            return Result<AttemptAnswersDetailDto>.Success(new AttemptAnswersDetailDto
            {
                Answers = answersResult.Value,
                Questions = questionsResult.Value,
                Options = optionsResult.Value
            });
        }
    }
}
