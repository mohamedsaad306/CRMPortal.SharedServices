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
    public class HelpDeskController : Controller
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

            List<Entity> requests = uof.HelpDeskModel.GetAllRequests(new Guid(Session["LoggedInUserId"].ToString()));

            List<HelpDeskRequest> viewRequests = new List<HelpDeskRequest>();
            foreach (var r in requests)
            {
                
                viewRequests.Add(new HelpDeskRequest
                {
                    CreatedAt = DateTime.Parse(r["createdon"].ToString()),
                    RequestTitle = r["new_name"].ToString(),
                    RequestNumber = r["new_requestnumber"].ToString(),
                    RequestDetails = r["new_requestdetails"].ToString(),
                    StatusReason = r.FormattedValues["statuscode"].ToString()
                });
            }
             
            HelpDeskIndexViewModel vm = new HelpDeskIndexViewModel() { Requests = viewRequests };
            uof.Dispose();
            return View(vm);
        }

        public ActionResult Edit(Guid? id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(HelpDeskFormViewModel _r)
        {
            if (Session["LoggedInUserId"] == null)
            {
                TempData["info"] = "Please Login to Access this Page.. ";
                return RedirectToAction("Login", "Account");
            }
            uof = Auth.GetContext(Session["LoggedInUser"].ToString(), Session["LoggedInPassword"].ToString());
            
            new_helpdeskrequest r = new new_helpdeskrequest();
           
            Entity req = new Entity("new_helpdeskrequest");
            req["new_name"] = _r.RequestTitle;
            req["new_requestdetails"] = _r.RequestDetails;
            req["new_action"] = new OptionSetValue(100000000);

            Guid uid = new Guid(Session["LoggedInUserId"].ToString());
            req["new_relatedemployeeid"] = new EntityReference("systemuser", uid);

          uof.HelpDeskModel.SubmitRequest(req);
            return RedirectToAction("index");
        }
    }
}