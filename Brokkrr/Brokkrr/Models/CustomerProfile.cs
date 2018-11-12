using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class CustomerProfile
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNo { get; set; }
        
        public string ZipCode { get; set; }

        public string EmailId { get; set; }

        [Display(Name = "House Type")]
        public string HouseType { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "No Of Cars")]
        public string NoOfCars { get; set; }

        [Display (Name="Occupation")]
        public string TypeOfEmployment { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        public string ProfilePicture { get; set; }

        public string IsCustomerProfileChanged { get; set; }

        public string NoofEmployee { get; set; }

        public string EstPremium { get; set; }

        public string Website { get; set; }

    }
}