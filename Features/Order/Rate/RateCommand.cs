namespace Coffee_Ecommerce.API.Features.Order.Rate
{
    public sealed class RateCommand
    {
        public Guid Id { get; set; }
        public int TimeRating { get; set; }
        public int QualityRating { get; set; }
        public string? UserComments { get; set; }
    }
}
