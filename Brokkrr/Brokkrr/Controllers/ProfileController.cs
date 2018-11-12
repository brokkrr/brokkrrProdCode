using BrokerMVC.App_Code;
using BrokerMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrokerMVC.Controllers
{
    public class ProfileController : Controller
    {
        public static string strDomainName = ConfigurationManager.AppSettings["DomainName"].ToString();
        //public static string strProfilePicForlderName = ConfigurationManager.AppSettings["ProfilePicForlderName"].ToString();

        public static string strProfilePicImageFolder = ConfigurationManager.AppSettings["ProfilePicImageFolder"].ToString();

        // GET: Profile
        [HttpGet]
        [Authorize]
        public ActionResult CustomerProfile()
        {
            List<spCheckUserExist_Result> oUserDetsils = null;
            CustomerProfile profile = new CustomerProfile();

            string Initials = "N/A";

            ViewBag.UserId = Session["UserId"].ToString();

            ViewBag.Company = Session["Company"].ToString();
            ViewBag.User = "Authorize";

            oUserDetsils = BrokerWebDB.BrokerWebDB.GetCustomerDetails(Session["EmailId"].ToString(), Session["UserId"].ToString());

            profile.Address = oUserDetsils[0].Address;
            profile.FirstName = oUserDetsils[0].FirstName;
            profile.LastName = oUserDetsils[0].LastName;
            profile.EmailId = oUserDetsils[0].EmailId;
            profile.PhoneNo = oUserDetsils[0].PhoneNo;

            profile.CompanyName = oUserDetsils[0].CompanyName;
            profile.HouseType = oUserDetsils[0].HouseType;
            profile.NoOfCars = Convert.ToString(oUserDetsils[0].NoOfCars);
            //profile.TypeOfEmployment = oUserDetsils[0].TypeOfEmployment;



            if (oUserDetsils[0].TypeOfEmployment != "" || oUserDetsils[0].TypeOfEmployment != null)
            {
                if (oUserDetsils[0].TypeOfEmployment == "Self Employed")
                {
                    profile.TypeOfEmployment = "Business Owner";
                }
                else
                {
                    profile.TypeOfEmployment = "Employed";
                }
            }

            profile.ZipCode = oUserDetsils[0].PinCode;

            Session["ZipCode"] = oUserDetsils[0].PinCode;

            profile.ProfilePicture = oUserDetsils[0].ProfilePictureImg;

            if (oUserDetsils[0].ProfilePictureImg != "" && oUserDetsils[0].ProfilePictureImg != null)
            {
                ViewBag.ProfilePic = strDomainName + "" + strProfilePicImageFolder + "" + oUserDetsils[0].ProfilePictureImg;
                ViewBag.Time = DateTime.Now;
                Session["ProfilePic"] = strDomainName + "" + strProfilePicImageFolder + "" + oUserDetsils[0].ProfilePictureImg + "?" + DateTime.Now;
            }
            else
            {
                ViewBag.ProfilePic = "";
            }

            //change 6 sep 2017
            if (oUserDetsils[0].FirstName != "" && oUserDetsils[0].LastName != "")
            {
                Initials = oUserDetsils[0].FirstName[0] + "" + oUserDetsils[0].LastName[0];
            }
            ViewBag.Initials = Initials.ToUpper();
            Session["Initials"] = Initials.ToUpper();
            ViewBag.UserName = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();

            if (Session["DetailsSaved"] != null)
            {
                ViewBag.DetailsSaved = Session["DetailsSaved"].ToString();
                Session["DetailsSaved"] = null;
            }
            else
            {
                ViewBag.DetailsSaved = "";
            }

            //System.Globalization.DateTimeFormatInfo format = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat;

            //string LocalDateTime = DateTime.Now.ToString(format.ShortDatePattern + " " +
            //     format.LongTimePattern);// shashi

            //DateTime utcdate = DateTime.UtcNow;

            //var istdate = TimeZoneInfo.ConvertTimeFromUtc(utcdate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

            //var pstdate = TimeZoneInfo.ConvertTimeFromUtc(utcdate, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
            //string LocalDateTime = DateTime.Now.ToString(); // shashi

            if (Session["ReturnUrl"] != null)
            {
                if (Session["ReturnUrl"].ToString() == "CustomerBriefcase")
                {
                    return RedirectToAction("Briefcase", "BrokkrrBriefcase");
                }
            }
            return View(profile);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CustomerProfile(CustomerProfile Cust, IEnumerable<HttpPostedFileBase> file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //if (Request.Form["Save"] != null)
                    //{
                    string FirstName = "", LastName = "", Email = "", Phone1 = "", Phone2 = "", Phone3 = "", Password = "",
                        TempPass = "", ZipCode = "", ProfilePhoto = "", HouseType = "", Address = "", IsCars = "",
                        Occupation = "", CompanyName = "", Phone = "", FieldName1 = "", RenamedImageName = "", FileName2 = "";
                    int NoOfCars = 0, UserId;

                    List<spUpdateCustomerForWeb_Result> CustDetails = null;
                    UserId = Convert.ToInt32(Session["UserId"].ToString());


                    if (Cust.FirstName != null)
                    {
                        FirstName = Cust.FirstName.ToString();
                        Session["FirstName"] = FirstName;
                    }
                    if (Cust.LastName != null)
                    {
                        LastName = Cust.LastName.ToString();
                        Session["LastName"] = LastName;
                    }

                    if (Cust.PhoneNo != null)
                    {
                        Phone1 = Cust.PhoneNo.ToString();
                    }

                    Phone = Phone1;

                    if (Cust.ZipCode != null)
                    {
                        ZipCode = Cust.ZipCode.ToString();
                    }
                    if (Cust.HouseType != null)
                    {
                        HouseType = Cust.HouseType.ToString();
                    }

                    if (Cust.Address != null)
                    {
                        Address = Cust.Address.ToString();
                    }

                    if (Cust.NoOfCars != null)
                    {
                        if (Cust.NoOfCars != "")
                        {
                            NoOfCars = Convert.ToInt32(Cust.NoOfCars.ToString());
                        }
                        else
                        {
                            NoOfCars = 0;
                        }
                    }
                    if (Cust.TypeOfEmployment != null)
                    {
                        if (Cust.TypeOfEmployment.ToString() == "Business Owner")
                        {
                            Occupation = "Self Employed";
                        }
                        else
                        {
                            Occupation = Cust.TypeOfEmployment.ToString();
                        }
                    }
                    if (Cust.CompanyName != null)
                    {
                        CompanyName = Cust.CompanyName.ToString();
                    }



                    //Save Details of Profile Picture

                    //if (Cust.IsCustomerProfileChanged == "Yes")
                    //{
                    //    if (file == null)
                    //    {

                    //    }
                    //    else
                    //    {
                    //        foreach (var f in file)
                    //        {
                    //            if (f != null)
                    //            {
                    //                if (f.ContentLength > 0)
                    //                {
                    //                    int MaxContentLength = 1024 * 1024 * 4;
                    //                    string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf", ".jpe", ".jpeg" };
                    //                    if (!AllowedFileExtensions.Contains
                    //                    (f.FileName.Substring(f.FileName.LastIndexOf('.')).ToLower()))
                    //                    {
                    //                        ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                    //                    }
                    //                    else if (f.ContentLength > MaxContentLength)
                    //                    {
                    //                        ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
                    //                    }
                    //                    else
                    //                    {
                    //                        var fileName = Path.GetFileName(f.FileName);

                    //                        var path = Path.Combine(Server.MapPath("~/UploadedDoc"), fileName);


                    //                        byte[] binaryData;
                    //                        binaryData = new Byte[f.InputStream.Length];
                    //                        long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                    //                        f.InputStream.Close();
                    //                        string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                    //                        string FileName1 = "";

                    //                        string FieldName = "";
                    //                        string ProfilePicFile = "";

                    //                        string ProfilePic = Cust.ProfilePicture.ToString().Replace("C:\\fakepath\\", "");

                    //                        if (ProfilePic == fileName)
                    //                        {
                    //                            FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/ProfilePicture/" + Cust.EmailId.ToString() + "_" + UserId + ".txt");
                    //                            FieldName = "ProfilePicture";
                    //                            FileName2 = Cust.EmailId.ToString() + "_" + UserId + ".txt";
                    //                            ProfilePicFile = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Cust.EmailId.ToString() + "_" + UserId + ".png");
                    //                            bool CheckFile1 = BrokerUtility.CheckFile(ProfilePicFile);
                    //                            byte[] imageBytes1 = Convert.FromBase64String(base64String);

                    //                            //MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);
                    //                            MemoryStream ms1 = new MemoryStream(binaryData, 0, binaryData.Length);

                    //                            //ms1.Write(imageBytes1, 0, imageBytes1.Length);
                    //                            ms1.Write(binaryData, 0, binaryData.Length);

                    //                            System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                    //                            //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Cust.Email.ToString() + "_" + UserId + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                    //                            System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                    //                            thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Cust.EmailId.ToString() + "_" + UserId + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                    //                            FieldName1 = "ProfilePictureImg";
                    //                            RenamedImageName = Cust.EmailId.ToString() + "_" + UserId + ".png";

                    //                            //Check for the file already exist or not 
                    //                            bool CheckFile = BrokerUtility.CheckFile(FileName1);
                    //                            if (CheckFile)
                    //                            {
                    //                                //Create a text file of Base 64 string
                    //                                bool result = BrokerUtility.WriteFile(FileName1, base64String);
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{

                    //}

                    //Save the Details of Customer

                    CustDetails = BrokerWebDB.BrokerWebDB.SaveCustomerProfileDetails(FirstName, LastName, Phone, Address, ZipCode, HouseType, IsCars, NoOfCars, Occupation, CompanyName, FileName2, RenamedImageName, "No", UserId, "", "", "");

                    if (CustDetails.Count > 0)
                    {
                        //TempData["CustDetails"] = Cust;
                        Session["DetailsSaved"] = "Save";
                        return RedirectToAction("CustomerProfile", "Profile");
                    }
                    else
                    {
                        return View();
                    }

                    //}
                    //else if (Request.Form["Cancel"] != null)
                    //{
                    //    //TempData["CustDetails"] = Cust;
                    //    return RedirectToAction("CustomerProfile", "Profile");
                    //    //return View("","")
                    //}
                }
                catch (Exception Ex)
                {
                    BrokerUtility.ErrorLog(Convert.ToInt32(Session["UserId"].ToString()), "Index_POST_Wesite", Ex.Message.ToString(), "CustomerRegistrationController.cs_Index_POST", BrokerUtility.GetIPAddress(Session["UserId"].ToString()));
                    return View();
                }
                return View();
            }
            else
            {
                return View();
            }
        }

        public bool ThumbnailCallback()
        {
            return false;
        }


        [HttpGet]
        [Authorize]
        public ActionResult BrokerProfile()
        {
            DataSet dsBrokerData = new DataSet();

            if (Session["BrokerData"] != null)
            {
                dsBrokerData = (DataSet)Session["BrokerData"];

                if (dsBrokerData.Tables.Count > 0)
                {
                    if (dsBrokerData.Tables[0].Rows.Count > 0)
                    {
                        string Initials = "N/A";
                        int UserID = 0;
                        if (Session["UserId"] != null)
                        {
                            UserID = Convert.ToInt32(Session["UserId"]);
                        }

                        if (dsBrokerData.Tables[0].Rows[0]["FirstName"].ToString() != "" && dsBrokerData.Tables[0].Rows[0]["LastName"].ToString() != "")
                        {
                            Initials = dsBrokerData.Tables[0].Rows[0]["FirstName"].ToString()[0] + "" + dsBrokerData.Tables[0].Rows[0]["LastName"].ToString()[0];
                        }
                        Session["Initials"] = Initials.ToUpper();
                        ViewBag.Initials = Initials.ToUpper();
                        ViewBag.UserName = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();

                        bool IsAvailable = BrokerUtility.DoGetBrokerAvailabilityStatus(UserID);
                        ViewBag.userdetails = dsBrokerData.Tables[0].Rows.Cast<DataRow>().ToList();
                        ViewBag.Experiencedetails = dsBrokerData.Tables[1].Rows.Cast<DataRow>().ToList();
                        ViewBag.Educationdetails = dsBrokerData.Tables[2].Rows.Cast<DataRow>().ToList();
                        ViewBag.IsAvailable = IsAvailable;

                        if (dsBrokerData.Tables[0].Rows[0]["ProfilePictureImg"] != "" && dsBrokerData.Tables[0].Rows[0]["ProfilePictureImg"] != null)
                        {
                            ViewBag.ProfilePic = dsBrokerData.Tables[0].Rows[0]["ProfilePictureImg"].ToString();
                            ViewBag.Time = DateTime.Now;
                        }
                        else
                        {
                            ViewBag.ProfilePic = "";
                        }
                        //string FirstName = dsBrokerData.Tables[0].Rows[0]["FirstName"].ToString();
                        //BrokerInfo.FirstName = FirstName;


                    }
                }
            }
            else
            {
                return RedirectToAction("BrokerLogin", "Login");
            }
            if (Session["ReturnUrl"] != null)
            {
                if (Session["ReturnUrl"].ToString() == "BrokerBriefcase")
                {
                    return RedirectToAction("Index", "BrokkrrBriefcase");
                }
            }
            return View();
        }


        [HttpGet]
        [Authorize]
        public JsonResult SetBrokerAvailability(bool Availability)
        {
            int status = 0, UserId = 0;


            if (Session["UserId"] != null)
            {
                UserId = Convert.ToInt32(Session["UserId"]);
            }

            status = BrokerUtility.DoSetBrokerAvailabilityStatus(UserId, Availability);
            if (status > 0)
            {
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }


        [Authorize]
        public JsonResult UploadProfilePicture()
        {
            string strImagePath = "";
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["HelpSectionImages"];

                if (pic.ContentLength > 0)
                {
                    int MaxContentLength = 1024 * 1024 * 4;
                    string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf", ".jpe", ".jpeg" };

                    var fileName = Path.GetFileName(pic.FileName);

                    var path = Path.Combine(Server.MapPath("~/UploadedDoc"), fileName);


                    byte[] binaryData;
                    binaryData = new Byte[pic.InputStream.Length];
                    long bytesRead = pic.InputStream.Read(binaryData, 0, (int)pic.InputStream.Length);
                    pic.InputStream.Close();
                    string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                    string FileName1 = "";

                    string FieldName = "";
                    string ProfilePicFile = "";

                    string ProfilePic = fileName.ToString().Replace("C:\\fakepath\\", "");

                    FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/ProfilePicture/" + Session["EmailId"].ToString() + "_" + Session["UserId"].ToString() + ".txt");
                    FieldName = "ProfilePicture";
                    string FileName2 = Session["EmailId"].ToString() + "_" + Session["UserId"].ToString() + ".txt";
                    ProfilePicFile = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Session["EmailId"].ToString() + "_" + Session["UserId"] + ".png");
                    bool CheckFile1 = BrokerUtility.CheckFile(ProfilePicFile);
                    byte[] imageBytes1 = Convert.FromBase64String(base64String);

                    //MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);
                    MemoryStream ms1 = new MemoryStream(binaryData, 0, binaryData.Length);

                    //ms1.Write(imageBytes1, 0, imageBytes1.Length);
                    ms1.Write(binaryData, 0, binaryData.Length);

                    System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                    Size thumbnailSize = GetThumbnailSize(image1);

                    //System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                    System.Drawing.Image thumbnail = image1.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, null, IntPtr.Zero);
                    thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Session["EmailId"].ToString() + "_" + Session["UserId"] + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                    string FieldName1 = "ProfilePictureImg";
                    string RenamedImageName = Session["EmailId"].ToString() + "_" + Session["UserId"].ToString() + ".png";

                    //Check for the file already exist or not 
                    bool CheckFile = BrokerUtility.CheckFile(FileName1);
                    if (CheckFile)
                    {
                        //Create a text file of Base 64 string
                        bool result = BrokerUtility.WriteFile(FileName1, base64String);

                        if (result)
                        {
                            int i = BrokerWebDB.BrokerWebDB.UpdateCustomerProfilePhoto(Convert.ToInt32(Session["UserId"]), FileName2, RenamedImageName);

                            if (i > 0)
                            {
                                strImagePath = strDomainName + "" + strProfilePicImageFolder + "" + RenamedImageName;
                                DateTime dt = DateTime.Now;
                                strImagePath = strImagePath + "?" + dt.ToString();
                                Session["ProfilePic"] = strDomainName + "" + strProfilePicImageFolder + "" + RenamedImageName + "?" + DateTime.Now;
                            }
                        }
                    }
                    //}
                    //}
                }
            }

            return Json(new { ImagePath = strImagePath }, JsonRequestBehavior.AllowGet);
        }

        static Size GetThumbnailSize(System.Drawing.Image original)
        {
            //// Maximum size of any dimension.
            //const int MaxWidth = 180;
            //const int Maxheight = 35;
            //// Width and height.
            //int originalWidth = original.Width;
            //int originalHeight = original.Height;

            //// Compute best factor to scale entire image based on larger dimension.
            //double factor;
            //double factor1;
            //if (originalWidth > originalHeight)
            //{
            //    factor = (double)MaxWidth / originalWidth;
            //    factor1 = (double)Maxheight / originalHeight;
            //}
            //else
            //{
            //    factor = (double)MaxWidth / originalWidth;
            //    factor1 = (double)Maxheight / originalHeight;
            //}

            //// Return thumbnail size.
            //return new Size((int)(originalWidth * factor), (int)(originalHeight * factor1));

            /***********************************************************************************************/


            // Width and height.
            int originalWidth = original.Width;
            int originalHeight = original.Height;


            // Return thumbnail size.
            return new Size((int)(originalWidth), (int)(originalHeight));
        }

        [HttpGet]
        [Authorize]
        public ActionResult MeinekeCustomerProfile()
        {
            List<spCheckUserExist_Result> oUserDetsils = null;
            CustomerProfile profile = new CustomerProfile();
            ViewBag.UserId = Session["UserId"].ToString();
            string Initials = "N/A";

            oUserDetsils = BrokerWebDB.BrokerWebDB.GetCustomerDetails(Session["EmailId"].ToString(), Session["UserId"].ToString());

            profile.Address = oUserDetsils[0].Address;
            profile.FirstName = oUserDetsils[0].FirstName;
            profile.LastName = oUserDetsils[0].LastName;
            profile.EmailId = oUserDetsils[0].EmailId;
            profile.PhoneNo = oUserDetsils[0].PhoneNo;

            profile.CompanyName = oUserDetsils[0].CompanyName;
            //profile.HouseType = oUserDetsils[0].HouseType;
            //profile.NoOfCars = Convert.ToString(oUserDetsils[0].NoOfCars);
            //profile.TypeOfEmployment = oUserDetsils[0].TypeOfEmployment;

            if (Session["Company"].ToString() == "Meineke")
            {
                profile.NoofEmployee = oUserDetsils[0].NoOfEmp;
                ViewBag.NoofEmployee = profile.NoofEmployee;

                profile.EstPremium = oUserDetsils[0].EstPremium;
                ViewBag.EstPremium = profile.EstPremium;

                profile.Website = oUserDetsils[0].Website;
            }

            //if (oUserDetsils[0].TypeOfEmployment != "" || oUserDetsils[0].TypeOfEmployment != null)
            //{
            //    if (oUserDetsils[0].TypeOfEmployment == "Self Employed")
            //    {
            //        profile.TypeOfEmployment = "Business Owner";
            //    }
            //    else
            //    {
            //        profile.TypeOfEmployment = "Employed";
            //    }
            //}

            profile.ZipCode = oUserDetsils[0].PinCode;

            Session["ZipCode"] = oUserDetsils[0].PinCode;

            profile.ProfilePicture = oUserDetsils[0].ProfilePictureImg;

            if (oUserDetsils[0].ProfilePictureImg != "" && oUserDetsils[0].ProfilePictureImg != null)
            {
                ViewBag.ProfilePic = strDomainName + "" + strProfilePicImageFolder + "" + oUserDetsils[0].ProfilePictureImg;
                ViewBag.Time = DateTime.Now;
                Session["ProfilePic"] = strDomainName + "" + strProfilePicImageFolder + "" + oUserDetsils[0].ProfilePictureImg + "?" + DateTime.Now;
            }
            else
            {
                ViewBag.ProfilePic = "";
            }

            //change 6 sep 2017
            if (oUserDetsils[0].FirstName != "" && oUserDetsils[0].LastName != "")
            {
                Initials = oUserDetsils[0].FirstName[0] + "" + oUserDetsils[0].LastName[0];
            }
            ViewBag.Initials = Initials.ToUpper();
            Session["Initials"] = Initials.ToUpper();
            ViewBag.UserName = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();

            if (Session["DetailsSaved"] != null)
            {
                ViewBag.DetailsSaved = Session["DetailsSaved"].ToString();
                Session["DetailsSaved"] = null;
            }
            else
            {
                ViewBag.DetailsSaved = "";
            }

            //System.Globalization.DateTimeFormatInfo format = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat;

            //string LocalDateTime = DateTime.Now.ToString(format.ShortDatePattern + " " +
            //     format.LongTimePattern);// shashi

            //DateTime utcdate = DateTime.UtcNow;

            //var istdate = TimeZoneInfo.ConvertTimeFromUtc(utcdate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

            //var pstdate = TimeZoneInfo.ConvertTimeFromUtc(utcdate, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
            //string LocalDateTime = DateTime.Now.ToString(); // shashi

            if (Session["ReturnUrl"] != null)
            {
                if (Session["ReturnUrl"].ToString() == "CustomerBriefcase")
                {
                    return RedirectToAction("Briefcase", "BrokkrrBriefcase");
                }
            }
            return View(profile);
        }

        [HttpPost]
        [Authorize]
        public ActionResult MeinekeCustomerProfile(CustomerProfile Cust, IEnumerable<HttpPostedFileBase> file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //if (Request.Form["Save"] != null)
                    //{
                    string FirstName = "", LastName = "", Email = "", Phone1 = "", Phone2 = "", Phone3 = "", Password = "",
                        TempPass = "", ZipCode = "", ProfilePhoto = "", HouseType = "", Address = "", IsCars = "",
                        Occupation = "", CompanyName = "", Phone = "", FieldName1 = "", RenamedImageName = "", FileName2 = "", NoofEmployee = "", EstPremium = "", Website = "";
                    int NoOfCars = 0, UserId;

                    List<spUpdateCustomerForWeb_Result> CustDetails = null;
                    UserId = Convert.ToInt32(Session["UserId"].ToString());


                    if (Cust.FirstName != null)
                    {
                        FirstName = Cust.FirstName.ToString();
                        Session["FirstName"] = FirstName;
                    }
                    if (Cust.LastName != null)
                    {
                        LastName = Cust.LastName.ToString();
                        Session["LastName"] = LastName;
                    }

                    if (Cust.PhoneNo != null)
                    {
                        Phone1 = Cust.PhoneNo.ToString();
                    }

                    Phone = Phone1;

                    if (Cust.ZipCode != null)
                    {
                        ZipCode = Cust.ZipCode.ToString();
                    }
                    if (Cust.HouseType != null)
                    {
                        HouseType = Cust.HouseType.ToString();
                    }

                    if (Cust.Address != null)
                    {
                        Address = Cust.Address.ToString();
                    }

                    if (Cust.NoOfCars != null)
                    {
                        if (Cust.NoOfCars != "")
                        {
                            NoOfCars = Convert.ToInt32(Cust.NoOfCars.ToString());
                        }
                        else
                        {
                            NoOfCars = 0;
                        }
                    }
                    if (Cust.TypeOfEmployment != null)
                    {
                        if (Cust.TypeOfEmployment.ToString() == "Business Owner")
                        {
                            Occupation = "Self Employed";
                        }
                        else
                        {
                            Occupation = Cust.TypeOfEmployment.ToString();
                        }
                    }
                    if (Cust.CompanyName != null)
                    {
                        CompanyName = Cust.CompanyName.ToString();
                    }

                    if (Cust.NoofEmployee != null)
                    {
                        NoofEmployee = Cust.NoofEmployee.ToString();
                    }

                    if (Cust.EstPremium != null)
                    {
                        EstPremium = Cust.EstPremium.ToString();
                    }

                    if (Cust.Website != null)
                    {
                        Website = Cust.Website.ToString();
                    }

                    //Save Details of Profile Picture

                    //if (Cust.IsCustomerProfileChanged == "Yes")
                    //{
                    //    if (file == null)
                    //    {

                    //    }
                    //    else
                    //    {
                    //        foreach (var f in file)
                    //        {
                    //            if (f != null)
                    //            {
                    //                if (f.ContentLength > 0)
                    //                {
                    //                    int MaxContentLength = 1024 * 1024 * 4;
                    //                    string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf", ".jpe", ".jpeg" };
                    //                    if (!AllowedFileExtensions.Contains
                    //                    (f.FileName.Substring(f.FileName.LastIndexOf('.')).ToLower()))
                    //                    {
                    //                        ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                    //                    }
                    //                    else if (f.ContentLength > MaxContentLength)
                    //                    {
                    //                        ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
                    //                    }
                    //                    else
                    //                    {
                    //                        var fileName = Path.GetFileName(f.FileName);

                    //                        var path = Path.Combine(Server.MapPath("~/UploadedDoc"), fileName);


                    //                        byte[] binaryData;
                    //                        binaryData = new Byte[f.InputStream.Length];
                    //                        long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                    //                        f.InputStream.Close();
                    //                        string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                    //                        string FileName1 = "";

                    //                        string FieldName = "";
                    //                        string ProfilePicFile = "";

                    //                        string ProfilePic = Cust.ProfilePicture.ToString().Replace("C:\\fakepath\\", "");

                    //                        if (ProfilePic == fileName)
                    //                        {
                    //                            FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/ProfilePicture/" + Cust.EmailId.ToString() + "_" + UserId + ".txt");
                    //                            FieldName = "ProfilePicture";
                    //                            FileName2 = Cust.EmailId.ToString() + "_" + UserId + ".txt";
                    //                            ProfilePicFile = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Cust.EmailId.ToString() + "_" + UserId + ".png");
                    //                            bool CheckFile1 = BrokerUtility.CheckFile(ProfilePicFile);
                    //                            byte[] imageBytes1 = Convert.FromBase64String(base64String);

                    //                            //MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);
                    //                            MemoryStream ms1 = new MemoryStream(binaryData, 0, binaryData.Length);

                    //                            //ms1.Write(imageBytes1, 0, imageBytes1.Length);
                    //                            ms1.Write(binaryData, 0, binaryData.Length);

                    //                            System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                    //                            //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Cust.Email.ToString() + "_" + UserId + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                    //                            System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                    //                            thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Cust.EmailId.ToString() + "_" + UserId + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                    //                            FieldName1 = "ProfilePictureImg";
                    //                            RenamedImageName = Cust.EmailId.ToString() + "_" + UserId + ".png";

                    //                            //Check for the file already exist or not 
                    //                            bool CheckFile = BrokerUtility.CheckFile(FileName1);
                    //                            if (CheckFile)
                    //                            {
                    //                                //Create a text file of Base 64 string
                    //                                bool result = BrokerUtility.WriteFile(FileName1, base64String);
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{

                    //}

                    //Save the Details of Customer

                    CustDetails = BrokerWebDB.BrokerWebDB.SaveCustomerProfileDetails(FirstName, LastName, Phone, Address, ZipCode, HouseType, IsCars, NoOfCars, Occupation, CompanyName, FileName2, RenamedImageName, "No", UserId, NoofEmployee, EstPremium, Website);

                    if (CustDetails.Count > 0)
                    {
                        //TempData["CustDetails"] = Cust;
                        Session["DetailsSaved"] = "Save";
                        return RedirectToAction("MeinekeCustomerProfile", "Profile");
                    }
                    else
                    {
                        return View();
                    }

                    //}
                    //else if (Request.Form["Cancel"] != null)
                    //{
                    //    //TempData["CustDetails"] = Cust;
                    //    return RedirectToAction("CustomerProfile", "Profile");
                    //    //return View("","")
                    //}
                }
                catch (Exception Ex)
                {
                    BrokerUtility.ErrorLog(Convert.ToInt32(Session["UserId"].ToString()), "MeinekeCustomerProfile_POST_Wesite", Ex.Message.ToString(), "CustomerRegistrationController.cs_Index_POST", BrokerUtility.GetIPAddress(Session["UserId"].ToString()));
                    return View();
                }
                return View();
            }
            else
            {
                return View();
            }
        }


        [HttpGet]
        [Authorize]
        public ActionResult APSPCustomerProfile()
        {
            List<spCheckUserExist_Result> oUserDetsils = null;
            CustomerProfile profile = new CustomerProfile();
            string Initials = "N/A";
            ViewBag.UserId = Session["UserId"].ToString();

            oUserDetsils = BrokerWebDB.BrokerWebDB.GetCustomerDetails(Session["EmailId"].ToString(), Session["UserId"].ToString());

            profile.Address = oUserDetsils[0].Address;
            profile.FirstName = oUserDetsils[0].FirstName;
            profile.LastName = oUserDetsils[0].LastName;
            profile.EmailId = oUserDetsils[0].EmailId;
            profile.PhoneNo = oUserDetsils[0].PhoneNo;

            profile.CompanyName = oUserDetsils[0].CompanyName;
            //profile.HouseType = oUserDetsils[0].HouseType;
            //profile.NoOfCars = Convert.ToString(oUserDetsils[0].NoOfCars);
            //profile.TypeOfEmployment = oUserDetsils[0].TypeOfEmployment;

            if (Session["Company"].ToString() == "APSP")
            {
                profile.NoofEmployee = oUserDetsils[0].NoOfEmp;
                ViewBag.NoofEmployee = profile.NoofEmployee;

                profile.EstPremium = oUserDetsils[0].EstPremium;
                ViewBag.EstPremium = profile.EstPremium;

                profile.Website = oUserDetsils[0].Website;
            }

            //if (oUserDetsils[0].TypeOfEmployment != "" || oUserDetsils[0].TypeOfEmployment != null)
            //{
            //    if (oUserDetsils[0].TypeOfEmployment == "Self Employed")
            //    {
            //        profile.TypeOfEmployment = "Business Owner";
            //    }
            //    else
            //    {
            //        profile.TypeOfEmployment = "Employed";
            //    }
            //}

            profile.ZipCode = oUserDetsils[0].PinCode;

            Session["ZipCode"] = oUserDetsils[0].PinCode;

            profile.ProfilePicture = oUserDetsils[0].ProfilePictureImg;

            if (oUserDetsils[0].ProfilePictureImg != "" && oUserDetsils[0].ProfilePictureImg != null)
            {
                ViewBag.ProfilePic = strDomainName + "" + strProfilePicImageFolder + "" + oUserDetsils[0].ProfilePictureImg;
                ViewBag.Time = DateTime.Now;
                Session["ProfilePic"] = strDomainName + "" + strProfilePicImageFolder + "" + oUserDetsils[0].ProfilePictureImg + "?" + DateTime.Now;
            }
            else
            {
                ViewBag.ProfilePic = "";
            }

            //change 6 sep 2017
            if (oUserDetsils[0].FirstName != "" && oUserDetsils[0].LastName != "")
            {
                Initials = oUserDetsils[0].FirstName[0] + "" + oUserDetsils[0].LastName[0];
            }
            ViewBag.Initials = Initials.ToUpper();
            Session["Initials"] = Initials.ToUpper();
            ViewBag.UserName = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();

            if (Session["DetailsSaved"] != null)
            {
                ViewBag.DetailsSaved = Session["DetailsSaved"].ToString();
                Session["DetailsSaved"] = null;
            }
            else
            {
                ViewBag.DetailsSaved = "";
            }

            //System.Globalization.DateTimeFormatInfo format = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat;

            //string LocalDateTime = DateTime.Now.ToString(format.ShortDatePattern + " " +
            //     format.LongTimePattern);// shashi

            //DateTime utcdate = DateTime.UtcNow;

            //var istdate = TimeZoneInfo.ConvertTimeFromUtc(utcdate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

            //var pstdate = TimeZoneInfo.ConvertTimeFromUtc(utcdate, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
            //string LocalDateTime = DateTime.Now.ToString(); // shashi

            if (Session["ReturnUrl"] != null)
            {
                if (Session["ReturnUrl"].ToString() == "CustomerBriefcase")
                {
                    return RedirectToAction("Briefcase", "BrokkrrBriefcase");
                }
            }
            return View(profile);
        }

        [HttpPost]
        [Authorize]
        public ActionResult APSPCustomerProfile(CustomerProfile Cust, IEnumerable<HttpPostedFileBase> file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //if (Request.Form["Save"] != null)
                    //{
                    string FirstName = "", LastName = "", Email = "", Phone1 = "", Phone2 = "", Phone3 = "", Password = "",
                        TempPass = "", ZipCode = "", ProfilePhoto = "", HouseType = "", Address = "", IsCars = "",
                        Occupation = "", CompanyName = "", Phone = "", FieldName1 = "", RenamedImageName = "", FileName2 = "", NoofEmployee = "", EstPremium = "", Website = "";
                    int NoOfCars = 0, UserId;

                    List<spUpdateCustomerForWeb_Result> CustDetails = null;
                    UserId = Convert.ToInt32(Session["UserId"].ToString());


                    if (Cust.FirstName != null)
                    {
                        FirstName = Cust.FirstName.ToString();
                        Session["FirstName"] = FirstName;
                    }
                    if (Cust.LastName != null)
                    {
                        LastName = Cust.LastName.ToString();
                        Session["LastName"] = LastName;
                    }

                    if (Cust.PhoneNo != null)
                    {
                        Phone1 = Cust.PhoneNo.ToString();
                    }

                    Phone = Phone1;

                    if (Cust.ZipCode != null)
                    {
                        ZipCode = Cust.ZipCode.ToString();
                    }
                    if (Cust.HouseType != null)
                    {
                        HouseType = Cust.HouseType.ToString();
                    }

                    if (Cust.Address != null)
                    {
                        Address = Cust.Address.ToString();
                    }

                    if (Cust.NoOfCars != null)
                    {
                        if (Cust.NoOfCars != "")
                        {
                            NoOfCars = Convert.ToInt32(Cust.NoOfCars.ToString());
                        }
                        else
                        {
                            NoOfCars = 0;
                        }
                    }
                    if (Cust.TypeOfEmployment != null)
                    {
                        if (Cust.TypeOfEmployment.ToString() == "Business Owner")
                        {
                            Occupation = "Self Employed";
                        }
                        else
                        {
                            Occupation = Cust.TypeOfEmployment.ToString();
                        }
                    }
                    if (Cust.CompanyName != null)
                    {
                        CompanyName = Cust.CompanyName.ToString();
                    }

                    if (Cust.NoofEmployee != null)
                    {
                        NoofEmployee = Cust.NoofEmployee.ToString();
                    }

                    if (Cust.EstPremium != null)
                    {
                        EstPremium = Cust.EstPremium.ToString();
                    }

                    if (Cust.Website != null)
                    {
                        Website = Cust.Website.ToString();
                    }

                    //Save Details of Profile Picture

                    //if (Cust.IsCustomerProfileChanged == "Yes")
                    //{
                    //    if (file == null)
                    //    {

                    //    }
                    //    else
                    //    {
                    //        foreach (var f in file)
                    //        {
                    //            if (f != null)
                    //            {
                    //                if (f.ContentLength > 0)
                    //                {
                    //                    int MaxContentLength = 1024 * 1024 * 4;
                    //                    string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf", ".jpe", ".jpeg" };
                    //                    if (!AllowedFileExtensions.Contains
                    //                    (f.FileName.Substring(f.FileName.LastIndexOf('.')).ToLower()))
                    //                    {
                    //                        ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                    //                    }
                    //                    else if (f.ContentLength > MaxContentLength)
                    //                    {
                    //                        ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
                    //                    }
                    //                    else
                    //                    {
                    //                        var fileName = Path.GetFileName(f.FileName);

                    //                        var path = Path.Combine(Server.MapPath("~/UploadedDoc"), fileName);


                    //                        byte[] binaryData;
                    //                        binaryData = new Byte[f.InputStream.Length];
                    //                        long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                    //                        f.InputStream.Close();
                    //                        string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                    //                        string FileName1 = "";

                    //                        string FieldName = "";
                    //                        string ProfilePicFile = "";

                    //                        string ProfilePic = Cust.ProfilePicture.ToString().Replace("C:\\fakepath\\", "");

                    //                        if (ProfilePic == fileName)
                    //                        {
                    //                            FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/ProfilePicture/" + Cust.EmailId.ToString() + "_" + UserId + ".txt");
                    //                            FieldName = "ProfilePicture";
                    //                            FileName2 = Cust.EmailId.ToString() + "_" + UserId + ".txt";
                    //                            ProfilePicFile = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Cust.EmailId.ToString() + "_" + UserId + ".png");
                    //                            bool CheckFile1 = BrokerUtility.CheckFile(ProfilePicFile);
                    //                            byte[] imageBytes1 = Convert.FromBase64String(base64String);

                    //                            //MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);
                    //                            MemoryStream ms1 = new MemoryStream(binaryData, 0, binaryData.Length);

                    //                            //ms1.Write(imageBytes1, 0, imageBytes1.Length);
                    //                            ms1.Write(binaryData, 0, binaryData.Length);

                    //                            System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                    //                            //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Cust.Email.ToString() + "_" + UserId + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                    //                            System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                    //                            thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Cust.EmailId.ToString() + "_" + UserId + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                    //                            FieldName1 = "ProfilePictureImg";
                    //                            RenamedImageName = Cust.EmailId.ToString() + "_" + UserId + ".png";

                    //                            //Check for the file already exist or not 
                    //                            bool CheckFile = BrokerUtility.CheckFile(FileName1);
                    //                            if (CheckFile)
                    //                            {
                    //                                //Create a text file of Base 64 string
                    //                                bool result = BrokerUtility.WriteFile(FileName1, base64String);
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{

                    //}

                    //Save the Details of Customer

                    CustDetails = BrokerWebDB.BrokerWebDB.SaveCustomerProfileDetails(FirstName, LastName, Phone, Address, ZipCode, HouseType, IsCars, NoOfCars, Occupation, CompanyName, FileName2, RenamedImageName, "No", UserId, NoofEmployee, EstPremium, Website);

                    if (CustDetails.Count > 0)
                    {
                        //TempData["CustDetails"] = Cust;
                        Session["DetailsSaved"] = "Save";
                        return RedirectToAction("APSPCustomerProfile", "Profile");
                    }
                    else
                    {
                        return View();
                    }

                    //}
                    //else if (Request.Form["Cancel"] != null)
                    //{
                    //    //TempData["CustDetails"] = Cust;
                    //    return RedirectToAction("CustomerProfile", "Profile");
                    //    //return View("","")
                    //}
                }
                catch (Exception Ex)
                {
                    BrokerUtility.ErrorLog(Convert.ToInt32(Session["UserId"].ToString()), "APSPCustomerProfile_POST_Wesite", Ex.Message.ToString(), "CustomerRegistrationController.cs_APSPCustomerProfile_POST", BrokerUtility.GetIPAddress(Session["UserId"].ToString()));
                    return View();
                }
                return View();
            }
            else
            {
                return View();
            }
        }
    }
}