using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Xrm.Sdk;
namespace CRMPortal.SharedServices.Models
{
    public class AccountModel : Repository
    {

        public AccountModel(OrganizationServiceContext ctx)
            : base(ctx)
        {
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public Guid UserId
        {
            get
            {
                return Context.CreateQuery("systemuser").Where(u => u["internalemailaddress"] == Email).FirstOrDefault().Id;
            }
        }
        public Entity Entity
        {
            get
            {
                return Context.CreateQuery("systemuser").Where(u => u["internalemailaddress"] == Email).FirstOrDefault();
            }
        }
        public string UserLastName { 
            get{
                return Entity["lastname"].ToString();
            }
        }


    }
}