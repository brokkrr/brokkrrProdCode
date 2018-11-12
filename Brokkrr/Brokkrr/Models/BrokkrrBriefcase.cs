using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class BrokkrrBriefcase
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "Url field is required.")]
        public string Url { get; set; }
        //[Required(ErrorMessage = "Title field is required.")]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public int UploadedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        //[Required(ErrorMessage = "Assigned Company field is required.")]
        public string AssignedCompany { get; set; }
        public string Actionsname { get; set; }
        public string IsExpired { get; set; }
    }
    public class Video
    {
        public string VideoId { get; set; }
    }
}