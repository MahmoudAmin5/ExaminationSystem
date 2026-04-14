namespace ExaminationSystem.Api.Shared.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public IReadOnlyList<Error> Errors { get; }

        protected Result(bool isSuccess, IEnumerable<Error> errors)
        {
            var errorList = errors.ToList();

            if (isSuccess && errorList.Any(e => e != Error.None))
                throw new ArgumentException("Success result cannot have errors.", nameof(errors));

            if (!isSuccess && !errorList.Any())
                throw new ArgumentException("Failure result must have at least one error.", nameof(errors));

            IsSuccess = isSuccess;
            Errors = errorList.AsReadOnly();
        }

        public static Result Success() => new(true, new[] { Error.None });
        public static Result Failure(Error error) => new(false, new[] { error });
        public static Result Failure(IEnumerable<Error> errors) => new(false, errors);
    }
}
