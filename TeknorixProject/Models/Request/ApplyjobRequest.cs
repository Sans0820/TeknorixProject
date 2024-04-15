using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeknorixProject.Models.Request
{
    public class ApplyjobRequest
    {
        public string title { get; set; }
        public string description { get; set; }
        public int locationId { get; set;}
        public int departmentId { get; set; }
        public DateTime closingDate { get; set; }
    }
}