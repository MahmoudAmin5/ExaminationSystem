using ExaminationSystem.Api.BuildingBlocks.Interfaces;
using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Features.Student_DashboardLearning.ViewDiplomas.DiplomasQueries;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ExaminationSystem.Api.Features.Diplomas.ViewDiplomas
{
    public record GetDiplomasQuery(int PageNumber, int PageSize) : IRequest<Result<PaginatedList<DiplomaDto>>>;
    public class GetDiplomasOrchestrator : IRequestHandler<GetDiplomasQuery, Result<PaginatedList<DiplomaDto>>>
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _CurrentUser;


        public GetDiplomasOrchestrator(IMediator mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _CurrentUser = currentUser;
        }

        public async Task<Result<PaginatedList<DiplomaDto>>> Handle(GetDiplomasQuery request, CancellationToken ct)
        {

            var studentId = _CurrentUser.UserId ?? Guid.Empty;

        
            var paginatedDiplomas = await _mediator.Send(new GetPublishedDiplomasQuery(request.PageNumber, request.PageSize), ct);

           
            var passedQuizIds = await _mediator.Send(new GetStudentPassedQuizIdsQuery(studentId), ct);

            
            var dtos = paginatedDiplomas.Items.Select(d => new DiplomaDto(
                d.Id,
                d.Title,
                d.Description ?? "",
                d.Quizzes.Count,
                d.Quizzes.Count == 0 ? 0 : (double)d.Quizzes.Count(q => passedQuizIds.Contains(q.Id)) / d.Quizzes.Count * 100
            )).ToList();

            var result = new PaginatedList<DiplomaDto>(dtos, paginatedDiplomas.TotalCount, paginatedDiplomas.PageNumber, request.PageSize);
            return Result<PaginatedList<DiplomaDto>>.Success(result);
        }
    }
}
   
    

