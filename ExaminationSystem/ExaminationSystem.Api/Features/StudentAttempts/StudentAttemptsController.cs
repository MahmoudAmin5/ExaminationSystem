using ExaminationSystem.Api.Extensions;
using ExaminationSystem.Api.Features.QuizEngine.ViewResult;
using ExaminationSystem.Api.Features.QuizEngine.ViewResult.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExaminationSystem.Api.Features.StudentAttempts
{
    [Route("api/student/attempts")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class StudentAttemptsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentAttemptsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAttempts(
            [FromQuery] int page = 1,
            [FromQuery] int perPage = 10,
            [FromQuery(Name = "quiz_id")] Guid? quizId = null,
            [FromQuery(Name = "diploma_id")] Guid? diplomaId = null,
            CancellationToken cancellationToken = default)
        {
            var studentId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _mediator.Send(
                new GetStudentAttemptsQuery(studentId, page, perPage, quizId, diplomaId),
                cancellationToken);

            return result.ToActionResult();
        }

        [HttpGet("{attemptId:guid}")]
        [ProducesResponseType(typeof(AttemptResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAttemptDetails(
            [FromRoute] Guid attemptId,
            CancellationToken cancellationToken)
        {
            var studentId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _mediator.Send(
                new ViewResultsQuery(attemptId, studentId, "Student"),
                cancellationToken);

            return result.ToActionResult();
        }
    }
}
