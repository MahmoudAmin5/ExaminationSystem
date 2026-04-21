using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.StudentAttempts
{
    public record GetStudentAttemptsQuery(
        Guid StudentId,
        int PageNumber,
        int PageSize,
        Guid? QuizId,
        Guid? DiplomaId) : IRequest<Result<PaginatedList<StudentAttemptHistoryItemDto>>>;

    public class GetStudentAttemptsHandler
        : IRequestHandler<GetStudentAttemptsQuery, Result<PaginatedList<StudentAttemptHistoryItemDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStudentAttemptsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedList<StudentAttemptHistoryItemDto>>> Handle(
            GetStudentAttemptsQuery request,
            CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize < 1 ? 10 : request.PageSize;

            var attemptsQuery = _unitOfWork
                .Repository<QuizAttempt, Guid>()
                .AsNoTracking()
                .Where(a =>
                    a.StudentId == request.StudentId &&
                    a.Status != AttemptStatus.InProgress);

            if (request.QuizId.HasValue)
            {
                attemptsQuery = attemptsQuery.Where(a => a.QuizId == request.QuizId.Value);
            }

            if (request.DiplomaId.HasValue)
            {
                attemptsQuery = attemptsQuery.Where(a => a.Quiz.DiplomaId == request.DiplomaId.Value);
            }

            var totalCount = await attemptsQuery.CountAsync(cancellationToken);

            var items = await attemptsQuery
                .OrderByDescending(a => a.SubmittedAt ?? a.StartedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new StudentAttemptHistoryItemDto
                {
                    AttemptId = a.Id,
                    QuizId = a.QuizId,
                    QuizTitle = a.Quiz.Title,
                    DiplomaId = a.Quiz.DiplomaId,
                    DiplomaTitle = a.Quiz.Diploma.Title,
                    Status = a.Status.ToString(),
                    Score = a.Score,
                    Passed = a.Passed,
                    TotalQuestions = a.Quiz.Questions.Count(),
                    StartedAt = a.StartedAt,
                    SubmittedAt = a.SubmittedAt
                })
                .ToListAsync(cancellationToken);

            var paginatedResult = new PaginatedList<StudentAttemptHistoryItemDto>(
                items,
                totalCount,
                pageNumber,
                pageSize);

            return Result<PaginatedList<StudentAttemptHistoryItemDto>>.Success(paginatedResult);
        }
    }
}
