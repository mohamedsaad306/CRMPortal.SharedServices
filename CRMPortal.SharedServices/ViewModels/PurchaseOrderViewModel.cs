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

        [Display(Name = "Details")]
        public string RequestDetails { get; set; }
        [Display(Name = "# Items")]
        public string NumberOfitems { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Status Reason")]
        public string StatusReason { get; set; }

    }

}