using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BlobStorageAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, OPTIONS")]
    public class BlobStorageController : ApiController
    {
        [HttpPost]
        public void UploadImage()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Files.Count > 0)
            {

                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/" + "Images\\" + postedFile.FileName);

                    string connectionString = ConfigurationManager.AppSettings["AzureBlobStorageConnectionString"];
                    string containerName = ConfigurationManager.AppSettings["ContainerName"];

                    BlobContainerClient container = new BlobContainerClient(connectionString, containerName);

                    var blobClient = container.GetBlobClient(postedFile.FileName);
                        blobClient.Upload(filePath);
                }
            }

            
        }

        [HttpGet]
        public List<string> GetImages()
        {
            string connectionString = ConfigurationManager.AppSettings["AzureBlobStorageConnectionString"];
            string containerName = ConfigurationManager.AppSettings["ContainerName"];

            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
            
            List<string> names = new List<string>();
            foreach (BlobItem blob in container.GetBlobs())
            {
                names.Add(blob.Name);
            }
            return names;
        }

        [HttpDelete]
        public void DeleteImage(string imageName)
        {
            string connectionString = ConfigurationManager.AppSettings["AzureBlobStorageConnectionString"];
            string containerName = ConfigurationManager.AppSettings["ContainerName"];

            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
            container.DeleteBlobIfExists(imageName);
        }

    }

    public class UploadFileRequest
    {
        public string filename { get; set; }
        public string filepath { get; set; }
    }
}
