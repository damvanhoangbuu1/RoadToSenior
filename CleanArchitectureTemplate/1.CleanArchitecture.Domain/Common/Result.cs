namespace _1.CleanArchitecture.Domain.Common
{
    public class Result<T>
    {
        public bool Succeeded { get; private set; }
        public T Data { get; private set; }
        public string Message { get; private set; }
        public List<string> Errors { get; private set; }

        public static Result<T> Success(T data, string message = null) =>
            new() { Succeeded = true, Data = data, Message = message };

        public static Result<T> Failure(IEnumerable<string> errors) =>
            new() { Succeeded = false, Errors = errors.ToList() };
    }
}
