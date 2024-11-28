namespace Coffee_Ecommerce.API.Features.Establishment.Create
{
    public sealed class CreateCommand
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PostalCode { get; set; }
        public string Complement { get; set; }
        public string CNPJ { get; set; }
        public Guid AdministratorId { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
