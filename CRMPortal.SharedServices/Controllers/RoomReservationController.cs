using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRMPortal.SharedServices.Controllers
{
	public class RoomReservationController : Controller
	{
		//
		// GET: /RoomReservation/
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult Edit(Guid? id)
		{

			return View();
		}
	}
}   