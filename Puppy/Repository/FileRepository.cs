using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using Puppy.Config;
using Puppy.Repository.IRepository;

namespace Puppy.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public FileRepository(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            Guid myuuid = Guid.NewGuid();
            string fileName = myuuid.ToString() + "." + file.ContentType.Split("/")[1].ToString();

            // Save the file to Azure Blob Storage
            using (var stream = file.OpenReadStream())
            {
                await UploadFileToStorage(stream, fileName);
            }

            return fileName;
        }
        

        public async Task<Stream> GetFile(string fileName)
        {
            return await DownloadFileFromStorage(fileName);
        }

        private async Task<bool> UploadFileToStorage(Stream fileStream, string fileName)
        {
            // Retrieve Azure Storage configuration from appsettings.json
            var storageConfig = _configuration.GetSection("AzureStorageConfig").Get<AzureStorageConfig>();

            // Create the BlobServiceClient
            var blobServiceClient = new BlobServiceClient(storageConfig.ConnectionString);

            // Get a reference to the container
            var containerClient = blobServiceClient.GetBlobContainerClient(storageConfig.ImageContainer);

            // Get a reference to the blob
            var blobClient = containerClient.GetBlobClient(fileName);

            // Upload the file
            await blobClient.UploadAsync(fileStream, true);

            return true;
        }

        private async Task<Stream> DownloadFileFromStorage(string fileName)
        {
            // Retrieve Azure Storage configuration from appsettings.json
            var storageConfig = _configuration.GetSection("AzureStorageConfig").Get<AzureStorageConfig>();

            // Create the BlobServiceClient
            var blobServiceClient = new BlobServiceClient(storageConfig.ConnectionString);

            // Get a reference to the container
            var containerClient = blobServiceClient.GetBlobContainerClient(storageConfig.ImageContainer);

            // Get a reference to the blob
            var blobClient = containerClient.GetBlobClient(fileName);

            // Download the blob
            var response = await blobClient.OpenReadAsync();
            return response;
        }
    }
}
