namespace Coffee_Ecommerce.API.Features.Report.GetPage
{
    public sealed class GetPageCommand
    {
        public int Page { get; set; }
        public int Items { get; set; }
        public string? OrderBy { get; set; } = default;
        public Guid? EstablishmentId { get; set; } = default;
        public Guid? CustomerId { get; set; } = default;
        public DateTime? Date { get; set; } = default;
        public int Status { get; set; } = -1;
    }
}
