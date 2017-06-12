using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.Models
{
    public class ContactModel : Repository
    {
        public ContactModel(OrganizationServiceContext ctx, IOrganizationService orgService)
            : base(ctx, orgService)
        {
        }
        public List<Entity> GetContatsByOwner(EntityReference ownerId)
        {
            EntityCollection contacts = new EntityCollection();
            return   Context.CreateQuery("contact").Where(c => c["ownerid"] == ownerId).ToList();
        }

    }
}