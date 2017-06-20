using CRMPortal.SharedServices.DomainModels;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.Models
{
    public class VacationModel:Repository
    {

        public VacationModel (OrganizationServiceContext ctx, IOrganizationService orgService)
            : base(ctx, orgService)
        {
        }

        internal List<VacationRequest> GetAllAsVacationRequests(Guid user_guid)
        {
                List<new_vacationrequest> requests = new List<new_vacationrequest>();
            using (crmcontgenerated OrgCtx = new crmcontgenerated(OrgService))
            {
                requests= OrgCtx.new_vacationrequestSet.Where(r => r.CreatedBy.Id == new EntityReference(SystemUser.EntityLogicalName, user_guid).Id).ToList();
            }

            List<VacationRequest> ViewRequestes = new List<VacationRequest>();
            foreach (new_vacationrequest r in requests)
            {
                VacationRequest v = new VacationRequest()
                {
                    Id=r.Id,
                    RequestTitle=r.new_name,
                    RequestNumber= r.new_RequestNumber,
                    RequestDetails= r.new_Comments,
                    CreatedAt= r.CreatedOn,
                    RequestOwner= r.FormattedValues["ownerid"],
                    StatusReason= r.FormattedValues["statuscode"],
                    StartDate= r.new_StartDate,
                    EndDate=r.new_Enddate,
                    iSAnnual = (r.new_Annual.Value == 100000000)?true:false,
                    
                };
                // check the status to  set is read only state and actions 

                ViewRequestes.Add(v);
            }


            return ViewRequestes;
        }
    }
}