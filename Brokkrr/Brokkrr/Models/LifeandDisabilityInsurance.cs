using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class LifeandDisabilityInsurance
    {
        [Display(Name = "User Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter valid ZipCode")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "City can not be blank")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Longitude")]
        public string Longitude { get; set; }

        [Display(Name = "Latitude")]
        public string Latitude { get; set; }

        [Required(ErrorMessage = "Please select value")]
        [Display(Name = "When do you need Insurance?")]
        public string CoverageExpires { get; set; }

        [Display(Name = "Language")]
        public string Language { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Required(ErrorMessage = "Please select value")]
        [Display(Name = "Coverage Amount?")]
        public string FaceValue { get; set; }

        public string DocPath { get; set; }
    }
}