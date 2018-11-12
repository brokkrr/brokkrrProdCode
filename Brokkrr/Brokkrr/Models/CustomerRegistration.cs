using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class CustomerRegistration
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "First Name field is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name field is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone field is required")]
        //[RegularExpression(@"^[0-9,+]{5}$", ErrorMessage = "Enter valid Phone Number.")]
        [Display(Name = "Phone")]
        public string PhoneNo1 { get; set; }

        //Change 29Dec16
        //[Required(ErrorMessage = "Phone field is required")]
        //[Display(Name = "Phone")]
        //[RegularExpression(@"^[0-9]{3}$", ErrorMessage = "Enter valid Phone Number.")]
        //public string PhoneNo2 { get; set; }

        //[Required(ErrorMessage = "Phone field is required")]
        //[Display(Name = "Phone")]
        //[RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Enter valid Phone Number.")]
        //public string PhoneNo3 { get; set; }

        //Change 29Dec16

        //[Required(ErrorMessage = "Email field is required.")]
        //[RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Enter valid Email.")]
        //[System.Web.Mvc.Remote("IsEmailIdAlreadyExist", "CustomerRegistration", HttpMethod = "POST", ErrorMessage = "EmailId already exists. Please enter a different EmailId.")]
        public string Email { get; set; }


        public string Address { get; set; }

        [Required  (ErrorMessage = "Enter valid ZipCode.")]
        [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "Enter valid ZipCode.")]
        public string ZipCode { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Number of cars")]
        public string NumberofCars { get; set; }

        [Display(Name = "Photo")]
        public string ProfilePhoto { get; set; }

        public string IsProfilePhotoChanged { get; set; }

        //[Required(ErrorMessage = "Password field is required.")]
        //[DataType(DataType.Password)]
        //[DisplayName("Password")]
        //[StringLength(15, MinimumLength = 8, ErrorMessage = "Password must be between 8 to 15 characters.")]
        //public string Password { get; set; }

        //[Required(ErrorMessage = "Password and Confirm Password do not match.")]
        //[DataType(DataType.Password)]
        //[Compare("Password")]
        //[DisplayName("Confirm Password")]
        //public string ConfirmPassword { get; set; }

        [DisplayName("House Type")]
        public string HouseType { get; set; }

        [DisplayName("Do you have cars")]
        public string IsCars { get; set; }

        public string Occupation { get; set; }
    }
}