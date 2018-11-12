using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrokerMVC.Models;
using BrokerMVC.App_Code;
using System.Security.Claims;
using System.Security.Authentication;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
using Facebook;
using System.Data;
using BrokerMVC.BrokerService;
using System.Configuration;
using System.Threading.Tasks;
using System.Net;
using System.Globalization;
using ASPSnippets.LinkedInAPI;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

namespace BrokerMVC.Controllers
{

    public class LoginController : Controller
    {
        public static string strDomainName = ConfigurationManager.AppSettings["DomainName"].ToString();
        public static string strProfilePicForlderName = ConfigurationManager.AppSettings["ProfilePicForlderName"].ToString();

        public static string strProfilePicImageFolder = ConfigurationManager.AppSettings["ProfilePicImageFolder"].ToString();
        public static string strResumeImageFolder = ConfigurationManager.AppSettings["ResumeImageFolder"].ToString();
        public static string strResumeForlderName = ConfigurationManager.AppSettings["ResumeForlderName"].ToString();

        public static string strUploadedCompLogoFolder = ConfigurationManager.AppSettings["UploadedCompLogoFolder"].ToString();
        public static string strEducationLogoFolder = ConfigurationManager.AppSettings["EducationLogo"].ToString();

        public static string strCompanyLogoFolder = ConfigurationManager.AppSettings["ExperienceCompLogoFolder"].ToString();

        public static string strLinkedIn_Client_Id = ConfigurationManager.AppSettings["LinkedIn_Client_Id"].ToString();
        public static string strLinkedIn_Secrete_Id = ConfigurationManager.AppSettings["LinkedIn_Secrete_Id"].ToString();

        public static string strFacebook_Client_Id = ConfigurationManager.AppSettings["Facebook_Client_Id"].ToString();
        public static string strFacebook_Secrete_Id = ConfigurationManager.AppSettings["Facebook_Secrete_Id"].ToString();

        public static string strWebServiceURL = ConfigurationManager.AppSettings["WebServiceURL"].ToString();

        // GET: Login
        BrokerDBEntities DB = new BrokerDBEntities();

        [AllowAnonymous]
        [HttpGet]
        public ActionResult BrokerLogin()
        {
            //return View();
            string Email = "";
            string Password = "";
            bool RememberMe = false;

            if (!(string.IsNullOrEmpty(Request.QueryString["ReturnUrl"])))
            {
                if (Request.QueryString["ReturnUrl"].ToString() == "BrokkrrBriefcase")
                {
                    if (Session["UserId"] != null && Session["UserType"]!=null)
                    {
                        if (Session["UserType"].ToString() == "Broker")
                        {
                            return RedirectToAction("Index", "BrokkrrBriefcase");
                        }                        
                    }
                    Session["ReturnUrl"] = "BrokerBriefcase";
                }
            }

           

            LoginModel model = new LoginModel() { Email = Email, Password = Password, RememberMe = RememberMe };
            if (Request.Cookies["BrokerLogin"] != null)
            {
                model.Email = Request.Cookies["BrokerLogin"].Values["EmailID"];
                model.Password = Request.Cookies["BrokerLogin"].Values["Password"];
                ViewData["Password"] = Request.Cookies["BrokerLogin"].Values["Password"];
                RememberMe = true;
                model.RememberMe = true;
            }

            if (Session["LoginFail"] != null)
            {
                @ViewData["LoginFail"] = Session["LoginFail"].ToString();
                Session["LoginFail"] = null;
            }

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult CustomerLogin()
        {
            //Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //clearSessionVariable();

            //if (Request.QueryString["ReturnUrl"].ToString() != null)
            if (!(string.IsNullOrEmpty(Request.QueryString["ReturnUrl"])))
            {
                if (Request.QueryString["ReturnUrl"].ToString() == "BrokkrrBriefcase/Briefcase")
                {
                    if (Session["UserId"] != null && Session["UserType"] != null)
                    {
                        if (Session["UserType"].ToString() == "Customer")
                        {
                            return RedirectToAction("Briefcase", "BrokkrrBriefcase");
                        }
                    }
                    Session["ReturnUrl"] = "CustomerBriefcase";
                }
            }

            //return View();
            string Email = "";
            string Password = "";
            bool RememberMe = false;
            LoginModel model = new LoginModel() { Email = Email, Password = Password, RememberMe = RememberMe };
            if (Request.Cookies["CustomerLogin"] != null)
            {
                model.Email = Request.Cookies["CustomerLogin"].Values["EmailID"];
                model.Password = Request.Cookies["CustomerLogin"].Values["Password"];
                ViewData["Password"] = Request.Cookies["CustomerLogin"].Values["Password"];
                RememberMe = true;
                model.RememberMe = true;
            }

            if (Session["LoginFail"] != null)
            {
                @ViewData["LoginFail"] = Session["LoginFail"].ToString();
                Session["LoginFail"] = null;
            }
            //LoginModel model = new LoginModel() { Email = Email, Password = Password, RememberMe = RememberMe };
            return View(model);
        }

        public void clearSessionVariable()
        {
            Session["UserId"] = null;
            Session["EmailId"] = null;

            Session["FirstName"] = null;
            Session["LastName"] = null;
            Session["ZipCode"] = null;

            Session["WebServiceURL"] = null;
            Session["Initials"] = null;
            Session["ProfilePic"] = null;

            Session["LinkedInLogInBy"] = null;
            Session["FacebookLogInBy"] = null;
            Session["Company"] = null;
            Session["Location"] = null;
            Session["LineType"] = null;
            Session["FromPage"] = null;
        }

        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult BrokerLogin(LoginModel data)
        {
            //string Initials = "N/A";
            try
            {
                if (ModelState.IsValid)
                {
                    List<uspGetBrokerDetails_Result> ouser = null;

                    DataSet dsCheckUserLogin = null;
                    DataSet dsGetBrokerDetails = null;

                    string UserName = "", Password = "", encryptedPassword = "";

                    if (data.Email != null)
                    {
                        UserName = data.Email.ToString();
                    }
                    if (data.Password != null)
                    {
                        Password = data.Password.ToString();
                    }
                    encryptedPassword = BrokerUtility.EncryptURL(Password);
                    Session["UserId"] = "0";

                    dsCheckUserLogin = BrokerWebDB.BrokerWebDB.CheckBrokerLogin(UserName, encryptedPassword);

                    if (dsCheckUserLogin.Tables.Count > 0)
                    {
                        if (dsCheckUserLogin.Tables[0].Rows.Count > 0)
                        {
                            if (dsCheckUserLogin.Tables[0].Rows[0][0].ToString() == "1")
                            {
                                dsGetBrokerDetails = BrokerWebDB.BrokerWebDB.GetBrokerDetails(UserName);

                                if (dsGetBrokerDetails.Tables.Count > 0)
                                {
                                    Session["UserId"] = dsGetBrokerDetails.Tables[0].Rows[0]["UserId"].ToString();
                                    Session["EmailId"] = dsGetBrokerDetails.Tables[0].Rows[0]["EmailId"].ToString();

                                    Session["FirstName"] = dsGetBrokerDetails.Tables[0].Rows[0]["FirstName"].ToString();
                                    Session["LastName"] = dsGetBrokerDetails.Tables[0].Rows[0]["LastName"].ToString();
                                    Session["WebServiceURL"] = strWebServiceURL;
                                    Session["UserType"] = "Broker";

                                    if (dsGetBrokerDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString() != "" && dsGetBrokerDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString() != null)
                                    {
                                        Session["ProfilePic"] = strDomainName + "" + strProfilePicImageFolder + "" + dsGetBrokerDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString() + "?" + DateTime.Now;
                                    }
                                    else
                                    {
                                        Session["ProfilePic"] = "";
                                    }

                                    Session["Company"] = "Brokkrr";


                                    CreateLoginIdentity(dsGetBrokerDetails.Tables[0].Rows[0]["FirstName"].ToString() + ' ' + dsGetBrokerDetails.Tables[0].Rows[0]["LastName"].ToString(), Convert.ToInt32(dsGetBrokerDetails.Tables[0].Rows[0]["UserId"].ToString()));

                                    if (data.RememberMe)
                                    {
                                        System.Web.HttpCookie cookie = new System.Web.HttpCookie("BrokerLogin");
                                        cookie.Values.Add("EmailID", data.Email);
                                        cookie.Values.Add("Password", data.Password);
                                        cookie.Expires = DateTime.Now.AddDays(15);
                                        Response.Cookies.Add(cookie);

                                    }
                                    else
                                    {
                                        var clearCookie = new System.Web.HttpCookie("BrokerLogin");
                                        clearCookie.Expires = DateTime.Now.AddDays(-1);
                                        Response.Cookies.Add(clearCookie);
                                    }

                                    dsGetBrokerDetails.Tables[0].TableName = "UserDetails";
                                    dsGetBrokerDetails.Tables[1].TableName = "ExperienceDetails";
                                    dsGetBrokerDetails.Tables[2].TableName = "EducationDetails";
                                    dsGetBrokerDetails.AcceptChanges();

                                    string binData = dsGetBrokerDetails.Tables[0].Rows[0]["ProfilePicture"].ToString();
                                    if (binData != "")
                                    {
                                        binData = strDomainName + "" + strProfilePicForlderName + "" + dsGetBrokerDetails.Tables[0].Rows[0]["ProfilePicture"].ToString();

                                        dsGetBrokerDetails.Tables[0].Rows[0]["ProfilePicture"] = binData;
                                    }

                                    string ResumeData = dsGetBrokerDetails.Tables[0].Rows[0]["Resume"].ToString();
                                    if (ResumeData != "")
                                    {
                                        ResumeData = strDomainName + "" + strResumeForlderName + "" + dsGetBrokerDetails.Tables[0].Rows[0]["Resume"].ToString();

                                        dsGetBrokerDetails.Tables[0].Rows[0]["Resume"] = ResumeData;
                                    }

                                    ////////////////////////////////////////////////////////////
                                    string ProfilePicImg = dsGetBrokerDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString();
                                    if (ProfilePicImg != "")
                                    {
                                        ProfilePicImg = strDomainName + "" + strProfilePicImageFolder + "" + dsGetBrokerDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString();

                                        dsGetBrokerDetails.Tables[0].Rows[0]["ProfilePictureImg"] = ProfilePicImg + "?" + DateTime.Now;
                                    }

                                    string ResumeImg = dsGetBrokerDetails.Tables[0].Rows[0]["ResumeDoc"].ToString();
                                    if (ResumeImg != "")
                                    {
                                        ResumeImg = strDomainName + "" + strResumeImageFolder + "" + dsGetBrokerDetails.Tables[0].Rows[0]["ResumeDoc"].ToString(); ;

                                        dsGetBrokerDetails.Tables[0].Rows[0]["ResumeDoc"] = ResumeImg + "?" + DateTime.Now;
                                    }

                                    string CompanyLogo = dsGetBrokerDetails.Tables[0].Rows[0]["CompanyLogo"].ToString();
                                    if (CompanyLogo != "")
                                    {
                                        CompanyLogo = strDomainName + "" + strUploadedCompLogoFolder + "" + dsGetBrokerDetails.Tables[0].Rows[0]["CompanyLogo"].ToString();

                                        dsGetBrokerDetails.Tables[0].Rows[0]["CompanyLogo"] = CompanyLogo + "?" + DateTime.Now;
                                    }

                                    if (dsGetBrokerDetails.Tables["ExperienceDetails"].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dsGetBrokerDetails.Tables["ExperienceDetails"].Rows.Count; i++)
                                        {
                                            string Logo = dsGetBrokerDetails.Tables["ExperienceDetails"].Rows[i]["Logo"].ToString();
                                            if (Logo != "")
                                            {
                                                Logo = strDomainName + "" + strCompanyLogoFolder + "" + dsGetBrokerDetails.Tables["ExperienceDetails"].Rows[i]["Logo"].ToString(); ;

                                                dsGetBrokerDetails.Tables["ExperienceDetails"].Rows[i]["Logo"] = Logo + "?" + DateTime.Now;
                                            }
                                        }
                                    }

                                    /******************************Add Server path to School logo ********************************/
                                    if (dsGetBrokerDetails.Tables["EducationDetails"].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dsGetBrokerDetails.Tables["EducationDetails"].Rows.Count; i++)
                                        {
                                            string Logo = dsGetBrokerDetails.Tables["EducationDetails"].Rows[i]["EducationLogo"].ToString();
                                            if (Logo != "")
                                            {
                                                Logo = strDomainName + "" + strEducationLogoFolder + "" + dsGetBrokerDetails.Tables["EducationDetails"].Rows[i]["EducationLogo"].ToString(); ;

                                                dsGetBrokerDetails.Tables["EducationDetails"].Rows[i]["EducationLogo"] = Logo + "?" + DateTime.Now;
                                            }
                                        }
                                    }

                                    Session["BrokerData"] = dsGetBrokerDetails;

                                    return RedirectToAction("BrokerProfile", "Profile");
                                }
                            }
                            else
                            {
                                int result = (from S in DB.Users
                                              where S.EmailId == UserName && S.Password == encryptedPassword && S.UserType == "Customer" && S.IsActive == true
                                              select S).Count();

                                if (result > 0)
                                {
                                    ViewData["LoginFailCustomer"] = "You are registered as customer.";
                                }
                                else
                                {
                                    ViewData["LoginFail"] = "Login failed, please try again !";
                                }
                            }
                        }
                        else
                        {
                            int result = (from S in DB.Users
                                          where S.EmailId == UserName && S.Password == encryptedPassword && S.UserType == "Customer" && S.IsActive == true
                                          select S).Count();

                            if (result > 0)
                            {
                                ViewData["LoginFailCustomer"] = "You are registered as customer.";
                            }
                            else
                            {
                                ViewData["LoginFail"] = "Login failed, please try again !";
                            }

                        }
                    }
                    else
                    {
                        ViewData["LoginFail"] = "Login failed, please try again !";
                    }
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "BrokerLogin_Website", Ex.Message.ToString(), "LoginController.cs_BrokerLogin", "0");
                return View();
            }
            //}
            return View();
        }

        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CustomerLogin(LoginModel data)
        {
            string Initials = "N/A";
            try
            {
                if (ModelState.IsValid)
                {
                    //if (Request.Form["NEXT"] != null)
                    //{
                    //}

                    List<spCheckUserLogin_Result> ouser = null;

                    string UserName = "", Password = "", encryptedPassword = "";

                    if (data.Email != null)
                    {
                        UserName = data.Email.ToString();
                    }
                    if (data.Password != null)
                    {
                        Password = data.Password.ToString();
                    }
                    encryptedPassword = BrokerUtility.EncryptURL(Password);
                    // Session["UserId"] = "0";
                    ouser = BrokerWebDB.BrokerWebDB.ValidateUserDetails(data.Email, encryptedPassword);

                    if (ouser != null)
                    {
                        if (ouser.Count > 0)
                        {
                            bool? bUpdaate = ouser[0].IsUpdateProfile.HasValue ? ouser[0].IsUpdateProfile : false;
                            //if (bUpdaate == true)
                            //{
                            Session["UserId"] = ouser[0].UserId;
                            Session["EmailId"] = ouser[0].EmailId;

                            Session["FirstName"] = ouser[0].FirstName;
                            Session["LastName"] = ouser[0].LastName;
                            Session["ZipCode"] = ouser[0].PinCode;
                            Session["WebServiceURL"] = strWebServiceURL;
                            Session["IsAdmin"] = ouser[0].IsAdmin;
                            Session["UserType"] = ouser[0].UserType;

                            string IsAdmin = Session["IsAdmin"].ToString();

                            if (ouser[0].ProfilePictureImg != "" && ouser[0].ProfilePictureImg != null)
                            {
                                Session["ProfilePic"] = strDomainName + "" + strProfilePicImageFolder + "" + ouser[0].ProfilePictureImg + "?" + DateTime.Now;
                            }
                            else
                            {
                                Session["ProfilePic"] = "";
                            }

                            if (ouser[0].FirstName != "" && ouser[0].LastName != "")
                            {
                                Initials = ouser[0].FirstName[0] + "" + ouser[0].LastName[0];
                            }
                            Session["Initials"] = Initials.ToUpper();

                            CreateLoginIdentity(ouser[0].FirstName + ' ' + ouser[0].LastName, ouser[0].UserId);

                            //if (data.RememberMe)
                            //{
                            //    System.Web.HttpCookie cookie = new System.Web.HttpCookie("CustomerLogin");
                            //    cookie.Values.Add("EmailID", data.Email);
                            //    cookie.Values.Add("Password", data.Password);
                            //    cookie.Expires = DateTime.Now.AddDays(15);
                            //    Response.Cookies.Add(cookie);

                            //}
                            //else
                            //{
                            //    var clearCookie = new System.Web.HttpCookie("CustomerLogin");
                            //    clearCookie.Expires = DateTime.Now.AddDays(-1);
                            //    Response.Cookies.Add(clearCookie);
                            //}

                            if (ouser[0].RegisteredFor == "Brokkrr")
                            {
                                Session["Company"] = "Brokkrr";
                                Session["CompanyId"] = "1";
                                return RedirectToAction("CustomerProfile", "Profile");
                            }
                            else if (ouser[0].RegisteredFor == "Meineke")
                            {
                                Session["Company"] = "Meineke";
                                Session["CompanyId"] = "2";
                                return RedirectToAction("MeinekeCustomerProfile", "Profile");
                            }
                            else if (ouser[0].RegisteredFor == "APSP")
                            {
                                Session["Company"] = "APSP";
                                Session["CompanyId"] = "3";
                                return RedirectToAction("APSPCustomerProfile", "Profile");
                            }
                            //}
                            //else
                            //{
                            //    ViewData["LoginFail"] = "Login failed, please try again !";
                            //}
                        }
                    }
                    else
                    {
                        int result = (from S in DB.Users
                                      where S.EmailId == UserName && S.Password == encryptedPassword && S.UserType == "Broker" && S.IsActive == true
                                      select S).Count();

                        if (result > 0)
                        {
                            ViewData["LoginFailBroker"] = "You are registered as brokkrr.";
                        }
                        else
                        {
                            ViewData["LoginFail"] = "Login failed, please enter valid credentials !";
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "CustomerLogin_Website", Ex.Message.ToString(), "LoginController.cs_CustomerLogin", "0");
                return View();
            }
            //}
            return View();
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
                identity.AddClaim(new Claim(ClaimTypes.Role, "guest"));
                // tell OWIN the identity provider, optional
                // identity.AddClaim(new Claim(IdentityProvider, "Simplest Auth"));

                Authentication.SignIn(new AuthenticationProperties
                {
                    IsPersistent = false
                }, identity);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(Session["UserId"].ToString()), "CreateLoginIdentity_Website", ex.Message.ToString(), "LoginController.cs_CreateLoginIdentity()", BrokerUtility.GetIPAddress(Session["UserId"].ToString()));
            }
        }


        [Authorize]
        [HttpGet]
        public ActionResult BrokerLogout()
        {
            try
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                Session["UserId"] = null;
                Session["EmailId"] = null;
                Session["ProfilePic"] = null;
                Session["ReturnUrl"] = null;
                Session["UserType"] = null;

                Session["FirstName"] = null;
                Session["LastName"] = null;
                Session["ZipCode"] = null;
                Session["LinkedInLogInBy"] = null;
                Session["FacebookLogInBy"] = null;
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(Session["UserId"].ToString()), "CreateLoginIdentity", ex.Message.ToString(), "LoginController.cs_CreateLoginIdentity()", BrokerUtility.GetIPAddress(Session["UserId"].ToString()));
            }
            return RedirectToAction("BrokerLogin", "Login");
        }

        [Authorize]
        [HttpGet]
        public ActionResult CustomerLogout()
        {
            try
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                //var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

                Session["UserId"] = null;
                Session["EmailId"] = null;

                Session["FirstName"] = null;
                Session["LastName"] = null;
                Session["ZipCode"] = null;

                Session["WebServiceURL"] = null;
                Session["Initials"] = null;
                Session["ProfilePic"] = null;

                Session["LinkedInLogInBy"] = null;
                Session["FacebookLogInBy"] = null;
                Session["Company"] = null;
                Session["Location"] = null;
                Session["LineType"] = null;
                Session["FromPage"] = null;

                Session["ReturnUrl"] = null;

                Session["UserType"] = null;
                Session["CompanyId"] = null;
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(Session["UserId"].ToString()), "CreateLoginIdentity", ex.Message.ToString(), "LoginController.cs_CreateLoginIdentity()", BrokerUtility.GetIPAddress(Session["UserId"].ToString()));
            }
            return RedirectToAction("CustomerLogin", "Login");
        }

        IAuthenticationManager Authentication
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        public JsonResult ForgetPassword(string emailID)
        {
            //Boolean BrokerDetails = false;


            //BrokerDetails = BrokerUtility.ForgetPassword(emailID);
            //if (BrokerDetails)
            //{
            //    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            //}

            DataTable BrokerDetails = null;

            BrokerDetails = BrokerUtility.ForgetPassword(emailID);
            if (BrokerDetails.Rows.Count > 0)
            {
                string Message = BrokerDetails.Rows[0][0].ToString();

                if (Message == "Success")
                {
                    return Json(new { Success = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else if (Message == "Fail")
                {
                    return Json(new { Success = "Fail" }, JsonRequestBehavior.AllowGet);
                }
                else if (Message == "Invalid")
                {
                    return Json(new { Success = "Invalid" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Success = "Fail" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = "Fail" }, JsonRequestBehavior.AllowGet);

        }

        /************ For Log in with LinkedIn **********************************************************/

        [AllowAnonymous]
        [HttpGet]
        public ActionResult CustomerLoginWithLinkedIn()
        {
            ViewBag.SiteUrl = strDomainName;
            Session["LinkedInLogInBy"] = "Customer";
            return View();
            //return JavaScript("location.href('https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=75buq9yv6kgp1a&redirect_uri=http://localhost:59993/Login/LinkedINAuth&state=987654321&scope=r_basicprofile,r_emailaddress')");

            // return Content("https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=75buq9yv6kgp1a&redirect_uri=http://localhost:59993/Login/LinkedINAuth&state=987654321&scope=r_basicprofile,r_emailaddress");

        }

        public ActionResult BrokerLoginWithLinkedIn()
        {
            ViewBag.SiteUrl = strDomainName;
            Session["LinkedInLogInBy"] = "Broker";
            return View();
        }

        public ActionResult LinkedINAuth(string code, string state)
        {
            //This method path is your return URL  
            try
            {
                //Get Accedd Token  
                var client = new RestClient("https://www.linkedin.com/oauth/v2/accessToken");
                var request = new RestRequest(Method.POST);
                request.AddParameter("grant_type", "authorization_code");
                request.AddParameter("code", code);
                request.AddParameter("redirect_uri", strDomainName + "Login/LinkedINAuth");
                request.AddParameter("client_id", strLinkedIn_Client_Id);
                request.AddParameter("client_secret", strLinkedIn_Secrete_Id);
                IRestResponse response = client.Execute(request);
                string content = response.Content;

                var dict = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(content);
                var access_token = dict["access_token"];

                //Get Profile Details  

                client = new RestClient("https://api.linkedin.com/v1/people/~:(id,first-name,last-name,email-address)?oauth2_access_token=" + access_token + "&format=json");

                request = new RestRequest(Method.GET);
                response = client.Execute(request);
                content = response.Content;

                var UserDetails1 = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(content);

                var EmailId = UserDetails1["emailAddress"];
                var FirstName = UserDetails1["firstName"];
                var LastName = UserDetails1["lastName"];

                /************************************************************************************************/

                DataSet UserDetails = BrokerUtility.LoginExternal("LinkedIn", FirstName.ToString(), LastName.ToString(), EmailId.ToString(), Session["LinkedInLogInBy"].ToString(), "0", "");

                if (UserDetails.Tables.Count > 0)
                {
                    if (UserDetails.Tables[0].Rows.Count > 0)
                    {
                        if (UserDetails.Tables[0].Columns.Count > 1)
                        {
                            if (Session["LinkedInLogInBy"].ToString() == UserDetails.Tables[0].Rows[0]["UserType"].ToString())
                            {
                                if (Session["LinkedInLogInBy"].ToString() == "Customer")
                                {
                                    Session["UserId"] = UserDetails.Tables[0].Rows[0]["UserId"].ToString();
                                    Session["EmailId"] = UserDetails.Tables[0].Rows[0]["EmailId"].ToString();

                                    Session["FirstName"] = UserDetails.Tables[0].Rows[0]["FirstName"].ToString();
                                    Session["LastName"] = UserDetails.Tables[0].Rows[0]["LastName"].ToString();
                                    Session["ZipCode"] = UserDetails.Tables[0].Rows[0]["PinCode"].ToString();

                                    CreateLoginIdentity(Session["FirstName"].ToString() + ' ' + Session["LastName"].ToString(), Convert.ToInt32(Session["UserId"].ToString()));

                                    if (Convert.ToBoolean(UserDetails.Tables[0].Rows[0]["IsUpdateProfile"].ToString()) == false)
                                    {
                                        return RedirectToAction("Index", "CustomerRegistration");
                                    }
                                    else
                                    {
                                        return RedirectToAction("Index", "Insurances");
                                    }
                                }
                                else if (Session["LinkedInLogInBy"].ToString() == "Broker")
                                {
                                    Session["UserId"] = UserDetails.Tables[0].Rows[0]["UserId"].ToString();
                                    Session["EmailId"] = UserDetails.Tables[0].Rows[0]["EmailId"].ToString();

                                    Session["FirstName"] = UserDetails.Tables[0].Rows[0]["FirstName"].ToString();
                                    Session["LastName"] = UserDetails.Tables[0].Rows[0]["LastName"].ToString();
                                    Session["ZipCode"] = UserDetails.Tables[0].Rows[0]["PinCode"].ToString();

                                    CreateLoginIdentity(Session["FirstName"].ToString() + ' ' + Session["LastName"].ToString(), Convert.ToInt32(Session["UserId"].ToString()));

                                    if (Convert.ToBoolean(UserDetails.Tables[0].Rows[0]["IsUpdateProfile"].ToString()) == false)
                                    {
                                        Session["BrokerData"] = UserDetails;
                                        return RedirectToAction("Index", "BrokerRegistration");
                                    }
                                    else
                                    {
                                        UserDetails.Tables[0].TableName = "UserDetails";
                                        UserDetails.Tables[1].TableName = "ExperienceDetails";
                                        UserDetails.Tables[2].TableName = "EducationDetails";
                                        UserDetails.AcceptChanges();

                                        string binData = UserDetails.Tables[0].Rows[0]["ProfilePicture"].ToString();
                                        if (binData != "")
                                        {
                                            binData = strDomainName + "" + strProfilePicForlderName + "" + UserDetails.Tables[0].Rows[0]["ProfilePicture"].ToString();

                                            UserDetails.Tables[0].Rows[0]["ProfilePicture"] = binData;
                                        }

                                        string ResumeData = UserDetails.Tables[0].Rows[0]["Resume"].ToString();
                                        if (ResumeData != "")
                                        {
                                            ResumeData = strDomainName + "" + strResumeForlderName + "" + UserDetails.Tables[0].Rows[0]["Resume"].ToString();

                                            UserDetails.Tables[0].Rows[0]["Resume"] = ResumeData;
                                        }

                                        ////////////////////////////////////////////////////////////
                                        string ProfilePicImg = UserDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString();
                                        if (ProfilePicImg != "")
                                        {
                                            ProfilePicImg = strDomainName + "" + strProfilePicImageFolder + "" + UserDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString();

                                            UserDetails.Tables[0].Rows[0]["ProfilePictureImg"] = ProfilePicImg;
                                        }

                                        string ResumeImg = UserDetails.Tables[0].Rows[0]["ResumeDoc"].ToString();
                                        if (ResumeImg != "")
                                        {
                                            ResumeImg = strDomainName + "" + strResumeImageFolder + "" + UserDetails.Tables[0].Rows[0]["ResumeDoc"].ToString(); ;

                                            UserDetails.Tables[0].Rows[0]["ResumeDoc"] = ResumeImg;
                                        }

                                        string CompanyLogo = UserDetails.Tables[0].Rows[0]["CompanyLogo"].ToString();
                                        if (CompanyLogo != "")
                                        {
                                            CompanyLogo = strDomainName + "" + strUploadedCompLogoFolder + "" + UserDetails.Tables[0].Rows[0]["CompanyLogo"].ToString();

                                            UserDetails.Tables[0].Rows[0]["CompanyLogo"] = CompanyLogo;
                                        }

                                        if (UserDetails.Tables["ExperienceDetails"].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < UserDetails.Tables["ExperienceDetails"].Rows.Count; i++)
                                            {
                                                string Logo = UserDetails.Tables["ExperienceDetails"].Rows[i]["Logo"].ToString();
                                                if (Logo != "")
                                                {
                                                    Logo = strDomainName + "" + strCompanyLogoFolder + "" + UserDetails.Tables["ExperienceDetails"].Rows[i]["Logo"].ToString(); ;

                                                    UserDetails.Tables["ExperienceDetails"].Rows[i]["Logo"] = Logo;
                                                }
                                            }
                                        }

                                        /******************************Add Server path to School logo ********************************/
                                        if (UserDetails.Tables["EducationDetails"].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < UserDetails.Tables["EducationDetails"].Rows.Count; i++)
                                            {
                                                string Logo = UserDetails.Tables["EducationDetails"].Rows[i]["EducationLogo"].ToString();
                                                if (Logo != "")
                                                {
                                                    Logo = strDomainName + "" + strEducationLogoFolder + "" + UserDetails.Tables["EducationDetails"].Rows[i]["EducationLogo"].ToString(); ;

                                                    UserDetails.Tables["EducationDetails"].Rows[i]["EducationLogo"] = Logo;
                                                }
                                            }
                                        }

                                        Session["BrokerData"] = UserDetails;
                                        /**************************************************************************************/
                                        return RedirectToAction("BrokerProfile", "Profile");
                                    }
                                }
                            }
                            else
                            {
                                if (Session["LinkedInLogInBy"].ToString() == "Customer")
                                {
                                    return RedirectToAction("CustomerLogin", "Login");
                                }
                                else if (Session["LinkedInLogInBy"].ToString() == "Broker")
                                {
                                    return RedirectToAction("BrokerLogin", "Login");
                                }
                            }
                        }
                        else //If user already registered with another User Type
                        {
                            if (Session["LinkedInLogInBy"].ToString() == "Customer")
                            {
                                //@ViewData["LoginFail"] = UserDetails.Tables[0].Rows[0][0].ToString();
                                Session["LoginFail"] = UserDetails.Tables[0].Rows[0][0].ToString();
                                return RedirectToAction("CustomerLogin", "Login");
                            }
                            else if (Session["LinkedInLogInBy"].ToString() == "Broker")
                            {
                                //@ViewData["LoginFail"] = UserDetails.Tables[0].Rows[0][0].ToString();
                                Session["LoginFail"] = UserDetails.Tables[0].Rows[0][0].ToString();
                                return RedirectToAction("BrokerLogin", "Login");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(0, "LinkedINAuth_Website", ex.Message.ToString(), "LoginController.cs_LinkedINAuth()", "");
            }
            return View("CustomerLogin");
        }

        /************ For Log in with Facebook **********************************************************/
        [AllowAnonymous]
        public ActionResult CustomerLoginWithFacebook()
        {
            Session["FacebookLogInBy"] = "Customer";
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = strFacebook_Client_Id,
                client_secret = strFacebook_Secrete_Id,
                redirect_uri = RedirectUri.AbsoluteUri,

                response_type = "code",
                scope = "email" // Add other permissions as needed
            });

            return Redirect(loginUrl.AbsoluteUri);

        }

        [AllowAnonymous]
        public ActionResult BrokerLoginWithFacebook()
        {
            Session["FacebookLogInBy"] = "Broker";
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = strFacebook_Client_Id,
                client_secret = strFacebook_Secrete_Id,
                redirect_uri = RedirectUri.AbsoluteUri,

                response_type = "code",
                scope = "email" // Add other permissions as needed
            });

            return Redirect(loginUrl.AbsoluteUri);

        }

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }


        public ActionResult FacebookCallback(string code)
        {
            try
            {
                var fb = new FacebookClient();
                dynamic result = fb.Post("oauth/access_token", new
                         {
                             client_id = strFacebook_Client_Id,
                             client_secret = strFacebook_Secrete_Id,
                             redirect_uri = RedirectUri.AbsoluteUri,

                             code = code
                         });

                var accessToken = result.access_token;

                // Store the access token in the session for farther use
                //Session["AccessToken"] = accessToken;

                // update the facebook client with the access token so 
                // we can make requests on behalf of the user
                fb.AccessToken = accessToken;

                // Get the user's information
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;

                // Set the auth cookie
                //  FormsAuthentication.SetAuthCookie(email, false);

                /************************************************************************************************/

                DataSet UserDetails = BrokerUtility.LoginExternal("Facebook", firstname, lastname, email, Session["FacebookLogInBy"].ToString(), "0", "");//22Feb18

                if (UserDetails.Tables.Count > 0)
                {
                    if (UserDetails.Tables[0].Rows.Count > 0)
                    {
                        if (UserDetails.Tables[0].Columns.Count > 1)
                        {
                            if (Session["FacebookLogInBy"].ToString() == UserDetails.Tables[0].Rows[0]["UserType"].ToString())
                            {
                                if (Session["FacebookLogInBy"].ToString() == "Customer")
                                {
                                    Session["UserId"] = UserDetails.Tables[0].Rows[0]["UserId"].ToString();
                                    Session["EmailId"] = UserDetails.Tables[0].Rows[0]["EmailId"].ToString();

                                    Session["FirstName"] = UserDetails.Tables[0].Rows[0]["FirstName"].ToString();
                                    Session["LastName"] = UserDetails.Tables[0].Rows[0]["LastName"].ToString();
                                    Session["ZipCode"] = UserDetails.Tables[0].Rows[0]["PinCode"].ToString();

                                    CreateLoginIdentity(Session["FirstName"].ToString() + ' ' + Session["LastName"].ToString(), Convert.ToInt32(Session["UserId"].ToString()));

                                    if (Convert.ToBoolean(UserDetails.Tables[0].Rows[0]["IsUpdateProfile"].ToString()) == false)
                                    {
                                        return RedirectToAction("Index", "CustomerRegistration");
                                    }
                                    else
                                    {
                                        return RedirectToAction("Index", "Insurances");
                                    }
                                }
                                else if (Session["FacebookLogInBy"].ToString() == "Broker")
                                {
                                    Session["UserId"] = UserDetails.Tables[0].Rows[0]["UserId"].ToString();
                                    Session["EmailId"] = UserDetails.Tables[0].Rows[0]["EmailId"].ToString();

                                    Session["FirstName"] = UserDetails.Tables[0].Rows[0]["FirstName"].ToString();
                                    Session["LastName"] = UserDetails.Tables[0].Rows[0]["LastName"].ToString();
                                    Session["ZipCode"] = UserDetails.Tables[0].Rows[0]["PinCode"].ToString();

                                    CreateLoginIdentity(Session["FirstName"].ToString() + ' ' + Session["LastName"].ToString(), Convert.ToInt32(Session["UserId"].ToString()));

                                    if (Convert.ToBoolean(UserDetails.Tables[0].Rows[0]["IsUpdateProfile"].ToString()) == false)
                                    {
                                        Session["BrokerData"] = UserDetails;
                                        return RedirectToAction("Index", "BrokerRegistration");
                                    }
                                    else
                                    {
                                        UserDetails.Tables[0].TableName = "UserDetails";
                                        UserDetails.Tables[1].TableName = "ExperienceDetails";
                                        UserDetails.Tables[2].TableName = "EducationDetails";
                                        UserDetails.AcceptChanges();

                                        string binData = UserDetails.Tables[0].Rows[0]["ProfilePicture"].ToString();
                                        if (binData != "")
                                        {
                                            binData = strDomainName + "" + strProfilePicForlderName + "" + UserDetails.Tables[0].Rows[0]["ProfilePicture"].ToString();

                                            UserDetails.Tables[0].Rows[0]["ProfilePicture"] = binData;
                                        }

                                        string ResumeData = UserDetails.Tables[0].Rows[0]["Resume"].ToString();
                                        if (ResumeData != "")
                                        {
                                            ResumeData = strDomainName + "" + strResumeForlderName + "" + UserDetails.Tables[0].Rows[0]["Resume"].ToString();

                                            UserDetails.Tables[0].Rows[0]["Resume"] = ResumeData;
                                        }

                                        ////////////////////////////////////////////////////////////
                                        string ProfilePicImg = UserDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString();
                                        if (ProfilePicImg != "")
                                        {
                                            ProfilePicImg = strDomainName + "" + strProfilePicImageFolder + "" + UserDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString();

                                            UserDetails.Tables[0].Rows[0]["ProfilePictureImg"] = ProfilePicImg;
                                        }

                                        string ResumeImg = UserDetails.Tables[0].Rows[0]["ResumeDoc"].ToString();
                                        if (ResumeImg != "")
                                        {
                                            ResumeImg = strDomainName + "" + strResumeImageFolder + "" + UserDetails.Tables[0].Rows[0]["ResumeDoc"].ToString(); ;

                                            UserDetails.Tables[0].Rows[0]["ResumeDoc"] = ResumeImg;
                                        }

                                        string CompanyLogo = UserDetails.Tables[0].Rows[0]["CompanyLogo"].ToString();
                                        if (CompanyLogo != "")
                                        {
                                            CompanyLogo = strDomainName + "" + strUploadedCompLogoFolder + "" + UserDetails.Tables[0].Rows[0]["CompanyLogo"].ToString();

                                            UserDetails.Tables[0].Rows[0]["CompanyLogo"] = CompanyLogo;
                                        }

                                        if (UserDetails.Tables["ExperienceDetails"].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < UserDetails.Tables["ExperienceDetails"].Rows.Count; i++)
                                            {
                                                string Logo = UserDetails.Tables["ExperienceDetails"].Rows[i]["Logo"].ToString();
                                                if (Logo != "")
                                                {
                                                    Logo = strDomainName + "" + strCompanyLogoFolder + "" + UserDetails.Tables["ExperienceDetails"].Rows[i]["Logo"].ToString(); ;

                                                    UserDetails.Tables["ExperienceDetails"].Rows[i]["Logo"] = Logo;
                                                }
                                            }
                                        }

                                        /******************************Add Server path to School logo ********************************/
                                        if (UserDetails.Tables["EducationDetails"].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < UserDetails.Tables["EducationDetails"].Rows.Count; i++)
                                            {
                                                string Logo = UserDetails.Tables["EducationDetails"].Rows[i]["EducationLogo"].ToString();
                                                if (Logo != "")
                                                {
                                                    Logo = strDomainName + "" + strEducationLogoFolder + "" + UserDetails.Tables["EducationDetails"].Rows[i]["EducationLogo"].ToString(); ;

                                                    UserDetails.Tables["EducationDetails"].Rows[i]["EducationLogo"] = Logo;
                                                }
                                            }
                                        }

                                        Session["BrokerData"] = UserDetails;
                                        /**************************************************************************************/
                                        return RedirectToAction("BrokerProfile", "Profile");
                                    }
                                }
                            }
                            else
                            {
                                if (Session["FacebookLogInBy"].ToString() == "Customer")
                                {
                                    return RedirectToAction("CustomerLogin", "Login");
                                }
                                else if (Session["FacebookLogInBy"].ToString() == "Broker")
                                {
                                    return RedirectToAction("BrokerLogin", "Login");
                                }
                            }
                        }
                        else //If user already registered with another User Type
                        {
                            if (Session["FacebookLogInBy"].ToString() == "Customer")
                            {
                                //@ViewData["LoginFail"] = UserDetails.Tables[0].Rows[0][0].ToString();
                                Session["LoginFail"] = UserDetails.Tables[0].Rows[0][0].ToString();
                                return RedirectToAction("CustomerLogin", "Login");
                            }
                            else if (Session["FacebookLogInBy"].ToString() == "Broker")
                            {
                                //@ViewData["LoginFail"] = UserDetails.Tables[0].Rows[0][0].ToString();
                                Session["LoginFail"] = UserDetails.Tables[0].Rows[0][0].ToString();
                                return RedirectToAction("BrokerLogin", "Login");
                            }
                        }
                    }
                }
                else
                {
                    if (Session["FacebookLogInBy"].ToString() == "Customer")
                    {
                        return RedirectToAction("CustomerLogin", "Login");
                    }
                    else if (Session["FacebookLogInBy"].ToString() == "Broker")
                    {
                        return RedirectToAction("BrokerLogin", "Login");
                    }
                }
                return RedirectToAction("CustomerLogin", "Login");
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "FacebookCallback_Website", Ex.Message.ToString(), "LoginController.cs_FacebookCallback", "0");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}