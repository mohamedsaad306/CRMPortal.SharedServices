﻿using CRMPortal.SharedServices.AuthenticationLayer;
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
            List<Entity> requests = uof.PurchaseOrderModel.GetAllRequests(new Guid(Session["LoggedInUserId"].ToString()));

            List<PurchaseOrderRequest> viewRequests = new List<PurchaseOrderRequest>();
            foreach (var r in requests)
            {
                viewRequests.Add(new PurchaseOrderRequest
                {
                    CreatedAt = DateTime.Parse(r["createdon"].ToString()),
                    RequestTitle = r["new_name"].ToString(),
                    RequestNumber = r["new_requestnumber"].ToString(),
                    NumberOfitems = r["new_numberofitems"].ToString(),
                    StatusReason = r.FormattedValues["statuscode"].ToString()
                });
            }

            PurchaseOrderViewModel vm = new PurchaseOrderViewModel() { Requests =   viewRequests };
            uof.Dispose();
            return View(vm);
        }

        public ActionResult Edit(Guid? id)
        {
            return View();
        }

        //[HttpPost]
        public ActionResult Save(PurchaseOrderFormViewModel _r)
        {
            if (Session["LoggedInUserId"] == null)
            {
                TempData["info"] = "Please Login to Access this Page.. ";
                return RedirectToAction("Login", "Account");
            }

            uof = Auth.GetContext(Session["LoggedInUser"].ToString(), Session["LoggedInPassword"].ToString());

            Entity req = new Entity("new_helpdeskrequest");
            req["new_name"] = _r.RequestTitle;
            req["new_numberofitems"] = _r.NumberOfitems;
            req["statuscode"] = _r.Status;
            req["new_action"] = new OptionSetValue(100000000);

            Guid uid = new Guid(Session["LoggedInUserId"].ToString());
            req["new_relatedemployeeid"] = new EntityReference("systemuser", uid);

            uof.PurchaseOrderModel.SubmitRequest(req);
            return RedirectToAction("index");
        }

    }
}