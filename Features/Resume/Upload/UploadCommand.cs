using System.Text.Json.Serialization;

namespace Coffee_Ecommerce.API.Features.Resume.Upload
{
    public sealed class UploadCommand
    {
        public byte[] FileBytes { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}
