using ExaminationSystem.Api.Extensions;
using ExaminationSystem.Api.Features.AdminManagement.Create_UpdateQuiz.Command;
using ExaminationSystem.Api.Features.AdminManagement.Create_UpdateQuiz.Requests;
using ExaminationSystem.Api.Features.AdminManagement.CreateQuiz.Command;
using ExaminationSystem.Api.Features.AdminManagement.CreateQuiz.ViewModels;
using ExaminationSystem.Api.Features.AdminManagement.Publish_UnpublishQuiz.Command;
using ExaminationSystem.Api.Features.AdminManagement.Questions.AddQuestions;
using ExaminationSystem.Api.Features.AdminManagement.Questions.DeleteQuestion;
using ExaminationSystem.Api.Features.AdminManagement.Questions.UpdateQuestion;
using ExaminationSystem.Api.Features.AdminManagement.ViewAllAttempts.Queries;
using ExaminationSystem.Api.Features.AdminManagement.ViewAllAttempts.Requests;
using ExaminationSystem.Api.Features.AdminManagement.ViewAllAttempts.ViewModels;
using ExaminationSystem.Api.Shared.Results;
using Mapster;
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
        [HttpPost("quizzes/create-quiz")]
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

        [HttpPut("quizzes/{id:guid}/update-quiz")]
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

        [HttpPatch("quizzes/{id:guid}/publish")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> PublishQuiz([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var command = new PublishQuizCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPatch("quizzes/{id:guid}/unpublish")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UnpublishQuiz([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var command = new UnpublishQuizCommand(id);
            var result = await _mediator.Send(command, cancellationToken);

            return result.ToActionResult();
        }
    

        [HttpPost("quizzes/{quiz_id}/questions")]
        public async Task<IActionResult> AddQuestion(Guid quiz_id, [FromBody] CreateQuestionViewModel request, CancellationToken token)
        {

            var command = request.Adapt<AddQuestionOrchestrator>() with { QuizId = quiz_id };

            var result = await _mediator.Send(command, token);

            if (result.IsFailure) return result.ToActionResult();


            return Result<object>.Success(new
            {
                question_id = result.Value
            })
                .ToCreatedActionResult();
        }

        [HttpPut("questions/{id}")]
        public async Task<IActionResult> UpdateQuestion(Guid id, [FromBody] UpdateQuestionViewModel request, CancellationToken token)
        {
            var command = request.Adapt<UpdateQuestionOrchestrator>() with { QuestionId = id };
            var result = await _mediator.Send(command, token);
            return result.ToActionResult();
        }
        [HttpDelete("questions/{id:guid}")]
        public async Task<IActionResult> DeleteQuestion(Guid id, CancellationToken token)
        {
          
            var result = await _mediator.Send(new DeleteQuestionOrchestrator(id), token);
            return result.ToActionResult();

        [HttpGet("attempts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAttempts([FromQuery] GetAttemptsQueryParameters parameters, CancellationToken cancellationToken)
        {
            var query = new GetAttemptsQuery(parameters);
            var result = await _mediator.Send(query, cancellationToken);
            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }
            var viewModels = result.Value!.Items.Select(dto => new AttemptListViewModel
            {
                AttemptId = dto.AttemptId,
                StudentId = dto.StudentId,
                StudentName = dto.StudentName,
                QuizTitle = dto.QuizTitle,
                Score = dto.Score,
                Status = dto.Status,
                SubmittedAt = dto.SubmittedAt
            }).ToList();  
            var paginatedViewModels = new PaginatedList<AttemptListViewModel>(
                viewModels,
                result.Value.TotalCount,
                result.Value.PageNumber,
                result.Value.PerPage
            );
            return Result<PaginatedList<AttemptListViewModel>>.Success(paginatedViewModels).ToActionResult();
        }
    }
}