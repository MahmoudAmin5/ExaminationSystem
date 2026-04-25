using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.AdminManagement.ViewAllAttempts.Dtos;
using ExaminationSystem.Api.Features.AdminManagement.ViewAllAttempts.Requests;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.AdminManagement.ViewAllAttempts.Queries
{
    public record GetAttemptsQuery(GetAttemptsQueryParameters Parameters) : IRequest<Result<PaginatedList<AttemptListDto>>>;
    public class GetAttemptsQueryHandler : IRequestHandler<GetAttemptsQuery, Result<PaginatedList<AttemptListDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAttemptsQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<Result<PaginatedList<AttemptListDto>>> Handle(GetAttemptsQuery request, CancellationToken cancellationToken)
        {
            var _attemptRepo = _unitOfWork.Repository<QuizAttempt, Guid>();
            var query = _attemptRepo.AsNoTracking();

            if(request.Parameters.QuizId.HasValue) query = query.Where(a => a.QuizId == request.Parameters.QuizId.Value);
            if(request.Parameters.StudentId.HasValue) query = query.Where(a => a.StudentId == request.Parameters.StudentId.Value);

            query = request.Parameters.SortBy switch
            {
                "score" => request.Parameters.Order == "asc" ? query.OrderBy(a => a.Score) : query.OrderByDescending(a => a.Score),
                _ => request.Parameters.Order == "asc" ? query.OrderBy(a => a.SubmittedAt) : query.OrderByDescending(a => a.SubmittedAt)
            };
            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
            .Skip((request.Parameters.Page - 1) * request.Parameters.PerPage)
            .Take(request.Parameters.PerPage)
            .Select(a => new AttemptListDto(
                a.Id,
                a.StudentId,
                a.Student.UserName,
                a.Quiz.Title,
                a.Score,
                a.Status.ToString(),
                a.SubmittedAt
            ))
            .ToListAsync(cancellationToken);

            var paginatedList = new PaginatedList<AttemptListDto>(items, totalCount, request.Parameters.Page, request.Parameters.PerPage);
            return Result<PaginatedList<AttemptListDto>>.Success(paginatedList);


        }
    }
}