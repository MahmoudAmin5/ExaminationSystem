using ExaminationSystem.Api.Extensions;
using ExaminationSystem.Api.Features.Diplomas.ViewDiplomas;
using ExaminationSystem.Api.Features.Student_DashboardLearning.ViewQuizzesByDiploma;
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
            return result.ToActionResult();
        }

        [HttpGet("{id}/quizzes")]
        public async Task<IActionResult> GetQuizzes(Guid id)
        {
           
            var result = await _mediator.Send(new GetQuizzesByDiplomaQuery(id));
            return result.ToActionResult();
        }
    }
}
