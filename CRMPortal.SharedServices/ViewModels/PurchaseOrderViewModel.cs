using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMPortal.SharedServices.Models;
using CRMPortal.SharedServices.Models.DomainModels;

namespace CRMPortal.SharedServices.ViewModels
{
    public class PurchaseOrderViewModel
    {
        public List<PurchaseOrderRequest> Requests { get; set; }
    }
}