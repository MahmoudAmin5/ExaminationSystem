using ExaminationSystem.Api.Extensions;
using ExaminationSystem.Api.Features.QuizEngine.AnswerQuestion;
using ExaminationSystem.Api.Features.QuizEngine.AnswerQuestion.Dtos;
using ExaminationSystem.Api.Features.QuizEngine.StartQuiz;
using ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos;
using ExaminationSystem.Api.Features.QuizEngine.TimerHandling.Dtos;
using ExaminationSystem.Api.Features.QuizEngine.TimerHandling.Queries;
using ExaminationSystem.Api.Features.QuizEngine.ViewResult;
using ExaminationSystem.Api.Features.QuizEngine.ViewResult.Dtos;
using ExaminationSystem.Api.Filters;
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

        
        [HttpGet("{attemptId:guid}/timer")]
        [ServiceFilter(typeof(AttemptDeadlineFilter))]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(typeof(AttemptTimerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> GetTimer([FromRoute] Guid attemptId,CancellationToken cancellationToken)
        {
            var studentId = Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _mediator.Send(
                new GetAttemptTimerQuery(attemptId, studentId), cancellationToken);

            return result.ToActionResult();
        }
        [Authorize]
        [HttpGet("{attemptId:guid}/results")]
        [ProducesResponseType(typeof(AttemptResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetResults([FromRoute] Guid attemptId, CancellationToken cancellationToken)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out Guid requesterId))
            {
                return Unauthorized();
            }

            var requesterRole = User.FindFirstValue(ClaimTypes.Role) ?? "Student";

            var result = await _mediator.Send(
                new ViewResultsQuery(attemptId, requesterId, requesterRole),
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpPost("{attemptId:guid}/answer")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(typeof(AnswerQuestionResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AnswerQuestion(
    [FromRoute] Guid attemptId,
    [FromBody] AnswerQuestionRequestDto request,
    CancellationToken cancellationToken)
        {
            var studentId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _mediator.Send(
                new AnswerQuestionOrchestratorCommand(
                    attemptId,
                    studentId,
                    request.QuestionId,
                    request.SelectedOptionId),
                cancellationToken);

            return result.ToActionResult();
        }
    }
}
