using System.Net;

namespace LibraryManagementSystem.Application.DTOs.Response
{
    public class ResponseMessage<T>
    {
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { set; get; } = false;
        public HttpStatusCode StatusCode { set; get; }

        public T? Data { set; get; }

        public static ResponseMessage<T> Created(T data, string message = "Created successfully")
        {
            return new ResponseMessage<T>
            {
                Message = message,
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Data = data
            };
        }

        public static ResponseMessage<T> Success(T data, string message = "Operation successful")
        {
            return new ResponseMessage<T>
            {
                Message = message,
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Data = data
            };
        }

        public static ResponseMessage<T> Failure(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ResponseMessage<T>
            {
                Message = message,
                IsSuccess = false,
                StatusCode = statusCode,
                Data = default
            };
        }

        public static ResponseMessage<T> NotFound(string message = "Resource not found")
        {
            return new ResponseMessage<T>
            {
                Message = message,
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Data = default
            };
        }

        public static ResponseMessage<T> Conflict(string message = "Conflict occurred")
        {
            return new ResponseMessage<T>
            {
                Message = message,
                IsSuccess = false,
                StatusCode = HttpStatusCode.Conflict,
                Data = default
            };
        }

        public static ResponseMessage<T> InternalError(string message = "Internal server error")
        {
            return new ResponseMessage<T>
            {
                Message = message,
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Data = default
            };
        }

        public static ResponseMessage<T> Unauthorized(string message = "Unauthorized")
        {
            return new ResponseMessage<T>
            {
                Message = message,
                IsSuccess = false,
                StatusCode = HttpStatusCode.Unauthorized,
                Data = default
            };
        }
    }
}
