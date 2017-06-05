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
        public Guid? UserId
        {
            get
            {
                try
                {
                    return Context.CreateQuery("systemuser").Where(u => u["internalemailaddress"] == Email).FirstOrDefault().Id;
                }
                catch
                {
                    return null;
                }
            }
        }
        public Entity Entity
        {
            get
            {
                try
                {
                    return Context.CreateQuery("systemuser").Where(u => u["internalemailaddress"] == Email).FirstOrDefault();
                }
                catch
                {
                    return null;
                }
            }
        }
        public string UserLastName
        {
            get
            {
                try
                {
                    return Entity["lastname"].ToString();
                }
                catch
                {
                    return null;
                }
            }
        }


    }
}