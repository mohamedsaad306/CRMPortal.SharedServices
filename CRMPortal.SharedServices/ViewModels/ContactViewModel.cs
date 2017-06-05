using CRMPortal.SharedServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.ViewModels
{
    public class ContactViewModel
    {
        public ContactModel ContactModel { get; set; }
        public string ContactLastName { get; set; }
    }
}