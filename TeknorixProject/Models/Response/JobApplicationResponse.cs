using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeknorixProject.Models.Response
{
    public class JobApplicationResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public JobResponseModel Job { get; set; }
    }
}