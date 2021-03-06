﻿using System;
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
        public bool isReadOnly { get; set; }

        public List<new_helpdeskcategory> Categories { get; set; }
        public List<new_helpdeskrequestsubcategory> SubCategories { get; set; }
        public Guid Category { get; set; }
        public Guid SubCategory { get; set; }

        public HelpDeskRequest HelpDeskRequest { get; set; }
        public string Action { get; set; }

    }


}
