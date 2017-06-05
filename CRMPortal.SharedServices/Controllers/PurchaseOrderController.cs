using CRMPortal.SharedServices.AuthenticationLayer;
using CRMPortal.SharedServices.Models;
using CRMPortal.SharedServices.Models.DomainModels;
using CRMPortal.SharedServices.ViewModels;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CRMPortal.SharedServices.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private UnitOfWork uof;


        //
        // GET: /HelpDesk/
        public ActionResult Index()
        {
            if (Session["LoggedInUserId"] == null)
            {
                TempData["info"] = "Please Login to Access this Page.. ";
                return RedirectToAction("Login", "Account");
            }


            uof = Auth.GetContext(Session["LoggedInUser"].ToString(), Session["LoggedInPassword"].ToString());
            List<Entity> requests = uof.PurchaseOrderModel.GetAllRequests(new Guid(Session["LoggedInUserId"].ToString()));

            List<PurchaseOrderRequest> viewRequests = new List<PurchaseOrderRequest>();
            foreach (var r in requests)
            {
                viewRequests.Add(new PurchaseOrderRequest
                {
                    CreatedAt = DateTime.Parse(r["createdon"].ToString()),
                    RequestTitle = r["new_name"].ToString(),
                    //RequestNumber = r["new_requestnumber"].ToString(),
                    NumberOfitems = r["new_numberofitems"].ToString(),
                    StatusReason = r["statuscode"].ToString()
                });
            }

            PurchaseOrderViewModel vm = new PurchaseOrderViewModel() { Requests = viewRequests };
            uof.Dispose();
            return View(vm);
        }
    }
}