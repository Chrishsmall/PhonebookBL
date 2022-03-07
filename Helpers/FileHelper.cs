using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using PhonebookBL.Data;
using PhonebookBL.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PhonebookBL.Helpers
{
    //Chris: I wanted to add a photo to each contact. But I ran out of time.
    //The photo would have been stored on my StorageAccount linked to my Azure profile.
    //I have removed my access key.
    public static class FileHelper
    {
        public async static Task<string> UploadImage(IFormFile file)
        {
            string connectionString = "";
            string containerName = "";

            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);
            var memoryStream = new MemoryStream();

            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            await blobClient.UploadAsync(memoryStream);
            return blobClient.Uri.AbsoluteUri;
        }
    }
}
