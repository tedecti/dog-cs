using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Puppy.Config;
using Puppy.Repository.Interfaces;

namespace Puppy.Repository
{
    public class FileRepository : IFileRepository
    {
        private const string BucketName = "Puppy";
        private readonly IConfiguration _configuration;

        public FileRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            var myUuid = Guid.NewGuid();
            var fileName = myUuid + "." + file.ContentType.Split("/")[1];
            await using var stream = file.OpenReadStream();
            await UploadFileToStorage(stream, fileName);

            return fileName;
        }
        

        public async Task<Stream> GetFile(string fileName)
        {
            return await DownloadFileFromStorage(fileName);
        }

        private async Task<bool> UploadFileToStorage(Stream fileStream, string fileName)
        {

            const string contentType = "image/png";
            var storageConfig = _configuration.GetSection("Minio").Get<MinioStorageConfig>();
            var minioClient = new MinioClient()
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
                await minioClient.PutObjectAsync(upload);
                return true;
            }
                
            catch (MinioException e)
            {
                Console.WriteLine($"[Minio Error] {e.Message}");
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
    }
}
