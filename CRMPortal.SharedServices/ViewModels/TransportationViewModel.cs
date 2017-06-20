using CRMPortal.SharedServices.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.ViewModels
{
    public class TransportationIndexViewModel
    {
        public List<Transportation> requests { get; set; }
    }

    public class TransportationFormViewModel
    {
        public string title { get; set; }
        public DateTime Date { get; set; }
        public string Address { get; set; }
        public string Details { get; set; }

        public Guid Client { get; set; }
        public List<Customer> AvailableCustomers { get; set; }
        public string CustomerType { get; set; }

        public string Action { get; set; }


        public Transportation Request { get; set; }
    }
}