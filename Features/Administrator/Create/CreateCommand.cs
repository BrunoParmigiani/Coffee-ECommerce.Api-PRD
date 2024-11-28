namespace Coffee_Ecommerce.API.Features.Administrator.Create
{
    public sealed class CreateCommand
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public Guid EstablishmentId { get; set; }
    }
}
