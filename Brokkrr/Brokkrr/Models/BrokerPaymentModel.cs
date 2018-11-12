using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class BrokerPaymentModel
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public string PaymentId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public Nullable<bool> IsPaid { get; set; }
        public string PaymentMode { get; set; }
    }
}