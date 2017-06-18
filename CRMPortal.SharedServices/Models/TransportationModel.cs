using CRMPortal.SharedServices.DomainModels;
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

         internal List<SharedServices.DomainModels.Transportation> GetAllAsTransportaion(Guid User_id)
         {
             List<Entity> requests = (List<Entity>)GetRequests(User_id);
             List<Transportation> viewRequests = new List<Transportation>();
             foreach (var r in requests)
             {
                 try
                 {
                     viewRequests.Add(new Transportation
                     {
                         CreatedAt = r.Attributes.Keys.Contains("createdon") ? DateTime.Parse(r["createdon"].ToString()) : new DateTime(),
                         RequestTitle = r.Attributes.Keys.Contains("new_name") ? r["new_name"].ToString() : "",
                         RequestNumber = r.Attributes.Keys.Contains("new_requestnumber") ? r["new_requestnumber"].ToString() : "",

                         StatusReason = r.Attributes.Keys.Contains("statuscode") ? r.FormattedValues["statuscode"].ToString() : "",

                         TransportationDate = r.Attributes.Keys.Contains("new_date") ? DateTime.Parse(r["new_date"].ToString()) : new DateTime(),
                         Address = r.Attributes.Keys.Contains("new_address") ? r["new_address"].ToString() : "",
                         RelatedClient = r.Attributes.Keys.Contains("new_clientname") ? r.FormattedValues["new_clientname"].ToString() : "",
                         RequestType = "Transportation"

                     });
                     //new_transportationrequest d = new new_transportationrequest{}
                 }
                 catch (KeyNotFoundException)
                 {
                     //TempData["info"] = "Some data didn't Load Correctly plase try again later .. ";
                     continue;
                     //throw;
                 }
             }
             return viewRequests;
         }
    }
}