namespace Pokedex.WebApi.Models
{
    public class Result<T>
    {
        public Result
        (
            bool success,
            T? data,
            string errorMessage
        )
        {
            IsSuccess = success;
            Data = data;
            ErrorMessage = errorMessage;
        }
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public static Result<T> Success(T data) => new(true, data, string.Empty);
        public static Result<T> Failure(string errorMessage) => new(false, default, errorMessage);
    }
}
