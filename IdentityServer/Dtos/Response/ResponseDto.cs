using System.Net;

namespace IdentityServer.Dtos.Response
{
    public class ResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public static ResponseDto<T> CreateSuccess(T data, string message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ResponseDto<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
        }

        public static ResponseDto<T> CreateFail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ResponseDto<T>
            {
                Success = false,
                Message = message,
                Data = default(T),
                StatusCode = statusCode
            };
        }
    }
}
