namespace ExaminationSystem.Api.Shared.Results
{

    public sealed record Error
    {
        public string Code { get; }
        public string Message { get; }
        public ErrorType Type { get; }

        private Error(string code, string message, ErrorType type)
        {
            Code = code;
            Message = message;
            Type = type;
        }

      

        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Unexpected);

      

        public static Error NotFound(string entity, object id) =>
            new($"{entity}.NotFound",
                $"{entity} with ID '{id}' was not found.",
                ErrorType.NotFound);

        public static Error Conflict(string entity, string reason) =>
            new($"{entity}.Conflict",
                reason,
                ErrorType.Conflict);

        public static Error Validation(string code, string message) =>
            new(code,
                message,
                ErrorType.Validation);

        public static Error Unauthorized(string message = "You are not authorized.") =>
            new("Auth.Unauthorized",
                message,
                ErrorType.Unauthorized);

        public static Error Forbidden(string message = "You don't have permission.") =>
            new("Auth.Forbidden",
                message,
                ErrorType.Forbidden);

        public static Error Unexpected(string message = "An unexpected error occurred.") =>
            new("Error.Unexpected",
                message,
                ErrorType.Unexpected);
    }

}
