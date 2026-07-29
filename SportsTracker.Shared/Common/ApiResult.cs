namespace SportsTracker.Shared.Common
{
    public sealed class ApiResult<T>
    {
        private ApiResult(bool success, T? value, Error? error, int? statusCode)
        {
            Success = success;
            Value = value;
            Error = error;
            StatusCode = statusCode;
        }
        
        public bool Success { get; }
        public T? Value { get; }
        public Error? Error { get; }
        public int? StatusCode { get; }

        public static ApiResult<T> Ok(T value, int? statusCode = null)
        {
            return new ApiResult<T>(true, value, null, statusCode);
        }

        public static ApiResult<T> Fail(Error error, int? statusCode = null)
        {
            return new ApiResult<T>(false, default, error, statusCode);
        }
    }
}