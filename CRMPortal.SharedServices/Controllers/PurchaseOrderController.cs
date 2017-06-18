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
<<<<<<< HEAD
            List<Entity> requests = uof.PurchaseOrderModel.GetAllRequests(new Guid(Session["LoggedInUserId"].ToString()));

            List<PurchaseOrderRequest> viewRequests = new List<PurchaseOrderRequest>();
            foreach (var r in requests)
            {
                try
                {
                    //bool x = r.Attributes.Keys.Contains("new_name");
                    viewRequests.Add(new PurchaseOrderRequest
                    {
                        PK = r.Id,
                        CreatedAt = r.Attributes.Keys.Contains("createdon") ? DateTime.Parse(r["createdon"].ToString()) : new DateTime(),
                        RequestTitle = r.Attributes.Keys.Contains("new_name") ? r["new_name"].ToString() : "",
                        RequestNumber = r.Attributes.Keys.Contains("new_requestnumber") ? r["new_requestnumber"].ToString() : "",
                        NumberOfitems = r.Attributes.Keys.Contains("new_numberofitems") ? r["new_numberofitems"].ToString() : "",
                        Purpose = r.Attributes.Keys.Contains("new_purpose") ? r["new_purpose"].ToString() : "",
                        StatusReason = r.Attributes.Keys.Contains("statuscode") ? r.FormattedValues["statuscode"].ToString() : ""
                        //CreatedAt = DateTime.Parse(r["createdon"].ToString()),
                        //RequestTitle = r["new_name"].ToString(),
                        //RequestNumber =(r.Contains("new_requestnumber"))? r["new_requestnumber"].ToString():string.Empty,
                        //NumberOfitems = r["new_numberofitems"].ToString(),
                        //StatusReason = r.FormattedValues["statuscode"].ToString()

                        //StatusReason = r.Attributes.Keys.Contains("statuscode") ? r.FormattedValues["statuscode"].ToString() : "",
                    });
                }
                catch (KeyNotFoundException)
                {
                    TempData["info"] = "Some Data Wasn't ready Please refresh this page again in few seconds ... ";
                    continue;
                }
            }
=======
            
            List<PurchaseOrderRequest> viewRequests = uof.PurchaseOrderModel.GetAllAsPurchaseOrderRequest(new Guid(Session["LoggedInUserId"].ToString()));
>>>>>>> cd227020990ff1dba16755aa538e2e26a41bf61a

            PurchaseOrderViewModel vm = new PurchaseOrderViewModel() { Requests = viewRequests };
            uof.Dispose();
            return View(vm);
        }

    
        public ActionResult Edit(Guid? id)
        {
            TempData["Info"] = id;
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
                req["new_purchaserequestid"] = _r.PK;
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