using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using System.IO;
using System.Threading.Tasks;
using AlpineSkiHouse.Web.Configuration.Models;
using AlpineSkiHouse.Web.Events;

namespace AlpineSkiHouse.Web.Services
{
    public class BlobFileUploadService : IBlobFileUploadService
    {
        private readonly ILogger<BlobFileUploadService> _logger;
        private readonly AzureStorageSettings _storageSettings;
        private readonly IMediator _bus;

        public BlobFileUploadService(IOptions<AzureStorageSettings> storageSettings, ILogger<BlobFileUploadService> logger, IMediator bus)
        {
            _storageSettings = storageSettings.Value;
            _logger = logger;
            _bus = bus;
        }

        public async Task<string> UploadFileFromStream(string containerName, string targetFilename, Stream imageStream)
        {
            // prepare the client
            var storageAccount = CloudStorageAccount.Parse(_storageSettings.AzureStorageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();

            // push image to container
            var blob = container.GetBlockBlobReference(targetFilename);
            await blob.UploadFromStreamAsync(imageStream);
            _logger.LogInformation($"Ski card image uploaded as {targetFilename}");

            // publish event that image was uploaded
            await _bus.Publish(new SkiCardImageUploaded { FileName = targetFilename });
            return blob?.Uri.ToString();
        }

    }
}
