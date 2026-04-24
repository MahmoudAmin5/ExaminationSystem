using ExaminationSystem.Api.Extensions;
using ExaminationSystem.Api.Features.Diplomas.ViewDiplomas;
using ExaminationSystem.Api.Features.Student_DashboardLearning.ViewDiplomas;
using ExaminationSystem.Api.Features.Student_DashboardLearning.ViewQuizzesByDiploma;
using ExaminationSystem.Api.Shared.Results;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace ExaminationSystem.Api.Features.Diplomas
{
    [Route("api/[controller]")]
    
    [ApiController]
    public class DiplomaController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DiplomaController(IMediator mediator) => _mediator = mediator;


        [HttpGet]
        public async Task<IActionResult> GetDiplomas([FromQuery] int page = 1, [FromQuery] int perPage = 10)
        {
            var result = await _mediator.Send(new GetDiplomasQuery(page, perPage));
            if (result.IsFailure) 
            return result.ToActionResult();
            var diplomas = result.Value.Items.Select(dto => new DiplomaViewModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                QuizCount = dto.QuizCount,
                StudentProgress = dto.StudentProgress
            }).ToList();

            var paginatedViewModels = new PaginatedList<DiplomaViewModel>
            (
                 diplomas,
                 result.Value.TotalCount,
                 result.Value.PageNumber,
                 perPage
            );

     
            return Result<PaginatedList<DiplomaViewModel>>.Success(paginatedViewModels).ToActionResult();
        }

        [HttpGet("{id}/quizzes")]
        public async Task<IActionResult> GetQuizzes(Guid id)
        {
           
            var result = await _mediator.Send(new GetQuizzesByDiplomaQuery(id));
            return result.ToActionResult();
        }
    }
}
