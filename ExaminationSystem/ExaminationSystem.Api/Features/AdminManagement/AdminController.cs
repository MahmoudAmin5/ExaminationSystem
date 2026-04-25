using ExaminationSystem.Api.Extensions;
using ExaminationSystem.Api.Features.AdminManagement.Questions.AddQuestions;
using ExaminationSystem.Api.Shared.Results;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Api.Features.AdminManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;
       
       
        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
            
        }

        [HttpPost("quizzes/{quiz_id}/questions")]
        public async Task<IActionResult> AddQuestion(Guid quiz_id, [FromBody] CreateQuestionViewModel request, CancellationToken token)
        {

            var command = request.Adapt<AddQuestionOrchestrator>() with { QuizId = quiz_id };
       
            var result = await _mediator.Send(command, token);
            if (result.IsFailure) 
                return result.ToActionResult();

            var responseData = new 
            {
                question_id = result.Value 
            };
            var successResult = Result<object>.Success(responseData);

            return successResult.ToCreatedActionResult();
        }
    }
}
