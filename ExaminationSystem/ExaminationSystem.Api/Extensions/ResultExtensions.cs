using ExaminationSystem.Api.Shared.Results;
using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Api.Extensions
{
    public static class ResultExtensions
    {
       
        public static IResult ToProblemDetails(this Result result)
        {
            if (result.IsSuccess)
                throw new InvalidOperationException(
                    "Cannot convert a success result to problem details.");

            return CreateProblemDetails(result.Error);
        }

        public static IResult ToProblemDetails<T>(this Result<T> result)
        {
            if (result.IsSuccess)
                throw new InvalidOperationException(
                    "Cannot convert a success result to problem details.");

            return CreateProblemDetails(result.Error);
        }

        private static IResult CreateProblemDetails(Error error)
        {
            var statusCode = GetStatusCode(error.Type);

            return Results.Problem(
                statusCode: statusCode,
                title: GetTitle(error.Type),
                extensions: new Dictionary<string, object?>
                {
                {
                    "errors", new[] { new { error.Code, error.Message } }
                }
                });
        }

        private static int GetStatusCode(ErrorType type) => type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        private static string GetTitle(ErrorType type) => type switch
        {
            ErrorType.Validation => "Bad Request",
            ErrorType.NotFound => "Not Found",
            ErrorType.Conflict => "Conflict",
            ErrorType.Unauthorized => "Unauthorized",
            ErrorType.Forbidden => "Forbidden",
            _ => "Internal Server Error"
        };
    }
}
