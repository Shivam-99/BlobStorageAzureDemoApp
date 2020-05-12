using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading.Tasks;
namespace BlobAppDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string localPath = "./BlobFiles/";
            Console.WriteLine("Enter the file name ");
            string fileName = Console.ReadLine() + Guid.NewGuid() + ".txt";

            Console.WriteLine("Enter the content to be entered and press Enter  ");
            string content = Console.ReadLine();

            UploadFile(fileName, localPath, content);
            Console.ReadLine();
        }

        static async void UploadFile(string fileName,string FilePath,string content)
        {
            string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
            string fullPath = Path.Combine(FilePath, fileName);
            ;
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            string containerName = "RandomBlobTest" + Guid.NewGuid();
            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
            await File.WriteAllTextAsync(FilePath, content);

            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            using FileStream uploadFileStream = File.OpenRead(fullPath);
            await blobClient.UploadAsync(uploadFileStream, true);
            uploadFileStream.Close();
        }
    }
}
