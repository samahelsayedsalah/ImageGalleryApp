using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BlobStorageAPI.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
    public class BlobStorageDeletionController : ApiController
    {
        [HttpDelete]
        public void DeleteAllImages()
        {
            string connectionString = ConfigurationManager.AppSettings["AzureBlobStorageConnectionString"];
            string containerName = ConfigurationManager.AppSettings["ContainerName"];

            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);

            List<string> names = new List<string>();
            foreach (BlobItem blob in container.GetBlobs())
            {
                container.DeleteBlobIfExists(blob.Name);
            }
        }
    }
}
