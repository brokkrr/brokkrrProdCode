using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class InsuranceModel
    {
        public string userId { get; set; }
        public string newPurchaseOrCurrentlyInsured { get; set; }
        public string insuranceCompany { get; set; }
        public string acquisitionIsInsured { get; set; }

        public string whenNeedInsurence { get; set; }
        public string noOfEmployee { get; set; }
        public string noOfEmployeeMeinekeBenefits { get; set; }
        public string industry { get; set; }
        public string siccode { get; set; }

        public string coverageamount { get; set; }
        public string grossrevenue { get; set; }
        public string typeOfAuto { get; set; }
        public string valueOfHome { get; set; }
        public string language { get; set; }
        public string notes { get; set; }
        public string city { get; set; }
        public string zipCode { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }

        public string NoOfUnits { get; set; }
        public string DeductibleIfAny { get; set; }
        public string CurrentLimit { get; set; }
        public string GrossSale { get; set; }
        public string GrossPayroll { get; set; }
        public string LineType { get; set; }


        public string insurancetype { get; set; }
        [Required]
        public string firstName { get; set; }

        public string lastName { get; set; }
        [Required]
        public string phone { get; set; }
        [Required]
        public string email { get; set; }

        public string NoOfStalls { get; set; }
        public string NoOfLocations { get; set; }
        public string grossrevenueforgarage { get; set; }

        public string CurrentPlan { get; set; }
        public string planSize { get; set; }
        public string NoOfEmp { get; set; }

        //11May18
        public string declaration { get; set; }
        public string uploaddeclaration { get; set; }
        public string Hiddenuploaddeclaration { get; set; }

    }
}