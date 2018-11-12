using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class BusinessInsurance
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
        [Display(Name = "Acquisition or Currently Insured?")]
        public string IsInsured { get; set; }

        [Display(Name = "Insurance Company")]
        public string InsuranceCompany { get; set; }

        [Required(ErrorMessage = "Please select value")]
        [Display(Name = "Gross Revenue?")]
        public string Revenue { get; set; }

        [Required(ErrorMessage = "Please select value")]
        [Display(Name = "When do you need Insurance?")]
        public string CoverageExpires { get; set; }

        [Required(ErrorMessage = "Please select value")]
        [Display(Name = "Language")]
        public string Language { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Display(Name = "Industry")]
        public string IndustryId { get; set; }

        [Display(Name = "Sub Industry Id")]
        public string SubIndustryId { get; set; }

        //[Required(ErrorMessage = "Please select value")]
        [Display(Name = "SIC Code")]
        public string SubIndustrySICCode { get; set; }

        public string DocPath { get; set; }
    }
}