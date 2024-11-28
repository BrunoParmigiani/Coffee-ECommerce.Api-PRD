namespace Coffee_Ecommerce.API.Features.Order.DTO
{
    public class OrderParser : IOrderParser<OrderEntity, OrderDTO>
    {
        public OrderDTO Parse(OrderEntity entity)
        {
            return new OrderDTO
            {
                Id = entity.Id,
                UserId = entity.UserId,
                TotalValue = entity.TotalValue,
                PaymentMethod = entity.PaymentMethod,
                Paid = entity.Paid,
                Delivered = entity.Delivered,
                EstablishmentId = entity.EstablishmentId,
                DeliveryTime = entity.DeliveryTime,
                DeliveredAtTime = entity.DeliveredAtTime,
                OrderedAt = entity.OrderedAt,
                Items = entity.Items.Split(";"),
                Rated = entity.Rated,
                TimeRating = entity.TimeRating,
                QualityRating = entity.QualityRating,
                UserComments = entity.UserComments,
                DeniedOrder = entity.DeniedOrder,
                DeniedReason = entity.DeniedReason
            };
        }

        public List<OrderDTO> Parse(List<OrderEntity> entites)
        {
            return entites.Select(Parse).ToList();
        }
    }
}
