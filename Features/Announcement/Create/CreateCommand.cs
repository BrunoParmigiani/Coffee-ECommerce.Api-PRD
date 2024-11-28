using System.Text.Json.Serialization;

namespace Coffee_Ecommerce.API.Features.Announcement.Create
{
    public sealed class CreateCommand
    {
        [JsonIgnore]
        public Guid CreatorId { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public Dictionary<int, bool> Recipients { get; set; }
    }
}
