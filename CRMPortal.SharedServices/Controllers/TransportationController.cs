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
    public class TransportationController : Controller
    {
        //
        // GET: /Transportation/
        private UnitOfWork uof;

        public ActionResult Index()
        {
            if (Session["LoggedInUserId"] == null)
                return RedirectToAction("Login", "Account");

            uof = Auth.GetContext(Session["LoggedInUser"].ToString(), Session["LoggedInPassword"].ToString());

            List<Transportation> viewRequests = uof.Transportations.GetAllAsTransportaion(new Guid(Session["LoggedInUserId"].ToString()));
            
            TransportationIndexViewModel vm = new TransportationIndexViewModel() { requests = viewRequests };

            return View(vm);
        }

  
        public ActionResult Edit()
        {
            if (Session["LoggedInUserId"] == null)
                return RedirectToAction("Login", "Account");

            uof = Auth.GetContext(Session["LoggedInUser"].ToString(), Session["LoggedInPassword"].ToString());

            //List<Customer> DummyViewCustomers = new List<Customer>() {
            //new Customer {Id=Guid.NewGuid(),LogicalName="contact",Name="Ahmed Gamal"},
            //new Customer {Id=Guid.NewGuid(),LogicalName="contact",Name="Mervat MOhsen"},
            //new Customer {Id=Guid.NewGuid(),LogicalName="account",Name="Ossama Nagy"},
            //new Customer {Id=Guid.NewGuid(),LogicalName="account",Name="Hafez BAsem"},
            //};

            List<Customer> viewCustomers = uof.Customers.GetAllAsCustomer();
            TransportationFormViewModel vm = new TransportationFormViewModel() { AvailableCustomers = viewCustomers, Date = DateTime.Today };
            return View(vm);
        }
 
        public ActionResult Save(TransportationFormViewModel r)
        {
            if (Session["LoggedInUserId"] == null)
            {
                TempData["info"] = "please login to continue with your request ...";
                return RedirectToAction("Login", "Account");
            }

            uof = Auth.GetContext(Session["LoggedInUser"].ToString(), Session["LoggedInPassword"].ToString());


            new_transportationrequest request = new new_transportationrequest();

            request.new_name = r.title;
            request.new_Requestdetails = r.Details;
            request.new_ClientName = new EntityReference(r.CustomerType, r.Client);
            request.new_Address = r.Address;
            request.new_Date = r.Date;

            request.new_relatedemployeeId = new EntityReference(SystemUser.EntityLogicalName, new Guid(Session["LoggedInUserId"].ToString()));
            request.new_Action = (r.Action == "Submit") ? new OptionSetValue(100000000)/*submit value*/: new OptionSetValue(100000001);/*save value */

            uof.Transportations.Add(request);



            return RedirectToAction("Edit");
        }
    }
}