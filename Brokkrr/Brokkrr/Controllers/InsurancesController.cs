using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrokerMVC.Models;
using BrokerMVC.App_Code;
using System.Data;
using BrokerMVC.BrokerService;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Configuration;
using BrokerMVC.BrokerWebDB;
using System.IO;

using System.Security.Authentication;
using Microsoft.AspNet.Identity.Owin;
using System.Xml;

namespace BrokerMVC.Controllers
{
    public class InsurancesController : Controller
    {
        public static string strWebServiceURL = ConfigurationManager.AppSettings["WebServiceURL"].ToString();
        public static string strDomainName = ConfigurationManager.AppSettings["DomainName"].ToString();
        public static string strDeclarationDoc = ConfigurationManager.AppSettings["DeclarationDocumentFolder"].ToString();

        public static string strGoogleMapKey = ConfigurationManager.AppSettings["GoogleMapKey"].ToString();

        // GET: Insurances

        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            ViewData["UserName"] = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult HomeInsurance()
        {
            if (Session["ZipCode"] != null)
            {
                ViewBag.ZipCode = Session["ZipCode"].ToString();
            }
            else
            {
                ViewBag.ZipCode = "";
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult HomeInsurance(HomeInsurance model)
        {
            if (model.IsInsured == null || model.IsInsured != "Currently insured")
            {
                this.ModelState.Remove("InsuranceCompany");
            }

            if (ModelState.IsValid)
            {
                DataSet BrokerDetails = null;
                BrokerDetails = BrokerUtility.SaveHomeInsuranceDetails(model);

                Session["SearchResult"] = BrokerDetails;

                Session["InsuranceType"] = "Home";
                return RedirectToAction("SearchBroker");
            }
            else
            {
                return View(model);
            }
            return RedirectToAction("SearchBroker");
        }

        [AllowAnonymous]//21Sep17 san
        public JsonResult GetCompany()
        {
            string jsonstring = "";

            jsonstring = BrokerUtility.DoGetCompanyMaster();

            return Json(jsonstring, JsonRequestBehavior.AllowGet);
            // return View();
        }


        [HttpGet]
        [Authorize]
        public ActionResult AutoInsurance()
        {
            if (Session["ZipCode"] != null)
            {
                ViewBag.ZipCode = Session["ZipCode"].ToString();
            }
            else
            {
                ViewBag.ZipCode = "";
            }

            return View();
        }

        [Authorize]
        public JsonResult VehiclePost(string vtype, string zip, string lng, string lat, string city)
        {
            Session["VType"] = vtype;
            Session["zip"] = zip;
            Session["lng"] = lng;
            Session["lat"] = lat;
            Session["city"] = city;

            // return RedirectToAction("SubAutoInsurance");
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult SubAutoInsurance()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult SubAutoInsurance(AutoInsurance model)
        {
            if (ModelState.IsValid)
            {
                DataSet BrokerDetails = null;
                BrokerDetails = BrokerUtility.SaveAutoInsuranceDetails(model);

                Session["VType"] = null;
                Session["zip"] = null;
                Session["lng"] = null;
                Session["lat"] = null;
                Session["city"] = null;

                Session["SearchResult"] = BrokerDetails;

                Session["InsuranceType"] = "Auto";
                return RedirectToAction("SearchBroker");
            }
            else
            {
                return View();
            }

        }


        [Authorize]
        [HttpGet]
        public ActionResult BusinessInsurance()
        {
            if (Session["ZipCode"] != null)
            {
                ViewBag.ZipCode = Session["ZipCode"].ToString();
            }
            else
            {
                ViewBag.ZipCode = "";
            }

            return View();
        }

        [Authorize]
        public JsonResult multipleselectSubIndustry(string subindustry)
        {
            Session["SubIndustryarray"] = subindustry;
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult BusinessInsurance(BusinessInsurance model)
        {
            if (ModelState.IsValid)
            {
                DataSet BrokerDetails = null;
                BrokerDetails = BrokerWebDB.BrokerWebDB.SaveBusinessInsuranceDetails(model);
                Session["SubIndustryarray"] = null;
                Session["SearchResult"] = BrokerDetails;

                Session["InsuranceType"] = "Business";
                return RedirectToAction("SearchBroker");
            }
            else
            {
                return View();
            }

        }


        [Authorize]
        [HttpGet]
        public ActionResult BenefitsInsurance()
        {
            if (Session["ZipCode"] != null)
            {
                ViewBag.ZipCode = Session["ZipCode"].ToString();
            }
            else
            {
                ViewBag.ZipCode = "";
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult BenefitsInsurance(BenefitsInsurance model)
        {
            if (model.IsInsured == null || model.IsInsured != "Currently insured")
            {
                this.ModelState.Remove("InsuranceCompany");
            }
            if (model.IndustryId == null || model.IndustryId == "")
            {
                this.ModelState.Remove("SubIndustryId");
            }

            if (ModelState.IsValid)
            {
                DataSet BrokerDetails = new DataSet();

                BrokerDetails = BrokerUtility.DoSaveBenefitInsuranceDetails(model);
                Session["SubIndustryarray"] = null;

                Session["SearchResult"] = BrokerDetails;

                Session["InsuranceType"] = "Benefits";
                return RedirectToAction("SearchBroker");

                // return View();
            }
            else
            {
                return View(model);

            }

            return RedirectToAction("SearchBroker");
        }

        [HttpGet]
        [Authorize]
        public ActionResult LifeandDisabilityInsurance()
        {
            if (Session["ZipCode"] != null)
            {
                ViewBag.ZipCode = Session["ZipCode"].ToString();
            }
            else
            {
                ViewBag.ZipCode = "";
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult LifeandDisabilityInsurance(LifeandDisabilityInsurance model)
        {
            if (ModelState.IsValid)
            {
                DataSet BrokerDetails = null;
                BrokerDetails = BrokerUtility.DoSaveLifeInsuranceDetails(model);

                Session["SearchResult"] = BrokerDetails;
                Session["InsuranceType"] = "Life";

                return RedirectToAction("SearchBroker");
            }
            else
            {
                return View(model);
            }
        }


        [AllowAnonymous]
        public ActionResult SearchBroker(string InsuranceType)
        {
            if (Session["UserId"] != null)
            {
                ViewBag.UserId = Session["UserId"].ToString();
            }

            DataSet dsuser = new DataSet();
            dsuser = (DataSet)Session["SearchResult"];

            ViewBag.WebServiceURL = strWebServiceURL;
            if (Session["UserId"] != null)
            {
                ViewBag.Company = Session["Company"].ToString();
                ViewBag.User = "Authorize";
                ViewBag.Initials = Session["Initials"];
                ViewBag.UserName = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();
            }
            else
            {
                if (Session["Company"] != null)
                {
                    ViewBag.Company = Session["Company"].ToString();
                    ViewBag.User = "Unauthorize";
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            if (dsuser != null)
            {
                if (dsuser.Tables.Count > 0)
                {
                    ViewBag.userdetails = dsuser.Tables[0].Rows.Cast<DataRow>().ToList();
                    ViewBag.Experiencedetails = dsuser.Tables[1].Rows.Cast<DataRow>().ToList();
                    ViewBag.Educationdetails = dsuser.Tables[2].Rows.Cast<DataRow>().ToList();
                    ViewBag.UserRating = dsuser.Tables[4].Rows.Cast<DataRow>().ToList();

                    //if (dsuser.Tables[1].Rows.Count == 0)
                    //{
                    //    ViewBag.ExperiencedetailsCount = 0;
                    //}

                    //if (dsuser.Tables[2].Rows.Count == 0)
                    //{
                    //    ViewBag.EducationdetailsCount = 0;
                    //}
                }
                else
                {
                    ViewBag.userdetails = null;
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        public string ContactBroker(string BrokerId, string InsuranceType, string BrokerName, string LocalDateTime)
        {
            string DeviceID = "", strmsgdetails = "";
            int success = 0;
            DataSet dsDeviceId = null;
            DataSet dsMsgDetails = null;

            dsDeviceId = BrokerWebDB.BrokerWebDB.GetDeviceId(Convert.ToInt32(BrokerId));
            if (dsDeviceId != null)
            {
                if (dsDeviceId.Tables.Count > 0)
                {
                    if (dsDeviceId.Tables[0].Rows.Count > 0)
                    {
                        DeviceID = dsDeviceId.Tables[0].Rows[0]["DeviceId"].ToString();
                    }
                }
            }

            dsMsgDetails = BrokerUtility.DoContactBroker(Convert.ToInt32(BrokerId), InsuranceType, BrokerName, DeviceID, LocalDateTime);
            if (dsMsgDetails != null)
            {
                Session["MessageDetails"] = dsMsgDetails;
                dsMsgDetails.Tables[0].TableName = "MsgDetails";
                dsMsgDetails.AcceptChanges();
            }


            strmsgdetails = BrokerWSUtility.CreateJsonFromDataset(dsMsgDetails, "MsgDetails", "true", "null");
            return strmsgdetails;//Json(new { strmsgdetails }, JsonRequestBehavior.AllowGet);


        }

        public string SendContactMail(string BrokerName, string BrokerId)
        {
            DataSet ds = new DataSet();

            if (Session["MessageDetails"] != null)
            {
                ds = (DataSet)Session["MessageDetails"];

                if (ds.Tables.Count > 0)
                {
                    DataSet dt = BrokerWebDB.BrokerWebDB.GetMessageDetails(ds.Tables[0].Rows[0]["CustMsgId"].ToString(), ds.Tables[0].Rows[0]["BrokerMsgId"].ToString());

                    string BrokerMessage = dt.Tables[0].Rows[0]["Message"].ToString();

                    string DeclarationDocPath = dt.Tables[0].Rows[0]["DeclarationDocPath"].ToString();

                    if (DeclarationDocPath != "" && DeclarationDocPath != null)
                    {
                        string DocPath = strDomainName + strDeclarationDoc + DeclarationDocPath;
                        BrokerMessage = BrokerMessage.Replace("Please reply back if you are interested.", "Declaration Document - <a style='text-decoration: underline;' href=" + DocPath + " download>Click here to download</a><br/>Please reply back from brokkrr app if you are interested.");
                    }
                    else
                    {
                        BrokerMessage = BrokerMessage.Replace("Please reply back if you are interested.", "Please reply back from brokkrr app if you are interested.");
                    }

                    List<spGetUserDetails_Result> oUserDetails = BrokerUtility.GetUserDetails(Convert.ToInt32(BrokerId));

                    if (oUserDetails.Count > 0)
                    {
                        bool flag = BrokerWSUtility.SendContactMessageToBrokerOnEmail(Convert.ToInt32(BrokerId), "", BrokerName, oUserDetails[0].EmailId, BrokerMessage);
                    }
                }
                Session["MessageDetails"] = null;
            }

            return "Sucsess";
        }

        [AllowAnonymous]
        public JsonResult GetIndustryMaster()
        {
            int CompanyId = 0;
            BrokerDBEntities Db = new BrokerDBEntities();

            //var resultData = Db.IndustryMasters.Select(c => new { Value = c.IndustryId, Text = c.IndustryName }).ToList();
            if (Session["CompanyId"] == null)
            {
                CompanyId = 1; ;
            }
            else
            {
                CompanyId = Convert.ToInt32(Session["CompanyId"].ToString());
            }

            var resultData = Db.IndustryMasters.Where(c => c.CompanyId == CompanyId).Select(c => new { Value = c.IndustryId, Text = c.IndustryName }).ToList().OrderBy(c => c.Text);

            return Json(new { result = resultData }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetSubIndustryMaster(int id)
        {
            BrokerDBEntities Db = new BrokerDBEntities();

            //var resultData = Db.SubIndustryMasters.Select(c => new { Value = c.IndustryId, Text = c.SubIndustryName }).ToList();
            var resultData = Db.SubIndustryMasters.Where(c => c.IndustryId == id).Select(c => new { Value = c.Id, Text = c.SICCode + " - " + c.SubIndustryName }).ToList();

            return Json(new { result = resultData }, JsonRequestBehavior.AllowGet);
        }


        //22Sept17 

        [AllowAnonymous]
        public ActionResult Insurance()
        {
            GetZipWithCityName model;
            ViewBag.GoogleMapKey = strGoogleMapKey;
            if (Session["Location"] != null)
            {
                model = (GetZipWithCityName)Session["Location"];
                ViewBag.City = model.City;
                ViewBag.ZipCode = model.ZipCode;
                ViewBag.Longitude = model.Longitude;
                ViewBag.Latitude = model.Latitude;
                ViewBag.Country = model.Country;
                ViewBag.State = model.State;

                if (Session["FromPage"] == "Index")
                {
                    ViewBag.User = "Unauthorize";
                    //clearSessionVariable();
                }
                else
                {
                    if (Session["UserId"] != null)
                    {
                        ViewBag.User = "Authorize";
                        ViewBag.Initials = Session["Initials"];
                        ViewBag.UserName = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();
                    }
                    else
                    {
                        ViewBag.User = "Unauthorize";
                    }
                }
            }
            else
            {
                if (Session["UserId"] != null)
                {
                    return RedirectToAction("HomePage", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();

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

        //21Sep17 san //4Oct17
        [AllowAnonymous]
        [HttpPost]
        public JsonResult SearchInsurance(InsuranceModel Model)
        {
            string strFirstName = "", strLastName = "";

            if (Model.LineType != null)
            {
                Session["LineType"] = Model.LineType;
            }
            else
            {
                Session["LineType"] = "";
            }

            if (Session["UserId"] != null)
            {
                Model.userId = Session["UserId"].ToString();
                if (Session["EmailId"] != null)
                {
                    Model.email = Session["EmailId"].ToString();
                }
                if (Session["FirstName"] != null)
                {
                    Model.firstName = Session["FirstName"].ToString();
                    strFirstName = Session["FirstName"].ToString();
                }
                if (Session["LastName"] != null)
                {
                    Model.lastName = Session["LastName"].ToString();
                    strLastName = Session["LastName"].ToString();
                }

                this.ModelState.Remove("firstName");
                this.ModelState.Remove("email");
                this.ModelState.Remove("phone");


            }

            DataSet dsuserstatus = null;
            string registrationCode = "";
            if (ModelState.IsValid)
            {
                if (Model.longitude == "" || Model.latitude == "" || Model.longitude == null || Model.latitude == null)
                {
                    try
                    {
                        string address = "";

                        address = "https://maps.googleapis.com/maps/api/geocode/xml?components=postal_code:" + Model.zipCode.Trim() + "&key="+strGoogleMapKey+"&sensor=false";

                        var result = new System.Net.WebClient().DownloadString(address);
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(result);
                        XmlNodeList parentNode = doc.GetElementsByTagName("location");
                        var lat = "";
                        var lng = "";
                        foreach (XmlNode childrenNode in parentNode)
                        {
                            lat = childrenNode.SelectSingleNode("lat").InnerText;
                            lng = childrenNode.SelectSingleNode("lng").InnerText;
                        }

                        Model.longitude = lng;
                        Model.latitude = lat;
                    }
                    catch (Exception Ex)
                    { 
                    
                    }
                }

                if (Session["UserId"] == null)
                {
                    if (Model.firstName != null)
                    {
                        string[] temp = Model.firstName.Split(' ');
                        if (temp.Length == 1)
                        {
                            strFirstName = temp[0].ToString();

                        }
                        else if (temp.Length >= 2)
                        {
                            strFirstName = temp[0].ToString();
                            strLastName = temp[1].ToString();
                        }
                    }

                    Session["FirstName"] = strFirstName;
                    Session["LastName"] = strLastName;

                    registrationCode = BrokerWSUtility.GetRandomNumber();
                    registrationCode = BrokerUtility.EncryptURL(registrationCode);

                    string CompName = Session["Company"].ToString();

                    dsuserstatus = BrokerWebDB.BrokerWebDB.GetUserStatus(Model.email, strFirstName, strLastName, Model.city, Model.zipCode, Model.phone, registrationCode, Session["Company"].ToString());
                    if (dsuserstatus.Tables.Count > 0)
                    {
                        if (dsuserstatus.Tables[0].Rows.Count > 0)
                        {
                            if (dsuserstatus.Tables[0].Rows[0]["UserStatus"].ToString() == "0")
                            {
                                if (dsuserstatus.Tables[1].Rows.Count > 0)
                                {
                                    //send registration Email.
                                    BrokerWSUtility.SendRegistrationEmailFromWebSite(Model.email, registrationCode, dsuserstatus.Tables[1].Rows[0]["UserId"].ToString(), "Customer");
                                    //GetList

                                    Model.userId = dsuserstatus.Tables[1].Rows[0]["UserId"].ToString();
                                    // Session["UserId"] = Model.userId;

                                    DataSet dsBrokerList = null;
                                    dsBrokerList = GetBrokerList(Model, Session["Company"].ToString());
                                    Session["SearchResult"] = dsBrokerList;
                                    SetInsuranceType(Model.insurancetype);

                                    Session["UserIdToContactBroker"] = Model.userId;
                                    DataSet ds = BrokerUtility.CheckUSerExist(Model.email);
                                    if (ds.Tables.Count > 0)
                                    {
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            Session["IsEmailVerified"] = ds.Tables[0].Rows[0]["IsEmailIdVerified"].ToString();
                                        }
                                    }
                                    return Json(new { result = "Success" }, JsonRequestBehavior.AllowGet);
                                }

                            }
                            else if (dsuserstatus.Tables[0].Rows[0]["UserStatus"].ToString() == "1")
                            {
                                return Json(new { result = "Email Id is already registered with broker profile. Please try with another EmailId." }, JsonRequestBehavior.AllowGet);
                            }
                            else if (dsuserstatus.Tables[0].Rows[0]["UserStatus"].ToString() == "2")
                            {
                                //login customer
                                if (dsuserstatus.Tables[1].Rows.Count > 0)
                                {
                                    Session["FirstName"] = dsuserstatus.Tables[1].Rows[0]["FirstName"].ToString();
                                    Session["LastName"] = dsuserstatus.Tables[1].Rows[0]["LastName"].ToString();

                                    string RegisteredComp = dsuserstatus.Tables[1].Rows[0]["RegisteredFor"].ToString();

                                    if (RegisteredComp == "")
                                    {
                                        RegisteredComp = "Brokkrr";
                                    }

                                    if (RegisteredComp == Session["Company"].ToString())
                                    {
                                        //GetList
                                        Model.userId = dsuserstatus.Tables[1].Rows[0]["UserId"].ToString();
                                        DataSet dsBrokerList = null;
                                        dsBrokerList = GetBrokerList(Model, Session["Company"].ToString());
                                        Session["SearchResult"] = dsBrokerList;
                                        SetInsuranceType(Model.insurancetype);
                                        Session["UserIdToContactBroker"] = Model.userId;
                                        DataSet ds = BrokerUtility.CheckUSerExist(Model.email);
                                        if (ds.Tables.Count > 0)
                                        {
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                Session["IsEmailVerified"] = ds.Tables[0].Rows[0]["IsEmailIdVerified"].ToString();
                                            }
                                        }
                                        return Json(new { result = "Success" }, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        return Json(new { result = "Email Id is registered with another association." }, JsonRequestBehavior.AllowGet);
                                    }
                                }

                            }
                            else if (dsuserstatus.Tables[0].Rows[0]["UserStatus"].ToString() == "3")
                            {
                                Session["FirstName"] = dsuserstatus.Tables[1].Rows[0]["FirstName"].ToString();
                                Session["LastName"] = dsuserstatus.Tables[1].Rows[0]["LastName"].ToString();

                                string RegisteredComp = dsuserstatus.Tables[1].Rows[0]["RegisteredFor"].ToString();

                                if (RegisteredComp == "")
                                {
                                    RegisteredComp = "Brokkrr";
                                }

                                if (RegisteredComp == Session["Company"].ToString())
                                {
                                    //Update User details with registration code

                                    BrokerWebDB.BrokerWebDB.UpdateUserDetails(Model.email, registrationCode, dsuserstatus.Tables[1].Rows[0]["UserId"].ToString(), strFirstName, strLastName, Model.phone);

                                    //Email Verification link
                                    BrokerWSUtility.SendMailVerificationEmail(Model.email, registrationCode, dsuserstatus.Tables[1].Rows[0]["UserId"].ToString(), "Customer");

                                    //getList

                                    Model.userId = dsuserstatus.Tables[1].Rows[0]["UserId"].ToString();
                                    // Session["UserId"] = Model.userId;
                                    DataSet dsBrokerList = null;
                                    dsBrokerList = GetBrokerList(Model, Session["Company"].ToString());
                                    Session["SearchResult"] = dsBrokerList;
                                    SetInsuranceType(Model.insurancetype);
                                    Session["UserIdToContactBroker"] = Model.userId;
                                    DataSet ds = BrokerUtility.CheckUSerExist(Model.email);
                                    if (ds.Tables.Count > 0)
                                    {
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            Session["IsEmailVerified"] = ds.Tables[0].Rows[0]["IsEmailIdVerified"].ToString();
                                        }
                                    }
                                    return Json(new { result = "Success" }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    return Json(new { result = "Email Id is registered with another association." }, JsonRequestBehavior.AllowGet);
                                }
                            }

                        }

                    }
                }
                else
                {

                    DataSet dsBrokerList = null;
                    dsBrokerList = GetBrokerList(Model, Session["Company"].ToString());
                    Session["SearchResult"] = dsBrokerList;
                    SetInsuranceType(Model.insurancetype);
                    Session["UserIdToContactBroker"] = Model.userId;
                    Session["IsEmailVerified"] = "";
                    return Json(new { result = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

            }

            return Json("success");

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

        IAuthenticationManager Authentication
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        //21Sep17 san //4Oct17
        public static DataSet GetBrokerList(InsuranceModel Model, string CompanyName)
        {
            DataSet BrokerDetails = null;
            string Path = "";
            if (Model.insurancetype == "Home Insurance")
            {
                HomeInsurance homemodel = new HomeInsurance();

                homemodel.Id = Convert.ToInt32(Model.userId);
                homemodel.ZipCode = Model.zipCode;
                homemodel.City = Model.city;
                homemodel.Longitude = Model.longitude;
                homemodel.Latitude = Model.latitude;
                homemodel.IsInsured = Model.newPurchaseOrCurrentlyInsured;
                homemodel.InsuranceCompany = Model.insuranceCompany;
                homemodel.CoverageExpires = Model.whenNeedInsurence;
                homemodel.Language = Model.language;
                homemodel.Notes = Model.notes;
                homemodel.EstimatedValue = Model.valueOfHome;

                if (Model.declaration == "Yes" && Model.uploaddeclaration != "" && Model.Hiddenuploaddeclaration != "")
                {
                    Path = BrokerUtility.SaveDeclarationDocument(Model.uploaddeclaration, Model.Hiddenuploaddeclaration, "HomeInsurance", Model.userId);
                }

                homemodel.DocPath = Path;

                BrokerDetails = BrokerUtility.SaveHomeInsuranceDetailsNew(homemodel,"");//Model.Hiddenuploaddeclaration


            }
            else if (Model.insurancetype == "Auto Insurance")
            {
                AutoInsurance automodel = new AutoInsurance();
               
                automodel.UserId = Convert.ToInt32(Model.userId);
                automodel.ZipCode = Model.zipCode;
                automodel.City = Model.city;
                automodel.VehicleType = Model.typeOfAuto;
                automodel.IsInsured = Model.newPurchaseOrCurrentlyInsured;
                automodel.InsuranceCompany = Model.insuranceCompany;
                automodel.CoverageExpires = Model.whenNeedInsurence;
                automodel.Language = Model.language;
                automodel.Notes = Model.notes;
                automodel.Latitude = Model.latitude;
                automodel.Longitude = Model.longitude;

                if (Model.declaration == "Yes" && Model.uploaddeclaration != "" && Model.Hiddenuploaddeclaration != "")
                {
                    Path = BrokerUtility.SaveDeclarationDocument(Model.uploaddeclaration, Model.Hiddenuploaddeclaration, "AutoInsurance", Model.userId);
                }
                automodel.DocPath = Path;

                BrokerDetails = BrokerUtility.SaveAutoInsuranceDetailsNew(automodel, "");
            }
            else if (Model.insurancetype == "Life Insurance")
            {
                LifeandDisabilityInsurance lifemodel = new LifeandDisabilityInsurance();
                //if (Model.whenNeedInsurence == "Need Insurance Now")
                //{
                //    Model.whenNeedInsurence = "Now";
                //}
                lifemodel.Id = Convert.ToInt32(Model.userId);
                lifemodel.ZipCode = Model.zipCode;
                lifemodel.City = Model.city;
                lifemodel.FaceValue = Model.coverageamount;
                lifemodel.Language = Model.language;
                lifemodel.Notes = Model.notes;
                lifemodel.Longitude = Model.longitude;
                lifemodel.Latitude = Model.latitude;

                lifemodel.CoverageExpires = Model.whenNeedInsurence;//29Sep17

                if (Model.declaration == "Yes" && Model.uploaddeclaration != "" && Model.Hiddenuploaddeclaration != "")
                {
                    Path = BrokerUtility.SaveDeclarationDocument(Model.uploaddeclaration, Model.Hiddenuploaddeclaration, "LifeInsurance", Model.userId);
                }

                lifemodel.DocPath = Path;

                BrokerDetails = BrokerUtility.DoSaveLifeInsuranceDetailsNew(lifemodel, "");
            }
            else if (Model.insurancetype == "Business Insurance")
            {
                string subindustry = "";

                BusinessInsurance businessmodel = new BusinessInsurance();
                //if (Model.whenNeedInsurence == "Need Insurance Now")
                //{
                //    Model.whenNeedInsurence = "Now";
                //}
                businessmodel.Id = Convert.ToInt32(Model.userId);
                businessmodel.ZipCode = Model.zipCode;
                businessmodel.City = Model.city;
                businessmodel.Revenue = Model.grossrevenue;
                businessmodel.IsInsured = Model.acquisitionIsInsured;
                businessmodel.InsuranceCompany = Model.insuranceCompany;
                businessmodel.CoverageExpires = Model.whenNeedInsurence;
                businessmodel.IndustryId = Model.industry;
                if (Model.industry != null)
                {
                    if (Model.siccode != null)
                    {
                        subindustry = Model.siccode.Replace(" ", "");
                    }
                }
                businessmodel.SubIndustryId = subindustry;

                businessmodel.Language = Model.language;
                businessmodel.Notes = Model.notes;
                businessmodel.Longitude = Model.longitude;
                businessmodel.Latitude = Model.latitude;

                if (Model.declaration == "Yes" && Model.uploaddeclaration != "" && Model.Hiddenuploaddeclaration != "")
                {
                    Path = BrokerUtility.SaveDeclarationDocument(Model.uploaddeclaration, Model.Hiddenuploaddeclaration, "BusinessInsurance", Model.userId);
                }

                businessmodel.DocPath = Path;

                BrokerDetails = BrokerUtility.SaveBusinessInsuranceDetailsNew(businessmodel, "");
            }
            else if (Model.insurancetype == "General Liability")
            {
                string subindustry = "";

                BusinessInsurance businessmodel = new BusinessInsurance();
                //if (Model.whenNeedInsurence == "Need Insurance Now")
                //{
                //    Model.whenNeedInsurence = "Now";
                //}
                businessmodel.Id = Convert.ToInt32(Model.userId);
                businessmodel.ZipCode = Model.zipCode;
                businessmodel.City = Model.city;
                businessmodel.Revenue = Model.grossrevenue;
                businessmodel.IsInsured = Model.acquisitionIsInsured;
                businessmodel.InsuranceCompany = Model.insuranceCompany;
                businessmodel.CoverageExpires = Model.whenNeedInsurence;
                businessmodel.IndustryId = Model.industry;
                if (Model.industry != null)
                {
                    if (Model.siccode != null)
                    {
                        subindustry = Model.siccode.Replace(" ", "");
                    }
                }
                businessmodel.SubIndustryId = subindustry;

                businessmodel.Language = Model.language;
                businessmodel.Notes = Model.notes;
                businessmodel.Longitude = Model.longitude;
                businessmodel.Latitude = Model.latitude;

                if (Model.declaration == "Yes" && Model.uploaddeclaration != "" && Model.Hiddenuploaddeclaration != "")
                {
                    Path = BrokerUtility.SaveDeclarationDocument(Model.uploaddeclaration, Model.Hiddenuploaddeclaration, "BusinessInsurance", Model.userId);
                }

                businessmodel.DocPath = Path;

                BrokerDetails = BrokerUtility.GeneralLiabilityDetails(businessmodel, "");
            }
            else if (Model.insurancetype == "Employee Benefits")
            {
                string subindustry = "";
                BenefitsInsurance benefitsmodel = new BenefitsInsurance();
                //if (Model.whenNeedInsurence == "Need Insurance Now")
                //{
                //    Model.whenNeedInsurence = "Now";
                //}
                benefitsmodel.Id = Convert.ToInt32(Model.userId);
                benefitsmodel.ZipCode = Model.zipCode;
                benefitsmodel.City = Model.city;
                benefitsmodel.IsInsured = Model.newPurchaseOrCurrentlyInsured;
                benefitsmodel.InsuranceCompany = Model.insuranceCompany;
                benefitsmodel.EmployeeStrength = Model.noOfEmployee;
                benefitsmodel.CoverageExpires = Model.whenNeedInsurence;
                benefitsmodel.Language = Model.language;
                benefitsmodel.Notes = Model.notes;
                benefitsmodel.Longitude = Model.longitude;
                benefitsmodel.Latitude = Model.latitude;

                benefitsmodel.IndustryId = Model.industry;
                if (Model.industry != null)
                {
                    if (Model.siccode != null)
                    {
                        subindustry = Model.siccode.Replace(" ", "");
                    }
                }
                benefitsmodel.SubIndustryId = subindustry;

                if (Model.declaration == "Yes" && Model.uploaddeclaration != "" && Model.Hiddenuploaddeclaration != "")
                {
                    Path = BrokerUtility.SaveDeclarationDocument(Model.uploaddeclaration, Model.Hiddenuploaddeclaration, "EmployeeBenefitsInsurance", Model.userId);
                }

                benefitsmodel.DocPath = Path;

                BrokerDetails = BrokerUtility.DoSaveBenefitInsuranceDetailsNew(benefitsmodel, "");
            }

                //For Meineke Insurance Company
            else if (Model.insurancetype == "Garage Keepers")
            {
                CommercialAutoInsurance automodel = new CommercialAutoInsurance();

                automodel.UserId = Convert.ToInt32(Model.userId);
                automodel.ZipCode = Model.zipCode;
                automodel.Longitude = Model.longitude;
                automodel.Latitude = Model.latitude;
                automodel.City = Model.city;
                automodel.NoOfStalls = Model.NoOfStalls;
                automodel.NoOfLocations = Model.NoOfLocations;
                automodel.grossrevenueforgarage = Model.grossrevenueforgarage;
                automodel.CurrentLimit = Model.CurrentLimit;

                if (Model.declaration == "Yes" && Model.uploaddeclaration != "" && Model.Hiddenuploaddeclaration != "")
                {
                    Path = BrokerUtility.SaveDeclarationDocument(Model.uploaddeclaration, Model.Hiddenuploaddeclaration, "GarageKeepersInsurance", Model.userId);
                }

                automodel.DocPath = Path;

                BrokerDetails = BrokerUtility.DoSaveCommercialAutoInsuranceDetails(automodel, "");

            }
            else if (Model.insurancetype == "Workers Compensation")
            {
                WorkersCompensation workcompmodel = new WorkersCompensation();

                workcompmodel.UserId = Convert.ToInt32(Model.userId);
                workcompmodel.ZipCode = Model.zipCode;
                workcompmodel.Longitude = Model.longitude;
                workcompmodel.Latitude = Model.latitude;
                workcompmodel.City = Model.city;

                if (CompanyName == "Brokkrr")
                {
                    workcompmodel.NoOfEmployees = Model.NoOfEmp;
                }
                else
                {
                    workcompmodel.NoOfEmployees = Model.noOfEmployee;
                }
                workcompmodel.GrossPayroll = Model.GrossPayroll;

                if (Model.declaration == "Yes" && Model.uploaddeclaration != "" && Model.Hiddenuploaddeclaration != "")
                {
                    Path = BrokerUtility.SaveDeclarationDocument(Model.uploaddeclaration, Model.Hiddenuploaddeclaration, "WorkersCompInsurance", Model.userId);
                }

                workcompmodel.DocPath = Path;

                BrokerDetails = BrokerUtility.DoSaveWorkersCompensationDetails(workcompmodel, "");

            }

            else if (Model.insurancetype == "Liability Insurance")
            {
                string subindustry = "";
                LiabilityInsurance Liability = new LiabilityInsurance();

                Liability.UserId = Convert.ToInt32(Model.userId);
                Liability.ZipCode = Model.zipCode;
                Liability.Longitude = Model.longitude;
                Liability.Latitude = Model.latitude;
                Liability.City = Model.city;
                Liability.GrossSale = Model.GrossSale;
                Liability.DeductibleIfAny = Model.DeductibleIfAny;

                Liability.IndustryId = Model.industry;
                if (Model.industry != null)
                {
                    if (Model.siccode != null)
                    {
                        subindustry = Model.siccode.Replace(" ", "");
                    }
                }
                Liability.SubIndustryId = subindustry;

                if (Model.declaration == "Yes" && Model.uploaddeclaration != "" && Model.Hiddenuploaddeclaration != "")
                {
                    Path = BrokerUtility.SaveDeclarationDocument(Model.uploaddeclaration, Model.Hiddenuploaddeclaration, "LiabilityInsurance", Model.userId);
                }

                Liability.DocPath = Path;

                BrokerDetails = BrokerUtility.DoSaveLiabilityInsuranceDetails(Liability, "");

            }

            else if (Model.insurancetype == "Benefits Insurance")
            {
                string subindustry = "";
                BenefitsInsurance benefitsmodel = new BenefitsInsurance();

                benefitsmodel.Id = Convert.ToInt32(Model.userId);
                benefitsmodel.ZipCode = Model.zipCode;
                benefitsmodel.City = Model.city;
                benefitsmodel.IsInsured = Model.newPurchaseOrCurrentlyInsured;
                benefitsmodel.InsuranceCompany = Model.insuranceCompany;
                benefitsmodel.EmployeeStrength = Model.noOfEmployeeMeinekeBenefits;
                benefitsmodel.CoverageExpires = Model.whenNeedInsurence;
                benefitsmodel.Language = Model.language;
                benefitsmodel.Notes = Model.notes;
                benefitsmodel.Longitude = Model.longitude;
                benefitsmodel.Latitude = Model.latitude;

                benefitsmodel.IndustryId = Model.industry;
                if (Model.industry != null)
                {
                    if (Model.siccode != null)
                    {
                        subindustry = Model.siccode.Replace(" ", "");
                    }
                }
                benefitsmodel.SubIndustryId = subindustry;

                if (Model.declaration == "Yes" && Model.uploaddeclaration != "" && Model.Hiddenuploaddeclaration != "")
                {
                    Path = BrokerUtility.SaveDeclarationDocument(Model.uploaddeclaration, Model.Hiddenuploaddeclaration, "BenefitsInsurance", Model.userId);
                }

                benefitsmodel.DocPath = Path;

                BrokerDetails = BrokerUtility.DoSaveMeinekeBenefitInsuranceDetails(benefitsmodel, "");

            }
            else if (Model.insurancetype == "Liability")
            {
                string subindustry = "";

                BusinessInsurance businessmodel = new BusinessInsurance();
                //if (Model.whenNeedInsurence == "Need Insurance Now")
                //{
                //    Model.whenNeedInsurence = "Now";
                //}
                businessmodel.Id = Convert.ToInt32(Model.userId);
                businessmodel.ZipCode = Model.zipCode;
                businessmodel.City = Model.city;
                businessmodel.Revenue = Model.grossrevenue;
                businessmodel.IsInsured = Model.acquisitionIsInsured;
                businessmodel.InsuranceCompany = Model.insuranceCompany;
                businessmodel.CoverageExpires = Model.whenNeedInsurence;
                businessmodel.IndustryId = Model.industry;
                if (Model.industry != null)
                {
                    if (Model.siccode != null)
                    {
                        subindustry = Model.siccode.Replace(" ", "");
                    }
                }
                businessmodel.SubIndustryId = subindustry;

                businessmodel.Language = Model.language;
                businessmodel.Notes = Model.notes;
                businessmodel.Longitude = Model.longitude;
                businessmodel.Latitude = Model.latitude;

                if (Model.declaration == "Yes" && Model.uploaddeclaration != "" && Model.Hiddenuploaddeclaration != "")
                {
                    Path = BrokerUtility.SaveDeclarationDocument(Model.uploaddeclaration, Model.Hiddenuploaddeclaration, "LiabilityInsurance", Model.userId);
                }

                businessmodel.DocPath = Path;

                BrokerDetails = BrokerUtility.SaveLiabilityInsuranceDetailsNew(businessmodel, "");
            }
            else if (Model.LineType == "401k")
            {
                string subindustry = "";
                _401kInsurance data = new _401kInsurance();

                data.UserId = Convert.ToInt32(Model.userId);
                data.ZipCode = Model.zipCode;
                data.Longitude = Model.longitude;
                data.Latitude = Model.latitude;
                data.City = Model.city;
                data.CurrentPlan = Model.CurrentPlan;
                data.NumberOfEmp = Model.noOfEmployeeMeinekeBenefits;
                data.PlanSize = Model.planSize;

                if (Model.declaration == "Yes" && Model.uploaddeclaration != "" && Model.Hiddenuploaddeclaration != "")
                {
                    Path = BrokerUtility.SaveDeclarationDocument(Model.uploaddeclaration, Model.Hiddenuploaddeclaration, "401kInsurance", Model.userId);
                }

                data.DocPath = Path;

                BrokerDetails = BrokerUtility.DoSave401kInsuranceDetails(data, "");
            }

            int r1 = BrokerWebDB.BrokerWebDB.SaveUserSerchedZipCodes(Convert.ToInt32(Model.userId), Model.zipCode);//12Sept17

            return BrokerDetails;
        }


        //21Sep17 san
        public string GetSubindustry()
        {

            string subIndustryList = "";

            DataSet dsSubIndustryMaster = null;
            dsSubIndustryMaster = BrokerWSUtility.GetSubIndustryMaster(0);
            if (dsSubIndustryMaster.Tables.Count > 0)
            {
                if (dsSubIndustryMaster.Tables[0].Rows.Count > 0)
                {
                    subIndustryList = BrokerWSUtility.CreateJsonFromDataset(dsSubIndustryMaster, "DoGetSubIndustryMaster", "true", "null");
                }
            }
            return subIndustryList;
        }

        //26Sept17 San
        public void SetInsuranceType(string insurancetype)
        {
            string type = "";
            if (insurancetype == "Home Insurance")
            {
                type = "Home";
            }
            else if (insurancetype == "Auto Insurance")
            {
                type = "Auto";
            }
            else if (insurancetype == "Life Insurance")
            {
                type = "Life";
            }
            else if (insurancetype == "Business Insurance")
            {
                type = "Business";
            }
            else if (insurancetype == "Employee Benefits")
            {
                type = "Benefits";
            }
            else if (insurancetype == "Garage Keepers")
            {
                type = "Garage Keepers";
            }
            else if (insurancetype == "Workers Compensation")
            {
                type = "Workers compensation";
            }
            else if (insurancetype == "Liability Insurance")
            {
                type = "Liability";
            }
            else if (insurancetype == "Benefits Insurance")
            {
                type = "Benefits";
            }
            else if (insurancetype == "Liability")
            {
                type = "Liability";
            }
            else if (insurancetype == "General Liability")
            {
                type = "General Liability";
            }


            Session["insurancetype"] = type;
        }



        //For Meineke Insurance Company

        [HttpGet]
        [Authorize]
        public ActionResult MeinekeInsurance()
        {
            GetZipWithCityName model;
            ViewBag.GoogleMapKey = strGoogleMapKey;
            if (Session["Location"] != null)
            {
                model = (GetZipWithCityName)Session["Location"];
                ViewBag.City = model.City;
                ViewBag.ZipCode = model.ZipCode;
                ViewBag.Longitude = model.Longitude;
                ViewBag.Latitude = model.Latitude;
                ViewBag.Country = model.Country;
                ViewBag.State = model.State;

                if (Session["UserId"] != null)
                {
                    ViewBag.User = "Authorize";
                    ViewBag.Initials = Session["Initials"];
                    ViewBag.UserName = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();
                }
                else
                {
                    ViewBag.User = "Unauthorize";
                }
            }
            else
            {
                if (Session["UserId"] != null)
                {
                    return RedirectToAction("HomePage", "Home");
                }
                else
                {
                    return RedirectToAction("MeinekeIndex", "Home");
                }
            }

            return View();

        }

        [AllowAnonymous]
        public ActionResult MeinekeInsuranceWithoutLogin()
        {

            ViewBag.User = "Unauthorize";
            Session["Company"] = "Meineke";
            ViewBag.IsAuthorize = "UnAuthorize";
            //GetZipWithCityName model;
            //if (Session["Location"] != null)
            //{
            //    model = (GetZipWithCityName)Session["Location"];
            //    ViewBag.City = model.City;
            //    ViewBag.ZipCode = model.ZipCode;
            //    ViewBag.Longitude = model.Longitude;
            //    ViewBag.Latitude = model.Latitude;
            //    ViewBag.Country = model.Country;

            //    if (Session["UserId"] != null)
            //    {
            //        ViewBag.User = "Authorize";
            //        ViewBag.Initials = Session["Initials"];
            //        ViewBag.UserName = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();
            //    }
            //    else
            //    {
            //        ViewBag.User = "Unauthorize";
            //    }
            //}
            //else
            //{
            //    if (Session["UserId"] != null)
            //    {
            //        return RedirectToAction("HomePage", "Home");
            //    }
            //    else
            //    {
            //        return RedirectToAction("MeinekeIndex", "Home");
            //    }
            //}

            return View();

        }

        [HttpGet]
        [Authorize]
        public ActionResult APSPInsurance()
        {
            GetZipWithCityName model;
            ViewBag.GoogleMapKey = strGoogleMapKey;
            if (Session["Location"] != null)
            {
                model = (GetZipWithCityName)Session["Location"];
                ViewBag.City = model.City;
                ViewBag.ZipCode = model.ZipCode;
                ViewBag.Longitude = model.Longitude;
                ViewBag.Latitude = model.Latitude;
                ViewBag.Country = model.Country;
                ViewBag.State = model.State;

                if (Session["UserId"] != null)
                {
                    ViewBag.User = "Authorize";
                    ViewBag.Initials = Session["Initials"];
                    ViewBag.UserName = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();
                }
                else
                {
                    ViewBag.User = "Unauthorize";
                }
            }
            else
            {
                if (Session["UserId"] != null)
                {
                    return RedirectToAction("APASPHomePage", "Home");
                }
                else
                {
                    return RedirectToAction("APASPHomePage", "Home");
                }
            }

            return View();

        }
    }
}