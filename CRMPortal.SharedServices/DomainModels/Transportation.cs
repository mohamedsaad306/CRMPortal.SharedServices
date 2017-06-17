using CRMPortal.SharedServices.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.DomainModels
{
    public class Transportation:Request
    {
        public DateTime TransportationDate { get; set; }
        public string RelatedClient { get; set; }
        public string Address { get; set; }
    }
}