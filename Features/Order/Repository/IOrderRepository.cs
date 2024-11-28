using Coffee_Ecommerce.API.Features.Order.GetPage;

namespace Coffee_Ecommerce.API.Features.Order.Repository
{
    public interface IOrderRepository
    {
        public Task<OrderEntity> CreateAsync(OrderEntity entity, CancellationToken cancellationToken);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<OrderEntity>> GetAllAsync(CancellationToken cancellationToken);
        public Task<OrderEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<OrderEntity>> GetPageAsync(GetPageCommand command, CancellationToken cancellationToken);
        public Task<OrderEntity> UpdateAsync(OrderEntity entity, CancellationToken cancellationToken);
        public Task<OrderEntity> RateAsync(OrderEntity entity, CancellationToken cancellationToken);
    }
}
