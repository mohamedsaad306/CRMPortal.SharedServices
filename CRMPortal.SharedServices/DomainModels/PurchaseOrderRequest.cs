using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.Models.DomainModels
{
    public class PurchaseOrderRequest:Request
    {
        public string ItemName { get; set; }
        public string NumberOfitems { get; set; }
        public string Purpose { get; set; }
        public Guid PK { get; set; }
        public string GetButtonValue
        {
            get
            {
                return RequestNumber != "" ? "View Details" : "Edit Request";
            }
        }
        public string GetButtonClass
        {
            get
            {
                return RequestNumber != "" ? "default" : "info";
            }
        }
    }
}