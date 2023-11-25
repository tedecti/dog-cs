using AutoMapper;
using Azure.Storage;
using Azure.Storage.Blobs;
using Curs.Data;
using Puppy.Config;
using Puppy.Repository.IRepository;

namespace Puppy.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;


        public FileRepository(AppDbContext context, IConfiguration configuration, IMapper mapper,
            IWebHostEnvironment environment)
        {
            _context = context;
            _mapper = mapper;
            _environment = environment;
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            Guid myuuid = Guid.NewGuid();
            string fName = myuuid.ToString() + "." + file.ContentType.Split("/")[1].ToString();


            string path = Path.Combine(_environment.ContentRootPath, "Images", fName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fName;

        }
        public static async Task<bool> UploadFileToStorage(Stream fileStream, string fileName,
            AzureStorageConfig _storageConfig)
        {
            // Create a URI to the blob
            Uri blobUri = new Uri("https://" +
                                  _storageConfig.AccountName +
                                  ".blob.core.windows.net/" +
                                  _storageConfig.ImageContainer +
                                  "/" + fileName);

            // Create StorageSharedKeyCredentials object by reading
            // the values from the configuration (appsettings.json)
            StorageSharedKeyCredential storageCredentials =
                new StorageSharedKeyCredential(_storageConfig.AccountName, _storageConfig.AccountKey);

            // Create the blob client.
            BlobClient blobClient = new BlobClient(blobUri, storageCredentials);

            // Upload the file
            await blobClient.UploadAsync(fileStream);

            return await Task.FromResult(true);
        }
    }
}
