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
            //List<Entity> requests = uof.HelpDeskModel.GetAllRequests();

            List<HelpDeskRequest> viewRequests = uof.HelpDeskModel.GetAllAsHelpDeskRequests(new Guid(Session["LoggedInUserId"].ToString()));


            HelpDeskIndexViewModel vm = new HelpDeskIndexViewModel() { Requests = viewRequests };
            uof.Dispose();
            return View(vm);
        }


        public ActionResult Edit(Guid? id)
        {
            if (Session["LoggedInUserId"] == null)
            {
                TempData["info"] = "Please Login to Access this Page.. ";
                return RedirectToAction("Login", "Account");
            }
            uof = Auth.GetContext(Session["LoggedInUser"].ToString(), Session["LoggedInPassword"].ToString());

            HelpDeskFormViewModel vm = new HelpDeskFormViewModel();

            HelpDeskRequest tr = new HelpDeskRequest() { StatusReason="New"};
            if (id != null)
            {
                //Guid usrId= new Guid(Session[""])
                tr = uof.HelpDeskModel.GetById(new Guid(Session["LoggedInUserId"].ToString()), id);
            }

            vm.Categories = uof.HelpDeskRequestCategories.GetAllCategories();
            vm.SubCategories = uof.HelpDeskRequestSubCategories.GetAllSubCategories();
            vm.HelpDeskRequest = tr;

            return View(vm);
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

            //new_helpdeskrequest r = new new_helpdeskrequest();
            Entity req = new Entity("new_helpdeskrequest");
           
            if (_r.HelpDeskRequest.Id == Guid.Empty)
            {
                req["new_name"] = _r.RequestTitle;
                req["new_requestdetails"] = _r.RequestDetails;

                req["new_helpdeskcategoryid"] = new EntityReference(new_helpdeskcategory.EntityLogicalName, _r.Category);
                req["new_subcategory"] = new EntityReference(new_helpdeskrequestsubcategory.EntityLogicalName, _r.SubCategory);
                Guid uid = new Guid(Session["LoggedInUserId"].ToString());
                req["new_relatedemployeeid"] = new EntityReference("systemuser", uid);

            }

            switch (_r.Action.ToLower())
            {
                case "submit":
                    if (_r.HelpDeskRequest.Id == Guid.Empty)
                    {
                        req["new_action"] = new OptionSetValue(100000000); //submit value 
                        uof.HelpDeskModel.SubmitRequest(req);
                    }
                    else
                    {
                        OptionSetValue submit = new OptionSetValue(100000000); //Save value 
                        uof.HelpDeskModel.UpdateRequest(_r.HelpDeskRequest, "submit", submit);
                    }
                    break;

                case "save":
                    if (_r.HelpDeskRequest.Id == Guid.Empty)
                    {
                        req["new_action"] = new OptionSetValue(100000001); //save value 
                        uof.HelpDeskModel.SubmitRequest(req);
                    }
                    else
                    {
                        OptionSetValue saveAction = new OptionSetValue(100000001); //Save value 
                        uof.HelpDeskModel.UpdateRequest(_r.HelpDeskRequest, "save", saveAction);
                    }
                    break;

                case "cancel":
                    OptionSetValue CancelAction = new OptionSetValue(100000009); //cancel value 
                    uof.HelpDeskModel.UpdateRequest(_r.HelpDeskRequest, _r.Action, CancelAction);
                    break;

                case "update":
                    OptionSetValue UpdateAction = new OptionSetValue(100000010); //Resubmit value 
                    uof.HelpDeskModel.UpdateRequest(_r.HelpDeskRequest, _r.Action, UpdateAction);
                    break;
                case "confirm":
                    OptionSetValue ConfirmAction = new OptionSetValue(100000008); // Confirm
                    uof.HelpDeskModel.UpdateRequest(_r.HelpDeskRequest, _r.Action, ConfirmAction);
                    break;
                default:
                    break;
            }

            return RedirectToAction("index");
        }
    }
}