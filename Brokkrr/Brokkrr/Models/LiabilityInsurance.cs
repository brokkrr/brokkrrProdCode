using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class LiabilityInsurance
    {

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public int UserId { get; set; }

        public string GrossSale { get; set; }
        public string DeductibleIfAny { get; set; }

        public string IndustryId { get; set; }

        public string SubIndustryId { get; set; }

        public string SubIndustrySICCode { get; set; }

        public string DocPath { get; set; }

    }
}