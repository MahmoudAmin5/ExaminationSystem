namespace ExaminationSystem.Api.Shared.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
                throw new ArgumentException("Success result cannot have an error.", nameof(error));

            if (!isSuccess && error == Error.None)
                throw new ArgumentException("Failure result must have an error.", nameof(error));

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, Error.None);

        public static Result Failure(Error error) => new(false, error);

        public static implicit operator Result(Error error) => Failure(error);
    }
}
