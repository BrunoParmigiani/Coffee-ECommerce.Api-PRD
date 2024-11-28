namespace Coffee_Ecommerce.API.Features.Administrator.Update
{
    public sealed class UpdateCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string PhoneNumber { get; set; }
        public Guid EstablishmentId { get; set; }
    }
}
