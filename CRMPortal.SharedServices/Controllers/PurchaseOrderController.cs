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
        // GET: /PurchaseOrder/
        public ActionResult Index()
        {
            if (Session["LoggedInUserId"] == null)
            {
                TempData["info"] = "Please Login to Access this Page.. ";
                return RedirectToAction("Login", "Account");
            }


            uof = Auth.GetContext(Session["LoggedInUser"].ToString(), Session["LoggedInPassword"].ToString());
            
            List<PurchaseOrderRequest> viewRequests = uof.PurchaseOrderModel.GetAllAsPurchaseOrderRequest(new Guid(Session["LoggedInUserId"].ToString()));

            PurchaseOrderViewModel vm = new PurchaseOrderViewModel() { Requests = viewRequests };
            uof.Dispose();
            return View(vm);
        }

    
        public ActionResult Edit(Guid? id)
        {
            return View();
        }

        //[HttpPost]
        public ActionResult Save(PurchaseOrderFormViewModel _r, string saveOrDraft)
        {
            return SaveOrDraft(_r, saveOrDraft == "Submit Request" ? 100000001 : 100000000);
        }
        ActionResult SaveOrDraft(PurchaseOrderFormViewModel _r, int actionNumber)
        {
            try
            {
                if (Session["LoggedInUserId"] == null)
                {
                    TempData["info"] = "Please Login to Access this Page.. ";
                    return RedirectToAction("Login", "Account");
                }

                uof = Auth.GetContext(Session["LoggedInUser"].ToString(), Session["LoggedInPassword"].ToString());

                Entity req = new Entity("new_purchaserequest");
                req["new_name"] = _r.RequestTitle;
                req["new_numberofitems"] = int.Parse(_r.NumberOfitems);
                req["new_itemname"] = _r.ItemName;
                req["new_purpose"] = _r.Purpose;
                req["new_actions"] = new OptionSetValue(actionNumber);

                Guid uid = new Guid(Session["LoggedInUserId"].ToString());
                req["new_relatedemployeeid"] = new EntityReference("systemuser", uid);

                uof.PurchaseOrderModel.SubmitRequest(req);
                TempData["info"] = _r.SaveOrDraft;
                return RedirectToAction("index");
                //new_purchaserequest p = new new_purchaserequest();
            }
            catch (Exception ex)
            {
                TempData["info"] = string.Format("Error: {0}", ex.Message);
                return RedirectToAction("Edit");
            }
        }
    }
}