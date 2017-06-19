using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMPortal.SharedServices.Models;
using CRMPortal.SharedServices.Models.DomainModels;
using System.ComponentModel.DataAnnotations;

namespace CRMPortal.SharedServices.ViewModels
{
    public class PurchaseOrderViewModel
    {
        public List<PurchaseOrderRequest> Requests { get; set; }
    }

    public class PurchaseOrderFormViewModel
    {
        [Display(Name = "Title")]
        public string RequestTitle { get; set; }

        [Display(Name = "SaveOrDraft")]
        public string SaveOrDraft { get; set; }
        [Display(Name = "# Items")]
        public string NumberOfitems { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }
        [Display(Name = "Status")]
        public string StatusReason { get; set; }
        [Display(Name = "Purpose")]
        public string Purpose { get; set; }
        [Display(Name = "PK")]
        public Guid PK { get; set; }

    }

}