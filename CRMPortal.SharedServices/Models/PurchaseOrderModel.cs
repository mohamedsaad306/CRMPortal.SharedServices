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
        public PurchaseOrderModel(OrganizationServiceContext ctx)
            : base(ctx)
        {
        }

        public List<Entity> GetAllRequests(Guid usr_id)
        {
<<<<<<< HEAD
            return Context.CreateQuery("new_purchaserequest").Where(r => (r["new_relatedemployeeid"] == new EntityReference("systemuser", usr_id)) || (r["createdby"] == new EntityReference("systemuser", usr_id))).ToList();
=======
            return Context.CreateQuery("new_purchaserequest").Where(r => r["new_relatedemployeeid"] == new EntityReference("systemuser", usr_id)).ToList();
>>>>>>> 898e6793cda1d4fcc87347dd0e273137f68d4ff0
        }

        internal void SubmitRequest(Entity req)
        {
            Context.AddObject(req);
            Context.SaveChanges();
        }
    }
}