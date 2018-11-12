using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class WorkersCompensation
    {

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public int UserId { get; set; }

        public string NoOfEmployees { get; set; }
        public string GrossPayroll { get; set; }

        public string DocPath { get; set; }
      
    }
}