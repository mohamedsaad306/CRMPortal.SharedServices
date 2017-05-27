using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.Models
{
    public class Repository
    {
        public Repository(OrganizationServiceContext _ctx)
        {
            Context=_ctx;
        }

        public OrganizationServiceContext Context { get; private set; }
        
    }
}