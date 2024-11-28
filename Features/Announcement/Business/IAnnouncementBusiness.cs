using Coffee_Ecommerce.API.Features.Announcement.Create;
using Coffee_Ecommerce.API.Features.Announcement.Delete;
using Coffee_Ecommerce.API.Features.Announcement.GetAll;
using Coffee_Ecommerce.API.Features.Announcement.GetById;
using Coffee_Ecommerce.API.Features.Announcement.GetByRole;
using Coffee_Ecommerce.API.Features.Announcement.Update;

namespace Coffee_Ecommerce.API.Features.Announcement.Business
{
    public interface IAnnouncementBusiness
    {
        public Task<CreateResult> CreateAsync(CreateCommand command, CancellationToken cancellationToken);
        public Task<DeleteResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public Task<GetAllResult> GetAllAsync(CancellationToken cancellationToken);
        public Task<GetByIdResult> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<GetByRoleResult> GetByRoleAsync(string role, Guid userId, CancellationToken cancellationToken);
        public Task<UpdateResult> UpdateAsync(UpdateCommand command, CancellationToken cancellationToken);
    }
}
