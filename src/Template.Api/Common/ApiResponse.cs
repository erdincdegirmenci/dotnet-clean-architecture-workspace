using Nest;

namespace Template.Api.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public string ErrorCode { get; set; }
        public string TransactionCode { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string? message = null, string? transactionCode = null)
        {
            return new()
            {
                Success = true,
                Data = data,
                Message = message,
                TransactionCode = transactionCode
            };
        }

        public static ApiResponse<T> FailResponse(string message, List<string>? errors = null, string? transactionCode = null)
        {
            return new()
            {
                Success = false,
                Message = message,
                Errors = errors,
                TransactionCode = transactionCode
            };
        }
    }
}
