
namespace Coffee_Ecommerce.API.Features.User.DTO
{
    public class UserParser : IUserParser<UserEntity, UserDTO>
    {
        public UserDTO Parse(UserEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Entity cannot be null");

            string[] segmentedAddress = entity.Address.Split(',');
            segmentedAddress = segmentedAddress.Select(x => x.Trim()).ToArray();
            string[] place = segmentedAddress[0].Split(" - ");
            string[] province = segmentedAddress[1].Split(" - ");

            return new UserDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                CPF = entity.CPF,
                PhoneNumber = entity.PhoneNumber,
                PostalCode = segmentedAddress[2],
                Street = place[0],
                District = place[1],
                City = province[0],
                State = province[1],
                Country = segmentedAddress[3],
                Complement = entity.Complement
            };
        }

        public List<UserDTO> Parse(List<UserEntity> entities)
        {
            return entities.Select(Parse).ToList();
        }
    }
}
