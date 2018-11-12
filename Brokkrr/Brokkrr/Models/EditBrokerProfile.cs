using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class EditBrokerProfile
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "First Name field is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name field is required")]
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
        //[System.Web.Mvc.Remote("IsEmailIdAlreadyExist", "BrokerRegistration", HttpMethod = "POST", ErrorMessage = "EmailId already exists. Please enter a different EmailId.")]
        public string Email { get; set; }


        public string Address { get; set; }

         [Display(Name = "City")]
         [Required(ErrorMessage = "Area field is required")]
        public string Area { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "Enter valid ZipCode.")]
        public string ZipCode1 { get; set; }
        public string ZipCode2 { get; set; }
        public string ZipCode3 { get; set; }
        public string ZipCode4 { get; set; }
        public string ZipCode5 { get; set; }


        [Display(Name = "Company Name")]
        public string Company { get; set; }

        public string Bio { get; set; }

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

        [Display(Name = "Company Logo")]
        public string CompanyLogo { get; set; }

        //[Required(ErrorMessage = "Password field is required.")]
        //[DataType(DataType.Password)]
        //[DisplayName("Password")]
        //[StringLength(15, MinimumLength = 8, ErrorMessage = "Password must be between 8 to 15 characters.")]
        //public string Password { get; set; }

        //[Required(ErrorMessage = "Password and Confirm Password do not match.")]
        //[DataType(DataType.Password)]
        //[Compare("Password")]//Change 29Dec16

        //[DisplayName("Confirm Password")]
        //public string ConfirmPassword { get; set; }

        [Display(Name = "Industry & NAICS Code")]
        public string IndustryName { get; set; }

        [Display(Name = "SubIndustry Name")]
        public string SubIndustryName { get; set; }

        /************ For Education **********/

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


        public string HiddenSchool1 { get; set; }
        public string HiddenDegree1 { get; set; }
        public string HiddenYear1 { get; set; }
        public string HiddenEducationLogo1 { get; set; }
        public string HiddenEducationLogoPath1 { get; set; }
        public string HiddenEducationLogoIsChanged1 { get; set; }

        public string HiddenSchool2 { get; set; }
        public string HiddenDegree2 { get; set; }
        public string HiddenYear2 { get; set; }
        public string HiddenEducationLogo2 { get; set; }
        public string HiddenEducationLogoPath2 { get; set; }
        public string HiddenEducationLogoIsChanged2 { get; set; }

        public string HiddenSchool3 { get; set; }
        public string HiddenDegree3 { get; set; }
        public string HiddenYear3 { get; set; }
        public string HiddenEducationLogo3 { get; set; }
        public string HiddenEducationLogoPath3 { get; set; }
        public string HiddenEducationLogoIsChanged3 { get; set; }

        public string HiddenSchool4 { get; set; }
        public string HiddenDegree4 { get; set; }
        public string HiddenYear4 { get; set; }
        public string HiddenEducationLogo4 { get; set; }
        public string HiddenEducationLogoPath4 { get; set; }
        public string HiddenEducationLogoIsChanged4 { get; set; }

        public string HiddenSchool5 { get; set; }
        public string HiddenDegree5 { get; set; }
        public string HiddenYear5 { get; set; }
        public string HiddenEducationLogo5 { get; set; }
        public string HiddenEducationLogoPath5 { get; set; }
        public string HiddenEducationLogoIsChanged5 { get; set; }

        [Display(Name = "Languages")]
        public string Languages { get; set; }


        [Display(Name = "Prior Employment")]
        public string CompanyNameRpt { get; set; }

        public string HiddenCompanyNameRpt { get; set; }

        public string HiddenProfilePhoto { get; set; }
        public string HiddenCompanyLogo { get; set; }

        public string IsVisibleIndustry1 { get; set; }
        public string IsVisibleIndustry2 { get; set; }
        public string IsVisibleIndustry3 { get; set; }

        public string IsDeletedIndustry1 { get; set; }
        public string IsDeletedIndustry2 { get; set; }
        public string IsDeletedIndustry3 { get; set; }


        public string HiddenZipCode1 { get; set; }
        public string HiddenZipCode2 { get; set; }
        public string HiddenZipCode3 { get; set; }
        public string HiddenZipCode4 { get; set; }
        public string HiddenZipCode5 { get; set; }

        //02May18

        public string ExpCompanyName1 { get; set; }
        public string ExpLogo1 { get; set; }
        public string ExpTitle1 { get; set; }
        public string ExpDurationFrom1 { get; set; }
        public string ExpDurationTo1 { get; set; }

        public string ExpCompanyName2 { get; set; }
        public string ExpLogo2 { get; set; }
        public string ExpTitle2 { get; set; }
        public string ExpDurationFrom2 { get; set; }
        public string ExpDurationTo2 { get; set; }

        public string ExpCompanyName3 { get; set; }
        public string ExpLogo3 { get; set; }
        public string ExpTitle3 { get; set; }
        public string ExpDurationFrom3 { get; set; }
        public string ExpDurationTo3 { get; set; }

        public string ExpCompanyName4 { get; set; }
        public string ExpLogo4 { get; set; }
        public string ExpTitle4 { get; set; }
        public string ExpDurationFrom4 { get; set; }
        public string ExpDurationTo4 { get; set; }

        public string ExpCompanyName5 { get; set; }
        public string ExpLogo5 { get; set; }
        public string ExpTitle5 { get; set; }
        public string ExpDurationFrom5 { get; set; }
        public string ExpDurationTo5 { get; set; }

        public string ExpCompanyName6 { get; set; }
        public string ExpLogo6 { get; set; }
        public string ExpTitle6 { get; set; }
        public string ExpDurationFrom6 { get; set; }
        public string ExpDurationTo6 { get; set; }

        public string ExpCompanyName7 { get; set; }
        public string ExpLogo7 { get; set; }
        public string ExpTitle7 { get; set; }
        public string ExpDurationFrom7 { get; set; }
        public string ExpDurationTo7 { get; set; }

        public string HiddenExpCompanyName1 { get; set; }
        public string HiddenExpLogo1 { get; set; }
        public string HiddenExpTitle1 { get; set; }
        public string HiddenExpDurationFrom1 { get; set; }
        public string HiddenExpDurationTo1 { get; set; }
        public string HiddenExpLogoPath1 { get; set; }
        public string HiddenExpLogoIsChanged1 { get; set; }

        public string HiddenExpCompanyName2 { get; set; }
        public string HiddenExpLogo2 { get; set; }
        public string HiddenExpTitle2 { get; set; }
        public string HiddenExpDurationFrom2 { get; set; }
        public string HiddenExpDurationTo2 { get; set; }
        public string HiddenExpLogoPath2 { get; set; }
        public string HiddenExpLogoIsChanged2 { get; set; }

        public string HiddenExpCompanyName3 { get; set; }
        public string HiddenExpLogo3 { get; set; }
        public string HiddenExpTitle3 { get; set; }
        public string HiddenExpDurationFrom3 { get; set; }
        public string HiddenExpDurationTo3 { get; set; }
        public string HiddenExpLogoPath3 { get; set; }
        public string HiddenExpLogoIsChanged3 { get; set; }

        public string HiddenExpCompanyName4 { get; set; }
        public string HiddenExpLogo4 { get; set; }
        public string HiddenExpTitle4 { get; set; }
        public string HiddenExpDurationFrom4 { get; set; }
        public string HiddenExpDurationTo4 { get; set; }
        public string HiddenExpLogoPath4 { get; set; }
        public string HiddenExpLogoIsChanged4 { get; set; }

        public string HiddenExpCompanyName5 { get; set; }
        public string HiddenExpLogo5 { get; set; }
        public string HiddenExpTitle5 { get; set; }
        public string HiddenExpDurationFrom5 { get; set; }
        public string HiddenExpDurationTo5 { get; set; }
        public string HiddenExpLogoPath5 { get; set; }
        public string HiddenExpLogoIsChanged5 { get; set; }

        public string HiddenExpCompanyName6 { get; set; }
        public string HiddenExpLogo6 { get; set; }
        public string HiddenExpTitle6 { get; set; }
        public string HiddenExpDurationFrom6 { get; set; }
        public string HiddenExpDurationTo6 { get; set; }
        public string HiddenExpLogoPath6 { get; set; }
        public string HiddenExpLogoIsChanged6 { get; set; }


        public string HiddenExpCompanyName7 { get; set; }
        public string HiddenExpLogo7 { get; set; }
        public string HiddenExpTitle7 { get; set; }
        public string HiddenExpDurationFrom7 { get; set; }
        public string HiddenExpDurationTo7 { get; set; }
        public string HiddenExpLogoPath7 { get; set; }
        public string HiddenExpLogoIsChanged7 { get; set; }

    }
}