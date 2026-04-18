using ExaminationSystem.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Api.Features.Diplomas
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Student")]
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
    }
}
