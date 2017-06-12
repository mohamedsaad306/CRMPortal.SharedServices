using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.Models
{
    public class Repository
    {
        public Repository(OrganizationServiceContext _ctx,IOrganizationService _orgService)
        {
            Context=_ctx;
            OrgService = _orgService;
        }

        public OrganizationServiceContext Context { get; private set; }
        public IOrganizationService OrgService { get; set; }
        
    }
}