namespace Coffee_Ecommerce.API.Features.Report.Update
{
    public sealed class UpdateCommand
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
    }
}
