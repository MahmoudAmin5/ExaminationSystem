using ExaminationSystem.Api.Extensions;
using ExaminationSystem.Api.Features.QuizEngine.StartQuiz;
using ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExaminationSystem.Api.Features.QuizEngine
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuizController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Student")]
        [HttpPost("{quizId:guid}/start")]
        [ProducesResponseType(typeof(StartQuizResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> StartQuiz([FromRoute] Guid quizId, CancellationToken cancellationToken)
        {
            var studentId = Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _mediator.Send(
                new StartQuizCommand(quizId, studentId), cancellationToken);

            if (result.IsFailure)
                return result.ToActionResult();

            return result.ToActionResult();
        }
    }
}
