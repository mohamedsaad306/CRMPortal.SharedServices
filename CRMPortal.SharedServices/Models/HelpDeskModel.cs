using CRMPortal.SharedServices.Models.DomainModels;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.Models
{
    public class HelpDeskModel : Repository
    {
        public HelpDeskModel(OrganizationServiceContext ctx, IOrganizationService orgService)
            : base(ctx, orgService)
        {
        }
        public List<Entity> GetAllRequests(Guid usr_id)
        {

 
            return Context.CreateQuery("new_helpdeskrequest").Where(r => r["new_relatedemployeeid"] == new EntityReference("systemuser", usr_id)).ToList();

        }
        internal void SubmitRequest(Entity req)
        {
            Context.AddObject(req);
            Context.SaveChanges();
        }
        public HelpDeskRequest GetById(Guid user_id, Guid? req_Id)
        {
            HelpDeskRequest req = GetAllAsHelpDeskRequests(user_id).Where(r => r.Id == req_Id).FirstOrDefault();
            return req;
        }
        public List<HelpDeskRequest> GetAllAsHelpDeskRequests(Guid usr_id)
        {
            List<Entity> requests = GetAllRequests(usr_id);

            List<HelpDeskRequest> viewRequests = new List<HelpDeskRequest>();
            foreach (var r in requests)
            {

                try
                {
                    HelpDeskRequest req = new HelpDeskRequest
                    {
                        Id = r.Id,
                        CreatedAt = DateTime.Parse(r["createdon"].ToString()),
                        RequestTitle = r["new_name"].ToString(),
                        RequestNumber = r.Attributes.Keys.Contains("new_requestnumber") ? r["new_requestnumber"].ToString() : "",
                        RequestDetails = r.Attributes.Keys.Contains("new_requestdetails") ? r["new_requestdetails"].ToString() : "",
                        StatusReason = r.FormattedValues["statuscode"].ToString(),
                        Category = r.Attributes.Keys.Contains("new_helpdeskcategoryid") ? ((EntityReference)r["new_helpdeskcategoryid"]).Id : Guid.Empty,
                        SubCategory = r.Attributes.Keys.Contains("new_subcategory") ? ((EntityReference)r["new_subcategory"]).Id : Guid.Empty,
                        RequestOwner = r.FormattedValues["ownerid"]

                    };

                    if (r.FormattedValues["statuscode"] == "Draft" || r.FormattedValues["statuscode"] == "New")
                    {
                        req.isReadOnly = false;
                        req.AvailableActions.Clear();
                        req.AvailableActions.Add("submit");
                        req.AvailableActions.Add("save");
                        req.AvailableActions.Add("update");
                        req.AvailableActions.Add("cancel");

                    }
                    else if (r.FormattedValues["statuscode"] == "Pending Resolution Confirmation")
                    {
                        req.isReadOnly = true;
                        req.AvailableActions.Clear();
                        req.AvailableActions.Add("confirm");

                    }else
                    {
                        req.isReadOnly = true;
                        req.AvailableActions.Clear();
                        req.AvailableActions.Add("cancel");
                    }

                    viewRequests.Add(req);
                }
                catch (KeyNotFoundException)
                {
                    //TempData["info"] = "Some Data Wasn't ready Please refresh this page again in few seconds ... ";
                    continue;
                }
            }

            return viewRequests;
        }

        internal void UpdateRequest(HelpDeskRequest helpDeskRequest, string p, OptionSetValue CRMAction)
        {
            using (crmcontgenerated ctx = new crmcontgenerated(OrgService))
            {
                // get obj to update 
                new_helpdeskrequest requestToUpdate = ctx.new_helpdeskrequestSet.Where(r => r.Id == helpDeskRequest.Id).FirstOrDefault();
                 
                // apply changes  assumming objecct is editable 
                requestToUpdate.new_RequestDetails = helpDeskRequest.RequestDetails;
                requestToUpdate.new_name = helpDeskRequest.RequestTitle;
                requestToUpdate.new_HelpDeskCategoryId = new EntityReference(new_helpdeskcategory.EntityLogicalName, helpDeskRequest.Category);
                requestToUpdate.new_Subcategory = new EntityReference(new_helpdeskrequestsubcategory.EntityLogicalName, helpDeskRequest.SubCategory);
                
                // pass the crm action 
                requestToUpdate.new_Action = CRMAction;
                
                // save operation 
                ctx.UpdateObject(requestToUpdate);
                ctx.SaveChanges();

            }
        }
    }
}