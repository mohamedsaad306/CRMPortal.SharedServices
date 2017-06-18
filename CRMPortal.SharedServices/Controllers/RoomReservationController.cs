using CRMPortal.SharedServices.AuthenticationLayer;
using CRMPortal.SharedServices.DomainModels;
using CRMPortal.SharedServices.Models;
using CRMPortal.SharedServices.ViewModels;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRMPortal.SharedServices.Controllers
{
    public class RoomReservationController : Controller
    {
        private UnitOfWork uof;

        //
        // GET: /RoomReservation/
        public ActionResult Index()
        {
            if (Session["LoggedInUserId"] == null)
            {
                TempData["info"] = "Please Login to Access this Page.. ";
                return RedirectToAction("Login", "Account");
            }

            uof = Auth.GetContext(Session["LoggedInUser"].ToString(), Session["LoggedInPassword"].ToString());


            List<RoomReservationRequest> viewRequests = uof.RoomReservationModel.GetAllAsReservationRequest(new Guid(Session["LoggedInUserId"].ToString()));
            RoomReservationViewModel vm = new RoomReservationViewModel() { requests = viewRequests };
            return View(vm);
        }
        public ActionResult Edit(Guid? id)
        {
            if (Session["LoggedInUserId"] == null)
            {
                TempData["info"] = "Please Login to Access this Page.. ";
                return RedirectToAction("Login", "Account");
            }
            uof = Auth.GetContext(Session["LoggedInUser"].ToString(), Session["LoggedInPassword"].ToString());

            List<Entity> _rooms = uof.Rooms.AllRooms();
            List<Room> roomsTosend = new List<Room>();
            foreach (Entity e in _rooms)
            {
                roomsTosend.Add(
                    new Room
                    {
                        Id = e.Id,
                        Name = e["new_name"].ToString()
                    }
                    );
            }
            RoomReservationFormViewModel vm = new RoomReservationFormViewModel() { AvailableRooms = roomsTosend };

            return View(vm);
        }
        [HttpPost]
        public ActionResult Save(RoomReservationFormViewModel r)
        {
            if (Session["LoggedInUserId"] == null)
            {
                TempData["info"] = "Please Login to Access this Page.. ";
                return RedirectToAction("Login", "Account");
            }
            uof = Auth.GetContext(Session["LoggedInUser"].ToString(), Session["LoggedInPassword"].ToString());

            DateTime DateFrom = new DateTime(r.Day.Year, r.Day.Month, r.Day.Day, r.DateFrom.Hour, r.DateFrom.Minute, 0, DateTimeKind.Utc);
            DateTime DateTo = new DateTime(r.Day.Year, r.Day.Month, r.Day.Day, r.DateTo.Hour, r.DateTo.Minute, 0, DateTimeKind.Utc);

            new_roomreservationrequest request = new new_roomreservationrequest();

            request.new_name = r.RequestTitle;
            request.new_Room = new EntityReference(new_room.EntityLogicalName, r.RoomToReserve);
            request.new_DateFrom = DateFrom;
            request.new_DateTo = DateTo;
            request.OwnerId = new EntityReference(SystemUser.EntityLogicalName, new Guid(Session["LoggedInUserId"].ToString()));
            request.new_Action = new OptionSetValue(100000000);

            bool reservationAvailable = uof.RoomReservationModel.CheckReservationAvilability(request);

            if (reservationAvailable)
            {
                uof.RoomReservationModel.MakeReservation(request);
                TempData["info"] = "Reservation Made Sucessfully ..";
                return RedirectToAction("index");
            }
            else
            {
                TempData["info"] = "Couldn't Make reservation Room is busy ";
                return RedirectToAction("edit", r);

            }

        }
    }
}