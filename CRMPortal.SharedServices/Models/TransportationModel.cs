using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.Models
{
    public class TransportationModel:Repository
    {
        public TransportationModel (OrganizationServiceContext ctx, IOrganizationService orgService)
            :base(ctx,orgService)
        {
        }
         public IEnumerable GetRequests(Guid userId){

             return Context.CreateQuery("new_transportationrequest")
                 .Where(r => r["new_relatedemployeeid"] == new EntityReference(SystemUser.EntityLogicalName, userId)).ToList();
         }

         internal void Add(new_transportationrequest r)
         {
             Context.AddObject(r);
             Context.SaveChanges();
             
             //using (crmcontgenerated ctx = new crmcontgenerated(OrgService))
             //{
             //    ctx.AddObject(r);
             //    ctx.SaveChanges();
             //}
         }
    }
}