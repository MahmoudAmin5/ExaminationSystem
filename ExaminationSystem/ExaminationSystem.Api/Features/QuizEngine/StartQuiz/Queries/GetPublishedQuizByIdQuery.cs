using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
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
            var response = await _unitOfWork.Repository<Quiz, Guid>()
                 .AsNoTracking()
                 .Where(x => x.Id == request.Id && x.Status == ContentStatus.Published)
                 .Select(quiz => new QuizDto
                 {
                     Id = quiz.Id,
                     DiplomaId = quiz.DiplomaId,
                     Title = quiz.Title,
                     DurationMinutes = quiz.DurationMinutes,
                     PassScore = quiz.PassScore,
                     MaxAttempts = quiz.MaxAttempts,
                     Status = quiz.Status
                 })
                 .FirstOrDefaultAsync(cancellationToken);
            if (response is null)
            {
                return Result<QuizDto>.Failure(Error.NotFound("Quiz.NotFound", "The specified published quiz was not found."));
            }

            return Result<QuizDto>.Success(response);
        }
    }


}
