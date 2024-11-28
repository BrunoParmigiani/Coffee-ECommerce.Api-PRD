using Amazon.Runtime.Internal;
using Amazon.S3;
using Coffee_Ecommerce.API.Features.Resume.Delete;
using Coffee_Ecommerce.API.Features.Resume.DTO;
using Coffee_Ecommerce.API.Features.Resume.GetAll;
using Coffee_Ecommerce.API.Features.Resume.GetById;
using Coffee_Ecommerce.API.Features.Resume.S3;
using Coffee_Ecommerce.API.Features.Resume.Upload;
using Coffee_Ecommerce.API.Shared.Models;

namespace Coffee_Ecommerce.API.Features.Resume.Business
{
    public sealed class ResumeBusiness : IResumeBusiness
    {
        private readonly IS3FileHandler _s3Handler;
        private readonly ILogger<ResumeBusiness> logger;

        public ResumeBusiness(IS3FileHandler s3Handler, ILogger<ResumeBusiness> logger)
        {
            _s3Handler = s3Handler;
            this.logger = logger;
        }

        public async Task<UploadResult> UploadFileAsync(UploadCommand command, CancellationToken cancellationToken)
        {
            var validationResult = UploadValidator.CheckForErrors(command);

            if (validationResult != null)
                return new UploadResult { Error = validationResult };

            var stream = new MemoryStream(command.FileBytes);
            var file = new FormFile(stream, 0, command.FileBytes.Length, "name", "fileName");

            var result = await _s3Handler.UploadFileAsync(file, command.UserId.ToString(), cancellationToken);

            stream.Dispose();

            return new UploadResult { Data = result };
        }

        public async Task<GetAllResult> GetAllFilesAsync(CancellationToken cancellationToken)
        {
            var result = await _s3Handler.GetAllFilesAsync(cancellationToken);

            return new GetAllResult
            {
                Data = result
            };
        }

        public async Task<GetByIdResult> GetFileAsync(Guid id, CancellationToken cancellationToken)
        {
            var validationResult = GetByIdValidator.CheckForErrors(id);

            if (validationResult != null)
                return new GetByIdResult { Error = validationResult };

            S3ObjectDTO result;

            try
            {
                result = await _s3Handler.GetFileByKeyAsync(id.ToString(), cancellationToken);
            }
            catch (AmazonS3Exception ex)
            {
                return new GetByIdResult { Error = new ApiError(ex.Message) };
            }

            return new GetByIdResult { Data = result };
        }

        public async Task<DeleteResult> DeleteFileAsync(Guid id, CancellationToken cancellationToken)
        {
            var validationResult = DeleteValidator.CheckForErrors(id);

            if (validationResult != null)
                return new DeleteResult { Error = validationResult };

            bool result;

            try
            {
                result = await _s3Handler.DeleteFileAsync(id.ToString(), cancellationToken);
            }
            catch (AmazonS3Exception ex)
            {
                return new DeleteResult { Error = new ApiError(ex.Message) };
            }

            return new DeleteResult { Data = result };
        }
    }
}
