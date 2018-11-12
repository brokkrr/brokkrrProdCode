using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class ResetPassword
    {
        [Required(ErrorMessage = "New Password is required.")]
        [DataType(DataType.Password)]
        [DisplayName("New Password")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Password must be between 8 to 15 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password and Confirm Password do not match.")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}