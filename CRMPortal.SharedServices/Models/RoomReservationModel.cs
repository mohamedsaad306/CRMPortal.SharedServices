using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.Models
{
    public class RoomReservationModel : Repository
    {
        public RoomReservationModel(OrganizationServiceContext ctx)
            : base(ctx)
        {
        }

    }
}