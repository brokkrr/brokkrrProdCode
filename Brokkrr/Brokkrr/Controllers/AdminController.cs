using BrokerMVC.App_Code;
using BrokerMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrokerMVC.BrokerWebDB;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using System.Security.Authentication;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PagedList;
using PagedList.Mvc;


namespace BrokerMVC.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [Authorize]
        [HttpGet]
        public ActionResult userList()
        {

            List<UserList> dsUsers = BrokerWebDB.BrokerWebDB.getUserList();
            if (dsUsers != null)
            {
                if (dsUsers.Count > 0)
                {
                    Session["UsersList"] = dsUsers;
                    Session["Error"] = "False";
                }
            }
            return RedirectToAction("AdminHomePage", "Admin");
        
        }



        [HttpPost]
        public ActionResult Login(Admin admin)
        {

            string encryptedPassword = BrokerUtility.EncryptURL(admin.Password);
            //DataSet dsUsers= BrokerWebDB.BrokerWebDB.validateAdminGetUsers(admin.Username, encryptedPassword);
            List<UserList> userList = new List<UserList>();
            userList = BrokerWebDB.BrokerWebDB.validateAdminGetUsers(admin.Username, encryptedPassword);
            User adminInfo = BrokerWebDB.BrokerWebDB.getAdminInfo(admin.Username, encryptedPassword);

            if (userList != null)
            {
                if (userList.Count > 0)
                {
                    Session["searchBy"] = "-1";
                    Session["UsersList"] = userList;
                    Session["Error"] = "False";
                    Session["AdminId"] = adminInfo.UserId;
                    Session["AdminUserName"] = adminInfo.EmailId;
                    CreateLoginIdentity(adminInfo.EmailId, adminInfo.UserId);
                    return RedirectToAction("AdminHomePage", "Admin");
                }
                else
                {
                    ViewBag.error = "Invalid username and password";
                }
            }
            else
            {
                ViewBag.error = "Invalid username and password";
            }

            return View();
            
        }

        IAuthenticationManager Authentication
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        public void CreateLoginIdentity(string Name, int UserId)
        {
            try
            {

                var identity = new ClaimsIdentity(new[] {
                                new Claim(ClaimTypes.Name, Name),
                                new Claim(ClaimTypes.NameIdentifier, Convert.ToString(UserId)),
                            },
                               DefaultAuthenticationTypes.ApplicationCookie,
                               ClaimTypes.Name, ClaimTypes.Role);

                // if you want roles, just add as many as you want here (for loop maybe?)
                identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                // tell OWIN the identity provider, optional
                // identity.AddClaim(new Claim(IdentityProvider, "Simplest Auth"));

                Authentication.SignIn(new AuthenticationProperties
                {
                    IsPersistent = false
                }, identity);
            }
            catch (Exception ex)
            {
              //  BrokerUtility.ErrorLog(Convert.ToInt32(Session["UserId"].ToString()), "CreateLoginIdentity_Website", ex.Message.ToString(), "LoginController.cs_CreateLoginIdentity()", BrokerUtility.GetIPAddress(Session["UserId"].ToString()));
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult AdminLogout()
        {
            try
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                Session["searchBy"] = null;
                Session["UsersList"] = null;
                Session["AdminId"] = null;
                Session["AdminUserName"] = null;
            }
            catch (Exception ex)
            {
               // BrokerUtility.ErrorLog(Convert.ToInt32(Session["UserId"].ToString()), "CreateLoginIdentity", ex.Message.ToString(), "LoginController.cs_CreateLoginIdentity()", BrokerUtility.GetIPAddress(Session["UserId"].ToString()));
            }
            return RedirectToAction("Login", "Admin");
        
        }

        [Authorize]
        [HttpGet]
        public ActionResult AdminHomePage(int? page)
        {
            List<UserList> dsuser = (List<UserList>)Session["UsersList"];

            @ViewBag.isListNull=Session["Error"];
            if (dsuser != null)
            {
                if (dsuser.Count > 0)
                {
                   // ViewBag.userdetails = dsuser.Tables[0].Rows.Cast<DataRow>().ToList();


                    @ViewBag.search = Session["searchBy"];

                    //switch (Session["searchBy"].ToString())
                    //{
                    //    case "UserType": if (Session["txtToSearch"] == "") { Session["txtToSearch"]="broker"; } break;
                    //    case "Active": if (Session["txtToSearch"] == "") { Session["txtToSearch"] = "True"; } break;
                    //}

                    @ViewBag.toSearch = Session["txtToSearch"];

                  //  List<UserList> userList = ViewBag.userdetails;
                }
            }

            if (dsuser != null)
            {
                return View(dsuser.ToPagedList(page ?? 1, 25));
            }
            else
            {
                return View(dsuser.ToPagedList(page??1,25));
            }

            
        }

        [Authorize]
        [HttpPost]
        public ActionResult Search(string search,string txtSearch)
        {

           
            Session["searchBy"] = search;
            Session["txtToSearch"] = txtSearch;
            List<UserList> dsUsers = BrokerWebDB.BrokerWebDB.getFilteredUsers(search, txtSearch);

            if (dsUsers != null)
            {
                if (dsUsers.Count > 0)
                {
                    Session["UsersList"] = dsUsers;
                    Session["Error"] = "False";
                    return RedirectToAction("AdminHomePage", "Admin");
                }
                else
                {
                    Session["Error"] = "True";
                }
                //else
                //{
                //    Session["UsersList"] = null;
                //}
            }
           
            return RedirectToAction("AdminHomePage", "Admin");
        
        }




    }
}