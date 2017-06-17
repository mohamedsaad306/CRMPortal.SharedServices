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
        public ActionResult Login(AccountViewModel acc, string x)
        {

            UnitOfWork uof = Auth.GetContext(acc.Email, acc.Password);
            if (uof != null)
            {
                Session["LoggedInUser"] = acc.Email;
                Session["LoggedInPassword"] = acc.Password;
                string sessionId = HttpContext.Session.SessionID;
                
                

                if (uof.AccModel.UserId != null)
                {
                    Session["LoggedInUserId"] = uof.AccModel.UserId;
                }
                else
                {
                    TempData["info"] = "Username / Password Incorrect!";
                    return RedirectToAction("Login", "Account");
                }
            }

            AccountViewModel vm = new AccountViewModel { Acc = uof.AccModel };
            uof.Dispose();
            return RedirectToAction("Home", vm);
            //return View("Home", vm);
        }


        public ActionResult GetContacts()
        {
            if (Session["LoggedInUserId"] == null)
                return RedirectToAction("Login", "Account");

            //Guid loggedInuserID = new Guid(Session["LoggedInUser"].ToString());
            UnitOfWork uof = Auth.GetContext(Session["LoggedInUser"].ToString(), Session["LoggedInPassword"].ToString());
            if (uof.AccModel.UserId != null)
            {
                EntityReference ContactsOwner = new EntityReference("systemuser", (Guid)uof.AccModel.UserId);
                var contatcts = uof.ContactModel.GetContatsByOwner(ContactsOwner);
                ContactViewModel vm = new ContactViewModel { ContactLastName = contatcts[0]["lastname"].ToString() };

                uof.Dispose();
                return View("Contacts", vm);
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Home(AccountViewModel model )
        {
            if (Session["LoggedInUserId"] == null)
                return RedirectToAction("Login", "Account");

            return View(model);
        }

    }
}
