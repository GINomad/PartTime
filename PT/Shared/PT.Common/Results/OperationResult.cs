namespace PT.Common.Results
{
    public class OperationResult
    {
        protected OperationResult()
        {
            Errors = new List<Error>();
        }

        public bool Succeeded { get; protected set; }

        public IReadOnlyCollection<Error> Errors { get; protected set; }

        public static OperationResult FromSuccess()
            => new()
            {
                Succeeded = true,
            };

        public static OperationResult FromFailed(IReadOnlyCollection<Error> errors)
            => new()
            {
                Succeeded = false,
                Errors = errors,
            };

        public OperationResult<TResult> TransformTo<TResult>()
            => Succeeded
            ? OperationResult<TResult>.FromSuccess(default!)
            : OperationResult<TResult>.FromFailed(Errors);
    }

    public class OperationResult<TResult> : OperationResult
    {
        protected OperationResult()
            : base()
        {
        }

        public TResult? Result { get; private set; }

        public static OperationResult<TResult> FromSuccess(TResult result)
            => new()
            {
                Succeeded = true,
                Result = result,
            };

        public static new OperationResult<TResult> FromFailed(IReadOnlyCollection<Error> errors)
            => new()
            {
                Succeeded = false,
                Errors = errors,
            };

        public OperationResult<TAnotherResult> TransformTo<TAnotherResult>(
            Func<TResult, TAnotherResult> transformFunc)
            => new OperationResult<TAnotherResult>
            {
                Errors = Errors,
                Succeeded = Succeeded,
                Result = transformFunc(Result!),
            };
    }
}
