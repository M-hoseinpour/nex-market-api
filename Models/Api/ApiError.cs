namespace ApiFramework.Models
{
    public class ApiError
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public ApiError()
        {
        }

        public ApiError(string message)
        {
            Message = message;
        }

        public ApiError(string type, string message)
        {
            Type = type;
            Message = message;
        }
    }
}