namespace PetService.Application.Core.Exceptions
{
    public class AppException : Exception

    {
        public int StateCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }

        private AppException(int statusCode, string message, string details = null)
        {
            StateCode = statusCode;
            Message = message;
            Details = details;
        }
        public static AppException Create(int statusCode, string message, string details = null)
        {
            return new AppException(statusCode, message, details);
        }
    }
}
