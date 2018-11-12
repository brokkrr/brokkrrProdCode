using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class UserList
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId{get;set;}
        public string UserType{get;set;}
        public bool IsActive { get; set; }
        public string ProfilePictureImg { get; set; }



    }
}