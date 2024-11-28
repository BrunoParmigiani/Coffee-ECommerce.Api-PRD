namespace Coffee_Ecommerce.API.Features.Report.Exceptions
{
    public sealed class ReportNotFoundException : ApplicationException
    {
        public ReportNotFoundException(string? message) : base(message)
        { }
    }
}
