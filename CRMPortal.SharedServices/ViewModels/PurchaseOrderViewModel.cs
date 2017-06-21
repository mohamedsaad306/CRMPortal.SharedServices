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
        [Required(ErrorMessage = "Enter Request Title")]
        public string RequestTitle { get; set; }

        [Display(Name = "SaveOrDraft")]
        public string SaveOrDraft { get; set; }
        [Display(Name = "# Items")]
        [ValidateScheme_NumberOfItems(ErrorMessage = "Enter Number Between 1 and 50")]
        public string NumberOfitems { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Display(Name = "Item Name")]
        [Required(ErrorMessage = "Enter Item Name")]
        public string ItemName { get; set; }
        [Display(Name = "Status")]
        public string StatusReason { get; set; }
        [Display(Name = "Purpose")]
        public string Purpose { get; set; }
        [Display(Name = "PK")]
        [Key]
        public Guid PK { get; set; }

    }

    public class ValidateScheme_NumberOfItems : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return (Convert.ToInt32(value) > 0 && Convert.ToInt32(value) < 50);
        }
    }

}