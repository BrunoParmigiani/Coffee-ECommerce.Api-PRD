using System.Text.Json.Serialization;

namespace Coffee_Ecommerce.API.Features.Product.Update
{
    public sealed class UpdateCommand
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid CreatorId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public Guid EstablishmentId { get; set; }
    }
}
