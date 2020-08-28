using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System.Collections.Generic;


namespace Company.Function
{
    public static class TimerTriggerCSharp1
    {
        [FunctionName("TimerTriggerCSharp1")]
        public static void Run([TimerTrigger("*/1 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=<STROAGEACCOUNTNAME>;AccountKey=<STORAGE ACCOUNT KEY>;EndpointSuffix=core.windows.net";
            var containerName = "upload";
         
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            // LISTING THE BLOB CONTAINER
            IEnumerable<IListBlobItem> blobs = container.ListBlobs();

            var _fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-5);
            int iBlobCount = 0;
            foreach(IListBlobItem item in blobs)
            {
                 var _lastblobModified = ((CloudBlockBlob)item).Properties.LastModified;

                 //If Blob is added in last 5 minutes, please note the trigger would happen in every 5 minutes. 
               if (_lastblobModified >= _fiveMinutesAgo)
                {
                    //There are  changes has happend :) 
                    log.LogInformation("*************** CHNAGE DETECTED ***************");
                    
                }
                iBlobCount = iBlobCount + 1;
            }

            log.LogInformation($"Total Blob Count ==>>  {iBlobCount}");


        }
    }
 }

