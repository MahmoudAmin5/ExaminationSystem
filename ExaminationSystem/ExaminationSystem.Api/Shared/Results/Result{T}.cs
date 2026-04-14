namespace ExaminationSystem.Api.Shared.Results
{
    public class Result<T> : Result
    {
        public T? Value { get; }

        protected internal Result(T? value, bool isSuccess, IEnumerable<Error> errors)
       : base(isSuccess, errors)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new(value, true, new[] { Error.None });
        public new static Result<T> Failure(Error error) => new(default, false, new[] { error });
        public new static Result<T> Failure(IEnumerable<Error> errors) => new(default, false, errors);

        public static implicit operator Result<T>(T value) => Success(value);
    }
}
