using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.QuizEngine.ViewResult.Dtos;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.QuizEngine.ViewResult.Queries
{
    public record GetAttemptWithQuizQuery(Guid AttemptId) : IRequest<AttemptWithQuizDto?>;
    public class GetAttemptWithQuizQueryHandler
    : IRequestHandler<GetAttemptWithQuizQuery, AttemptWithQuizDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAttemptWithQuizQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AttemptWithQuizDto?> Handle(GetAttemptWithQuizQuery request, CancellationToken cancellationToken)
        {
            var attempt = await _unitOfWork
                .Repository<QuizAttempt, Guid>()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    a => a.Id == request.AttemptId,
                    cancellationToken);

            if (attempt is null)
                return null;

            var quiz = await _unitOfWork
                .Repository<Quiz, Guid>()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    q => q.Id == attempt.QuizId,
                    cancellationToken);

            if (quiz is null)
                return null;

            return new AttemptWithQuizDto
            {
                Attempt = new AttemptDto
                {
                    Id = attempt.Id,
                    QuizId = attempt.QuizId,
                    StudentId = attempt.StudentId,
                    Status = attempt.Status.ToString(),
                    Score = attempt.Score,
                    Passed = attempt.Passed,
                    TotalQuestions = quiz.Questions.Count,
                    SubmittedAt = attempt.SubmittedAt
                },
                Quiz = new QuizDto
                {
                    Id = quiz.Id,
                    Title = quiz.Title
                }
            };
        }
    }

}
