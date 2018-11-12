using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrokerMVC.Models
{
    public class BrokerProfile
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }
                
        public string LastName { get; set; }

        public string PhoneNo1 { get; set; }

        public string Email { get; set; }
    
        public string Address { get; set; }

        public string Area { get; set; }

        public string ZipCode { get; set; }

        public string Company { get; set; }
        
        public string Title { get; set; }
        
        public string Resume { get; set; }
        
        public string Awards { get; set; }

        public string Specialities { get; set; }

        public string HomeValue { get; set; }
        public string AutoType { get; set; }
        public string Revenue { get; set; }
        public string Employees { get; set; }
        public string CoverageAmt { get; set; }

        public string Skills { get; set; }

        public string Recommendations { get; set; }

        public string License { get; set; }
             
        public DateTime ExpiryDate { get; set; }
               
        public string ProfilePhoto { get; set; }
               
        public string CompanyLogo { get; set; }
              
        public string IndustryName { get; set; }

        public string SubIndustryName { get; set; }
    }

    public class BrokerProfileEduction
    {
        [Display(Name = "Education")]
        public string School { get; set; }

        public string School1 { get; set; }
        public string Degree1 { get; set; }
        public string Year1 { get; set; }
        public string EducationLogo1 { get; set; }

        public string School2 { get; set; }
        public string Degree2 { get; set; }
        public string Year2 { get; set; }
        public string EducationLogo2 { get; set; }

        public string School3 { get; set; }
        public string Degree3 { get; set; }
        public string Year3 { get; set; }
        public string EducationLogo3 { get; set; }

        public string School4 { get; set; }
        public string Degree4 { get; set; }
        public string Year4 { get; set; }
        public string EducationLogo4 { get; set; }

        public string School5 { get; set; }
        public string Degree5 { get; set; }
        public string Year5 { get; set; }
        public string EducationLogo5 { get; set; }
    }

    public class BrokerProfileIndustry//Change 17Jan17
    {
        public string Industry1 { get; set; }
        public string Industry2 { get; set; }
        public string Industry3 { get; set; }

        public string SubIndustry1 { get; set; }
        public string SubIndustry2 { get; set; }
        public string SubIndustry3 { get; set; }


        //public IEnumerable<SelectListItem> SubIndustry1 { get; set; }
        //public IEnumerable<SelectListItem> SubIndustry2 { get; set; }
        //public IEnumerable<SelectListItem> SubIndustry3 { get; set; }

    }

    public class BrokerProfileCompanies
    {
        [Display(Name = "Company You Represent")]
        public IEnumerable<SelectListItem> CompanyName { get; set; }
    }

    public class BrokerProfileLanguages //Change 29Dec16
    {
        public string Language { get; set; }
    }

    public class AllProfileModels
    {
        public List<BrokerProfile> BrokerInfo { get; set; }
        public List<BrokerProfileEduction> BrokerEduction { get; set; }
        public List<BrokerProfileCompanies> BrokerCompanies { get; set; }
        public List<BrokerProfileLanguages> BrokerLanguages { get; set; }
        public List<BrokerProfileIndustry> BrokerIndustry { get; set; }
    }
}