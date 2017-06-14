using CRMPortal.SharedServices.AuthenticationLayer;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMPortal.SharedServices.Models
{
    public class RoomReservationModel : Repository
    {
        public RoomReservationModel(OrganizationServiceContext ctx, IOrganizationService orgService)
            : base(ctx, orgService)
        {
        }
        public List<Entity> GetAllRequests(Guid usr_id)
        {
            return Context.CreateQuery("new_roomreservationrequest").Where(r => r["ownerid"] == new EntityReference("systemuser", usr_id)).ToList();
        
        }

        internal void MakeReservation(new_roomreservationrequest request)
        {
            Context.AddObject(request);
            Context.SaveChanges();
        }

        internal bool CheckReservationAvilability(new_roomreservationrequest request)
        {
            bool dateValid = true;
            List<new_roomoccupation> occupations = GetRoomOccupations(request);

            foreach (var o in occupations)
            {
                if (// reservation from  is not within testing occupation 
                    (request.new_DateFrom.Value.TimeOfDay > o.new_Occupiedfrom.Value.TimeOfDay
                    && request.new_DateFrom.Value.TimeOfDay < o.new_OccupiedTo.Value.TimeOfDay)
                    ||// reservation to  is not within testing occupation
                    (request.new_DateTo.Value.TimeOfDay > o.new_Occupiedfrom.Value.TimeOfDay
                    && request.new_DateTo.Value.TimeOfDay < o.new_OccupiedTo.Value.TimeOfDay)
                    ||// Testing occupation is not within reservation 
                    ((o.new_Occupiedfrom.Value.TimeOfDay >= request.new_DateFrom.Value.TimeOfDay)
                    && o.new_OccupiedTo.Value.TimeOfDay <= request.new_DateTo.Value.TimeOfDay)
                    )
                {
                    dateValid = false;
                    break;
                }
            }
            //IOrganizationService  serv =Auth.GetOrgService();

            return dateValid; 
        }

        public List<new_roomoccupation> GetRoomOccupations(new_roomreservationrequest request)
        {
            List<new_roomoccupation> occupations;
            using (crmcontgenerated OrgContext = new crmcontgenerated(OrgService))
            {
                occupations =
                   (
                   from oc in OrgContext.new_roomoccupationSet
                   where oc.new_relatedRoom == request.new_Room
                   select oc).ToList();
            }

            occupations = occupations.Where(oo => oo.new_Occupiedfrom.Value.Date == request.new_DateFrom.Value.Date).ToList();
            return occupations;
         }
    }
}