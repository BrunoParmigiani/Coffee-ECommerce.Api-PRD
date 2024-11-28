namespace Coffee_Ecommerce.API.Features.Establishment.Update
{
    public sealed class UpdateCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PostalCode { get; set; }
        public string Complement { get; set; }
        public string CNPJ { get; set; }
        public Guid AdministratorId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
