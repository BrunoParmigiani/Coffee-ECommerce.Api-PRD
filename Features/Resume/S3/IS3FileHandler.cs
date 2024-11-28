using Amazon.S3.Model;
using Coffee_Ecommerce.API.Features.Resume.DTO;

namespace Coffee_Ecommerce.API.Features.Resume.S3
{
    public interface IS3FileHandler
    {
        public Task<bool> UploadFileAsync(IFormFile file, string userId, CancellationToken cancellationToken);
        public Task<List<S3ObjectDTO>> GetAllFilesAsync(CancellationToken cancellationToken);
        public Task<S3ObjectDTO> GetFileByKeyAsync(string userId, CancellationToken cancellationToken);
        public Task<bool> DeleteFileAsync(string userId, CancellationToken cancellationToken);
    }
}
