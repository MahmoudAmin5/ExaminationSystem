using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.Student_DashboardLearning.ViewDiplomas.DiplomasQueries
{
   
        public record GetPublishedDiplomasQuery(int Page, int PageSize) : IRequest<PaginatedList<Diploma>>;

        public class GetPublishedDiplomasHandler : IRequestHandler<GetPublishedDiplomasQuery, PaginatedList<Diploma>>
        {
            private readonly IUnitOfWork _unitOfWork;
            public GetPublishedDiplomasHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

            public async Task<PaginatedList<Diploma>> Handle(GetPublishedDiplomasQuery request, CancellationToken ct)
            {
                var repo = _unitOfWork.Repository<Diploma, Guid>();

                var query = repo.AsNoTracking()
                    .Where(d => d.Status == ContentStatus.Published);

                var totalCount = await query.CountAsync(ct);

                var diplomas = await query
                    .Include(d => d.Quizzes)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(ct);

                return new PaginatedList<Diploma>(diplomas, totalCount, request.Page, request.PageSize);
            }
        }
}
