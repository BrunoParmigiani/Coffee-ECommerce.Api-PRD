using System.Text.Json.Serialization;

namespace Coffee_Ecommerce.API.Features.Favorite.GetByUser
{
    public sealed class CreateCommand
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
    }
}