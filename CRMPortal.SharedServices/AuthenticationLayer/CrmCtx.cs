using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Web;

namespace CRMPortal.SharedServices.AuthenticationLayer
{
    public class CrmCtx
    {
        public OrganizationServiceContext Context { get; set; }
        public CrmCtx()
        {
            ClientCredentials authenticationCredential = new ClientCredentials();// client cridential

            authenticationCredential.UserName.UserName ="Mina@OurGP.onmicrosoft.com";
            authenticationCredential.UserName.Password = "md27-1-2013";
            OrganizationServiceProxy serviceProxy = new OrganizationServiceProxy(new Uri("https://ourgp.api.crm4.dynamics.com/XRMServices/2011/Organization.svc"), null, authenticationCredential, null);
            IOrganizationService orgService = (IOrganizationService)serviceProxy;
            this.Context = new OrganizationServiceContext(orgService);

        }
    }
}