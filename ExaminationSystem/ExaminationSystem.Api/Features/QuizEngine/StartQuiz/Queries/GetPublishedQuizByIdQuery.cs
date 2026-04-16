using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Queries
{
    public record GetPublishedQuizByIdQuery(Guid Id) : IRequest<Result<QuizDto>>;

    public class GetPublishedQuizByIdQueryHandler : IRequestHandler<GetPublishedQuizByIdQuery, Result<QuizDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPublishedQuizByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

         async Task<Result<QuizDto>> IRequestHandler<GetPublishedQuizByIdQuery, Result<QuizDto>>.Handle(GetPublishedQuizByIdQuery request, CancellationToken cancellationToken)
        {
            var quiz = await _unitOfWork.Repository<Quiz, Guid>().AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (quiz is null || quiz.Status != Domain.Enums.ContentStatus.Published ) 
                return Result<QuizDto>.Failure(Error.NotFound("Quiz.NotFound", "Quiz not found"));

            var response = new QuizDto
            {
                Id = quiz.Id,
                DiplomaId = quiz.DiplomaId,
                Title = quiz.Title,
                DurationMinutes = quiz.DurationMinutes,
                PassScore = quiz.PassScore,
                MaxAttempts = quiz.MaxAttempts,
                Status = quiz.Status
            };
           return Result<QuizDto>.Success(response);
        }
    }


}
