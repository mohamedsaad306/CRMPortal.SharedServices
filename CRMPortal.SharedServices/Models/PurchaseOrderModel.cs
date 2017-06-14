﻿using Microsoft.Xrm.Sdk;
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
            return Context.CreateQuery("new_purchaserequest").Where(r => r["new_relatedemployeeid"] == new EntityReference("systemuser", usr_id)).ToList();
        }

        internal void SubmitRequest(Entity req)
        {
            Context.AddObject(req);
            Context.SaveChanges();
        }
    }
}