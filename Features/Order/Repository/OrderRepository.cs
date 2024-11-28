using Coffee_Ecommerce.API.Features.Establishment.Exceptions;
using Coffee_Ecommerce.API.Features.Order.Exceptions;
using Coffee_Ecommerce.API.Features.Order.GetPage;
using Coffee_Ecommerce.API.Features.User.Exceptions;
using Coffee_Ecommerce.API.Infraestructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Coffee_Ecommerce.API.Features.Order.Repository
{
    public sealed class OrderRepository : IOrderRepository
    {
        private readonly PostgreContext _context;

        public OrderRepository(PostgreContext context)
        {
            _context = context;
        }

        public async Task<OrderEntity> CreateAsync(OrderEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");

            var userExists = await _context.Users.AnyAsync(user => user.Id == entity.UserId);

            if (!userExists)
                throw new UserNotFoundException("Invalid user");

            var establishmentExists = await _context.Establishments.AnyAsync(establishment => establishment.Id == entity.EstablishmentId);

            if (!establishmentExists)
                throw new EstablishmentNotFoundException("Invalid establishment");

            await _context.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await GetByIdAsync(id, cancellationToken);

                _context.Remove(result);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (OrderNotFoundException ex)
            {
                throw new OrderNotFoundException(ex.Message);
            }
        }

        public async Task<List<OrderEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Orders.ToListAsync(cancellationToken);
        }

        public async Task<OrderEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _context.Orders.SingleOrDefaultAsync(order => order.Id == id, cancellationToken);

            if (result == null)
                throw new OrderNotFoundException("Order not found");

            return result;
        }

        public async Task<List<OrderEntity>> GetPageAsync(GetPageCommand command, CancellationToken cancellationToken)
        {
            int position = command.Items * (command.Page - 1);

            var filteredOrders = await GetFilteredAsync(command, cancellationToken);

            List<OrderEntity> orderedOrders = new List<OrderEntity>();
            
            switch (command.OrderBy)
            {
                case "value_descending":
                    orderedOrders = filteredOrders.OrderByDescending(order => order.TotalValue).ToList();
                    break;
                case "value_ascending":
                    orderedOrders = filteredOrders.OrderBy(order => order.TotalValue).ToList();
                    break;
                case "date_descending":
                    orderedOrders = filteredOrders.OrderByDescending(order => order.OrderedAt).ToList();
                    break;
                case "date_ascending":
                    orderedOrders = filteredOrders.OrderBy(order => order.OrderedAt).ToList();
                    break;
                default:
                    orderedOrders = filteredOrders;
                    break;
            }
            
            var result = orderedOrders
                .Skip(position)
                .Take(command.Items)
                .ToList();

            if (result.IsNullOrEmpty())
                throw new OrderNotFoundException("There are no associated orders to this page");

            return result;
        }

        private async Task<List<OrderEntity>> GetFilteredAsync(GetPageCommand command, CancellationToken cancellationToken)
        {
            List<OrderEntity> orders = await _context.Orders.ToListAsync(cancellationToken);

            if (command.EstablishmentId != Guid.Empty && command.EstablishmentId != null)
                orders = orders.Where(order => order.EstablishmentId == command.EstablishmentId).ToList();

            if (command.CustomerId != Guid.Empty && command.CustomerId != null)
                orders = orders.Where(order => order.UserId == command.CustomerId).ToList();

            if (command.Date != DateTime.MinValue && command.Date != null)
                orders = orders.Where(order => order.OrderedAt.Date == command.Date.Value.Date).ToList();

            if (command.TotalValue >= 0)
                orders = orders.Where(order => order.TotalValue == command.TotalValue).ToList();

            return orders;
        }

        public async Task<OrderEntity> UpdateAsync(OrderEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Order cannot be null");

            var order = await _context.Orders
                .AsNoTracking()
                .SingleOrDefaultAsync(order => order.Id == entity.Id, cancellationToken);

            if (order == null)
                throw new OrderNotFoundException("Order not found");

            if (order.Delivered || order.DeniedOrder)
                throw new InvalidOperationException("Cannot update closed order");

            entity.UserId = order.UserId;
            entity.TotalValue = order.TotalValue;
            entity.PaymentMethod = order.PaymentMethod;
            entity.EstablishmentId = order.EstablishmentId;
            entity.OrderedAt = order.OrderedAt.ToUniversalTime();
            entity.Items = order.Items;
            entity.Rated = order.Rated;
            entity.TimeRating = order.TimeRating;
            entity.QualityRating = order.QualityRating;
            entity.UserComments = order.UserComments;
            entity.DeniedOrder = order.DeniedOrder;
            entity.DeniedReason = order.DeniedReason;

            /*if (!order.UserId.Equals(entity.UserId)
                || !order.TotalValue.Equals(entity.TotalValue)
                || !order.PaymentMethod.Equals(entity.PaymentMethod)
                || !order.EstablishmentId.Equals(entity.EstablishmentId)
                || !order.OrderedAt.Equals(entity.OrderedAt)
            )
                throw new ArgumentException("Forbidden property change");*/

            _context.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<OrderEntity> RateAsync(OrderEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentException("Entity cannot be null");

            var order = await _context.Orders
                .AsNoTracking()
                .SingleOrDefaultAsync(order => order.Id == entity.Id, cancellationToken);

            if (order == null)
                throw new OrderNotFoundException("Order not found");

            if (!order.Delivered)
                throw new ArgumentException("This order has not been delivered yet");
            
            if (order.Rated)
                throw new ArgumentException("This order has already been rated");

            if (order.DeliveredAtTime.ToUniversalTime().AddDays(2) < DateTime.UtcNow)
                throw new ApplicationException("Order rating period has ended");

            // I'm nowhere proud of this omg
            // I should just quit

            entity.UserId = order.UserId;
            entity.TotalValue = order.TotalValue;
            entity.PaymentMethod = order.PaymentMethod;
            entity.Paid = order.Paid;
            entity.Delivered = order.Delivered;
            entity.EstablishmentId = order.EstablishmentId;
            entity.DeliveryTime = order.DeliveryTime;
            entity.DeliveredAtTime = order.DeliveredAtTime.ToUniversalTime();
            entity.OrderedAt = order.OrderedAt.ToUniversalTime();
            entity.Items = order.Items;
            entity.DeniedOrder = order.DeniedOrder;
            entity.DeniedReason = order.DeniedReason;

            _context.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
