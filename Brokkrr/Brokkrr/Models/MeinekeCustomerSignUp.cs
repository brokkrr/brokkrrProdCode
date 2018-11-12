using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class MeinekeCustomerSignUp
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "First name field is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

       [Required(ErrorMessage = "Last name field is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone field is required")]
        //[RegularExpression(@"^[0-9,+]{5}$", ErrorMessage = "Enter valid Phone Number.")]
        [Display(Name = "Phone")]
        public string PhoneNo { get; set; }

     
        [Required(ErrorMessage = "Email field is required.")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Enter valid Email.")]
        [System.Web.Mvc.Remote("IsEmailIdAlreadyExist", "CustomerRegistration", HttpMethod = "POST", ErrorMessage = "EmailId already exists. Please enter a different EmailId.")]
        [Display(Name = "Email Id")]
        public string EmailId { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "Enter valid ZipCode.")]
        [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "Enter valid ZipCode.")]
        public string ZipCode { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Photo")]
        public string ProfilePicture { get; set; }

        //[RegularExpression(@"^(http\:\/\/|https\:\/\/)?([a-z0-9][a-z0-9\-]*\.)+[a-z0-9][a-z0-9\-]*$@i")]
        public string Website { get; set; }

        public string EstPremium { get; set; }

        public string NoofEmployees { get; set; }


        public string IsCustomerProfileChanged { get; set; }

    }
}