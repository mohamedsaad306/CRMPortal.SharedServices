using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMPortal.SharedServices.Models;
using CRMPortal.SharedServices.Models.DomainModels;

namespace CRMPortal.SharedServices.ViewModels
{
    public class HelpDeskViewModel
    {
        public List<HelpDeskRequest> Requests { get; set; }
    }
}