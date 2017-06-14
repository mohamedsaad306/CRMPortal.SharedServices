using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMPortal.SharedServices.Models;
using CRMPortal.SharedServices.Models.DomainModels;
using System.ComponentModel.DataAnnotations;

namespace CRMPortal.SharedServices.ViewModels
{
    public class HelpDeskIndexViewModel
    {
        public List<HelpDeskRequest> Requests { get; set; }
        
    }
    public class HelpDeskFormViewModel
    {
        [Display(Name="Title")]
        public string RequestTitle { get; set; }
        
        [Display(Name = "Details")]
        public string RequestDetails { get; set; }
        
    }


}
