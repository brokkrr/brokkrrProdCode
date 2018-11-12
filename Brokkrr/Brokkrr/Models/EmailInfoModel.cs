using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class EmailInfoModel
    {

        public string FromEmailid { get; set; }
        public string ToEmailid { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string MailType { get; set; }
        public string UserId { get; set; }

    }
}