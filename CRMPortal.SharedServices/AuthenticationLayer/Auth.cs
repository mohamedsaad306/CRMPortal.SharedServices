using CRMPortal.SharedServices.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Web;

namespace CRMPortal.SharedServices.AuthenticationLayer
{
    public static class Auth
    {
        static IOrganizationService orgService;
        static OrganizationServiceProxy serviceProxy; // to handle authontication with proxy
        //public static Dictionary<Guid, UnitOfWork> UnitsOfWork = new Dictionary<Guid, UnitOfWork>();



        public static UnitOfWork GetContext(string _email, string _pwd)
        {

            ClientCredentials authenticationCredential = new ClientCredentials();// client cridential

            authenticationCredential.UserName.UserName = _email;//"Mina@OurGP.onmicrosoft.com";
            authenticationCredential.UserName.Password = _pwd;// "md27-1-2013";
            serviceProxy = new OrganizationServiceProxy(new Uri("https://itigpproject.api.crm4.dynamics.com/XRMServices/2011/Organization.svc"), null, authenticationCredential, null);
            orgService = (IOrganizationService)serviceProxy;
            
            OrganizationServiceContext context = new OrganizationServiceContext(orgService);
            //Guid loggedInUserId = context.CreateQuery("systemuser").Where(u => u["internalemailaddress"] == email).FirstOrDefault().Id;
            UnitOfWork uof = new UnitOfWork(context);
            uof.AccModel.Email = _email;
            Guid uid = uof.AccModel.UserId;
            //UnitsOfWork.Add(uid, uof);
            return uof;
        }



        //internal static UnitOfWork GetContext(Guid loggedInuserID)
        //{
        //    return UnitsOfWork[loggedInuserID];
        //}
    }
}