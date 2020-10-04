using System;
using System.Collections.Generic;
using System.Text;

namespace AzureLibrary.Models
{
    public class BodyMessageModel
    {
        public string TargetDeviceId { get; set; }
        public string Message { get; set; }


        public BodyMessageModel(string message)
        {
            Message = message;
        }
    }


}
