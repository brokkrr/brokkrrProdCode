using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class ContactUs
    {
        [Required(ErrorMessage = "Name field is required.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email field is required.")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Enter valid Email.")]
        //[System.Web.Mvc.Remote("IsEmailIdAlreadyExist", "CustomerRegistration", HttpMethod = "POST", ErrorMessage = "EmailId already exists. Please enter a different EmailId.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Subject field is required.")]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message field is required.")]
        [Display(Name = "Message")]
        public string Message { get; set; }

        //[DefaultValue(false)]
        //public bool IsShowAlert { get; set; }
    }
}