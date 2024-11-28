using Coffee_Ecommerce.API.Features.Order;

namespace Coffee_Ecommerce.API.Features.NewOrder.DTO
{
    public sealed class NewOrderParser : INewOrderParser<NewOrderEntity, NewOrderDTO>
    {
        public NewOrderDTO Parse(NewOrderEntity entity)
        {
            string[] serializedItems = entity.Items.Split(";;");
            List<OrderItem> items = new List<OrderItem>();

            foreach (var item in serializedItems)
            {
                string[] values = item.Split(";");
                items.Add(
                        new OrderItem
                        {
                            Name = values[0],
                            Price = float.Parse(values[1]),
                            Quantity = int.Parse(values[2])
                        }
                    );
            }

            return new NewOrderDTO
            {
                Id = entity.Id,
                UserId = entity.UserId,
                PaymentMethod = entity.PaymentMethod,
                EstablishmentId = entity.EstablishmentId,
                DeliveryTime = 0,
                Items = items.ToArray(),
                DeniedOrder = entity.DeniedOrder,
                UserName = entity.UserName,
                UserAddress = entity.UserAddress,
                UserComplement = entity.UserComplement,
                DeniedReason = entity.DeniedReason
            };
        }

        public List<NewOrderDTO> Parse(List<NewOrderEntity> entites)
        {
            return entites.Select(Parse).ToList();
        }
    }
}
