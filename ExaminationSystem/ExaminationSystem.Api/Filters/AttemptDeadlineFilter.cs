
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

            await next();
        }
    }
}
