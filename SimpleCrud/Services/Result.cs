namespace SimpleCrud.Services
{
    public class Result
    {
        public bool Success { get; init; }
        public string Message { get; init; }
        public IEnumerable<string> Errors { get; init; }

        public static Result Ok(string message = null) => new Result { Success = true, Message = message };
        public static Result Fail(IEnumerable<string> errors, string message = null) => new Result { Success = false, Errors = errors, Message = message };
        public static Result Fail(string error, string message = null) => new Result { Success = false, Errors = new[] { error }, Message = message };
    }

    public class Result<T> : Result
    {
        public T Data { get; init; }

        public static Result<T> Ok(T data, string message = null) => new Result<T> { Success = true, Data = data, Message = message };
        public static new Result<T> Fail(IEnumerable<string> errors, string message = null) => new Result<T> { Success = false, Errors = errors, Message = message };
        public static new Result<T> Fail(string error, string message = null) => new Result<T> { Success = false, Errors = new[] { error }, Message = message };
    }
}
