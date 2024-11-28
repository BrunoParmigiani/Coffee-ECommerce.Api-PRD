namespace Coffee_Ecommerce.API.Features.NewOrder.DTO
{
    public interface INewOrderParser<Origin, Destiny>
    {
        public Destiny Parse(Origin entity);
        public List<Destiny> Parse(List<Origin> entites);
    }
}
