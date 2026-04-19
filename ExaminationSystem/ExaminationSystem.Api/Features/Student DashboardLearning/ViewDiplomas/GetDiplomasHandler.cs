using ExaminationSystem.Api.BuildingBlocks.Interfaces;
using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.Diplomas.ViewDiplomas
{
    
        public class GetDiplomasHandler : IRequestHandler<GetDiplomasQuery, Result<PaginatedList<DiplomaDto>>>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly ICurrentUserService _CurrentUser;


            public GetDiplomasHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
            {
                this.unitOfWork = unitOfWork;
                _CurrentUser = currentUser;
            }



            public async Task<Result<PaginatedList<DiplomaDto>>> Handle(GetDiplomasQuery request, CancellationToken cancellationToken)
            {

                var studentId = _CurrentUser.UserId;

                var diplomas = unitOfWork.Repository<Diploma, Guid>();


                var Query = diplomas.AsNoTracking().Where(d => d.Status == ContentStatus.Published);


                var totalCount = Query.Count();


                var items = await Query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(d => new DiplomaDto(
                        d.Id,
                        d.Title,
                        d.Description ?? "",
                        d.Quizzes.Count,
                        // Progress: (Quizzes Passed by this student / Total Quizzes) * 100
                        d.Quizzes.Count == 0 ? 0 :
                        (double)d.Quizzes.Count(q => q.Attempts.Any(a => a.StudentId == studentId && a.Passed == true))
                        / d.Quizzes.Count * 100
                    ))
                    .ToListAsync<DiplomaDto>(cancellationToken);


                var paginatedResult = new PaginatedList<DiplomaDto>(items, totalCount, request.PageNumber, request.PageSize);

                return Result<PaginatedList<DiplomaDto>>.Success(paginatedResult);
            }
        }
}
