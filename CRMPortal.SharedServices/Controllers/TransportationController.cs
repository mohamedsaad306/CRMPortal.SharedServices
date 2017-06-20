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


        public ActionResult Edit(Guid? id)
        {

            if (Session["LoggedInUserId"] == null)
                return RedirectToAction("Login", "Account");

            uof = Auth.GetContext(Session["LoggedInUser"].ToString(), Session["LoggedInPassword"].ToString());

            TransportationFormViewModel vm = new TransportationFormViewModel();
            Transportation RecordToView = new Transportation { StatusReason = "New", TransportationDate = DateTime.Today };

            if (id != null)
            {
                RecordToView = uof.Transportations.GetById(id, new Guid(Session["LoggedInUserId"].ToString()));
            }

            List<Customer> viewCustomers = uof.Customers.GetAllAsCustomer();
            vm.AvailableCustomers = viewCustomers;
            vm.Request = RecordToView;

            // TransportationFormViewModel vm = new TransportationFormViewModel() { AvailableCustomers = viewCustomers, Date = DateTime.Today };
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
            Transportation requestToUpdate = null;
            // new request 
            if (r.Request.Id == Guid.Empty)
            {
                request.new_name = r.Request.RequestTitle;
                request.new_Requestdetails = r.Request.RequestDetails;
                request.new_ClientName = new EntityReference(r.Request.CustomerType, r.Request.RelatedClient);
                request.new_Address = r.Request.Address;
                request.new_Date = r.Request.TransportationDate;

                request.new_relatedemployeeId = new EntityReference(SystemUser.EntityLogicalName, new Guid(Session["LoggedInUserId"].ToString()));
            }
            else
            {
                Guid user_id = new Guid(Session["LoggedInUserId"].ToString());
                requestToUpdate = uof.Transportations.GetById(r.Request.Id, user_id);
                requestToUpdate.CustomerType = r.Request.CustomerType;
            }


            switch (r.Action.ToLower())
            {
                case "save":
                    if (r.Request.Id == Guid.Empty)
                    {
                        request.new_Action = new OptionSetValue(100000001);/*save value */
                        uof.Transportations.Add(request);
                    }
                    else
                    {
                        OptionSetValue submitAction = new OptionSetValue(100000001);
                        uof.Transportations.UpdateRequest(requestToUpdate, "save", submitAction);
                    }
                    break;

                case "submit":
                    if (r.Request.Id == Guid.Empty)
                    {
                        request.new_Action = new OptionSetValue(100000000);/*submit new req*/
                        uof.Transportations.Add(request);
                    }
                    else
                    {
                        OptionSetValue submitAction = new OptionSetValue(100000000); // submit saved 
                        uof.Transportations.UpdateRequest(requestToUpdate, "submit", submitAction);
                    }

                    break;

                case "cancel":
                    OptionSetValue cancelAction = new OptionSetValue(100000006); // cancel value 
                    uof.Transportations.UpdateRequest(r.Request, "cancel", cancelAction);
                    break;

                case "resubmit":
                    OptionSetValue updateAction = new OptionSetValue(100000007); // cancel value 
                    uof.Transportations.UpdateRequest(r.Request, "update", updateAction);
                    break;

                case "confirm":
                    OptionSetValue confirmAction = new OptionSetValue(100000005); //  confirm 
                    uof.Transportations.UpdateRequest(r.Request, "confirm", confirmAction);
                    break;

                default:
                    break;
            }
          //  request.new_Action = (r.Action == "Submit") ? new OptionSetValue(100000000)/*submit value*/: new OptionSetValue(100000001);/*save value */

            //uof.Transportations.Add(request);

            return RedirectToAction("Edit");
        }
    }
}