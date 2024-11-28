namespace Coffee_Ecommerce.API.Features.User.Create
{
    public sealed class CreateCommand
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalCode { get; set; }
        public string Complement { get; set; }
    }
}
