namespace Coffee_Ecommerce.API.Features.Establishment.DTO
{
    public interface IEstablishmentParser<Origin, Destiny>
    {
        public Destiny Parse(Origin entity);
        public List<Destiny> Parse(List<Origin> entities);
    }
}
