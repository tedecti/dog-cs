using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Puppy.Config;
using Puppy.Repositories.Interfaces;
using static System.Guid;

namespace Puppy.Repositories
{
    public class FileRepository : IFileRepository
    {
        private const string BucketName = "Puppy";
        private readonly IConfiguration _configuration;
        private IMinioClient _minioClient;

        public FileRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            var myUuid = NewGuid();
            var fileName = myUuid + "." + file.ContentType.Split("/")[1];
            await using var stream = file.OpenReadStream();
            var uploadFile = await UploadFileToStorage(stream, fileName);
            return uploadFile ? fileName : "";
        }

        public async Task<Stream> GetFile(string fileName)
        {
            return await DownloadFileFromStorage(fileName);
        }

        private async Task<bool> UploadFileToStorage(Stream fileStream, string fileName)
        {
            const string contentType = "image/png";
            var storageConfig = _configuration.GetSection("Minio").Get<MinioStorageConfig>();
            _minioClient = new MinioClient()
                .WithEndpoint(storageConfig?.Endpoint)
                .WithCredentials(storageConfig?.AccessKey, storageConfig?.SecretKey)
                .WithSSL()
                .Build();
            try
            {
                var upload = new PutObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject(fileName)
                    .WithStreamData(fileStream)
                    .WithObjectSize(fileStream.Length)
                    .WithContentType(contentType);
                await _minioClient.PutObjectAsync(upload);
                return true;
            }

            catch (MinioException e)
            {
                return false;
            }
        }

        private async Task<Stream> DownloadFileFromStorage(string fileName)
        {
            var storageConfig = _configuration.GetSection("Minio").Get<MinioStorageConfig>();
            var minioClient = new MinioClient()
                .WithEndpoint(storageConfig?.Endpoint)
                .WithCredentials(storageConfig?.AccessKey, storageConfig?.SecretKey)
                .WithSSL()
                .Build();
            var statObjectArgs = new StatObjectArgs().WithBucket(BucketName).WithObject(fileName);
            await minioClient.StatObjectAsync(statObjectArgs);

            var getObjectArgs = new GetObjectArgs()
                .WithBucket(BucketName)
                .WithObject(fileName);
            var responseStream = new MemoryStream();
            await minioClient.GetObjectAsync(getObjectArgs);

            return responseStream;
        }

        public async Task<bool> DeleteFileFromStorage(string fileName)
        {
            try
            {
                var args = new RemoveObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject(fileName);
                await _minioClient.RemoveObjectAsync(args);
                return true;
            }
            catch (MinioException e)
            {
                return false;
            }
        }
    }
}