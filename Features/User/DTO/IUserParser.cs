namespace Coffee_Ecommerce.API.Features.User.DTO
{
    public interface IUserParser<Origin, Destiny>
    {
        public Destiny Parse(Origin entity);
        public List<Destiny> Parse(List<Origin> entities);
    }
}
