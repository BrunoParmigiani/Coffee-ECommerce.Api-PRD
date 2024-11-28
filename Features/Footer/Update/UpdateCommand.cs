namespace Coffee_Ecommerce.API.Features.FooterItem.Update
{
    public sealed class UpdateCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    }
}
