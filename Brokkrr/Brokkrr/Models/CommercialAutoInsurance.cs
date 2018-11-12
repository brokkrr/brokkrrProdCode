using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class CommercialAutoInsurance
    {

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public int UserId { get; set; }
        public string NoOfUnits { get; set; }
        public string DeductibleIfAny { get; set; }
        public string CurrentLimit { get; set; }

        public string NoOfStalls { get; set; }
        public string NoOfLocations { get; set; }
        public string grossrevenueforgarage { get; set; }

        public string DocPath { get; set; }

    }
}