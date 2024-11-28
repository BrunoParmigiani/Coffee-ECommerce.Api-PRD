namespace Coffee_Ecommerce.API.Shared.Models
{
    public class Result<T>
    {
        public T? Data { get; set; } = default;
        public ApiError? Error { get; set; } = default;
    }
}
