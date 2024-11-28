namespace Coffee_Ecommerce.API.Features.Resume.Exceptions
{
    public sealed class ResumeNotFoundException : ApplicationException
    {
        public ResumeNotFoundException(string? message) : base(message)
        { }
    }
}
