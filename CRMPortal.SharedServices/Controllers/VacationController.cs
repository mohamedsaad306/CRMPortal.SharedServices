using CRMPortal.SharedServices.AuthenticationLayer;
using CRMPortal.SharedServices.DomainModels;
using CRMPortal.SharedServices.Models;
using CRMPortal.SharedServices.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRMPortal.SharedServices.Controllers
{
    public class VacationController : Controller
    {
        private UnitOfWork uof;
        //
        // GET: /Vacation/
        public ActionResult Index()
        {
            if (Session["LoggedInUserId"] == null)
            {
                TempData["info"] = "Please Login to Access this Page.. ";
                return RedirectToAction("Login", "Account");
            }
            uof = Auth.GetContext(Session["LoggedInUser"].ToString(), Session["LoggedInPassword"].ToString());
            //List<Entity> requests = uof.HelpDeskModel.GetAllRequests();

            List<VacationRequest> viewRequests = uof.VacationRequests.GetAllAsVacationRequests(new Guid(Session["LoggedInUserId"].ToString()));


            VacationRequestIndexViewModel vm = new VacationRequestIndexViewModel() { Requests = viewRequests };
            uof.Dispose();
            return View(vm);
        }
	}
}