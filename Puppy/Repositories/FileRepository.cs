using System.Net;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Puppy.Config;
using Puppy.Repositories.Interfaces;
using static System.Guid;

namespace Puppy.Repositories
{
    public class FileRepository : IFileRepository
    {
        private const string BucketName = "puppy";
        private readonly IConfiguration _configuration;

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
            const string contentType = "application/octet-stream";
            var storageConfig = _configuration.GetSection("Minio").Get<MinioStorageConfig>();

            if (fileStream.CanSeek)
                fileStream.Position = 0;
            try
            {
                var minioClient = new MinioClient()
                    .WithEndpoint(storageConfig.Endpoint)
                    .WithCredentials(storageConfig.AccessKey, storageConfig.SecretKey)
                    .WithSSL()
                    .Build();
                var bArgs = new BucketExistsArgs().WithBucket("puppy");
                var existsBucketsAsync = minioClient.BucketExistsAsync(bArgs);
                if (existsBucketsAsync != null)
                {
                    var args = new MakeBucketArgs().WithBucket("puppy");
                    await minioClient.MakeBucketAsync(args);
                }

                var upload = new PutObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject(fileName)
                    .WithStreamData(fileStream)
                    .WithObjectSize(fileStream.Length);
                await minioClient.PutObjectAsync(upload);
                return true;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private async Task<MemoryStream> DownloadFileFromStorage(string fileName)
        {
            var storageConfig = _configuration.GetSection("Minio").Get<MinioStorageConfig>();
            var minioClient = new MinioClient()
                .WithEndpoint(storageConfig.Endpoint)
                .WithCredentials(storageConfig.AccessKey, storageConfig.SecretKey)
                .WithSSL()
                .Build();

            var statObjectArgs = new StatObjectArgs()
                .WithBucket(BucketName)
                .WithObject(fileName);

            var memoryStream = new MemoryStream();
            try
            {
                var getObjectArgs = new GetObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject(fileName)
                    .WithCallbackStream( stream =>
                    {
                        stream.CopyTo(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                    });
                await minioClient.GetObjectAsync(getObjectArgs);

                return memoryStream;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public async Task<bool> DeleteFileFromStorage(string fileName)
        {
            var storageConfig = _configuration.GetSection("Minio").Get<MinioStorageConfig>();
            var minioClient = new MinioClient()
                .WithEndpoint(storageConfig?.Endpoint)
                .WithCredentials(storageConfig?.AccessKey, storageConfig?.SecretKey)
                .WithSSL()
                .Build();
            try
            {
                var args = new RemoveObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject(fileName);
                await minioClient.RemoveObjectAsync(args);
                return true;
            }
            catch (MinioException e)
            {
                return false;
            }
        }
    }
}