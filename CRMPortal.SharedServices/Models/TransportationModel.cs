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
    public class TransportationModel : Repository
    {
        public TransportationModel(OrganizationServiceContext ctx, IOrganizationService orgService)
            : base(ctx, orgService)
        {
        }
        public IEnumerable GetRequests(Guid userId)
        {

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

        internal List<Transportation> GetAllAsTransportaion(Guid User_id)
        {
            List<Entity> requests = (List<Entity>)GetRequests(User_id);
            List<Transportation> viewRequests = new List<Transportation>();
            foreach (var r in requests)
            {
                try
                {
                    Transportation req = new Transportation
                    {
                        Id = r.Id,
                        CreatedAt = r.Attributes.Keys.Contains("createdon") ? DateTime.Parse(r["createdon"].ToString()) : new DateTime(),
                        RequestTitle = r.Attributes.Keys.Contains("new_name") ? r["new_name"].ToString() : "",
                        RequestNumber = r.Attributes.Keys.Contains("new_requestnumber") ? r["new_requestnumber"].ToString() : "",
                        RequestDetails = r.Attributes.Keys.Contains("new_requestdetails") ? r["new_requestdetails"].ToString() : "",
                        StatusReason = r.Attributes.Keys.Contains("statuscode") ? r.FormattedValues["statuscode"].ToString() : "",

                        TransportationDate = r.Attributes.Keys.Contains("new_date") ? DateTime.Parse(r["new_date"].ToString()) : new DateTime(),
                        Address = r.Attributes.Keys.Contains("new_address") ? r["new_address"].ToString() : "",
                        CustomerType= r.Attributes.Contains("new_clientname") ? ((EntityReference)r["new_clientname"]).LogicalName:"",
                        RelatedClient = r.Attributes.Contains("new_clientname") ? ((EntityReference)r["new_clientname"]).Id : Guid.Empty,
                        RelatedClientName = r.Attributes.Keys.Contains("new_clientname") ? r.FormattedValues["new_clientname"].ToString() : "",
                        RequestOwner = r.FormattedValues["ownerid"]

                    };


                    if (r.FormattedValues["statuscode"] == "Draft" || r.FormattedValues["statuscode"] == "New")
                    {
                        req.isReadOnly = false;
                        req.AvailableActions.Clear();
                        req.AvailableActions.Add("cancel");
                        req.AvailableActions.Add("save");
                        req.AvailableActions.Add("submit");
                        req.AvailableActions.Add("update");

                    }
                    else if (r.FormattedValues["statuscode"] == "Approved")
                    {
                        req.isReadOnly = true;
                        req.AvailableActions.Clear();
                        req.AvailableActions.Add("confirm");
                    }
                    else if (r.FormattedValues["statuscode"] == "Rejected")
                    {
                        req.isReadOnly = false;
                        req.AvailableActions.Clear();
                        req.AvailableActions.Add("resubmit");
                        req.AvailableActions.Add("cancel");
                    }
                    else if (r.FormattedValues["statuscode"] == "Closed")
                    {
                        req.isReadOnly = true;
                        req.AvailableActions.Clear();
                    }
                    else
                    {
                        req.isReadOnly = true;
                        req.AvailableActions.Clear();
                        req.AvailableActions.Add("cancel");

                    }

                    viewRequests.Add(req);
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

        internal Transportation GetById(Guid? req_id, Guid user_id)
        {

            return GetAllAsTransportaion(user_id).Where(r => r.Id == req_id).FirstOrDefault();
        }

        internal void UpdateRequest(Transportation transportationRequest, string p, OptionSetValue CRMActionValue)
        {
            using (crmcontgenerated ctx = new crmcontgenerated(OrgService))
            {
                // get obj to update 
                new_helpdeskrequest requestToUpdate = ctx.new_helpdeskrequestSet.Where(r => r.Id == transportationRequest.Id).FirstOrDefault();
                new_transportationrequest reqToUpdate = ctx.new_transportationrequestSet.Where(r => r.Id == transportationRequest.Id).FirstOrDefault();

                // apply changes  assumming objecct is editable 

                reqToUpdate.new_Address = transportationRequest.Address;
                reqToUpdate.new_name = transportationRequest.RequestTitle;
                reqToUpdate.new_Date = transportationRequest.TransportationDate;
                reqToUpdate.new_ClientName = new EntityReference(transportationRequest.CustomerType, transportationRequest.RelatedClient);

                // pass the crm action 
                reqToUpdate.new_Action = CRMActionValue;

                // save operation 
                ctx.UpdateObject(reqToUpdate);
                ctx.SaveChanges();
            }
        }
    }
}