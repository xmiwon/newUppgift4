using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ClassLibrary2.Models;
using Microsoft.Azure.Devices;
using ClassLibrary2.Services;

namespace AzureFunctions
{
    public static class SendMessageToDevice
    {
        //Istället för hardcode så sätter vi strängen till en json fil och hämtar därifrån istället med IotHubConnection som namn
        private static readonly ServiceClient serviceClient =
            ServiceClient.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHubConnection"));


        [FunctionName("SendMessageToDevice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            //QueryString = localhost:7071/api/sendmessagetodevice?targetid=consoleapp&message=dettaarmeddelandet
            string targetDeviceId = req.Query["targetdeviceid"];
            string message = req.Query["message"];

            //Http body = { "targetdeviceid": "consoleapp", "message": "detta är ett meddelandde" }
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();



            var data = JsonConvert.DeserializeObject<BodyMessageModel>(requestBody);

            targetDeviceId = targetDeviceId ?? data?.TargetDeviceId;
            message = message ?? data?.Message;
             Console.WriteLine(message);

            await DeviceService.SendMessageToDeviceAsync(serviceClient, targetDeviceId, message);


            return new OkResult();
        }
    }
}