using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class GetZipWithCityName
    {
        [Required(ErrorMessage = "ZipCode is required")]
        [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "Enter valid ZipCode.")]
        public string ZipCode { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Country { get; set; }
        public string Company { get; set; }
    }
}