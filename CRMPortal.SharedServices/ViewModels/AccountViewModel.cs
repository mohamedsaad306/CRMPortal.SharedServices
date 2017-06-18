using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMPortal.SharedServices.Models;
using CRMPortal.SharedServices.Models.DomainModels;

namespace CRMPortal.SharedServices.ViewModels
{
    public class AccountViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public AccountModel Acc { get; set; }

    }
    public class AccountHomeViewModel
    {
        public List<Request> Requests { get; set; }
    }
}