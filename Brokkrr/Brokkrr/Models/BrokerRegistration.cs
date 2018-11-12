using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using System.Web.WebPages.Html;
//using System.Web.Mvc;
//using System.Web.Mvc;

namespace BrokerMVC.Models
{

    public class BrokerRegistration
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "First Name field is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        //[Required(ErrorMessage = "Last Name field is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone field is required")]

        //[RegularExpression(@"^[0-9,+]{5}$", ErrorMessage = "Enter valid Phone Number.")]//Change 29Dec16
        [Display(Name = "Phone")]
        public string PhoneNo1 { get; set; }

        //[Required(ErrorMessage = "Phone field is required")]//Change 29Dec16
        //[Display(Name = "Phone")]
        //[RegularExpression(@"^[0-9-, ()]{3}$", ErrorMessage = "Enter valid Phone Number.")]
        //public string PhoneNo2 { get; set; }//Change 29Dec16

        //[Required(ErrorMessage = "Phone field is required")]//Change 29Dec16
        //[Display(Name = "Phone")]
        //[RegularExpression(@"^[0-9-, ()]{4}$", ErrorMessage = "Enter valid Phone Number.")]
        //public string PhoneNo3 { get; set; }//Change 29Dec16

        [Required(ErrorMessage = "Email field is required.")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Enter valid Email.")]
        [System.Web.Mvc.Remote("IsEmailIdAlreadyExist", "BrokerRegistration", HttpMethod = "POST", ErrorMessage = "EmailId already exists. Please enter a different EmailId.")]
        public string Email { get; set; }


        public string Address { get; set; }

        [Required(ErrorMessage = "Area field is required.")]
        public string Area { get; set; }

        public string Bio { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "Enter valid ZipCode.")]
        public string ZipCode { get; set; }

        public string ZipCode2 { get; set; }
        public string ZipCode3 { get; set; }
        public string ZipCode4 { get; set; }
        public string ZipCode5 { get; set; }

        public string longitude { get; set; }
        public string latitude { get; set; }

        public string longitude2 { get; set; }
        public string latitude2 { get; set; }

        public string longitude3 { get; set; }
        public string latitude3 { get; set; }

        public string longitude4 { get; set; }
        public string latitude4 { get; set; }

        public string longitude5 { get; set; }
        public string latitude5 { get; set; }

        [Display(Name = "Company Name")]
        public string Company { get; set; }


        public string Title { get; set; }


        public string Resume { get; set; }


        public string Awards { get; set; }

        //[Required(ErrorMessage = "Language field is required.")]
        //public string Language { get; set; }


        public string Specialities { get; set; }

        public string HomeValue { get; set; }
        public string AutoType { get; set; }
        public string Revenue { get; set; }
        public string Employees { get; set; }
        public string CoverageAmt { get; set; }

        [Display(Name = "Skills/Endorsement")]
        public string Skills { get; set; }


        public string Recommendations { get; set; }


        public string License { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; }

        [Display(Name = "Profile Photo")]
        public string ProfilePhoto { get; set; }        
        public string HiddenProfilePhoto { get; set; }

        [Display(Name = "Company Logo")]
        public string CompanyLogo { get; set; }
        public string HiddenCompanyLogo { get; set; }

        [Required(ErrorMessage = "Password field is required.")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Password must be between 8 to 15 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password and Confirm Password do not match.")]
        [DataType(DataType.Password)]
        [Compare("Password")]//Change 29Dec16

        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Industry & SIC Code")]
        public string IndustryName { get; set; }

        [Display(Name = "SubIndustry Name")]
        public string SubIndustryName { get; set; }

    }

    public class BrokerEduction
    {
        [Display(Name = "Education")]
        public string School { get; set; }

        public string School1 { get; set; }
        public string Degree1 { get; set; }
        public string Year1 { get; set; }
        public string EducationLogo1 { get; set; }
        public string HiddenEducationLogo1 { get; set; }

        public string School2 { get; set; }
        public string Degree2 { get; set; }
        public string Year2 { get; set; }
        public string EducationLogo2 { get; set; }
        public string HiddenEducationLogo2 { get; set; }

        public string School3 { get; set; }
        public string Degree3 { get; set; }
        public string Year3 { get; set; }
        public string EducationLogo3 { get; set; }
        public string HiddenEducationLogo3 { get; set; }

        public string School4 { get; set; }
        public string Degree4 { get; set; }
        public string Year4 { get; set; }
        public string EducationLogo4 { get; set; }
        public string HiddenEducationLogo4 { get; set; }

        public string School5 { get; set; }
        public string Degree5 { get; set; }
        public string Year5 { get; set; }
        public string EducationLogo5 { get; set; }
        public string HiddenEducationLogo5 { get; set; }
    }

    public class BrokerPriorEmployment
    {
        [Display(Name = "Prior Employment")]
        public string CmpName { get; set; }

        public string CmpName1 { get; set; }
        public string Desig1 { get; set; }
        //public string DurFrom1 { get; set; }
        //public string DurTo1 { get; set; }
        public string CmpLogo1 { get; set; }
        public string HiddenCmpLogo1 { get; set; }
        public string DurMonthFrom1 { get; set; }
        public string DurYearFrom1 { get; set; }
        public string DurMonthTo1 { get; set; }
        public string DurYearTo1 { get; set; }

        public string CmpName2 { get; set; }
        public string Desig2 { get; set; }
        //public string DurFrom2 { get; set; }
        //public string DurTo2 { get; set; }
        public string CmpLogo2 { get; set; }
        public string HiddenCmpLogo2 { get; set; }
        public string DurMonthFrom2 { get; set; }
        public string DurYearFrom2 { get; set; }
        public string DurMonthTo2 { get; set; }
        public string DurYearTo2 { get; set; }

        public string CmpName3 { get; set; }
        public string Desig3 { get; set; }
        //public string DurFrom3 { get; set; }
        //public string DurTo3 { get; set; }
        public string CmpLogo3 { get; set; }
        public string HiddenCmpLogo3 { get; set; }
        public string DurMonthFrom3 { get; set; }
        public string DurYearFrom3 { get; set; }
        public string DurMonthTo3 { get; set; }
        public string DurYearTo3 { get; set; }

        public string CmpName4 { get; set; }
        public string Desig4 { get; set; }
        //public string DurFrom4 { get; set; }
        //public string DurTo4 { get; set; }
        public string CmpLogo4 { get; set; }
        public string HiddenCmpLogo4 { get; set; }
        public string DurMonthFrom4 { get; set; }
        public string DurYearFrom4 { get; set; }
        public string DurMonthTo4 { get; set; }
        public string DurYearTo4 { get; set; }

        public string CmpName5 { get; set; }
        public string Desig5 { get; set; }
        //public string DurFrom5 { get; set; }
        //public string DurTo5 { get; set; }
        public string CmpLogo5 { get; set; }
        public string HiddenCmpLogo5 { get; set; }
        public string DurMonthFrom5 { get; set; }
        public string DurYearFrom5 { get; set; }
        public string DurMonthTo5 { get; set; }
        public string DurYearTo5 { get; set; }

        public string CmpName6 { get; set; }
        public string Desig6 { get; set; }
        //public string DurFrom6 { get; set; }
        //public string DurTo6 { get; set; }
        public string CmpLogo6 { get; set; }
        public string HiddenCmpLogo6 { get; set; }
        public string DurMonthFrom6{ get; set; }
        public string DurYearFrom6 {get; set; }
        public string DurMonthTo6 { get; set; }
        public string DurYearTo6 { get; set; }

        public string CmpName7 { get; set; }
        public string Desig7 { get; set; }
        //public string DurFrom7 { get; set; }
        //public string DurTo7 { get; set; }
        public string CmpLogo7 { get; set; }
        public string HiddenCmpLogo7 { get; set; }
        public string DurMonthFrom7 { get; set; }
        public string DurYearFrom7 { get; set; }
        public string DurMonthTo7 { get; set; }
        public string DurYearTo7 { get; set; }
    }

    public class BrokerIndustry//Change 17Jan17
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

    public class BrokerCompanies
    {
        [Display(Name = "Company You Represent")]
        public IEnumerable<SelectListItem> CompanyName { get; set; }
    }

    public class BrokerLanguages //Change 29Dec16
    {
        public string Language { get; set; }
    }

    ////public class MultipleModels
    ////{
    ////    public PagedList.IPagedList<BrokerInfo> hotelClaimModel { get; set; }
    ////    public PagedList.IPagedList<BrokerEduction> nonHotelClaimModel { get; set; }
    ////    public PagedList.IPagedList<BrokerCompanies> pendingClaimModel { get; set; }
    ////}

    public class AllModels
    {
        public List<BrokerRegistration> BrokerInfo { get; set; }
        //public PagedList.IPagedList<BrokerRegistration> BrokerInfo { get; set; }
        public List<BrokerEduction> BrokerEduction { get; set; }
        public List<BrokerCompanies> BrokerCompanies { get; set; }
        public List<BrokerLanguages> BrokerLanguages { get; set; }
        public List<BrokerIndustry> BrokerIndustry { get; set; }
        public List<BrokerPriorEmployment> BrokerPriorEmployment { get; set; }

    }
}