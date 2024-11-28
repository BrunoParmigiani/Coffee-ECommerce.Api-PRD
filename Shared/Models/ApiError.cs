namespace Coffee_Ecommerce.API.Shared.Models
{
    public sealed class ApiError
    {
        public string Message { get; private set; }

        public ApiError(string message)
        {
            Message = message;
        }
    }
}
