using System.Text.Json.Serialization;

namespace Coffee_Ecommerce.API.Features.Announcement.Update
{
    public sealed class UpdateCommand
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid CreatorId { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public Dictionary<int, bool> Recipients { get; set; }
    }
}
