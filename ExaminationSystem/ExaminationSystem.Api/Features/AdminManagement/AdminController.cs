using ExaminationSystem.Api.Extensions;
using ExaminationSystem.Api.Features.AdminManagement.Create_UpdateQuiz.Command;
using ExaminationSystem.Api.Features.AdminManagement.Create_UpdateQuiz.Requests;
using ExaminationSystem.Api.Features.AdminManagement.CreateQuiz.Command;
using ExaminationSystem.Api.Features.AdminManagement.CreateQuiz.ViewModels;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Api.Features.AdminManagement
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("create-quiz")]
        [ProducesResponseType(typeof(QuizCreatedViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }


            var response = new QuizCreatedViewModel
            {
                Id = result.Value
            };


            return Result<QuizCreatedViewModel>.Success(response).ToActionResult();
        }

        [HttpPut("update-quiz/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateQuiz([FromRoute] Guid id, [FromBody] UpdateQuizRequest request,
    CancellationToken cancellationToken)
        {
            var command = new UpdateQuizCommand(
                id,
                request.Title,
                request.DiplomaId,
                request.DurationMinutes,
                request.PassScore,
                request.MaxAttempts,
                request.Instructions,
                request.Status
            );

            var result = await _mediator.Send(command, cancellationToken);

            return result.ToActionResult();
        }

    }
}
