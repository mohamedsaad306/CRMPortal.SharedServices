using CRMPortal.SharedServices.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.DomainModels
{
    public class Transportation:Request
    {
        public Transportation()
        {
            RequestType = "Transportation";
            AvailableActions = new List<string>();
        }
        public DateTime TransportationDate { get; set; }
        public Guid RelatedClient { get; set; }
        public string Address { get; set; }

        public string CustomerType { get; set; }

        public string RelatedClientName { get; set; }
    }
}