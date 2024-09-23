using Pokedex.WebApi.DTOs;
using System.Net;

namespace Pokedex.WebApi.Models
{
    public class ResultModel<T>
    {
        public ResultModel
        (
            bool success,
            T? data,
            string errorMessage,
            HttpStatusCode? statusCode = null
        )
        {
            IsSuccess = success;
            Data = data;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public HttpStatusCode? StatusCode { get; set; } = null;

        public static ResultModel<T> Success(T data, HttpStatusCode? httpStatusCode = null) => new(true, data, string.Empty, httpStatusCode);
        public static ResultModel<T> Failure(string errorMessage, HttpStatusCode? httpStatusCode = null) => new(false, default, errorMessage, httpStatusCode);
    }
}
