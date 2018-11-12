using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BrokerMVC.Models
{
    public class UpdatePaymentModel
    {
        //public int Id { get; set; }
        public int Amount { get; set; }
        public DateTime DateOfEffect { get; set; }
    }
    public class UpdatePaymentContext : DbContext
    {
        public DbSet<UpdatePaymentModel> Payment { get; set; }

    }
}