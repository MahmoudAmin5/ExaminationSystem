using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.QuizEngine.ViewResult.Dtos;
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
            return await _unitOfWork
                .Repository<QuizAttempt, Guid>()
                .AsNoTracking()
                .Where(a => a.Id == request.AttemptId)
                .Select(a => new AttemptWithQuizDto
                {
                    Attempt = new AttemptDto
                    {
                        Id = a.Id,
                        QuizId = a.QuizId,
                        StudentId = a.StudentId,
                        Status = a.Status.ToString(),
                        Score = a.Score,
                        Passed = a.Passed,
                        TotalQuestions = a.Quiz.Questions.Count(),
                        SubmittedAt = a.SubmittedAt
                    },
                    Quiz = new QuizDto
                    {
                        Id = a.Quiz.Id,
                        Title = a.Quiz.Title
                    }
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
