using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class Admin
    {
        //[Required]
        public string Username { get; set; }

        //[Required]
        public string Password { get; set; }

        public int UserId { get; set; }
    }
}