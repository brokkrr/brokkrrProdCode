using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrokerMVC.Models;
using BrokerMVC.App_Code;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using System.Data;
using System.Configuration;
using BrokerMVC.BrokerService;
using System.Xml;

namespace BrokerMVC.Controllers
{
    public class HomeController : Controller
    {
        public static string strServerLink = ConfigurationManager.AppSettings["ServerLink"].ToString();
        public static string strEmailVerifyMessage = ConfigurationManager.AppSettings["EmailVerifyMessage"].ToString();

        public static string striPhoneAppPath = ConfigurationManager.AppSettings["iPhoneAppPath"].ToString();
        public static string strAndroidAppPath = ConfigurationManager.AppSettings["AndroidAppPath"].ToString();

        public static string strGoogleMapKey = ConfigurationManager.AppSettings["GoogleMapKey"].ToString();

        // GET: Home
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            clearSessionVariable();
            //string userid = User.Identity.GetUserId();
            ViewBag.iPhoneAppPath = striPhoneAppPath;
            ViewBag.AndroidAppPath = strAndroidAppPath;
            ViewBag.GoogleMapKey = strGoogleMapKey;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult MeinekeIndex()
        {
            Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            clearSessionVariable();
            //string userid = User.Identity.GetUserId();
            ViewBag.iPhoneAppPath = striPhoneAppPath;
            ViewBag.AndroidAppPath = strAndroidAppPath;
            return View();
        }

        //Clear previous session variable if available

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

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(GetZipWithCityName model)
        {
            //if (Request.Form["Brokkrr"] != null)
            //{
            if (ModelState.IsValid)
            {
                Session["Location"] = model;
                Session["Company"] = "Brokkrr";
                Session["FromPage"] = "Index";
                ViewBag.IsAuthorize = "UnAuthorize";               

                //if(model.Company=="Brokkrr")
                //{
                //    return RedirectToAction("Insurance", "Insurances");
                //}
                //else if (model.Company == "Meineke")
                //{
                //    return RedirectToAction("MeinekeInsurance", "Insurances");
                //}
                return RedirectToAction("Insurance", "Insurances");

            }
            //}
            //else if (Request.Form["Meineke"] != null)
            //{
            //    if (ModelState.IsValid)
            //    {
            //        Session["Location"] = model;
            //        ViewBag.IsAuthorize = "UnAuthorize";
            //        Session["Company"] = "Meineke";
            //        return RedirectToAction("MeinekeInsurance", "Insurances");
            //    }
            //}


            return View();

        }


        [HttpPost]
        [AllowAnonymous]
        public JsonResult JsonIndex(GetZipWithCityName model)
        {

            if (ModelState.IsValid)
            {
                Session["Location"] = model;
                Session["Company"] = "Brokkrr";
                ViewBag.IsAuthorize = "UnAuthorize";


                //return RedirectToAction("Insurance", "Insurances");
            }


            return Json(new { result = "Success" }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult MeinekeIndex(GetZipWithCityName model)
        {
            if (Request.Form["Brokkrr"] != null)
            {
                if (ModelState.IsValid)
                {
                    Session["Location"] = model;
                    Session["Company"] = "Brokkrr";
                    ViewBag.IsAuthorize = "UnAuthorize";
                    return RedirectToAction("Insurance", "Insurances");
                }
            }
            else if (Request.Form["Meineke"] != null)
            {
                if (ModelState.IsValid)
                {
                    Session["Location"] = model;
                    ViewBag.IsAuthorize = "UnAuthorize";
                    Session["Company"] = "Meineke";
                    return RedirectToAction("MeinekeInsurance", "Insurances");
                }
            }


            return View();

        }

        [HttpGet]
        [Authorize]
        public ActionResult HomePage()
        {
            try
            {
                ViewBag.UserId = Session["UserId"].ToString();
                ViewBag.IsAuthorize = "Authorize";
                ViewBag.Initials = Session["Initials"];
                ViewBag.Name = Session["FirstName"] + " " + Session["LastName"];
                ViewBag.GoogleMapKey = strGoogleMapKey;
            }
            catch (Exception Ex)
            {

            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult MeinekeHomePage()
        {
            try
            {
                ViewBag.UserId = Session["UserId"].ToString();
                ViewBag.IsAuthorize = "Authorize";
                ViewBag.Initials = Session["Initials"];
                ViewBag.Name = Session["FirstName"] + " " + Session["LastName"];
                ViewBag.GoogleMapKey = strGoogleMapKey;
            }
            catch (Exception Ex)
            {

            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult HomePage(GetZipWithCityName model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Session["Location"] = model;
                    //ViewBag.Company = "Brokkrr";
                    return RedirectToAction("Insurance", "Insurances");
                }
                ViewBag.Initials = Session["Initials"];

            }
            catch (Exception Ex)
            {

            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult MeinekeHomePage(GetZipWithCityName model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Session["Location"] = model;
                    //ViewBag.Company = "Meineke";
                    return RedirectToAction("MeinekeInsurance", "Insurances");
                }
                ViewBag.Initials = Session["Initials"];

            }
            catch (Exception Ex)
            {

            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult UnderConstruction()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult BrokerRegistration()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult VerifyEmail(string EmailId, string RegistrationCode)
        {
            try
            {
                DataSet dsResult = null;
                string DecryptEmail = BrokerUtility.DecryptURL(EmailId);
                Session["DecryptedEmailId"] = DecryptEmail;
                DataSet dsValidEmail = BrokerWSUtility.CheckForValidEmailId(DecryptEmail);

                //Check for Customer Email verification
                if (Convert.ToBoolean(dsValidEmail.Tables[0].Rows[0]["IsEmailIdVerified"].ToString()) == false && dsValidEmail.Tables[0].Rows[0]["UserType"].ToString() == "Customer")
                {
                    DataSet ds = new DataSet();
                    ds = BrokerWebDB.BrokerWebDB.CheckForValidUserDetails(Session["DecryptedEmailId"].ToString(), RegistrationCode);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return View();
                        }
                        else
                        {
                            ViewData["EmailVerifyMessage"] = "Email verification link is expired. Please try again";
                            ViewData["Login"] = "";
                            return View("VerifyEmailFail");
                        }
                    }
                    else
                    {
                        ViewData["EmailVerifyMessage"] = "Email verification link is expired. Please try again";
                        ViewData["Login"] = "";
                        return View("VerifyEmailFail");
                    }

                }
                //Check for Broker Email verification
                else if (Convert.ToBoolean(dsValidEmail.Tables[0].Rows[0]["IsEmailIdVerified"].ToString()) == false && dsValidEmail.Tables[0].Rows[0]["UserType"].ToString() == "Broker")
                {
                    dsResult = BrokerWSUtility.VerifyUserDetails(DecryptEmail, RegistrationCode);

                    if (dsResult.Tables.Count > 0)
                    {
                        ViewData["EmailVerifyMessage"] = strEmailVerifyMessage;

                        ViewData["LoginUrl"] = "BrokerLogin";
                        return View("VerifyEmailSuccess");
                    }

                    else
                    {
                        ViewData["EmailVerifyMessage"] = "Email verification link is expired. Please try again";
                        ViewData["Login"] = "";
                        return View("VerifyEmailFail");
                    }
                }
                //If both Customer and Broker are already verified.
                else
                {
                    ViewData["EmailVerifyMessage"] = "Your email address is already verified.";
                    if (dsValidEmail.Tables[0].Rows[0]["UserType"].ToString() == "Customer")
                    {
                        ViewData["Login"] = "LoginCustomer";
                    }
                    else
                    {
                        ViewData["Login"] = "LoginBroker";
                    }
                    return View("VerifyEmailFail");
                }
            }
            catch (Exception Ex)
            {
                ViewData["EmailVerifyMessage"] = "Email Verification Failed. Please try again";
                ViewData["Login"] = "";
                return View("VerifyEmailFail");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult VerifyEmail(ResetPassword Info)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string Password = "", TempPass = "";

                    if (Info.Password != "" && Info.Password != null)
                    {
                        Password = Info.Password.ToString();

                        TempPass = BrokerUtility.EncryptURL(Password);

                        int Flag = BrokerWSUtility.CreateNewPassword(Session["DecryptedEmailId"].ToString(), TempPass);
                        if (Flag > 0)
                        {
                            ViewData["EmailVerifyMessage"] = strEmailVerifyMessage;

                            ViewData["LoginUrl"] = "CustomerLogin";
                            return View("VerifyEmailSuccess");
                        }
                        else
                        {
                            ViewData["EmailVerifyMessage"] = "Email verification link is expired. Please try again";
                            ViewData["Login"] = "";
                            return View("VerifyEmailFail");
                        }
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    return View();
                }

            }
            catch (Exception Ex)
            {
                ViewData["EmailVerifyMessage"] = "Email Verification Failed. Please try again";
                ViewData["Login"] = "";
                return View("VerifyEmailFail");
            }

            //return View("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult VerifyEmailForApp(string EmailId, string RegistrationCode)
        {
            DataSet dsResult = null;
            DataSet dsValidEmail = null;
            try
            {

                string DecryptEmail = BrokerUtility.DecryptURL(EmailId);

                dsValidEmail = BrokerWSUtility.CheckForValidEmailId(DecryptEmail);

                if (Convert.ToBoolean(dsValidEmail.Tables[0].Rows[0]["IsEmailIdVerified"].ToString()) == false)
                {
                    dsResult = BrokerWSUtility.VerifyUserDetails(DecryptEmail, RegistrationCode);

                    if (dsResult.Tables.Count > 0)
                    {
                        ViewData["EmailVerifyMessage"] = strEmailVerifyMessage;
                        if (dsResult.Tables[0].Rows[0]["UserType"].ToString() == "Customer")
                        {
                            //ViewData["LoginUrl"] = "CustomerLogin";
                            ViewData["Login"] = "LoginCustomer";
                        }
                        else
                        {
                            //ViewData["LoginUrl"] = "BrokerLogin";
                            ViewData["Login"] = "LoginBroker";
                        }
                        return View();
                    }
                    else
                    {
                        ViewData["EmailVerifyMessage"] = "Email Verification Failed. Please try again";
                        ViewData["Login"] = "";
                        return View("VerifyEmailFail");
                    }
                }
                else
                {
                    ViewData["EmailVerifyMessage"] = "Your email address is already verified.";
                    if (dsValidEmail.Tables[0].Rows[0]["IsEmailIdVerified"].ToString() == "Customer")
                    {
                        ViewData["Login"] = "LoginCustomer";
                    }
                    else
                    {
                        ViewData["Login"] = "LoginBroker";
                    }
                    return View("VerifyEmailFail");
                }
            }
            catch (Exception Ex)
            {
                ViewData["EmailVerifyMessage"] = "Email Verification Failed. Please try again";
                ViewData["Login"] = "";
                return View("VerifyEmailFail");
            }

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string EmailId, string Code)
        {
            try
            {
                string DecryptedEmailId = BrokerUtility.DecryptURL(EmailId);

                Session["EmailId"] = DecryptedEmailId;

                DataSet dsValidEmail = BrokerWSUtility.CheckForValidEmailId(DecryptedEmailId);

                if (dsValidEmail.Tables.Count > 0)
                {
                    if (dsValidEmail.Tables[0].Rows.Count > 0)
                    {
                        if (dsValidEmail.Tables[0].Rows[0][0].ToString() != "0")
                        {
                            if (dsValidEmail.Tables[0].Rows[0]["ForgetPasswordRanNum"].ToString() != "")
                            {
                                if (dsValidEmail.Tables[0].Rows[0]["ForgetPasswordRanNum"].ToString() == Code)
                                {
                                    Session["ResetPasswordUserType"] = dsValidEmail.Tables[0].Rows[0]["UserType"].ToString();

                                    return View();
                                }
                                else
                                {
                                    ViewBag.Message = "Password could not reset. Please try again";
                                    return View("ResetResult");
                                }
                            }
                            else
                            {
                                ViewBag.Message = "Reset password link expired.";
                                return View("ResetResult");
                            }
                        }
                        else
                        {
                            ViewBag.Message = "Password could not reset. Please try again";
                            return View("ResetResult");
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Password could not reset. Please try again";
                        return View("ResetResult");
                    }

                }
                else
                {
                    ViewBag.Message = "Password could not reset. Please try again";
                    return View("ResetResult");
                }
            }
            catch (Exception Ex)
            {
                ViewBag.Message = "Password could not reset. Please try again";
                return View("ResetResult");
            }

        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassword(ResetPassword Info)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string Password = "", TempPass = "";

                    if (Info.Password != "" && Info.Password != null)
                    {
                        Password = Info.Password.ToString();

                        TempPass = BrokerUtility.EncryptURL(Password);

                        int Flag = BrokerUtility.ResetPassWord(Session["EmailId"].ToString(), TempPass);
                        if (Flag > 0)
                        {
                            int Flag1 = BrokerWSUtility.ForgetPasswordRanNum(Session["EmailId"].ToString(), "");
                            string UserType = Session["ResetPasswordUserType"].ToString();

                            if (UserType == "Customer")
                            {
                                ViewBag.LoginUrl = "CustomerLogin";
                            }
                            else if (UserType == "Broker")
                            {
                                ViewBag.LoginUrl = "BrokerLogin";
                            }

                            ViewBag.Message = "Your password has been reset successfully.";

                            return View("ResetResult");
                        }
                        else
                        {
                            ViewBag.Message = "Password could not reset. Please try again. ";
                            return View("ResetResult");
                        }
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    return View();
                }

            }
            catch (Exception Ex)
            {
                ViewBag.Message = "Password could not reset. Please try again. ";
                return View("ResetResult");
            }
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public JsonResult Index(ContactUs details)
        //{
        //    bool EmailFlag = false;
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            string Name = "", EmailId = "", Subject = "", Message = "";

        //            if (details.Name != null)
        //            {
        //                Name = details.Name.ToString();
        //            }
        //            if (details.Email != null)
        //            {
        //                EmailId = details.Email.ToString();
        //            }
        //            if (details.Subject != null)
        //            {
        //                Subject = details.Subject.ToString();
        //            }
        //            if (details.Message != null)
        //            {
        //                Message = details.Message.ToString();
        //            }


        //            //send mail from user.
        //            EmailFlag = BrokerUtility.SendContactUsMail(Name, EmailId, Subject, Message);
        //            //EmailFlag = true;
        //            if (EmailFlag == true)
        //            {
        //                ViewBag.message = "Thank you,Your contact request sent successfully !";
        //                ModelState.Clear();
        //                return Json(new { Success = true });

        //            }
        //            else
        //            {
        //                ViewBag.message = "Error occured while sending mail.";
        //                ModelState.Clear();
        //                return Json(new { Success = false });
        //            }

        //        }
        //    }
        //    catch (Exception Ex)
        //    {

        //    }
        //    if (EmailFlag == true)
        //    {
        //        return Json(new { Success = true });
        //    }
        //    else
        //    {
        //        return Json(new { Success = false });
        //    }
        //}


        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult UpdateProfileView()
        {
            UpdateModel UpdateModel = new UpdateModel();
            List<spGetUserDetails_Result> oUserDetails = null;
            if (Session["UserId"] != null)
            {
                int iUserId = Convert.ToInt32(Session["UserId"]);
                oUserDetails = BrokerUtility.GetUserDetails(iUserId);
                UpdateModel.FirstName = oUserDetails[0].FirstName;
                UpdateModel.LastName = oUserDetails[0].LastName;
            }
            return View(UpdateModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult UpdateProfileView(UpdateModel data, FormCollection val)
        {
            int user;
            try
            {
                if (ModelState.IsValid)
                {
                    string encryptedPassword = BrokerUtility.EncryptURL(data.Password);
                    int? state = data.StateId;
                    int? country = data.CountryId;
                    user = BrokerUtility.UpdateUser(Session["UserId"].ToString(), encryptedPassword, data.Address, data.City, state, country, data.PinCode, data.MobNo, "1", data.UserType, "1");
                    if (user > 0)
                    {
                        CreateLoginIdentity(data.FirstName + ' ' + data.LastName, Convert.ToInt32(Session["UserId"].ToString()));
                        return RedirectToAction("Index", "Home");
                    }

                    else
                    {
                        ViewBag.UserExist = "Error occured while updating your profile.";
                    }
                }
                else
                {
                    ViewBag.validateerror = "Please validate all input";
                }
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(Session["UserId"].ToString()), "UpdateProfileView", ex.Message.ToString(), "HomeController.cs_UpdateProfileView()", BrokerUtility.GetIPAddress(Session["UserId"].ToString()));
            }
            return View(data);

            //return View(model);
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
                BrokerUtility.ErrorLog(Convert.ToInt32(Session["UserId"].ToString()), "UpdateProfileView", ex.Message.ToString(), "HomeController.cs_UpdateProfileView()", BrokerUtility.GetIPAddress(Session["UserId"].ToString()));
            }
        }

        IAuthenticationManager Authentication
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult BrokerPayment()
        {
            return RedirectToAction("Index", "BrokerPayment");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult UpdatePayment()
        {
            return RedirectToAction("Index", "ViewPayment");
        }

        public ActionResult SystemError()
        {
            try
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                //var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

                if (Session["UserId"] != null)
                {
                    Session["UserId"] = null;
                }

                if (Session["EmailId"] != null)
                {
                    Session["EmailId"] = null;
                }

                if (Session["FirstName"] != null)
                {
                    Session["FirstName"] = null;
                }

                if (Session["LastName"] != null)
                {
                    Session["LastName"] = null;
                }

                if (Session["ZipCode"] != null)
                {
                    Session["ZipCode"] = null;
                }

                if (Session["LinkedInLogInBy"] != null)
                {
                    Session["LinkedInLogInBy"] = null;
                }

                if (Session["FacebookLogInBy"] != null)
                {
                    Session["FacebookLogInBy"] = null;
                }
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(Session["UserId"].ToString()), "CreateLoginIdentity", ex.Message.ToString(), "LoginController.cs_CreateLoginIdentity()", BrokerUtility.GetIPAddress(Session["UserId"].ToString()));
            }

            return RedirectToAction("Index", "Home");
        }

        //12Sept2017
        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetSearchedZipCodes(string Prefix)
        {
            BrokerDBEntities db = new BrokerDBEntities();
            int UserId = Convert.ToInt32(Session["UserId"]);
            var ZipCodes = (from c in db.UserSerchedZipCodes
                            where c.ZipCode.StartsWith(Prefix) && c.UserId == UserId
                            select new { c.ZipCode, c.Id });
            return Json(ZipCodes, JsonRequestBehavior.AllowGet);
        }

        //05Oct17
        [AllowAnonymous]
        [HttpGet]
        public ActionResult SendEmailForRating()
        {

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GiveRating(string CustMsgId)
        {
            try
            {
                string[] CustMsgIdForRating = null;


                if (CustMsgId != "")
                {


                    CustMsgIdForRating = CustMsgId.Split(',');

                    foreach (string Id in CustMsgIdForRating)
                    {

                    }
                }
                else //if CustMsgId=""
                {

                }

                return View();
            }
            catch
            {
                return View();
            }
        }


        [HttpGet]
        [Authorize]
        public ActionResult APSPHomePage()
        {
            try
            {
                ViewBag.UserId = Session["UserId"].ToString();
                ViewBag.IsAuthorize = "Authorize";
                ViewBag.Initials = Session["Initials"];
                ViewBag.Name = Session["FirstName"] + " " + Session["LastName"];

                ViewBag.GoogleMapKey = strGoogleMapKey;

            }
            catch (Exception Ex)
            {

            }
            return View();
        }


        [HttpPost]
        [Authorize]
        public ActionResult APSPHomePage(GetZipWithCityName model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Session["Location"] = model;
                    //ViewBag.Company = "Meineke";
                    return RedirectToAction("APSPInsurance", "Insurances");
                }
                ViewBag.Initials = Session["Initials"];

            }
            catch (Exception Ex)
            {

            }
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult TermsAndConditions()
        {

            return View();
        }      

        [HttpGet]
        [AllowAnonymous]
        public ActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}