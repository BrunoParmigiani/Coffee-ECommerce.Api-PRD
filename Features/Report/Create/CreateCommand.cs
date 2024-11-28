using System.Text.Json.Serialization;

namespace Coffee_Ecommerce.API.Features.Report.Create
{
    public sealed class CreateCommand
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public Guid EstablishmentId { get; set; }
        public Guid OrderId { get; set; }
        public int Problem { get; set; }
        public string Description { get; set; }
    }
}
