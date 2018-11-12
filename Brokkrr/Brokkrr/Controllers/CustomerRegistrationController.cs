using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrokerMVC.Models;
using System.Data;
using BrokerMVC.App_Code;
using BrokerMVC.BrokerService;
using System.IO;
using System.Data.SqlClient;
using BrokerMVC.BrokerWebDB;

namespace BrokerMVC.Controllers
{
    public class CustomerRegistrationController : Controller
    {
        // GET: CustomerRegistration
        BrokerDBEntities DB = new BrokerDBEntities();

        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            CustomerRegistration Data = new CustomerRegistration();

            List<spCheckUserExist_Result> oUserDetsils = null;
            try
            {
                if (Session["EmailId"].ToString() != "")
                {
                    oUserDetsils = BrokerWebDB.BrokerWebDB.GetCustomerDetails(Session["EmailId"].ToString(), Session["UserId"].ToString());

                    if (oUserDetsils != null)
                    {
                        Data.FirstName = oUserDetsils[0].FirstName;
                        Data.LastName = oUserDetsils[0].LastName;
                        Data.Email = oUserDetsils[0].EmailId;
                        Data.PhoneNo1 = oUserDetsils[0].PhoneNo;
                        Data.HouseType = oUserDetsils[0].HouseType;
                        Data.Address = oUserDetsils[0].AddressofHouse;
                        Data.ZipCode = oUserDetsils[0].PinCode;
                        Data.CompanyName = oUserDetsils[0].CompanyName;

                        if (oUserDetsils[0].TypeOfEmployment != "" || oUserDetsils[0].TypeOfEmployment != null)
                        {
                            if (oUserDetsils[0].TypeOfEmployment == "Self Employed")
                            {
                                Data.Occupation = "Business Owner";
                            }
                            else
                            {
                                Data.Occupation = oUserDetsils[0].TypeOfEmployment;
                            }
                        }
                       

                        Data.IsProfilePhotoChanged = "No";

                        if (oUserDetsils[0].IsHavingCar == true)
                        {
                            Data.IsCars = "Yes";
                            Data.NumberofCars = oUserDetsils[0].NoOfCars.ToString();
                        }
                        else
                        {
                            Data.IsCars = "No";
                            Data.NumberofCars = "0";
                        }

                        if (oUserDetsils[0].ProfilePicture != "" && oUserDetsils[0].ProfilePicture != null)
                        {
                            Data.ProfilePhoto = oUserDetsils[0].FirstName + "_" + oUserDetsils[0].LastName + ".Png";
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "Index_GET_Website", Ex.Message.ToString(), "CustomerRegistrationController.cs_Index_GET()", "");
            }
            return View(Data);

        }

        [HttpPost]
        [Authorize]
        public ActionResult Index(CustomerRegistration Cust, IEnumerable<HttpPostedFileBase> file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Request.Form["Submit"] != null)
                    {
                        string FirstName = "", LastName = "", Email = "", Phone1 = "", Phone2 = "", Phone3 = "", Password = "",
                            TempPass = "", ZipCode = "", ProfilePhoto = "", HouseType = "", Address = "", IsCars = "",
                            Occupation = "", CompanyName = "", Phone = "", FieldName1 = "", RenamedImageName = "", FileName2 = "";
                        int NoOfCars = 0, UserId;

                        //bool IsHavingCars = false;

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

                        if (Cust.PhoneNo1 != null)
                        {
                            Phone1 = Cust.PhoneNo1.ToString();
                        }

                        Phone = Phone1;//Change 29Dec16

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
                        if (Cust.IsCars != null)
                        {
                            IsCars = Cust.IsCars.ToString();
                        }
                        if (Cust.NumberofCars != null)
                        {
                            if (IsCars == "Yes")
                            {
                                NoOfCars = Convert.ToInt32(Cust.NumberofCars.ToString());
                            }
                            else
                            {
                                NoOfCars = 0;
                            }
                        }
                        if (Cust.Occupation != null)
                        {
                            if (Cust.Occupation.ToString() == "Business Owner")
                            {
                                Occupation = "Self Employed";
                            }
                            else
                            {
                                Occupation = Cust.Occupation.ToString();
                            }
                        }
                        if (Cust.CompanyName != null)
                        {
                            CompanyName = Cust.CompanyName.ToString();
                        }

                        //Save Details of Profile Picture

                        if (Cust.IsProfilePhotoChanged == "Yes")
                        {
                            if (file == null)
                            {

                            }
                            else
                            {
                                foreach (var f in file)
                                {
                                    if (f != null)
                                    {
                                        if (f.ContentLength > 0)
                                        {
                                            int MaxContentLength = 1024 * 1024 * 4; //Size = 4 MB
                                            string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf", ".jpe", ".jpeg" };
                                            if (!AllowedFileExtensions.Contains
                                            (f.FileName.Substring(f.FileName.LastIndexOf('.')).ToLower()))
                                            {
                                                ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                                            }
                                            else if (f.ContentLength > MaxContentLength)
                                            {
                                                ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
                                            }
                                            else
                                            {
                                                var fileName = Path.GetFileName(f.FileName);
                                                //var Name = Path.GetFileNameWithoutExtension(f.FileName);
                                                var path = Path.Combine(Server.MapPath("~/UploadedDoc"), fileName);
                                                //Save file on server.
                                                //f.SaveAs(path);

                                                //Convert input file to Base 64 string

                                                byte[] binaryData;
                                                binaryData = new Byte[f.InputStream.Length];
                                                long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                                f.InputStream.Close();
                                                string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                                string FileName1 = "";

                                                string FieldName = "";
                                                string ProfilePicFile = "";

                                                string ProfilePic = Cust.ProfilePhoto.ToString().Replace("C:\\fakepath\\", "");

                                                if (ProfilePic == fileName)
                                                {
                                                    FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/ProfilePicture/" + Cust.Email.ToString() + "_" + UserId + ".txt");
                                                    FieldName = "ProfilePicture";
                                                    FileName2 = Cust.Email.ToString() + "_" + UserId + ".txt";
                                                    ProfilePicFile = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Cust.Email.ToString() + "_" + UserId + ".png");
                                                    bool CheckFile1 = BrokerUtility.CheckFile(ProfilePicFile);
                                                    byte[] imageBytes1 = Convert.FromBase64String(base64String);

                                                    //MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);
                                                    MemoryStream ms1 = new MemoryStream(binaryData, 0, binaryData.Length);

                                                    //ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                                    ms1.Write(binaryData, 0, binaryData.Length);

                                                    System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                                    //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Cust.Email.ToString() + "_" + UserId + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                                    System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                                    thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Cust.Email.ToString() + "_" + UserId + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                                    FieldName1 = "ProfilePictureImg";
                                                    RenamedImageName = Cust.Email.ToString() + "_" + UserId + ".png";

                                                    //Check for the file already exist or not 
                                                    bool CheckFile = BrokerUtility.CheckFile(FileName1);
                                                    if (CheckFile)
                                                    {
                                                        //Create a text file of Base 64 string
                                                        bool result = BrokerUtility.WriteFile(FileName1, base64String);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {

                        }

                        //Save the Details of Customer

                        CustDetails = BrokerWebDB.BrokerWebDB.SaveCustomerProfileDetails(FirstName, LastName, Phone, Address, ZipCode, HouseType, IsCars, NoOfCars, Occupation, CompanyName, FileName2, RenamedImageName, Cust.IsProfilePhotoChanged, UserId,"","","");

                        if (CustDetails.Count > 0)
                        {
                            //TempData["CustDetails"] = Cust;
                            return RedirectToAction("CustomerProfile", "Profile");
                        }
                        else
                        {
                            return View();
                        }
                        //BrokerUtility.SaveCustomerBasicDetails(FirstName, LastName, Phone, Email, Address, ZipCode, TempPass, Encryptrandom, HouseType, IsCars, NoOfCars, Occupation, CompanyName);                   
                    }
                    else if (Request.Form["Cancel"] != null)
                    {
                        //TempData["CustDetails"] = Cust;
                        return RedirectToAction("CustomerProfile", "Profile");
                        //return View("","")
                    }
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
        [AllowAnonymous]
        public ActionResult CustomerSignUp()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CustomerSignUp(CustomerSignUp Cust, IEnumerable<HttpPostedFileBase> file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string FirstName = "", LastName = "", Email = "", Phone1 = "", Phone2 = "", Phone3 = "", Password = "",
                        TempPass = "", ZipCode = "", ProfilePhoto = "", HouseType = "", Address = "", IsCars = "",
                        Occupation = "", CompanyName = "", Phone = "", FieldName1 = "", RenamedImageName = "";
                    int NoOfCars = 0, UserId;

                    //bool IsHavingCars = false;

                    List<uspSaveCustomerBasicDetails_Result> User1 = null;

                    if (Cust.FirstName != null)
                    {
                        FirstName = Cust.FirstName.ToString();
                    }
                    if (Cust.LastName != null)
                    {
                        LastName = Cust.LastName.ToString();
                    }
                    if (Cust.Email != null)
                    {
                        Email = Cust.Email.ToString();
                        Session["EmailId"] = Email;
                    }
                    if (Cust.PhoneNo1 != null)
                    {
                        Phone1 = Cust.PhoneNo1.ToString();
                    }

                    Phone = Phone1;//Change 29Dec16

                    if (Cust.Password != null)
                    {
                        Password = Cust.Password.ToString();
                        TempPass = BrokerUtility.EncryptURL(Password);
                    }
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
                    if (Cust.IsCars != null)
                    {
                        IsCars = Cust.IsCars.ToString();
                    }
                    if (Cust.NumberofCars != null)
                    {
                        if (IsCars == "Yes")
                        {
                            NoOfCars = Convert.ToInt32(Cust.NumberofCars.ToString());
                        }
                        else
                        {
                            NoOfCars = 0;
                        }
                    }
                    if (Cust.Occupation != null)
                    {
                        if (Cust.Occupation.ToString() == "Business Owner")
                        {
                            Occupation = "Self Employed";
                        }
                        else
                        {
                            Occupation = Cust.Occupation.ToString();
                        }
                    }
                    if (Cust.CompanyName != null)
                    {
                        CompanyName = Cust.CompanyName.ToString();
                    }

                    string random = BrokerWSUtility.GetRandomNumber();
                    string Encryptrandom = BrokerUtility.EncryptURL(random);
                    Session["random"] = Encryptrandom;

                    User1 = BrokerUtility.SaveCustomerBasicDetails(FirstName, LastName, Phone, Email, Address, ZipCode, TempPass, Encryptrandom, HouseType, IsCars, NoOfCars, Occupation, CompanyName,"","","","Brokkrr");
                    int Flag = 0;

                    if (User1.Count > 0)
                    {
                        UserId = Convert.ToInt32(User1[0].UserId.ToString());
                        Session["UserId"] = UserId;

                        //////////////////// Access Profile Pic and Resume Files //////////////////////////
                        if (file == null)
                        {

                        }
                        else
                        {
                            foreach (var f in file)
                            {
                                if (f != null)
                                {
                                    if (f.ContentLength > 0)
                                    {
                                        int MaxContentLength = 1024 * 1024 * 4; //Size = 4 MB
                                        string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf", ".jpe", ".jpeg" };
                                        if (!AllowedFileExtensions.Contains
                                        (f.FileName.Substring(f.FileName.LastIndexOf('.')).ToLower()))
                                        {
                                            ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                                        }
                                        else if (f.ContentLength > MaxContentLength)
                                        {
                                            ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
                                        }
                                        else
                                        {
                                            var fileName = Path.GetFileName(f.FileName);
                                            //var Name = Path.GetFileNameWithoutExtension(f.FileName);
                                            var path = Path.Combine(Server.MapPath("~/UploadedDoc"), fileName);
                                            //Save file on server.
                                            //f.SaveAs(path);

                                            //Convert input file to Base 64 string

                                            byte[] binaryData;
                                            binaryData = new Byte[f.InputStream.Length];
                                            long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                            f.InputStream.Close();
                                            string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                            string FileName1 = "";
                                            string FileName2 = "";
                                            string FieldName = "";

                                            string ProfilePic = Cust.ProfilePhoto.ToString().Replace("C:\\fakepath\\", "");

                                            if (ProfilePic == fileName)
                                            {
                                                FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/ProfilePicture/" + Cust.Email.ToString() + "_" + UserId + ".txt");
                                                FieldName = "ProfilePicture";
                                                FileName2 = Cust.Email.ToString() + "_" + UserId + ".txt";

                                                //Save Image on Server also.

                                                // Convert byte[] to Image

                                                byte[] imageBytes1 = Convert.FromBase64String(base64String);
                                                MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                                ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                                System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                                //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Email + "_" + UserId + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                                System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                                thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Email + "_" + UserId + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                                FieldName1 = "ProfilePictureImg";
                                                RenamedImageName = Cust.Email.ToString() + "_" + UserId + ".png";
                                            }

                                            //Check for the file already exist or not 
                                            bool CheckFile = BrokerUtility.CheckFile(FileName1);
                                            if (CheckFile)
                                            {
                                                //Create a text file of Base 64 string
                                                bool result = BrokerUtility.WriteFile(FileName1, base64String);

                                                if (result)
                                                {
                                                    Flag = BrokerUtility.SaveBrokerFiles(FileName2, UserId, FieldName, FieldName1, RenamedImageName);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Send Verification Link on Email Id

                        //string random = BrokerWSUtility.GetRandomNumber();

                        bool EmailFlag = false;

                        EmailFlag = BrokerWSUtility.SendRegistrationEmail(Session["EmailId"].ToString(), Session["random"].ToString(), Session["UserId"].ToString(), "Customer");//SendRegistrationEmailFromWebSite
                        if (EmailFlag)
                        {
                            ViewBag.VerificationMessage = "You are registered successfully but yet not activated. ";
                            ViewBag.VerificationMessage1 = "Please accept your verification email.";
                            return View("CustomerSuccess");
                        }
                        else
                        {
                            //If User registerd successfully, but verification link has not
                            //been sent over EmailId
                            //ViewBag.VerificationMessage = "You are registered successfully but yet not activated. <br/>Please accept your verification email.";
                            return View("CustomerError");
                        }

                    }
                    // return View();
                }
                catch (Exception Ex)
                {
                    BrokerUtility.ErrorLog(Convert.ToInt32(Session["UserId"].ToString()), "CustomerSignUp_Website", Ex.Message.ToString(), "CustomerRegistrationController.cs_CustomerSignUp", BrokerUtility.GetIPAddress(Session["UserId"].ToString()));
                }
                return View();
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult IsEmailIdAlreadyExist(MeinekeCustomerSignUp Cust)
        {
            DataSet dsCheckUserExist = BrokerUtility.CheckUSerExist(Cust.EmailId.ToString());

            if (dsCheckUserExist.Tables.Count > 0)
            {
                return Json("Email Id already registered.", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult MeinekeCustomerSignUp()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult MeinekeCustomerSignUp(MeinekeCustomerSignUp Cust, IEnumerable<HttpPostedFileBase> file)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    string FirstName = "", LastName = "", Email = "", Phone1 = "", Phone2 = "", Phone3 = "", Password = "",
                         TempPass = "", ZipCode = "", ProfilePhoto = "", HouseType = "", Address = "", IsCars = "",
                         Occupation = "", CompanyName = "", Phone = "", FieldName1 = "", RenamedImageName = "", FileName2 = "", NoofEmployee = "", EstPremium = "", Website = "";
                    int NoOfCars = 0, UserId = 0;

                    List<uspSaveCustomerBasicDetails_Result> User1 = null;
                    //UserId = Convert.ToInt32(Session["UserId"].ToString());

                    if (Cust.FirstName != null)
                    {
                        FirstName = Cust.FirstName.ToString();
                    }
                    if (Cust.LastName != null)
                    {
                        LastName = Cust.LastName.ToString();
                    }
                    if (Cust.CompanyName != null)
                    {
                        CompanyName = Cust.CompanyName.ToString();
                    }
                    if (Cust.Address != null)
                    {
                        Address = Cust.Address.ToString();
                    }
                    if (Cust.EmailId != null)
                    {
                        Email = Cust.EmailId.ToString();
                    }
                    if (Cust.PhoneNo != null)
                    {
                        Phone = Cust.PhoneNo.ToString();
                    }
                    if (Cust.Website != null)
                    {
                        Website = Cust.Website.ToString();
                    }
                    if (Cust.ZipCode != null)
                    {
                        ZipCode = Cust.ZipCode.ToString();
                    }
                    if (Cust.NoofEmployees != null)
                    {
                        NoofEmployee = Cust.NoofEmployees.ToString();
                    }
                    if (Cust.EstPremium != null)
                    {
                        EstPremium = Cust.EstPremium.ToString();
                    }

                    string random = BrokerWSUtility.GetRandomNumber();
                    string Encryptrandom = BrokerUtility.EncryptURL(random);
                    Session["random"] = Encryptrandom;

                    User1 = BrokerUtility.SaveCustomerBasicDetails(FirstName, LastName, Phone, Email, Address, ZipCode, TempPass, Encryptrandom, HouseType, IsCars, NoOfCars, Occupation, CompanyName, NoofEmployee, EstPremium, Website, "Meineke");
                    int Flag = 0;

                    if (User1.Count > 0)
                    {
                        UserId = Convert.ToInt32(User1[0].UserId.ToString());
                        Session["UserId"] = UserId;
                        Session["EmailId"] = User1[0].EmailId.ToString();

                        //////////////////// Access Profile Pic and Resume Files //////////////////////////
                        if (file == null)
                        {

                        }
                        else
                        {
                            foreach (var f in file)
                            {
                                if (f != null)
                                {
                                    if (f.ContentLength > 0)
                                    {
                                        int MaxContentLength = 1024 * 1024 * 4; //Size = 4 MB
                                        string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf", ".jpe", ".jpeg" };
                                        if (!AllowedFileExtensions.Contains
                                        (f.FileName.Substring(f.FileName.LastIndexOf('.')).ToLower()))
                                        {
                                            ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                                        }
                                        else if (f.ContentLength > MaxContentLength)
                                        {
                                            ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
                                        }
                                        else
                                        {
                                            var fileName = Path.GetFileName(f.FileName);
                                            //var Name = Path.GetFileNameWithoutExtension(f.FileName);
                                            var path = Path.Combine(Server.MapPath("~/UploadedDoc"), fileName);
                                            //Save file on server.
                                            //f.SaveAs(path);

                                            //Convert input file to Base 64 string

                                            byte[] binaryData;
                                            binaryData = new Byte[f.InputStream.Length];
                                            long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                            f.InputStream.Close();
                                            string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                            string FileName1 = "";
                                            //  string FileName2 = "";
                                            string FieldName = "";

                                            string ProfilePic = Cust.ProfilePicture.ToString().Replace("C:\\fakepath\\", "");

                                            if (ProfilePic == fileName)
                                            {
                                                FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/ProfilePicture/" + Cust.EmailId.ToString() + "_" + UserId + ".txt");
                                                FieldName = "ProfilePicture";
                                                FileName2 = Cust.EmailId.ToString() + "_" + UserId + ".txt";

                                                //Save Image on Server also.

                                                // Convert byte[] to Image

                                                byte[] imageBytes1 = Convert.FromBase64String(base64String);
                                                MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                                ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                                System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                                //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Email + "_" + UserId + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                                System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                                thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Email + "_" + UserId + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                                FieldName1 = "ProfilePictureImg";
                                                RenamedImageName = Cust.EmailId.ToString() + "_" + UserId + ".png";
                                            }

                                            //Check for the file already exist or not 
                                            bool CheckFile = BrokerUtility.CheckFile(FileName1);
                                            if (CheckFile)
                                            {
                                                //Create a text file of Base 64 string
                                                bool result = BrokerUtility.WriteFile(FileName1, base64String);

                                                if (result)
                                                {
                                                    Flag = BrokerUtility.SaveBrokerFiles(FileName2, UserId, FieldName, FieldName1, RenamedImageName);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Send Verification Link on Email Id

                        //string random = BrokerWSUtility.GetRandomNumber();

                        bool EmailFlag = false;

                        EmailFlag = BrokerWSUtility.SendRegistrationEmailFromWebSite(Session["EmailId"].ToString(), Session["random"].ToString(), Session["UserId"].ToString(), "Customer");
                        if (EmailFlag)
                        {
                            ViewBag.VerificationMessage = "You are registered successfully but yet not activated. ";
                            ViewBag.VerificationMessage1 = "Please accept your verification email.";
                            ViewBag.Company = "Meineke";
                            return View("CustomerSuccess");
                        }
                        else
                        {
                            //If User registerd successfully, but verification link has not
                            //been sent over EmailId
                            //ViewBag.VerificationMessage = "You are registered successfully but yet not activated. <br/>Please accept your verification email.";
                            ViewBag.Company = "Meineke";
                            return View("CustomerError");
                        }

                    }
                }
                catch (Exception Ex)
                {
                    BrokerUtility.ErrorLog(Convert.ToInt32(Session["UserId"].ToString()), "Index_POST_Wesite", Ex.Message.ToString(), "CustomerRegistrationController.cs_Index_POST", BrokerUtility.GetIPAddress(Session["UserId"].ToString()));
                    return View();
                }
                return View();
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult APSPCustomerSignUp()
        {
            //ViewBag.Authentication = "Unauthorize";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult APSPCustomerSignUp(MeinekeCustomerSignUp Cust, IEnumerable<HttpPostedFileBase> file)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    string FirstName = "", LastName = "", Email = "", Phone1 = "", Phone2 = "", Phone3 = "", Password = "",
                         TempPass = "", ZipCode = "", ProfilePhoto = "", HouseType = "", Address = "", IsCars = "",
                         Occupation = "", CompanyName = "", Phone = "", FieldName1 = "", RenamedImageName = "", FileName2 = "", NoofEmployee = "", EstPremium = "", Website = "";
                    int NoOfCars = 0, UserId = 0;

                    List<uspSaveCustomerBasicDetails_Result> User1 = null;
                    //UserId = Convert.ToInt32(Session["UserId"].ToString());

                    if (Cust.FirstName != null)
                    {
                        FirstName = Cust.FirstName.ToString();
                    }
                    if (Cust.LastName != null)
                    {
                        LastName = Cust.LastName.ToString();
                    }
                    if (Cust.CompanyName != null)
                    {
                        CompanyName = Cust.CompanyName.ToString();
                    }
                    if (Cust.Address != null)
                    {
                        Address = Cust.Address.ToString();
                    }
                    if (Cust.EmailId != null)
                    {
                        Email = Cust.EmailId.ToString();
                    }
                    if (Cust.PhoneNo != null)
                    {
                        Phone = Cust.PhoneNo.ToString();
                    }
                    if (Cust.Website != null)
                    {
                        Website = Cust.Website.ToString();
                    }
                    if (Cust.ZipCode != null)
                    {
                        ZipCode = Cust.ZipCode.ToString();
                    }
                    if (Cust.NoofEmployees != null)
                    {
                        NoofEmployee = Cust.NoofEmployees.ToString();
                    }
                    if (Cust.EstPremium != null)
                    {
                        EstPremium = Cust.EstPremium.ToString();
                    }

                    string random = BrokerWSUtility.GetRandomNumber();
                    string Encryptrandom = BrokerUtility.EncryptURL(random);
                    Session["random"] = Encryptrandom;

                    User1 = BrokerUtility.SaveCustomerBasicDetails(FirstName, LastName, Phone, Email, Address, ZipCode, TempPass, Encryptrandom, HouseType, IsCars, NoOfCars, Occupation, CompanyName, NoofEmployee, EstPremium, Website,"APSP");
                    int Flag = 0;

                    if (User1.Count > 0)
                    {
                        UserId = Convert.ToInt32(User1[0].UserId.ToString());
                        Session["UserId"] = UserId;
                        Session["EmailId"] = User1[0].EmailId.ToString();

                        //////////////////// Access Profile Pic and Resume Files //////////////////////////
                        if (file == null)
                        {

                        }
                        else
                        {
                            foreach (var f in file)
                            {
                                if (f != null)
                                {
                                    if (f.ContentLength > 0)
                                    {
                                        int MaxContentLength = 1024 * 1024 * 4; //Size = 4 MB
                                        string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf", ".jpe", ".jpeg" };
                                        if (!AllowedFileExtensions.Contains
                                        (f.FileName.Substring(f.FileName.LastIndexOf('.')).ToLower()))
                                        {
                                            ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                                        }
                                        else if (f.ContentLength > MaxContentLength)
                                        {
                                            ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
                                        }
                                        else
                                        {
                                            var fileName = Path.GetFileName(f.FileName);
                                            //var Name = Path.GetFileNameWithoutExtension(f.FileName);
                                            var path = Path.Combine(Server.MapPath("~/UploadedDoc"), fileName);
                                            //Save file on server.
                                            //f.SaveAs(path);

                                            //Convert input file to Base 64 string

                                            byte[] binaryData;
                                            binaryData = new Byte[f.InputStream.Length];
                                            long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                            f.InputStream.Close();
                                            string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                            string FileName1 = "";
                                            //  string FileName2 = "";
                                            string FieldName = "";

                                            string ProfilePic = Cust.ProfilePicture.ToString().Replace("C:\\fakepath\\", "");

                                            if (ProfilePic == fileName)
                                            {
                                                FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/ProfilePicture/" + Cust.EmailId.ToString() + "_" + UserId + ".txt");
                                                FieldName = "ProfilePicture";
                                                FileName2 = Cust.EmailId.ToString() + "_" + UserId + ".txt";

                                                //Save Image on Server also.

                                                // Convert byte[] to Image

                                                byte[] imageBytes1 = Convert.FromBase64String(base64String);
                                                MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                                ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                                System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                                //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Email + "_" + UserId + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                                System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                                thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Email + "_" + UserId + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                                FieldName1 = "ProfilePictureImg";
                                                RenamedImageName = Cust.EmailId.ToString() + "_" + UserId + ".png";
                                            }

                                            //Check for the file already exist or not 
                                            bool CheckFile = BrokerUtility.CheckFile(FileName1);
                                            if (CheckFile)
                                            {
                                                //Create a text file of Base 64 string
                                                bool result = BrokerUtility.WriteFile(FileName1, base64String);

                                                if (result)
                                                {
                                                    Flag = BrokerUtility.SaveBrokerFiles(FileName2, UserId, FieldName, FieldName1, RenamedImageName);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Send Verification Link on Email Id

                        //string random = BrokerWSUtility.GetRandomNumber();

                        bool EmailFlag = false;

                        EmailFlag = BrokerWSUtility.SendRegistrationEmailFromWebSite(Session["EmailId"].ToString(), Session["random"].ToString(), Session["UserId"].ToString(), "Customer");
                        //EmailFlag = true;
                        if (EmailFlag)
                        {
                            ViewBag.VerificationMessage = "You are registered successfully but yet not activated. ";
                            ViewBag.VerificationMessage1 = "Please accept your verification email.";
                            ViewBag.Company = "APSP";
                            return View("CustomerSuccess");
                        }
                        else
                        {
                            //If User registerd successfully, but verification link has not
                            //been sent over EmailId
                            //ViewBag.VerificationMessage = "You are registered successfully but yet not activated. <br/>Please accept your verification email.";
                            ViewBag.Company = "APSP";
                            return View("CustomerError");
                        }

                    }
                }
                catch (Exception Ex)
                {
                    BrokerUtility.ErrorLog(Convert.ToInt32(Session["UserId"].ToString()), "Index_POST_Wesite", Ex.Message.ToString(), "CustomerRegistrationController.cs_Index_POST", BrokerUtility.GetIPAddress(Session["UserId"].ToString()));
                    return View();
                }
                return View();
            }
            return View();
        }


    }
}