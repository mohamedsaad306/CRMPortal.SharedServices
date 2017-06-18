using CRMPortal.SharedServices.AuthenticationLayer;
using CRMPortal.SharedServices.Models;
using CRMPortal.SharedServices.Models.DomainModels;
using CRMPortal.SharedServices.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace CRMPortal.SharedServices.Controllers.Api
{
    public class RoomReservationsController : ApiController
    {
        //[HttpGet]
        //[ActionName("GetRoomReservations")]
        //public List<new_roomreservationrequest> GetRoomReservations(Guid roomId ,DateTime day)
        //{

            //CookieHeaderValue c = Request.Headers.GetCookies("session-id").FirstOrDefault();
            //if (c!=null)
            //{
            //    //HttpContext.Current.
                
               
            //}
            ////UnitOfWork uof = Auth.GetContext(Request.coo)
            //List<new_roomreservationrequest> reservations = new List<new_roomreservationrequest>();
            
            //return reservations;

        //}

        [HttpPost]
        [ActionName("CheckRoomAvailability")]
        public IHttpActionResult CheckRoomAvailability([FromBody]RoomReservationFormViewModel occ)
        {
            Guid roomId = occ.RoomToReserve;
            // chek if user is authorized to acess this data 
            if (HttpContext.Current.Session["LoggedInUserId"].ToString()==null)
                return NotFound();
            
            if (occ.Day == null || roomId == null)
                return BadRequest("Please provide day and select room to complete the request ");
            

            UnitOfWork uof;
            new_roomreservationrequest r = new new_roomreservationrequest(){
                new_DateFrom= occ.DateFrom,   
                new_DateTo=occ.DateTo,
                new_Room = new Microsoft.Xrm.Sdk.EntityReference(new_room.EntityLogicalName, roomId)
            };

            uof = Auth.GetSystemAdminContext();

            List<new_roomoccupation> requests =
                uof.RoomReservationModel.GetRoomOccupations(r);

            return Ok(requests);
        }

        [HttpGet]
        [ActionName("Bye")]
        public Request Bye()
        {
            return new Request { RequestNumber = "Baarrrr" };
        }


    }
}
