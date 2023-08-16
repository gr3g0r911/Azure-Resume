using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Company.Function
{
    public static class GetResumeCounter
    {
        [FunctionName("GetResumeCounter")]
        public static /*async*/ HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName:"AzResume", containerName:"counter", Connection = "AzureResumeConnectionString", Id = "1", PartitionKey = "1")] Counter counter,
            [CosmosDB(databaseName:"AzResume", containerName:"counter", Connection = "AzureResumeConnectionString", Id = "1", PartitionKey = "1")] Counter updatedCounter,
            ILogger log)
        {
            // Here is where the counter gets updated
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            updatedCounter = counter;
            updatedCounter.Count += 1;

            var jsonToReturn = JsonConvert.SerializeObject(counter);

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json")
            };

            /*string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name ??= data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);*/
        }
    }
}
