using Amazon.Runtime.Internal;
using Amazon.S3;
using Amazon.S3.Model;
using Coffee_Ecommerce.API.Features.Resume.DTO;

namespace Coffee_Ecommerce.API.Features.Resume.S3
{
    public class S3FileHandler : IS3FileHandler
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string bucketName;

        public S3FileHandler(IAmazonS3 s3Client, IConfiguration configuration)
        {
            _s3Client = s3Client;
            bucketName = configuration["BucketName"]!;
        }

        public async Task<bool> UploadFileAsync(IFormFile file, string userId, CancellationToken cancellationToken)
        {
            var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
            if (!bucketExists)
                return false;

            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = $"{userId}/resume.pdf",
                InputStream = file.OpenReadStream()
            };

            request.Metadata.Add("Content-Type", "application/pdf");
            await _s3Client.PutObjectAsync(request);
            return true;
        }

        public async Task<List<S3ObjectDTO>> GetAllFilesAsync(CancellationToken cancellationToken)
        {
            var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
            if (!bucketExists)
                return null!;

            var request = new ListObjectsV2Request
            {
                BucketName = bucketName,
            };

            var result = await _s3Client.ListObjectsV2Async(request, cancellationToken);
            var s3Objects = result.S3Objects.Select(s =>
            {
                var urlRequest = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = s.Key,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                };

                return new S3ObjectDTO
                {
                    Name = s.Key.ToString(),
                    PresignedUrl = _s3Client.GetPreSignedURL(urlRequest)
                };
            });

            return s3Objects.ToList();
        }

        public async Task<S3ObjectDTO> GetFileByKeyAsync(string userId, CancellationToken cancellationToken)
        {
            var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
            if (!bucketExists)
                return null!;

            GetObjectResponse? s3Object;

            try
            {
                s3Object = await _s3Client.GetObjectAsync(bucketName, $"{userId}/resume.pdf", cancellationToken);
            }
            catch (AmazonS3Exception)
            {
                throw;
            }

            var urlRequest = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = s3Object.Key,
                Expires = DateTime.UtcNow.AddMinutes(30)
            };

            return new S3ObjectDTO
            {
                Name = s3Object.Key.ToString(),
                PresignedUrl = _s3Client.GetPreSignedURL(urlRequest)
            };
        }

        public async Task<bool> DeleteFileAsync(string userId, CancellationToken cancellationToken)
        {
            var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
            if (!bucketExists)
                return false;

            try
            {
                await _s3Client.DeleteObjectAsync(bucketName, $"{userId}/resume.pdf", cancellationToken);
            }
            catch (AmazonS3Exception)
            {
                throw;
            }

            return true;
        }
    }
}
