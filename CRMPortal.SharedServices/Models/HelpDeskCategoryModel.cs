using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.Models
{
    public class HelpDeskCategoryModel : Repository
    {
        public HelpDeskCategoryModel(OrganizationServiceContext ctx, IOrganizationService orgService)
            : base(ctx, orgService)
        {
        }
        public List<new_helpdeskcategory> GetAllCategories()
        {
            List<new_helpdeskcategory> categories;
            using (crmcontgenerated orCtx = new crmcontgenerated(OrgService))
            {
                categories = orCtx.new_helpdeskcategorySet.ToList();
            }
            //new_helpdeskcategory c = new new_helpdeskcategory();
            
            return categories;
        }


    }
    public class HelpDeskSubCategoryModel:Repository
    {
        public HelpDeskSubCategoryModel(OrganizationServiceContext ctx, IOrganizationService orgService)
            : base(ctx, orgService)
        {
        }
        public List<new_helpdeskrequestsubcategory> GetAllSubCategories()
        {
            List<new_helpdeskrequestsubcategory> categories;
            using (crmcontgenerated orCtx = new crmcontgenerated(OrgService))
            {
                categories = orCtx.new_helpdeskrequestsubcategorySet.ToList();
            }
            return categories;
        }
    }
}