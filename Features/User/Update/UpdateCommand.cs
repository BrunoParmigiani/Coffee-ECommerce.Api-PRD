using System.Text.Json.Serialization;

namespace Coffee_Ecommerce.API.Features.User.Update
{
    public sealed class UpdateCommand
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalCode { get; set; }
        public string Complement { get; set; }
    }
}
