using CRMPortal.SharedServices.AuthenticationLayer;
using CRMPortal.SharedServices.Models;
using CRMPortal.SharedServices.ViewModels;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Web;
using System.Web.Mvc;

namespace CRMPortal.SharedServices.Controllers
{
    public class AccountController : Controller
    {


        //
        // GET: /AccountModel/
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(AccountViewModel acc)
        {

            UnitOfWork uof = Auth.GetContext(acc.Email, acc.Password);
            if (uof != null)
            {
                Session["LoggedInUser"] = acc.Email;
                Session["LoggedInPassword"] = acc.Password;
                Guid uid = uof.AccModel.UserId;
                Session["LoggedInUserId"] = uid;

            }

            AccountViewModel vm = new AccountViewModel { Acc = uof.AccModel };
            uof.Dispose();
            return View("Home", vm);
        }


        public ActionResult GetContacts()
        {
            if (Session["LoggedInUserId"] == null)
                return RedirectToAction("Login", "Account");
            
            
            //Guid loggedInuserID = new Guid(Session["LoggedInUser"].ToString());
            UnitOfWork uof = Auth.GetContext(Session["LoggedInUser"].ToString(), Session["LoggedInPassword"].ToString());
            EntityReference ContactsOwner = new EntityReference("systemuser", uof.AccModel.UserId);
            var contatcts = uof.ContactModel.GetContatsByOwner(ContactsOwner);
            ContactViewModel vm = new ContactViewModel { ContactLastName = contatcts[0]["lastname"].ToString() };

            uof.Dispose();
            return View("Contacts", vm);
        }



    }
}