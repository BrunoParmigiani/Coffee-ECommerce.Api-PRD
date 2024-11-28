
namespace Coffee_Ecommerce.API.Features.Establishment.DTO
{
    public class EstablishmentParser : IEstablishmentParser<EstablishmentEntity, EstablishmentDTO>
    {
        public EstablishmentDTO Parse(EstablishmentEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Entity cannot be null");

            string[] segmentedAddress = entity.Address.Split(',');
            segmentedAddress = segmentedAddress.Select(x => x.Trim()).ToArray();
            string[] place = segmentedAddress[0].Split(" - ");
            string[] province = segmentedAddress[1].Split(" - ");

            return new EstablishmentDTO
            {
                Id = entity.Id,
                Email = entity.Email,
                Name = entity.Name,
                PostalCode = segmentedAddress[2],
                Street = place[0],
                District = place[1],
                City = province[0],
                State = province[1],
                Country = segmentedAddress[3],
                CNPJ = entity.CNPJ,
                AdministratorId = entity.AdministratorId,
                PhoneNumber = entity.PhoneNumber,
                Role = entity.Role,
                Complement = entity.Complement,
            };
        }

        public List<EstablishmentDTO> Parse(List<EstablishmentEntity> entities)
        {
            return entities.Select(Parse).ToList();
        }
    }
}
