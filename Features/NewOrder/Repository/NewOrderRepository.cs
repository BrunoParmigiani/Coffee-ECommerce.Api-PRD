
using Coffee_Ecommerce.API.Features.NewOrder.Exceptions;
using Coffee_Ecommerce.API.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace Coffee_Ecommerce.API.Features.NewOrder.Repository
{
    public sealed class NewOrderRepository : INewOrderRepository
    {
        private readonly PostgreContext _context;

        public NewOrderRepository(PostgreContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(NewOrderEntity entity, CancellationToken cancellationToken)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity), "Cannot be null");

            await _context.AddAsync(entity, cancellationToken);
            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result < 1)
                return false;

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _context.NewOrders.SingleOrDefaultAsync(newOrder => newOrder.Id == id);

            if (result is null)
                throw new NewOrderNotFoundException("New order not found");

            _context.Remove(result);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<List<NewOrderEntity>> GetByEstablishmentAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.NewOrders.Where(NewOrder => NewOrder.EstablishmentId == id).ToListAsync(cancellationToken);
        }
    }
}
