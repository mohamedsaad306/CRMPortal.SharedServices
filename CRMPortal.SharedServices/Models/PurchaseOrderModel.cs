using CRMPortal.SharedServices.Models.DomainModels;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.Models
{
    public class PurchaseOrderModel : Repository
    {
        public PurchaseOrderModel(OrganizationServiceContext ctx, IOrganizationService orgService)
            : base(ctx, orgService)
        {
        }

        public List<Entity> GetAllRequests(Guid usr_id)
        {
            return Context.CreateQuery("new_purchaserequest").Where(r => r["new_relatedemployeeid"] == new EntityReference("systemuser", usr_id)|| r["ownerid"] == new EntityReference("systemuser", usr_id)).ToList();
        }
        internal List<PurchaseOrderRequest> GetAllAsPurchaseOrderRequest(Guid user_id)
        {
            List<Entity> requests = GetAllRequests(user_id);

            List<PurchaseOrderRequest> viewRequests = new List<PurchaseOrderRequest>();
            foreach (var r in requests)
            {
                try
                {
                    //bool x = r.Attributes.Keys.Contains("new_name");
                    viewRequests.Add(new PurchaseOrderRequest
                    {
                        PK = r.Id,
                        CreatedAt = r.Attributes.Keys.Contains("createdon") ? DateTime.Parse(r["createdon"].ToString()) : new DateTime(),
                        ItemName = r.Attributes.Keys.Contains("new_itemname") ? r["new_itemname"].ToString() : "",
                        RequestTitle = r.Attributes.Keys.Contains("new_name") ? r["new_name"].ToString() : "",
                        RequestNumber = r.Attributes.Keys.Contains("new_requestnumber") ? r["new_requestnumber"].ToString() : "",
                        NumberOfitems = r.Attributes.Keys.Contains("new_numberofitems") ? r["new_numberofitems"].ToString() : "",
                        Purpose = r.Attributes.Keys.Contains("new_purpose") ? r["new_purpose"].ToString() : "",
                        StatusReason = r.Attributes.Keys.Contains("statuscode") ? r.FormattedValues["statuscode"].ToString() : "",
                        RequestType = "Purchase Order", 
                        

                    });
                }
                catch (KeyNotFoundException)
                {
                    //TempData["info"] = "Some Data Wasn't ready Please refresh this page again in few seconds ... ";
                    continue;
                }
            }
            return viewRequests;
        }
        internal PurchaseOrderRequest GetPurchaseRequestById(Guid user_id,Guid requestId)
        {
            return GetAllAsPurchaseOrderRequest(user_id).FirstOrDefault(r => r.PK == requestId);
        }

        internal void SubmitRequest(Entity req)
        {
            Context.AddObject(req);
            Context.SaveChanges();
        }
        internal void EditRequest(Entity req)
        {
            Context.UpdateObject(req);
            Context.SaveChanges();
        }
    }
}