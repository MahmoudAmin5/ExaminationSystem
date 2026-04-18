using ExaminationSystem.Api.Features.QuizEngine.TimerHandling.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExaminationSystem.Api.Filters
{
    public class AttemptDeadlineFilter : IAsyncActionFilter
    {
        private readonly IMediator _mediator;

        public AttemptDeadlineFilter(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {

            if (!context.ActionArguments.TryGetValue("attemptId", out var attemptIdObj)
                || attemptIdObj is not Guid attemptId)
            {
                await next();
                return;
            }

           
            var result = await _mediator.Send(
                new AutoSubmitExpiredAttemptCommand(attemptId));

            if (result.IsSuccess && result.Value.WasAutoSubmitted)
            {
                context.Result = new ObjectResult(new
                {
                    success = false,
                    data = (object?)null,
                    error = new
                    {
                        code = "QuizAttempt.TimedOut",
                        message = "This attempt has timed out and was auto-submitted.",
                        details = new[]
                        {
                        new
                        {
                            code = "QuizAttempt.TimedOut",
                            message = "This attempt has timed out and was auto-submitted.",
                            score = result.Value.Score,
                            passed = result.Value.IsPassed
                        }
                    }
                    }
                })
                {
                    StatusCode = StatusCodes.Status410Gone
                };

                return; 
            }

            await next();
        }
    }
}
