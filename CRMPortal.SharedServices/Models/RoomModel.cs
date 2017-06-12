using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.Models
{
    public class RoomModel:Repository
    {
        public RoomModel(OrganizationServiceContext ctx, IOrganizationService orgService)
            : base(ctx, orgService)
        {
        }

        public List<Entity> AllRooms()
        {
            return Context.CreateQuery("new_room").ToList();
        }
    }
}