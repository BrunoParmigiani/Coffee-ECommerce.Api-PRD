namespace Coffee_Ecommerce.API.Features.Administrator.DTO
{
    public interface IAdministratorParser<Origin, Destiny>
    {
        public Destiny Parse(Origin entity);
        public List<Destiny> Parse(List<Origin> entities);
    }
}
