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

        public static Error BadRequest(string code, string message) => new(code, message, ErrorType.BadRequest);
        public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);
        public static Error Validation(string code, string message) => new(code, message, ErrorType.Validation);
        public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);
        public static Error Unauthorized(string code, string message) => new(code, message, ErrorType.Unauthorized);
        public static Error Forbidden(string code, string message) => new(code, message, ErrorType.Forbidden);
        public static Error TooManyRequests(string code, string message) => new(code, message, ErrorType.TooManyRequests);
        public static Error Gone(string code, string message) => new(code, message, ErrorType.Gone);
        public static Error Unexpected(string code, string message) => new(code, message, ErrorType.Unexpected);

    }

}
