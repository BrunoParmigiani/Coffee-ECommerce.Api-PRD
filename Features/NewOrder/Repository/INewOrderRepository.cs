namespace Coffee_Ecommerce.API.Features.NewOrder.Repository
{
    public interface INewOrderRepository
    {
        public Task<bool> CreateAsync(NewOrderEntity entity, CancellationToken cancellationToken);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<NewOrderEntity>> GetByEstablishmentAsync(Guid id, CancellationToken cancellationToken);
    }
}
