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
            List<Entity> requests = (List<Entity>)uof.Transportations.GetRequests(new Guid(Session["LoggedInUserID"].ToString()));
            List<Transportation> viewRequests = new List<Transportation>();
            foreach (var r in requests)
            {
                try
                {
                    viewRequests.Add(new Transportation
                    {
                        CreatedAt = r.Attributes.Keys.Contains("createdon") ? DateTime.Parse(r["createdon"].ToString()) : new DateTime(),
                        RequestTitle = r.Attributes.Keys.Contains("new_name") ? r["new_name"].ToString() : "",
                        RequestNumber = r.Attributes.Keys.Contains("new_requestnumber") ? r["new_requestnumber"].ToString() : "",
 
                        StatusReason = r.Attributes.Keys.Contains("statuscode") ? r.FormattedValues["statuscode"].ToString() : "",

                        TransportationDate = r.Attributes.Keys.Contains("new_date") ? DateTime.Parse(r["new_date"].ToString()) : new DateTime(),
                        Address = r.Attributes.Keys.Contains("new_address") ? r["new_address"].ToString() : "",
                        RelatedClient = r.Attributes.Keys.Contains("new_clientname") ? r.FormattedValues["new_clientname"].ToString() : "",

                    });
                    //new_transportationrequest d = new new_transportationrequest{}
                }
                catch (KeyNotFoundException)
                {
                    TempData["info"] = "Some data didn't Load Correctly plase try again later .. ";
                    continue;
                    throw;
                }
            }
            TransportationIndexViewModel vm = new TransportationIndexViewModel() { requests = viewRequests };

            return View(vm);
        }
        public ActionResult Edit()
        {
            //if (Session["LoggedInUserId"] == null)
            //    return RedirectToAction("Login", "Account");
            
            //uof = Auth.GetContext(Session["LoggedInUser"].ToString(), Session["LoggedInPassword"].ToString());

            //List<Entity> customers  = uof.Customers.GetAllCustomers();
            List<Customer> viewCustomers = new List<Customer>();

            List<Customer> DummyViewCustomers = new List<Customer>() {
            new Customer {Id=Guid.NewGuid(),LogicalName="contact",Name="Ahmed Gamal"},
            new Customer {Id=Guid.NewGuid(),LogicalName="contact",Name="Mervat MOhsen"},
            new Customer {Id=Guid.NewGuid(),LogicalName="account",Name="Ossama Nagy"},
            new Customer {Id=Guid.NewGuid(),LogicalName="account",Name="Hafez BAsem"},
            };
            
            //foreach (var c in customers)
            //{
            //    Customer tc = new Customer()
            //    {
            //        Id = c.Id,
            //        LogicalName = c.LogicalName,
            //        Name = (c.LogicalName == Contact.EntityLogicalName) ? c["lastname"].ToString() : c["name"].ToString()
            //    };

            //    viewCustomers.Add(tc);
            //}

            TransportationFormViewModel vm = new TransportationFormViewModel() { AvailableCustomers=DummyViewCustomers};
            return View(vm);
        }
        public ActionResult Save(TransportationFormViewModel  r )
        {
            if (Session["LoggedInUserId"] == null)
                TempData["info"] = "please login to continue with your request ...";
                return RedirectToAction("Login", "Account");

            uof = Auth.GetContext(Session["LoggedInUser"].ToString(), Session["LoggedInPassword"].ToString());


            new_transportationrequest request = new new_transportationrequest();
            
            request.new_name = r.title;
            request.new_Requestdetails = r.Details;
            request.new_ClientName = new EntityReference(r.CustomerType, r.Client);
            request.new_Address = r.Address;
            request.new_Date = r.Date;
            
            request.OwnerId = new EntityReference(SystemUser.EntityLogicalName, new Guid(Session["LoggedInUserId"].ToString()));
            request.new_Action = (r.Action == "Submit") ? new OptionSetValue(100000000)/*submit value*/: new OptionSetValue(100000001);/*save value */

            uof.Transportations.Add(request);



            return RedirectToAction("Edit");
        }
    }
}