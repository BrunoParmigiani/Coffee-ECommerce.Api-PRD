namespace Coffee_Ecommerce.API.Features.Order.DTO
{
    public interface IOrderParser<Origin, Destiny>
    {
        public Destiny Parse(Origin entity);
        public List<Destiny> Parse(List<Origin> entites);
    }
}
