using Coffee_Ecommerce.API.Features.Resume.Delete;
using Coffee_Ecommerce.API.Features.Resume.GetAll;
using Coffee_Ecommerce.API.Features.Resume.GetById;
using Coffee_Ecommerce.API.Features.Resume.Upload;

namespace Coffee_Ecommerce.API.Features.Resume.Business
{
    public interface IResumeBusiness
    {
        public Task<UploadResult> UploadFileAsync(UploadCommand command, CancellationToken cancellationToken);
        public Task<GetAllResult> GetAllFilesAsync(CancellationToken cancellationToken);
        public Task<GetByIdResult> GetFileAsync(Guid id, CancellationToken cancellationToken);
        public Task<DeleteResult> DeleteFileAsync(Guid id, CancellationToken cancellationToken);
    }
}
