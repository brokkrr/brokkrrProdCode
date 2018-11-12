using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrokerMVC.Models
{
    public class LanguagesViewModel
    {
        public int[] LanguageId { get; set; }
        public MultiSelectList Language { get; set; }
    }
}