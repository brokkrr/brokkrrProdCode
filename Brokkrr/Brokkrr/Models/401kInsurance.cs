using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class _401kInsurance
    {
        public string ZipCode { get; set; }

        public string City { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public int UserId { get; set; }

        public string CurrentPlan { get; set; }

        public string NumberOfEmp { get; set; }

        public string PlanSize { get; set; }

        public string DocPath { get; set; }
     
    }
}