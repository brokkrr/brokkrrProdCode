using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
         [Required]
        public string FirstName { get; set; }
         [Required]
        public string LastName { get; set; }
         [Required]
         [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Please enter correct email format")]
        public string EmailId { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least 6 to 20 characters long.", MinimumLength = 6)]
        public string Password { get; set; }
         public string Address { get; set; }
        public string City { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<int> CountryId { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9-, ()]{1,10}$", ErrorMessage = "Zip number allow only Number,Text,Comma Maximum 10.")]
        public string PinCode { get; set; }
         [RegularExpression(@"^[a-zA-Z0-9-+,. ()]{1,25}$", ErrorMessage = "Other Phone number allow only Text, Number, Dash,Pluse,Comma,Dot,Round bracket and Maximum length is 25.")]
        public string PhoneNo { get; set; }
         [RegularExpression(@"^[a-zA-Z0-9-+,. ()]{1,25}$", ErrorMessage = "Other Phone number allow only Text, Number, Dash,Pluse,Comma,Dot,Round bracket and Maximum length is 25.")]
       public string MobNo { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string ProfilePicture { get; set; }
        public Nullable<bool> IsActive { get; set; }
        [Required]
        public string UserType { get; set; }
        public Nullable<System.DateTime> RegisteredDate { get; set; }
        public string RegisterdType { get; set; }
        public Nullable<bool> IsUpdateProfile { get; set; }
    }
}