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
        public HelpDeskRequest GetById(Guid user_id,Guid? req_Id)
        {
            HelpDeskRequest req = GetAllAsHelpDeskRequests(user_id).Where(r=>r.Id==req_Id).FirstOrDefault();
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
                        Id=r.Id,
                        CreatedAt = DateTime.Parse(r["createdon"].ToString()),
                        RequestTitle = r["new_name"].ToString(),
                        RequestNumber = r.Attributes.Keys.Contains("new_requestnumber") ? r["new_requestnumber"].ToString() : "",
                        RequestDetails = r.Attributes.Keys.Contains("new_requestdetails") ? r["new_requestdetails"].ToString() : "",
                        StatusReason = r.FormattedValues["statuscode"].ToString(),
                        RequestType = "Help Desk"
                    };

                    if (r["statuscode"] != "Draft" || r["statuscode"] != "New")
                    {
                        req.isReadOnly = true;
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
    }
}