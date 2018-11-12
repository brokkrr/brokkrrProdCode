using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class AutoInsurance
    {


        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }


        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Longitude")]
        public string Longitude { get; set; }

        [Display(Name = "Latitude")]
        public string Latitude { get; set; }

        [Required(ErrorMessage = "Please select value")]
        [Display(Name = "New purchase or currently insured?")]
        public string IsInsured { get; set; }

        [Display(Name = "Insurance Company")]
        public string InsuranceCompany { get; set; }

        [Required(ErrorMessage = "Please select value")]
        [Display(Name = "When do you need insurance?")]
        public string CoverageExpires { get; set; }


        [Display(Name = "Language")]
        public string Language { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        public string VehicleType { get; set; }
        public int UserId { get; set; }

        public string DocPath { get; set; }
    }
}