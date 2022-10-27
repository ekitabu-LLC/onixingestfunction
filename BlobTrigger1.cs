using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using System.Threading.Tasks;

namespace Company.Function
{
    public class BlobTrigger1
    {
        [FunctionName("BlobTrigger1")]
        public static async Task Run([BlobTrigger("samples-workitems/{name}", Connection = "ekitabuepubtools9324_STORAGE")]Stream inputBlob, string myBlob, ILogger log)
        {
            // retrieve the SOURCE and DESTINATION Storage Account Connection Strings
var sourceConnString = Environment.GetEnvironmentVariable("ekitabuepubtools9324_STORAGE");
var destConnString = Environment.GetEnvironmentVariable("ekitabuepubtools9324_STORAGE");


// Create SOURCE Blob Client
var sourceBlobClient = new BlobClient(sourceConnString, "containerName", myBlob);
// Generate SAS Token for reading the SOURCE Blob with a 2 hour expiration
var sourceBlobSasToken = sourceBlobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.Now);

// Create DESTINATION Blob Client
var destBlobClient = new BlobClient(sourceConnString, "containerName", myBlob);

// Initiate Blob Copy from SOURCE to DESTINATION
await destBlobClient.StartCopyFromUriAsync(sourceBlobSasToken);
        }
    }
}
