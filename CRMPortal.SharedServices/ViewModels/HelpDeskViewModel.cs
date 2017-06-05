using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMPortal.SharedServices.Models;

namespace CRMPortal.SharedServices.ViewModels
{
    public class HelpDeskViewModel
    {
        public List<HelpDeskModel> Requests { get; set; }
    }
}