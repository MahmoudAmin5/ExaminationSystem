using ExaminationSystem.Api.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Api.Extensions;

public static class ResultExtensions
{
   
    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(new { success = true, data = (object?)null });
        }

        return CreateErrorResult(result.Errors);
    }

    
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(new { success = true, data = result.Value,
                error = (object?)null,
                meta = new { timestamp = DateTime.UtcNow }
            });
        }

        return CreateErrorResult(result.Errors);
    }

    private static IActionResult CreateErrorResult(IReadOnlyCollection<Error> errors)
    {
        var firstError = errors.First();

        
        var response = new
        {
            success = false,
            data = (object?)null,
            error = new
            {
                code = firstError.Code,
                message = firstError.Message,
                details = errors.Select(e => new { e.Code, e.Message }).ToList() 
            }
        };

        return new ObjectResult(response)
        {
            StatusCode = (int)firstError.Type 
        };
    }
}