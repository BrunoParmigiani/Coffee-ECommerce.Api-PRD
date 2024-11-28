namespace Coffee_Ecommerce.API.Features.Administrator.DTO
{
    public sealed class AdministratorParser : IAdministratorParser<AdministratorEntity, AdministratorDTO>
    {
        public AdministratorDTO Parse(AdministratorEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Entity cannot be null");

            return new AdministratorDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                CPF = entity.CPF,
                PhoneNumber = entity.PhoneNumber,
                Role = entity.Role,
                EstablishmentId = entity.EstablishmentId
            };
        }

        public List<AdministratorDTO> Parse(List<AdministratorEntity> entities)
        {
            return entities.Select(Parse).ToList();
        }
    }
}
