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
            if (id != null)
            {
                if (Session["LoggedInUserId"] == null)
                {
                    TempData["info"] = "Please Login to Access this Page.. ";
                    return RedirectToAction("Login", "Account");
                }
                uof = Auth.GetContext(Session["LoggedInUser"].ToString(), Session["LoggedInPassword"].ToString());
                PurchaseOrderRequest po = uof.PurchaseOrderModel.GetPurchaseRequestById(new Guid(Session["LoggedInUserId"].ToString()), (Guid)id);
                PurchaseOrderFormViewModel r;
                if (po != null)
                {
                    r = new PurchaseOrderFormViewModel
                    {
                        ItemName = po.ItemName,
                        NumberOfitems = po.NumberOfitems,
                        PK = po.PK,
                        Purpose = po.Purpose,
                        RequestTitle = po.RequestTitle,
                        Status = po.Status,
                        StatusReason = po.StatusReason
                    };
                    TempData["PK"] = id;
                    TempData["Flag"] = true;
                }
                else
                {
                    r = null;
                }

                if (po.RequestNumber != "")
                {
                    ViewBag.IsReadOnly = "readonly";
                }
                return View(r);
            }
            else
            {
                return View();
            }
        }

        //[HttpPost]
        public ActionResult Save(PurchaseOrderFormViewModel _r, string saveOrDraft)
        {
            return SaveOrDraft(_r, saveOrDraft);
        }
        ActionResult SaveOrDraft(PurchaseOrderFormViewModel _r, string saveOrDraft)
        {
            try
            {
                if (Session["LoggedInUserId"] == null)
                {
                    TempData["info"] = "Please Login to Access this Page.. ";
                    return RedirectToAction("Login", "Account");
                }

                uof = Auth.GetContext(Session["LoggedInUser"].ToString(), Session["LoggedInPassword"].ToString());

                if (TempData["Flag"] == null || !(bool)TempData["Flag"])
                {
                    Entity req = new Entity("new_purchaserequest");
                    req["new_name"] = _r.RequestTitle;
                    req["new_numberofitems"] = int.Parse(_r.NumberOfitems);
                    req["new_itemname"] = _r.ItemName;
                    req["new_purpose"] = _r.Purpose;
                    req["new_actions"] = new OptionSetValue(saveOrDraft == "Submit Request" ? 100000001 : 100000000);
                    //req["new_purchaserequestid"] = _r.PK;
                    Guid uid = new Guid(Session["LoggedInUserId"].ToString());
                    req["new_relatedemployeeid"] = new EntityReference("systemuser", uid);

                    uof.PurchaseOrderModel.SubmitRequest(req);
                }
                else
                {
                    new_purchaserequest e = uof.PurchaseOrderModel.Context.CreateQuery<new_purchaserequest>().Where(r => r.Id == (Guid)TempData["PK"]).FirstOrDefault();
                    e.new_name = _r.RequestTitle;
                    e.new_Numberofitems = int.Parse(_r.NumberOfitems);
                    e.new_ItemName = _r.ItemName;
                    e.new_Purpose = _r.Purpose;
                    e.new_Actions = new OptionSetValue(saveOrDraft == "Submit Request" ? 100000001 : 100000000);
                    Guid uid = new Guid(Session["LoggedInUserId"].ToString());
                    e["new_relatedemployeeid"] = new EntityReference("systemuser", uid);

                    uof.PurchaseOrderModel.EditRequest(e);
                    ViewBag.Flag = false;
                }

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