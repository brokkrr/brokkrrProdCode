using BrokerMVC.App_Code;
using BrokerMVC.BrokerService;
using BrokerMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;


namespace BrokerMVC.Controllers
{
    public class BrokerRegistrationController : Controller
    {
        // GET: BrokerRegistration
        public static string strDomainName = ConfigurationManager.AppSettings["DomainName"].ToString();
        public static string strProfilePicForlderName = ConfigurationManager.AppSettings["ProfilePicForlderName"].ToString();

        public static string strProfilePicImageFolder = ConfigurationManager.AppSettings["ProfilePicImageFolder"].ToString();
        public static string strResumeImageFolder = ConfigurationManager.AppSettings["ResumeImageFolder"].ToString();
        public static string strResumeForlderName = ConfigurationManager.AppSettings["ResumeForlderName"].ToString();

        public static string strUploadedCompLogoFolder = ConfigurationManager.AppSettings["UploadedCompLogoFolder"].ToString();
        public static string strEducationLogoFolder = ConfigurationManager.AppSettings["EducationLogo"].ToString();

        public static string strCompanyLogoFolder = ConfigurationManager.AppSettings["ExperienceCompLogoFolder"].ToString();
        public static string striPhoneAppPath = ConfigurationManager.AppSettings["iPhoneAppPath"].ToString();
        public static string strAndroidAppPath = ConfigurationManager.AppSettings["AndroidAppPath"].ToString();

        public static string strGoogleMapKey = ConfigurationManager.AppSettings["GoogleMapKey"].ToString();


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.iPhoneAppPath = striPhoneAppPath;
            ViewBag.AndroidAppPath = strAndroidAppPath;

            var db = new BrokerDBEntities();
            ViewBag.CompanyMaster = new SelectList(db.CompanyMasters.ToList(), "CompanyId", "CompanyName");

            ViewBag.IndustryMaster = new SelectList(db.IndustryMasters.ToList(), "IndustryId", "IndustryName").OrderBy(c => c.Text);
            int id = 0;

            ViewBag.SubIndustryMaster = new SelectList(db.SubIndustryMasters.Where(c => c.IndustryId == id).Select(c => new { Value = c.Id, Text = c.SICCode + " - " + c.SubIndustryName }).ToList().OrderBy(c => c.Text), "Id", "SubIndustryName");

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult BrokerIndex()
        {
            var db = new BrokerDBEntities();
            ViewBag.CompanyMaster = new SelectList(db.CompanyMasters.ToList(), "CompanyId", "CompanyName");

            ViewBag.IndustryMaster = new SelectList(db.IndustryMasters.ToList(), "IndustryId", "IndustryName").OrderBy(c => c.Text);
            int id = 1;

            ViewBag.SubIndustryMaster = new SelectList(db.SubIndustryMasters.Where(c => c.IndustryId == id).Select(c => new { Value = c.Id, Text = c.SICCode + " - " + c.SubIndustryName }).ToList().OrderBy(c => c.Text), "Id", "SubIndustryName");

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(AllModels data, IEnumerable<HttpPostedFileBase> file, FormCollection Form, string[] chkLanguages, string[] CompanyNameRpt, string Industry1, string[] chkSubIndustry1, string Industry2, string[] chkSubIndustry2, string Industry3, string[] chkSubIndustry3)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Request.Form["SUBMIT"] != null)
                    {
                        //string strDDLValue = Form["CompanyName1"].ToString();

                        #region Save Broker Details

                        List<uspSaveBrokerBasicDetails_Result> User = null;

                        string FirstName = "", LastName = "", Phone = "", Email = "", Title = "", Bio = "",
                           Language = "", Specialities = "", Phone1 = "", Password = "", TempPass = "", random = "", Area = "", Company = "",
                            ProfilePic = "", CompanyLogo = "", CompanyLogoPath = "", ProfilePicPath = "";

                        string Degree1 = "", Degree2 = "", Degree3 = "", Degree4 = "", Degree5 = "";
                        string School1 = "", School2 = "", School3 = "", School4 = "", School5 = "";
                        string Year1 = "", Year2 = "", Year3 = "", Year4 = "", Year5 = "";
                        string EducationLogo1 = "", EducationLogo2 = "", EducationLogo3 = "", EducationLogo4 = "", EducationLogo5 = "";

                        string HomeValue = "", AutoType = "", Revenue = "", Employees = "", CoverageAmt = "";

                        string IndustryId = "", SubIndustryId = "", longitude = "", latitude = "";

                        string Addr1 = "", Addr2 = "", Addr3 = "", Addr4 = "", Addr5 = "";
                        string ZipCode = "", ZipCode1 = "", ZipCode2 = "", ZipCode3 = "", ZipCode4 = "", ZipCode5 = "", OnlyZipCodes = "";

                        string ExpLogo1 = "", ExpLogo2 = "", ExpLogo3 = "", ExpLogo4 = "", ExpLogo5 = "", ExpLogo6 = "", ExpLogo7 = "";
                        string ExpComp1 = "", ExpComp2 = "", ExpComp3 = "", ExpComp4 = "", ExpComp5 = "", ExpComp6 = "", ExpComp7 = "";
                        string ExpDesig1 = "", ExpDesig2 = "", ExpDesig3 = "", ExpDesig4 = "", ExpDesig5 = "", ExpDesig6 = "", ExpDesig7 = "";
                        string ExpFromMonth1 = "", ExpFromMonth2 = "", ExpFromMonth3 = "", ExpFromMonth4 = "", ExpFromMonth5 = "", ExpFromMonth6 = "", ExpFromMonth7 = "";
                        string ExpFromYear1 = "", ExpFromYear2 = "", ExpFromYear3 = "", ExpFromYear4 = "", ExpFromYear5 = "", ExpFromYear6 = "", ExpFromYear7 = "";
                        string ExpToMonth1 = "", ExpToMonth2 = "", ExpToMonth3 = "", ExpToMonth4 = "", ExpToMonth5 = "", ExpToMonth6 = "", ExpToMonth7 = "";
                        string ExpToYear1 = "", ExpToYear2 = "", ExpToYear3 = "", ExpToYear4 = "", ExpToYear5 = "", ExpToYear6 = "", ExpToYear7 = "";


                        int UserId = 0, Flag = 0;

                        if (data.BrokerInfo[0].FirstName != null)
                        {
                            FirstName = data.BrokerInfo[0].FirstName.ToString();
                        }
                        if (data.BrokerInfo[0].LastName != null)
                        {
                            LastName = data.BrokerInfo[0].LastName.ToString();
                        }

                        if (data.BrokerInfo[0].PhoneNo1 != null)
                        {
                            Phone1 = data.BrokerInfo[0].PhoneNo1.ToString();
                        }

                        Phone = Phone1;

                        if (data.BrokerInfo[0].Email != null)
                        {
                            Email = data.BrokerInfo[0].Email.ToString();
                        }

                        if (data.BrokerInfo[0].Bio != null)
                        {
                            Bio = data.BrokerInfo[0].Bio.ToString();
                        }

                        if (data.BrokerInfo[0].Password != null)
                        {
                            Password = data.BrokerInfo[0].Password.ToString();
                            TempPass = BrokerUtility.EncryptURL(Password);
                        }

                        if (data.BrokerInfo[0].Area != null)
                        {
                            Area = data.BrokerInfo[0].Area.ToString();
                        }

                        if (data.BrokerInfo[0].ZipCode != null)
                        {
                            ZipCode1 = data.BrokerInfo[0].ZipCode.ToString();
                            Addr1 = GetPositionFromZip(ZipCode1);

                            ZipCode = ZipCode1 + ":" + Addr1;
                            OnlyZipCodes = ZipCode1;
                        }

                        if (data.BrokerInfo[0].ZipCode2 != null)
                        {
                            ZipCode2 = data.BrokerInfo[0].ZipCode2.ToString();
                            Addr2 = GetPositionFromZip(ZipCode2);

                            ZipCode = ZipCode + ";" + ZipCode2 + ":" + Addr2;
                            OnlyZipCodes = OnlyZipCodes + "," + ZipCode2;
                        }

                        if (data.BrokerInfo[0].ZipCode3 != null)
                        {
                            ZipCode3 = data.BrokerInfo[0].ZipCode3.ToString();
                            Addr3 = GetPositionFromZip(ZipCode3);

                            ZipCode = ZipCode + ";" + ZipCode3 + ":" + Addr3;
                            OnlyZipCodes = OnlyZipCodes + "," + ZipCode3;
                        }

                        if (data.BrokerInfo[0].ZipCode4 != null)
                        {
                            ZipCode4 = data.BrokerInfo[0].ZipCode4.ToString();
                            Addr4 = GetPositionFromZip(ZipCode4);

                            ZipCode = ZipCode + ";" + ZipCode4 + ":" + Addr4;
                            OnlyZipCodes = OnlyZipCodes + "," + ZipCode4;
                        }

                        if (data.BrokerInfo[0].ZipCode5 != null)
                        {
                            ZipCode5 = data.BrokerInfo[0].ZipCode5.ToString();
                            Addr5 = GetPositionFromZip(ZipCode5);

                            ZipCode = ZipCode + ";" + ZipCode5 + ":" + Addr5;
                            OnlyZipCodes = OnlyZipCodes + "," + ZipCode5;
                        }

                        /***************Get Longitude and Latitude from Zip Code***************/
                        /***************End of Get Longitude and Latitude from Zip Code***************/


                        if (data.BrokerInfo[0].Title != null)
                        {
                            Title = data.BrokerInfo[0].Title.ToString();
                        }

                        if (data.BrokerInfo[0].Company != null)
                        {
                            Company = data.BrokerInfo[0].Company.ToString();
                        }

                        if (data.BrokerInfo[0].ProfilePhoto != null)
                        {
                            ProfilePic = data.BrokerInfo[0].ProfilePhoto.ToString().Replace("C:\\fakepath\\", "");
                        }

                        if (data.BrokerInfo[0].CompanyLogo != null)
                        {
                            CompanyLogo = data.BrokerInfo[0].CompanyLogo.ToString().Replace("C:\\fakepath\\", ""); ;
                        }

                        #region Save Profile Pic--New Logic
                        if (ProfilePic != "")
                        {
                            if (data.BrokerInfo[0].HiddenProfilePhoto.ToString() != "" && data.BrokerInfo[0].HiddenProfilePhoto.ToString() != null)
                            {

                                ProfilePicPath = Email + ".png";

                                //byte[] binaryData;
                                //binaryData = new Byte[f.InputStream.Length];
                                //long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                //f.InputStream.Close();
                                //base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                byte[] imageBytes1 = Convert.FromBase64String(data.BrokerInfo[0].HiddenProfilePhoto.ToString());
                                MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + data.BrokerInfo[0].Email.ToString() + ".png");
                                bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Email + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                            }
                        }
                        #endregion Save Profile Pic--New Logic

                        #region Save Company Logo--New Logic
                        if (CompanyLogo != "")
                        {
                            if (data.BrokerInfo[0].HiddenCompanyLogo.ToString() != "" && data.BrokerInfo[0].HiddenCompanyLogo.ToString() != null)
                            {

                                CompanyLogoPath = Email + ".png";

                                //byte[] binaryData;
                                //binaryData = new Byte[f.InputStream.Length];
                                //long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                //f.InputStream.Close();
                                //base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                byte[] imageBytes1 = Convert.FromBase64String(data.BrokerInfo[0].HiddenCompanyLogo.ToString());
                                MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/UploadedCompanyLogo/" + data.BrokerInfo[0].Email.ToString() + ".png");
                                bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/UploadedCompanyLogo/" + Email + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                            }
                        }
                        #endregion  Save Company Logo--New Logic

                        if (chkLanguages != null)
                        {
                            foreach (var Lang in chkLanguages)
                            {
                                Language = Language + "," + Lang;
                            }
                            Language = Language.TrimStart(',');
                        }

                        if (data.BrokerInfo[0].Specialities != null)
                        {
                            Specialities = data.BrokerInfo[0].Specialities.ToString();
                        }

                        if (data.BrokerInfo[0].HomeValue != null)
                        {
                            HomeValue = data.BrokerInfo[0].HomeValue.ToString();
                        }

                        if (data.BrokerInfo[0].AutoType != null)
                        {
                            AutoType = data.BrokerInfo[0].AutoType.ToString();
                        }

                        if (data.BrokerInfo[0].Revenue != null)
                        {
                            Revenue = data.BrokerInfo[0].Revenue.ToString();
                        }

                        if (data.BrokerInfo[0].Employees != null)
                        {
                            Employees = data.BrokerInfo[0].Employees.ToString();
                        }

                        if (data.BrokerInfo[0].CoverageAmt != null)
                        {
                            CoverageAmt = data.BrokerInfo[0].CoverageAmt.ToString();
                        }


                        /*************** Regarding Industry and SubIndustry *********************/

                        string SubIndustry1 = "", SubIndustryIds1 = "";
                        string SubIndustry2 = "", SubIndustryIds2 = "";
                        string SubIndustry3 = "", SubIndustryIds3 = "";

                        string IndustryId1 = ""; string IndustryId2 = ""; string IndustryId3 = "";
                        if (Industry1 != null)
                        {
                            if (chkSubIndustry1 != null)
                            {
                                IndustryId1 = IndustryId1 + "," + Industry1;
                                IndustryId1 = IndustryId1.TrimStart(',');

                                foreach (var Id in chkSubIndustry1)
                                {
                                    SubIndustry1 = SubIndustry1 + "," + Id;
                                }
                                SubIndustry1 = SubIndustry1.TrimStart(',');
                                SubIndustryIds1 = IndustryId1 + ":" + SubIndustry1;
                            }
                        }

                        if (Industry2 != null)
                        {
                            if (chkSubIndustry2 != null)
                            {
                                IndustryId2 = IndustryId2 + "," + Industry2;
                                IndustryId2 = IndustryId2.TrimStart(',');

                                foreach (var Id in chkSubIndustry2)
                                {
                                    SubIndustry2 = SubIndustry2 + "," + Id;
                                }
                                SubIndustry2 = SubIndustry2.TrimStart(',');
                                SubIndustryIds2 = IndustryId2 + ":" + SubIndustry2;
                            }
                        }

                        if (Industry3 != null)
                        {
                            if (chkSubIndustry3 != null)
                            {
                                IndustryId3 = IndustryId3 + "," + Industry3;
                                IndustryId3 = IndustryId3.TrimStart(',');

                                foreach (var Id in chkSubIndustry3)
                                {
                                    SubIndustry3 = SubIndustry3 + "," + Id;
                                }
                                SubIndustry3 = SubIndustry3.TrimStart(',');
                                SubIndustryIds3 = IndustryId3 + ":" + SubIndustry3;
                            }
                        }

                        IndustryId = IndustryId1 + "," + IndustryId2 + "," + IndustryId3;
                        SubIndustryId = SubIndustryIds1 + ";" + SubIndustryIds2 + ";" + SubIndustryIds3;

                        IndustryId = IndustryId.Trim(',');
                        SubIndustryId = SubIndustryId.Trim(';');

                        /*************** End of Regarding Industry and SubIndustry *********************/


                        //ExpiryDate = Request["datepicker"].ToString();
                        random = BrokerWSUtility.GetRandomNumber();
                        string Encryptrandom = BrokerUtility.EncryptURL(random);
                        Session["random"] = Encryptrandom;

                        /***************************** Save Broker Basic Details ***************************************/
                        User = BrokerWebDB.BrokerWebDB.SaveBrokerBasicDetails(FirstName, LastName, Phone, Email, Area, OnlyZipCodes, Title, Company, Language, Specialities, TempPass, Encryptrandom, CompanyLogoPath, ProfilePicPath, HomeValue, AutoType, Revenue, Employees, CoverageAmt, IndustryId, SubIndustryId, longitude, latitude, Bio);

                        if (User.Count > 0)
                        {
                            bool f1 = false, f2 = false, f3 = false, f4 = false, f5 = false;
                            string EduLogoPath1 = "", EduLogoPath2 = "", EduLogoPath3 = "", EduLogoPath4 = "", EduLogoPath5 = "";

                            string ExpLogoPath1 = "", ExpLogoPath2 = "", ExpLogoPath3 = "", ExpLogoPath4 = "", ExpLogoPath5 = "", ExpLogoPath6 = "", ExpLogoPath7 = "";

                            UserId = Convert.ToInt32(User[0].UserId.ToString());
                            Session["UserId"] = UserId;
                            Session["EmailId"] = User[0].EmailId.ToString();

                            /*For Saving ZipCode details*/

                            #region Save ZipCode
                            int Result2 = BrokerWebDB.BrokerWebDB.DeleteZipCode(UserId.ToString());

                            if (ZipCode1 != "" && Addr1 != "")
                            {
                                string[] strZipcode1 = Addr1.Split(',');

                                string lng = strZipcode1[0];
                                string lat = strZipcode1[1];

                                int Result = BrokerWebDB.BrokerWebDB.InsertUserZipCode(UserId, ZipCode1, lng, lat);
                            }

                            if (ZipCode2 != "" && Addr2 != "")
                            {
                                string[] strZipcode2 = Addr2.Split(',');

                                string lng = strZipcode2[0];
                                string lat = strZipcode2[1];

                                int Result = BrokerWebDB.BrokerWebDB.InsertUserZipCode(UserId, ZipCode2, lng, lat);
                            }

                            if (ZipCode3 != "" && Addr3 != "")
                            {
                                string[] strZipcode3 = Addr3.Split(',');

                                string lng = strZipcode3[0];
                                string lat = strZipcode3[1];

                                int Result = BrokerWebDB.BrokerWebDB.InsertUserZipCode(UserId, ZipCode3, lng, lat);
                            }

                            if (ZipCode4 != "" && Addr4 != "")
                            {
                                string[] strZipcode4 = Addr4.Split(',');

                                string lng = strZipcode4[0];
                                string lat = strZipcode4[1];

                                int Result = BrokerWebDB.BrokerWebDB.InsertUserZipCode(UserId, ZipCode4, lng, lat);
                            }

                            if (ZipCode5 != "" && Addr5 != "")
                            {
                                string[] strZipcode5 = Addr5.Split(',');

                                string lng = strZipcode5[0];
                                string lat = strZipcode5[1];

                                int Result = BrokerWebDB.BrokerWebDB.InsertUserZipCode(UserId, ZipCode5, lng, lat);
                            }

                            #endregion

                            if (data.BrokerEduction != null)
                            {
                                /*For Education 1*/
                                #region Education 1


                                if (data.BrokerEduction[0].School1 != "" && data.BrokerEduction[0].School1 != null)
                                {
                                    School1 = data.BrokerEduction[0].School1.ToString();
                                    f1 = true;
                                }

                                if (data.BrokerEduction[0].Degree1 != "" && data.BrokerEduction[0].Degree1 != null)
                                {
                                    Degree1 = data.BrokerEduction[0].Degree1.ToString();
                                    f1 = true;
                                }

                                if (data.BrokerEduction[0].Year1 != "" && data.BrokerEduction[0].Year1 != null)
                                {
                                    Year1 = data.BrokerEduction[0].Year1.ToString();
                                    f1 = true;
                                }

                                if (data.BrokerEduction[0].EducationLogo1 != "" && data.BrokerEduction[0].EducationLogo1 != null)
                                {
                                    EducationLogo1 = data.BrokerEduction[0].EducationLogo1.ToString().Replace("C:\\fakepath\\", "");

                                    //foreach (var f in file)
                                    //{
                                    //    if (f != null)
                                    //    {
                                    //        if (f.ContentLength > 0)
                                    //        {
                                    //            if (Path.GetFileName(f.FileName) == EducationLogo1)
                                    //            {

                                    if (data.BrokerEduction[0].HiddenEducationLogo1 != "" && data.BrokerEduction[0].HiddenEducationLogo1 != null)
                                    {
                                        EduLogoPath1 = UserId + "_" + School1 + "_" + Degree1 + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerEduction[0].HiddenEducationLogo1.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1 + "_" + Degree1 + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1 + "_" + Degree1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1 + "_" + Degree1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }

                                    //            }
                                    //        }
                                    //    }

                                    //}
                                }

                                if (f1 == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerEducationDetails(UserId, School1, Degree1, Year1, EduLogoPath1);
                                }

                                #endregion Education 1
                                /*For Education 1*/

                                /*For Education 2*/
                                #region Education 2

                                if (data.BrokerEduction[0].School2 != "" && data.BrokerEduction[0].School2 != null)
                                {
                                    School2 = data.BrokerEduction[0].School2.ToString();
                                    f2 = true;
                                }

                                if (data.BrokerEduction[0].Degree2 != "" && data.BrokerEduction[0].Degree2 != null)
                                {
                                    Degree2 = data.BrokerEduction[0].Degree2.ToString();
                                    f2 = true;
                                }

                                if (data.BrokerEduction[0].Year2 != "" && data.BrokerEduction[0].Year2 != null)
                                {
                                    Year2 = data.BrokerEduction[0].Year2.ToString();
                                    f2 = true;
                                }

                                if (data.BrokerEduction[0].EducationLogo2 != "" && data.BrokerEduction[0].EducationLogo2 != null)
                                {
                                    EducationLogo2 = data.BrokerEduction[0].EducationLogo2.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerEduction[0].HiddenEducationLogo2 != "" && data.BrokerEduction[0].HiddenEducationLogo2 != null)
                                    {
                                        EduLogoPath2 = UserId + "_" + School2 + "_" + Degree2 + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerEduction[0].HiddenEducationLogo2.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School2 + "_" + Degree2 + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1 + "_" + Degree1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School2 + "_" + Degree2 + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (f2 == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerEducationDetails(UserId, School2, Degree2, Year2, EduLogoPath2);
                                }

                                #endregion Education 2
                                /*For Education 2*/

                                /*For Education 3*/
                                #region Education 3

                                if (data.BrokerEduction[0].School3 != "" && data.BrokerEduction[0].School3 != null)
                                {
                                    School3 = data.BrokerEduction[0].School3.ToString();
                                    f3 = true;
                                }

                                if (data.BrokerEduction[0].Degree3 != "" && data.BrokerEduction[0].Degree3 != null)
                                {
                                    Degree3 = data.BrokerEduction[0].Degree3.ToString();
                                    f3 = true;
                                }

                                if (data.BrokerEduction[0].Year3 != "" && data.BrokerEduction[0].Year3 != null)
                                {
                                    Year3 = data.BrokerEduction[0].Year3.ToString();
                                    f3 = true;
                                }

                                if (data.BrokerEduction[0].EducationLogo3 != "" && data.BrokerEduction[0].EducationLogo3 != null)
                                {
                                    EducationLogo3 = data.BrokerEduction[0].EducationLogo3.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerEduction[0].HiddenEducationLogo3 != "" && data.BrokerEduction[0].HiddenEducationLogo3 != null)
                                    {
                                        EduLogoPath3 = UserId + "_" + School3 + "_" + Degree3 + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerEduction[0].HiddenEducationLogo3.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School3 + "_" + Degree3 + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1 + "_" + Degree1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School3 + "_" + Degree3 + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (f3 == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerEducationDetails(UserId, School3, Degree3, Year3, EduLogoPath3);
                                }

                                #endregion Education 2
                                /*For Education 3*/

                                /*For Education 4*/
                                #region Education 4

                                if (data.BrokerEduction[0].School4 != "" && data.BrokerEduction[0].School4 != null)
                                {
                                    School4 = data.BrokerEduction[0].School4.ToString();
                                    f4 = true;
                                }

                                if (data.BrokerEduction[0].Degree4 != "" && data.BrokerEduction[0].Degree4 != null)
                                {
                                    Degree4 = data.BrokerEduction[0].Degree4.ToString();
                                    f4 = true;
                                }

                                if (data.BrokerEduction[0].Year4 != "" && data.BrokerEduction[0].Year4 != null)
                                {
                                    Year4 = data.BrokerEduction[0].Year4.ToString();
                                    f4 = true;
                                }

                                if (data.BrokerEduction[0].EducationLogo4 != "" && data.BrokerEduction[0].EducationLogo4 != null)
                                {
                                    EducationLogo4 = data.BrokerEduction[0].EducationLogo4.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerEduction[0].HiddenEducationLogo4 != "" && data.BrokerEduction[0].HiddenEducationLogo4 != null)
                                    {
                                        EduLogoPath4 = UserId + "_" + School4 + "_" + Degree4 + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerEduction[0].HiddenEducationLogo4.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School4 + "_" + Degree4 + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1 + "_" + Degree1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School4 + "_" + Degree4 + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (f4 == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerEducationDetails(UserId, School4, Degree4, Year4, EduLogoPath4);
                                }

                                #endregion Education 2
                                /*For Education 4*/

                                /*For Education 5*/
                                #region Education 5

                                if (data.BrokerEduction[0].School5 != "" && data.BrokerEduction[0].School5 != null)
                                {
                                    School5 = data.BrokerEduction[0].School5.ToString();
                                    f5 = true;
                                }

                                if (data.BrokerEduction[0].Degree5 != "" && data.BrokerEduction[0].Degree5 != null)
                                {
                                    Degree5 = data.BrokerEduction[0].Degree5.ToString();
                                    f5 = true;
                                }

                                if (data.BrokerEduction[0].Year5 != "" && data.BrokerEduction[0].Year5 != null)
                                {
                                    Year5 = data.BrokerEduction[0].Year5.ToString();
                                    f5 = true;
                                }

                                if (data.BrokerEduction[0].EducationLogo5 != "" && data.BrokerEduction[0].EducationLogo5 != null)
                                {
                                    EducationLogo5 = data.BrokerEduction[0].EducationLogo5.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerEduction[0].HiddenEducationLogo5 != "" && data.BrokerEduction[0].HiddenEducationLogo5 != null)
                                    {
                                        EduLogoPath5 = UserId + "_" + School5 + "_" + Degree5 + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerEduction[0].HiddenEducationLogo5.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School5 + "_" + Degree5 + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1 + "_" + Degree1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School5 + "_" + Degree5 + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (f5 == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerEducationDetails(UserId, School5, Degree5, Year5, EduLogoPath5);
                                }

                                #endregion Education 2
                                /*For Education 5*/
                            }

                            /*For Saving Company Details*/
                            #region For Experience Details

                            if (data.BrokerPriorEmployment != null)
                            {
                                bool ExpFlag = false;
                                /*For Company 1*/
                                #region Company 1

                                if (data.BrokerPriorEmployment[0].CmpName1 != "" && data.BrokerPriorEmployment[0].CmpName1 != null)
                                {
                                    ExpComp1 = data.BrokerPriorEmployment[0].CmpName1.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].Desig1 != "" && data.BrokerPriorEmployment[0].Desig1 != null)
                                {
                                    ExpDesig1 = data.BrokerPriorEmployment[0].Desig1.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthFrom1 != "" && data.BrokerPriorEmployment[0].DurMonthFrom1 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthFrom1 != "Select")
                                    {
                                        ExpFromMonth1 = data.BrokerPriorEmployment[0].DurMonthFrom1.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearFrom1 != "" && data.BrokerPriorEmployment[0].DurYearFrom1 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearFrom1 != "Select")
                                    {
                                        ExpFromYear1 = data.BrokerPriorEmployment[0].DurYearFrom1.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthTo1 != "" && data.BrokerPriorEmployment[0].DurMonthTo1 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthTo1 != "Select")
                                    {
                                        ExpToMonth1 = data.BrokerPriorEmployment[0].DurMonthTo1.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearTo1 != "" && data.BrokerPriorEmployment[0].DurYearTo1 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearTo1 != "Select")
                                    {
                                        ExpToYear1 = data.BrokerPriorEmployment[0].DurYearTo1.ToString();
                                        ExpFlag = true;
                                    }
                                }

                                if (data.BrokerPriorEmployment[0].CmpLogo1 != "" && data.BrokerPriorEmployment[0].CmpLogo1 != null)
                                {
                                    ExpLogo1 = data.BrokerPriorEmployment[0].CmpLogo1.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerPriorEmployment[0].HiddenCmpLogo1 != "" && data.BrokerPriorEmployment[0].HiddenCmpLogo1 != null)
                                    {
                                        ExpLogoPath1 = UserId + "_" + ExpComp1 + "_" + ExpFromYear1 + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerPriorEmployment[0].HiddenCmpLogo1.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp1 + "_" + ExpFromYear1 + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp1 + "_" + ExpFromYear1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (ExpFlag == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerExperienceDetails(UserId, ExpComp1, ExpDesig1, ExpFromMonth1, ExpFromYear1, ExpToMonth1, ExpToYear1, ExpLogoPath1);
                                }
                                #endregion Company 1
                                /*End For Company 1*/

                                /*For Company 2*/
                                #region Company 2
                                ExpFlag = false;
                                if (data.BrokerPriorEmployment[0].CmpName2 != "" && data.BrokerPriorEmployment[0].CmpName2 != null)
                                {
                                    ExpComp2 = data.BrokerPriorEmployment[0].CmpName2.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].Desig2 != "" && data.BrokerPriorEmployment[0].Desig2 != null)
                                {
                                    ExpDesig2 = data.BrokerPriorEmployment[0].Desig2.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthFrom2 != "" && data.BrokerPriorEmployment[0].DurMonthFrom2 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthFrom2 != "Select")
                                    {
                                        ExpFromMonth2 = data.BrokerPriorEmployment[0].DurMonthFrom2.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearFrom2 != "" && data.BrokerPriorEmployment[0].DurYearFrom2 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearFrom2 != "Select")
                                    {
                                        ExpFromYear2 = data.BrokerPriorEmployment[0].DurYearFrom2.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthTo2 != "" && data.BrokerPriorEmployment[0].DurMonthTo2 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthTo2 != "Select")
                                    {
                                        ExpToMonth2 = data.BrokerPriorEmployment[0].DurMonthTo2.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearTo2 != "" && data.BrokerPriorEmployment[0].DurYearTo2 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearTo2 != "Select")
                                    {
                                        ExpToYear2 = data.BrokerPriorEmployment[0].DurYearTo2.ToString();
                                        ExpFlag = true;
                                    }
                                }

                                if (data.BrokerPriorEmployment[0].CmpLogo2 != "" && data.BrokerPriorEmployment[0].CmpLogo2 != null)
                                {
                                    ExpLogo2 = data.BrokerPriorEmployment[0].CmpLogo2.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerPriorEmployment[0].HiddenCmpLogo2 != "" && data.BrokerPriorEmployment[0].HiddenCmpLogo2 != null)
                                    {
                                        ExpLogoPath2 = UserId + "_" + ExpComp2 + "_" + ExpFromYear2 + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerPriorEmployment[0].HiddenCmpLogo2.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp2 + "_" + ExpFromYear2 + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp2 + "_" + ExpFromYear2 + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (ExpFlag == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerExperienceDetails(UserId, ExpComp2, ExpDesig2, ExpFromMonth2, ExpFromYear2, ExpToMonth2, ExpToYear2, ExpLogoPath2);
                                }
                                #endregion Company 2
                                /*End For Company 2*/

                                /*For Company 3*/
                                #region Company 3
                                ExpFlag = false;
                                if (data.BrokerPriorEmployment[0].CmpName3 != "" && data.BrokerPriorEmployment[0].CmpName3 != null)
                                {
                                    ExpComp3 = data.BrokerPriorEmployment[0].CmpName3.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].Desig3 != "" && data.BrokerPriorEmployment[0].Desig3 != null)
                                {
                                    ExpDesig3 = data.BrokerPriorEmployment[0].Desig3.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthFrom3 != "" && data.BrokerPriorEmployment[0].DurMonthFrom3 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthFrom3 != "Select")
                                    {
                                        ExpFromMonth3 = data.BrokerPriorEmployment[0].DurMonthFrom3.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearFrom3 != "" && data.BrokerPriorEmployment[0].DurYearFrom3 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearFrom3 != "Select")
                                    {
                                        ExpFromYear3 = data.BrokerPriorEmployment[0].DurYearFrom3.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthTo3 != "" && data.BrokerPriorEmployment[0].DurMonthTo3 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthTo3 != "Select")
                                    {
                                        ExpToMonth3 = data.BrokerPriorEmployment[0].DurMonthTo3.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearTo3 != "" && data.BrokerPriorEmployment[0].DurYearTo3 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearTo3 != "Select")
                                    {
                                        ExpToYear3 = data.BrokerPriorEmployment[0].DurYearTo3.ToString();
                                        ExpFlag = true;
                                    }
                                }

                                if (data.BrokerPriorEmployment[0].CmpLogo3 != "" && data.BrokerPriorEmployment[0].CmpLogo3 != null)
                                {
                                    ExpLogo3 = data.BrokerPriorEmployment[0].CmpLogo3.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerPriorEmployment[0].HiddenCmpLogo3 != "" && data.BrokerPriorEmployment[0].HiddenCmpLogo3 != null)
                                    {
                                        ExpLogoPath3 = UserId + "_" + ExpComp3 + "_" + ExpFromYear3 + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerPriorEmployment[0].HiddenCmpLogo3.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp3 + "_" + ExpFromYear3 + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp3 + "_" + ExpFromYear3 + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (ExpFlag == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerExperienceDetails(UserId, ExpComp3, ExpDesig3, ExpFromMonth3, ExpFromYear3, ExpToMonth3, ExpToYear3, ExpLogoPath3);
                                }
                                #endregion Company 3
                                /*End For Company 3*/

                                /*For Company4*/
                                #region Company 4
                                ExpFlag = false;
                                if (data.BrokerPriorEmployment[0].CmpName4 != "" && data.BrokerPriorEmployment[0].CmpName4 != null)
                                {
                                    ExpComp4 = data.BrokerPriorEmployment[0].CmpName4.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].Desig4 != "" && data.BrokerPriorEmployment[0].Desig4 != null)
                                {
                                    ExpDesig4 = data.BrokerPriorEmployment[0].Desig4.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthFrom4 != "" && data.BrokerPriorEmployment[0].DurMonthFrom4 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthFrom4 != "Select")
                                    {
                                        ExpFromMonth4 = data.BrokerPriorEmployment[0].DurMonthFrom4.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearFrom4 != "" && data.BrokerPriorEmployment[0].DurYearFrom4 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearFrom4 != "Select")
                                    {
                                        ExpFromYear4 = data.BrokerPriorEmployment[0].DurYearFrom4.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthTo4 != "" && data.BrokerPriorEmployment[0].DurMonthTo4 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthTo4 != "Select")
                                    {
                                        ExpToMonth4 = data.BrokerPriorEmployment[0].DurMonthTo4.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearTo4 != "" && data.BrokerPriorEmployment[0].DurYearTo4 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearTo4 != "Select")
                                    {
                                        ExpToYear4 = data.BrokerPriorEmployment[0].DurYearTo4.ToString();
                                        ExpFlag = true;
                                    }
                                }

                                if (data.BrokerPriorEmployment[0].CmpLogo4 != "" && data.BrokerPriorEmployment[0].CmpLogo4 != null)
                                {
                                    ExpLogo4 = data.BrokerPriorEmployment[0].CmpLogo4.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerPriorEmployment[0].HiddenCmpLogo4 != "" && data.BrokerPriorEmployment[0].HiddenCmpLogo4 != null)
                                    {
                                        ExpLogoPath4 = UserId + "_" + ExpComp4 + "_" + ExpFromYear4 + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerPriorEmployment[0].HiddenCmpLogo4.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp4 + "_" + ExpFromYear4 + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp4 + "_" + ExpFromYear4 + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (ExpFlag == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerExperienceDetails(UserId, ExpComp4, ExpDesig4, ExpFromMonth4, ExpFromYear4, ExpToMonth4, ExpToYear4, ExpLogoPath4);
                                }
                                #endregion Company 4
                                /*End For Company 4*/

                                /*For Company5*/
                                #region Company 5
                                ExpFlag = false;
                                if (data.BrokerPriorEmployment[0].CmpName5 != "" && data.BrokerPriorEmployment[0].CmpName5 != null)
                                {
                                    ExpComp5 = data.BrokerPriorEmployment[0].CmpName5.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].Desig5 != "" && data.BrokerPriorEmployment[0].Desig5 != null)
                                {
                                    ExpDesig5 = data.BrokerPriorEmployment[0].Desig5.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthFrom5 != "" && data.BrokerPriorEmployment[0].DurMonthFrom5 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthFrom5 != "Select")
                                    {
                                        ExpFromMonth5 = data.BrokerPriorEmployment[0].DurMonthFrom5.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearFrom5 != "" && data.BrokerPriorEmployment[0].DurYearFrom5 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearFrom5 != "Select")
                                    {
                                        ExpFromYear5 = data.BrokerPriorEmployment[0].DurYearFrom5.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthTo5 != "" && data.BrokerPriorEmployment[0].DurMonthTo5 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthTo5 != "Select")
                                    {
                                        ExpToMonth5 = data.BrokerPriorEmployment[0].DurMonthTo5.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearTo5 != "" && data.BrokerPriorEmployment[0].DurYearTo5 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearTo5 != "Select")
                                    {
                                        ExpToYear5 = data.BrokerPriorEmployment[0].DurYearTo5.ToString();
                                        ExpFlag = true;
                                    }
                                }

                                if (data.BrokerPriorEmployment[0].CmpLogo5 != "" && data.BrokerPriorEmployment[0].CmpLogo5 != null)
                                {
                                    ExpLogo5 = data.BrokerPriorEmployment[0].CmpLogo5.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerPriorEmployment[0].HiddenCmpLogo5 != "" && data.BrokerPriorEmployment[0].HiddenCmpLogo5 != null)
                                    {
                                        ExpLogoPath5 = UserId + "_" + ExpComp5 + "_" + ExpFromYear5 + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerPriorEmployment[0].HiddenCmpLogo5.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp5 + "_" + ExpFromYear5 + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp5 + "_" + ExpFromYear5 + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (ExpFlag == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerExperienceDetails(UserId, ExpComp5, ExpDesig5, ExpFromMonth5, ExpFromYear5, ExpToMonth5, ExpToYear5, ExpLogoPath5);
                                }
                                #endregion Company 5
                                /*End For Company 5*/

                                /*For Company6*/
                                #region Company 6
                                ExpFlag = false;
                                if (data.BrokerPriorEmployment[0].CmpName6 != "" && data.BrokerPriorEmployment[0].CmpName6 != null)
                                {
                                    ExpComp6 = data.BrokerPriorEmployment[0].CmpName6.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].Desig6 != "" && data.BrokerPriorEmployment[0].Desig6 != null)
                                {
                                    ExpDesig6 = data.BrokerPriorEmployment[0].Desig6.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthFrom6 != "" && data.BrokerPriorEmployment[0].DurMonthFrom6 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthFrom6 != "Select")
                                    {
                                        ExpFromMonth6 = data.BrokerPriorEmployment[0].DurMonthFrom6.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearFrom6 != "" && data.BrokerPriorEmployment[0].DurYearFrom6 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearFrom6 != "Select")
                                    {
                                        ExpFromYear6 = data.BrokerPriorEmployment[0].DurYearFrom6.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthTo6 != "" && data.BrokerPriorEmployment[0].DurMonthTo6 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthTo6 != "Select")
                                    {
                                        ExpToMonth6 = data.BrokerPriorEmployment[0].DurMonthTo6.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearTo6 != "" && data.BrokerPriorEmployment[0].DurYearTo6 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearTo6 != "Select")
                                    {
                                        ExpToYear6 = data.BrokerPriorEmployment[0].DurYearTo6.ToString();
                                        ExpFlag = true;
                                    }
                                }

                                if (data.BrokerPriorEmployment[0].CmpLogo6 != "" && data.BrokerPriorEmployment[0].CmpLogo6 != null)
                                {
                                    ExpLogo6 = data.BrokerPriorEmployment[0].CmpLogo6.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerPriorEmployment[0].HiddenCmpLogo6 != "" && data.BrokerPriorEmployment[0].HiddenCmpLogo6 != null)
                                    {
                                        ExpLogoPath6 = UserId + "_" + ExpComp6 + "_" + ExpFromYear6 + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerPriorEmployment[0].HiddenCmpLogo6.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp6 + "_" + ExpFromYear6 + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp6 + "_" + ExpFromYear6 + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (ExpFlag == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerExperienceDetails(UserId, ExpComp6, ExpDesig6, ExpFromMonth6, ExpFromYear6, ExpToMonth6, ExpToYear6, ExpLogoPath6);
                                }
                                #endregion Company 6
                                /*End For Company 6*/

                                /*For Company7*/
                                #region Company 7
                                ExpFlag = false;
                                if (data.BrokerPriorEmployment[0].CmpName7 != "" && data.BrokerPriorEmployment[0].CmpName7 != null)
                                {
                                    ExpComp7 = data.BrokerPriorEmployment[0].CmpName7.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].Desig7 != "" && data.BrokerPriorEmployment[0].Desig7 != null)
                                {
                                    ExpDesig7 = data.BrokerPriorEmployment[0].Desig7.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthFrom7 != "" && data.BrokerPriorEmployment[0].DurMonthFrom7 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthFrom7 != "Select")
                                    {
                                        ExpFromMonth7 = data.BrokerPriorEmployment[0].DurMonthFrom7.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearFrom7 != "" && data.BrokerPriorEmployment[0].DurYearFrom7 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearFrom7 != "Select")
                                    {
                                        ExpFromYear7 = data.BrokerPriorEmployment[0].DurYearFrom7.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthTo7 != "" && data.BrokerPriorEmployment[0].DurMonthTo7 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthTo7 != "Select")
                                    {
                                        ExpToMonth7 = data.BrokerPriorEmployment[0].DurMonthTo7.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearTo7 != "" && data.BrokerPriorEmployment[0].DurYearTo7 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearTo7 != "Select")
                                    {
                                        ExpToYear7 = data.BrokerPriorEmployment[0].DurYearTo7.ToString();
                                        ExpFlag = true;
                                    }
                                }

                                if (data.BrokerPriorEmployment[0].CmpLogo7 != "" && data.BrokerPriorEmployment[0].CmpLogo7 != null)
                                {
                                    ExpLogo7 = data.BrokerPriorEmployment[0].CmpLogo7.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerPriorEmployment[0].HiddenCmpLogo7 != "" && data.BrokerPriorEmployment[0].HiddenCmpLogo7 != null)
                                    {
                                        ExpLogoPath7 = UserId + "_" + ExpComp7 + "_" + ExpFromYear7 + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerPriorEmployment[0].HiddenCmpLogo7.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp7 + "_" + ExpFromYear7 + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp7 + "_" + ExpFromYear7 + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (ExpFlag == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerExperienceDetails(UserId, ExpComp7, ExpDesig7, ExpFromMonth7, ExpFromYear7, ExpToMonth7, ExpToYear7, ExpLogoPath7);
                                }
                                #endregion Company 7
                                /*End For Company 7*/
                            }
                            #endregion For Experience Details


                            /*End of For Saving Company Details*/


                            /*For Saving Industry Details*/
                            #region For Industry Details
                            if (Industry1 != null)
                            {
                                if (chkSubIndustry1 != null)
                                {
                                    foreach (var Ids in chkSubIndustry1)
                                    {
                                        Flag = BrokerWebDB.BrokerWebDB.SaveBrokerIndustryDetails(UserId, Industry1, Ids);
                                    }
                                }
                            }

                            if (Industry2 != null)
                            {
                                if (chkSubIndustry2 != null)
                                {
                                    foreach (var Ids in chkSubIndustry2)
                                    {
                                        Flag = BrokerWebDB.BrokerWebDB.SaveBrokerIndustryDetails(UserId, Industry2, Ids);
                                    }
                                }
                            }

                            if (Industry3 != null)
                            {
                                if (chkSubIndustry3 != null)
                                {
                                    foreach (var Ids in chkSubIndustry3)
                                    {
                                        Flag = BrokerWebDB.BrokerWebDB.SaveBrokerIndustryDetails(UserId, Industry3, Ids);
                                    }
                                }
                            }
                            #endregion For Industry Details
                            /*End of For Saving Industry Details*/

                            bool EmailFlag = false;

                            EmailFlag = BrokerWSUtility.SendRegistrationEmail(Session["EmailId"].ToString(), Session["random"].ToString(), Session["UserId"].ToString(), "Broker");
                            //EmailFlag = true;
                            if (EmailFlag)
                            {
                                //ViewBag.VerificationMessage = "You are registered successfully but yet not activated. ";
                                //ViewBag.VerificationMessage1 = "Please accept your verification email.";
                                return View("BrokerSuccess");
                            }
                            else
                            {
                                //If User registerd successfully, but verification link has not
                                //been sent over EmailId
                                //ViewBag.VerificationMessage = "You are registered successfully but yet not activated. <br/>Please accept your verification email.";

                                ViewBag.VerificationMessage = "You are registered successfully but can't send mail to you.";
                                ViewBag.VerificationMessage1 = "Please contact to admin.";
                                return View("BrokerError");
                            }

                            //return RedirectToAction("MakePayment", "BrokerRegistration", new { EmailId = BrokerUtility.EncryptURL(Email), RegistrationCode = BrokerUtility.EncryptURL(random) });
                        }
                        ViewBag.VerificationMessage = "Error occured while saving details. ";
                        ViewBag.VerificationMessage1 = "Please try again.";
                        return View("BrokerError");
                        //}
                        #endregion Save Broker Details

                        //return RedirectToAction("MakePayment", "BrokerRegistration", new { EmailId = BrokerUtility.EncryptURL("samplemail@gmail.com"), RegistrationCode = BrokerUtility.EncryptURL("123456789") });
                    }
                    //ViewBag.VerificationMessage = "Error occured while saving details. ";
                    //ViewBag.VerificationMessage1 = "Please try again.";
                    //return View("BrokerError");
                }
                //ViewBag.VerificationMessage = "Error occured while saving details. ";
                //ViewBag.VerificationMessage1 = "Please try again.";
                //return View("BrokerError");
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "BrokerSignUp_POST_Website", Ex.Message.ToString(), "BrokerRegistrationController.cs_BrokerSignUp()", "");
                ViewBag.VerificationMessage = "Error occured while saving details. ";
                ViewBag.VerificationMessage1 = "Please try again.";
                return View("BrokerError");
            }
            ViewBag.VerificationMessage = "Error occured while saving details. ";
            ViewBag.VerificationMessage1 = "Please try again.";
            return View("BrokerError");
        }

        [HttpGet]
        [Authorize]
        public ActionResult EditBroker()
        {

            //AllModels Data=null;
            EditBrokerProfile Data = new EditBrokerProfile();
            DataTable dtIndustryId = new DataTable();
            dtIndustryId.Columns.Add("SubIndustryId");

            string IndustryId = "";
            string SubIndustryId = "";
            int cntZipCodes = 0;

            try
            {
                DataSet dsBrokerData = new DataSet();
                dsBrokerData = (DataSet)Session["BrokerData"];

                if (dsBrokerData.Tables.Count > 0)
                {
                    ViewBag.Initials = Session["Initials"].ToString().ToUpper();
                    ViewBag.UserName = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();

                    ViewBag.UserDetails = dsBrokerData.Tables[0].Rows.Cast<DataRow>().ToList();
                    ViewBag.ExperienceDetails = dsBrokerData.Tables[1].Rows.Cast<DataRow>().ToList();
                    ViewBag.EducationDetails = dsBrokerData.Tables[2].Rows.Cast<DataRow>().ToList();

                    if (dsBrokerData.Tables[0].Rows.Count > 0)
                    {
                        Data.UserId = Convert.ToInt32(dsBrokerData.Tables[0].Rows[0]["UserId"].ToString());
                        Data.FirstName = dsBrokerData.Tables[0].Rows[0]["FirstName"].ToString();
                        Data.LastName = dsBrokerData.Tables[0].Rows[0]["LastName"].ToString();
                        Data.PhoneNo1 = dsBrokerData.Tables[0].Rows[0]["PhoneNo"].ToString();
                        Data.Email = dsBrokerData.Tables[0].Rows[0]["EmailId"].ToString();
                        Data.Area = dsBrokerData.Tables[0].Rows[0]["City"].ToString();
                        //Data.ZipCode = dsBrokerData.Tables[0].Rows[0]["PinCode"].ToString();
                        Data.Title = dsBrokerData.Tables[0].Rows[0]["Title"].ToString();
                        Data.Company = dsBrokerData.Tables[0].Rows[0]["CompanyName"].ToString();
                        if (dsBrokerData.Tables[0].Rows[0]["CompanyLogo"].ToString() != "")
                        {
                            if (dsBrokerData.Tables[0].Rows[0]["CompanyName"].ToString() != "")
                            {
                                Data.CompanyLogo = dsBrokerData.Tables[0].Rows[0]["CompanyName"].ToString() + ".png";
                            }
                            else
                            {
                                Data.CompanyLogo = "CompanyLogo.png";
                            }
                        }

                        Data.Specialities = dsBrokerData.Tables[0].Rows[0]["Specialities"].ToString();
                        Data.Languages = dsBrokerData.Tables[0].Rows[0]["Languages"].ToString();

                        if (dsBrokerData.Tables[0].Rows[0]["ProfilePictureImg"].ToString() != "")
                        {
                            Data.ProfilePhoto = dsBrokerData.Tables[0].Rows[0]["FirstName"].ToString() + "_" + dsBrokerData.Tables[0].Rows[0]["LastName"].ToString() + ".png";
                        }

                        IndustryId = dsBrokerData.Tables[0].Rows[0]["IndustryId"].ToString();
                        SubIndustryId = dsBrokerData.Tables[0].Rows[0]["SubIndustryId"].ToString();

                        Data.HomeValue = dsBrokerData.Tables[0].Rows[0]["HomeValue"].ToString();
                        Data.AutoType = dsBrokerData.Tables[0].Rows[0]["AutoType"].ToString();
                        Data.Employees = dsBrokerData.Tables[0].Rows[0]["Employees"].ToString();
                        Data.Revenue = dsBrokerData.Tables[0].Rows[0]["Revenue"].ToString();
                        Data.CoverageAmt = dsBrokerData.Tables[0].Rows[0]["CoverageAmt"].ToString();
                        Data.Bio = dsBrokerData.Tables[0].Rows[0]["Bio"].ToString();
                        //Start to split Sub Industry Ids
                        #region Split Sub Industry Ids

                        //int Result1 = BrokerWSUtility.DeleteIndustryId(UserId);

                        if (SubIndustryId != "")
                        {
                            string[] SubIndustryIds1 = null;
                            string[] SubIndustryIds2 = null;

                            SubIndustryIds1 = SubIndustryId.Split(';');

                            foreach (string Id in SubIndustryIds1)
                            {
                                SubIndustryIds2 = Id.Split(':');
                                if (SubIndustryIds2.Length == 2)
                                {
                                    dtIndustryId.Rows.Add(SubIndustryIds2[1]);
                                }
                            }
                        }

                        #endregion Split Industry Ids

                        if (dsBrokerData.Tables[0].Rows[0]["PinCode"].ToString() != "")
                        {
                            string MainZipCode = dsBrokerData.Tables[0].Rows[0]["PinCode"].ToString();

                            string[] ArrayZipCodes = MainZipCode.Split(',');

                            int i = 1;

                            foreach (var Id in ArrayZipCodes)
                            {
                                if (i == 1)
                                {
                                    Data.ZipCode1 = Id;
                                    Data.HiddenZipCode1 = Id;
                                    i++;
                                    cntZipCodes++;
                                }
                                else if (i == 2)
                                {
                                    Data.ZipCode2 = Id;
                                    Data.HiddenZipCode2 = Id;
                                    i++;
                                    cntZipCodes++;
                                }
                                else if (i == 3)
                                {
                                    Data.ZipCode3 = Id;
                                    Data.HiddenZipCode3 = Id;
                                    i++;
                                    cntZipCodes++;
                                }
                                else if (i == 4)
                                {
                                    Data.ZipCode4 = Id;
                                    Data.HiddenZipCode4 = Id;
                                    i++;
                                    cntZipCodes++;
                                }
                                else if (i == 5)
                                {
                                    Data.ZipCode5 = Id;
                                    Data.HiddenZipCode5 = Id;
                                    i++;
                                    cntZipCodes++;
                                }
                            }
                        }
                    }

                    int cnt = 0;
                    if (dsBrokerData.Tables[2].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsBrokerData.Tables[2].Rows.Count; i++)
                        {
                            if (i == 0)
                            {
                                //Data.School1 = dsBrokerData.Tables[2].Rows[i]["UniversityName"].ToString();
                                Data.HiddenSchool1 = dsBrokerData.Tables[2].Rows[i]["UniversityName"].ToString();
                                Data.HiddenDegree1 = dsBrokerData.Tables[2].Rows[i]["CourseName"].ToString();
                                Data.HiddenYear1 = dsBrokerData.Tables[2].Rows[i]["DurationFrom"].ToString();
                                if (dsBrokerData.Tables[2].Rows[i]["EducationLogo"].ToString() != "")
                                {
                                    Data.HiddenEducationLogo1 = dsBrokerData.Tables[2].Rows[i]["UniversityName"].ToString() + ".png";
                                }
                                Data.HiddenEducationLogoPath1 = dsBrokerData.Tables[2].Rows[i]["EducationLogo"].ToString();
                                Data.HiddenEducationLogoIsChanged1 = "false";

                                cnt = cnt + 1;
                            }
                            else if (i == 1)
                            {
                                Data.HiddenSchool2 = dsBrokerData.Tables[2].Rows[i]["UniversityName"].ToString();
                                Data.HiddenDegree2 = dsBrokerData.Tables[2].Rows[i]["CourseName"].ToString();
                                Data.HiddenYear2 = dsBrokerData.Tables[2].Rows[i]["DurationFrom"].ToString();
                                if (dsBrokerData.Tables[2].Rows[i]["EducationLogo"].ToString() != "")
                                {
                                    Data.HiddenEducationLogo2 = dsBrokerData.Tables[2].Rows[i]["UniversityName"].ToString() + ".png";
                                }
                                Data.HiddenEducationLogoPath2 = dsBrokerData.Tables[2].Rows[i]["EducationLogo"].ToString();
                                Data.HiddenEducationLogoIsChanged2 = "false";

                                cnt = cnt + 1;
                            }
                            else if (i == 2)
                            {
                                Data.HiddenSchool3 = dsBrokerData.Tables[2].Rows[i]["UniversityName"].ToString();
                                Data.HiddenDegree3 = dsBrokerData.Tables[2].Rows[i]["CourseName"].ToString();
                                Data.HiddenYear3 = dsBrokerData.Tables[2].Rows[i]["DurationFrom"].ToString();
                                if (dsBrokerData.Tables[2].Rows[i]["EducationLogo"].ToString() != "")
                                {
                                    Data.HiddenEducationLogo3 = dsBrokerData.Tables[2].Rows[i]["UniversityName"].ToString() + ".png";
                                }
                                Data.HiddenEducationLogoPath3 = dsBrokerData.Tables[2].Rows[i]["EducationLogo"].ToString();
                                Data.HiddenEducationLogoIsChanged3 = "false";

                                cnt = cnt + 1;
                            }
                            else if (i == 3)
                            {
                                Data.HiddenSchool4 = dsBrokerData.Tables[2].Rows[i]["UniversityName"].ToString();
                                Data.HiddenDegree4 = dsBrokerData.Tables[2].Rows[i]["CourseName"].ToString();
                                Data.HiddenYear4 = dsBrokerData.Tables[2].Rows[i]["DurationFrom"].ToString();
                                if (dsBrokerData.Tables[2].Rows[i]["EducationLogo"].ToString() != "")
                                {
                                    Data.HiddenEducationLogo4 = dsBrokerData.Tables[2].Rows[i]["UniversityName"].ToString() + ".png";
                                }
                                Data.HiddenEducationLogoPath4 = dsBrokerData.Tables[2].Rows[i]["EducationLogo"].ToString();
                                Data.HiddenEducationLogoIsChanged4 = "false";

                                cnt = cnt + 1;
                            }
                            else if (i == 4)
                            {
                                Data.HiddenSchool5 = dsBrokerData.Tables[2].Rows[i]["UniversityName"].ToString();
                                Data.HiddenDegree5 = dsBrokerData.Tables[2].Rows[i]["CourseName"].ToString();
                                Data.HiddenYear5 = dsBrokerData.Tables[2].Rows[i]["DurationFrom"].ToString();
                                if (dsBrokerData.Tables[2].Rows[i]["EducationLogo"].ToString() != "")
                                {
                                    Data.HiddenEducationLogo5 = dsBrokerData.Tables[2].Rows[i]["UniversityName"].ToString() + ".png";
                                }
                                Data.HiddenEducationLogoPath5 = dsBrokerData.Tables[2].Rows[i]["EducationLogo"].ToString();
                                Data.HiddenEducationLogoIsChanged5 = "false";

                                cnt = cnt + 1;
                            }
                        }
                    }

                    string CompanyId = "";
                    int ExpCnt = 0;
                    if (dsBrokerData.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsBrokerData.Tables[1].Rows.Count; i++)
                        {
                            CompanyId = CompanyId + "," + dsBrokerData.Tables[1].Rows[i]["CompanyName"].ToString();

                            if (i == 0)
                            {
                                Data.HiddenExpCompanyName1 = dsBrokerData.Tables[1].Rows[i]["CompanyName"].ToString();
                                if (dsBrokerData.Tables[1].Rows[i]["Logo"].ToString() != "")
                                {
                                    Data.HiddenExpLogo1 = dsBrokerData.Tables[1].Rows[i]["CompanyName"].ToString() + ".png";
                                }
                                Data.HiddenExpTitle1 = dsBrokerData.Tables[1].Rows[i]["Designation"].ToString();
                                Data.HiddenExpDurationFrom1 = dsBrokerData.Tables[1].Rows[i]["DurationFrom"].ToString();
                                Data.HiddenExpDurationTo1 = dsBrokerData.Tables[1].Rows[i]["DurationTo"].ToString();
                                Data.HiddenExpLogoPath1 = dsBrokerData.Tables[1].Rows[i]["Logo"].ToString();
                                Data.HiddenExpLogoIsChanged1 = "false";
                                ExpCnt = ExpCnt + 1;
                            }
                            if (i == 1)
                            {
                                Data.HiddenExpCompanyName2 = dsBrokerData.Tables[1].Rows[i]["CompanyName"].ToString();
                                if (dsBrokerData.Tables[1].Rows[i]["Logo"].ToString() != "")
                                {
                                    Data.HiddenExpLogo2 = dsBrokerData.Tables[1].Rows[i]["CompanyName"].ToString() + ".png";
                                }
                                Data.HiddenExpTitle2 = dsBrokerData.Tables[1].Rows[i]["Designation"].ToString();
                                Data.HiddenExpDurationFrom2 = dsBrokerData.Tables[1].Rows[i]["DurationFrom"].ToString();
                                Data.HiddenExpDurationTo2 = dsBrokerData.Tables[1].Rows[i]["DurationTo"].ToString();
                                Data.HiddenExpLogoPath2 = dsBrokerData.Tables[1].Rows[i]["Logo"].ToString();
                                Data.HiddenExpLogoIsChanged2 = "false";
                                ExpCnt = ExpCnt + 1;
                            }
                            if (i == 2)
                            {
                                Data.HiddenExpCompanyName3 = dsBrokerData.Tables[1].Rows[i]["CompanyName"].ToString();
                                if (dsBrokerData.Tables[1].Rows[i]["Logo"].ToString() != "")
                                {
                                    Data.HiddenExpLogo3 = dsBrokerData.Tables[1].Rows[i]["CompanyName"].ToString() + ".png";
                                }
                                Data.HiddenExpTitle3 = dsBrokerData.Tables[1].Rows[i]["Designation"].ToString();
                                Data.HiddenExpDurationFrom3 = dsBrokerData.Tables[1].Rows[i]["DurationFrom"].ToString();
                                Data.HiddenExpDurationTo3 = dsBrokerData.Tables[1].Rows[i]["DurationTo"].ToString();
                                Data.HiddenExpLogoPath3 = dsBrokerData.Tables[1].Rows[i]["Logo"].ToString();
                                Data.HiddenExpLogoIsChanged3 = "false";
                                ExpCnt = ExpCnt + 1;
                            }
                            if (i == 3)
                            {
                                Data.HiddenExpCompanyName4 = dsBrokerData.Tables[1].Rows[i]["CompanyName"].ToString();
                                if (dsBrokerData.Tables[1].Rows[i]["Logo"].ToString() != "")
                                {
                                    Data.HiddenExpLogo4 = dsBrokerData.Tables[1].Rows[i]["CompanyName"].ToString() + ".png";
                                }
                                Data.HiddenExpTitle4 = dsBrokerData.Tables[1].Rows[i]["Designation"].ToString();
                                Data.HiddenExpDurationFrom4 = dsBrokerData.Tables[1].Rows[i]["DurationFrom"].ToString();
                                Data.HiddenExpDurationTo4 = dsBrokerData.Tables[1].Rows[i]["DurationTo"].ToString();
                                Data.HiddenExpLogoPath4 = dsBrokerData.Tables[1].Rows[i]["Logo"].ToString();
                                Data.HiddenExpLogoIsChanged4 = "false";
                                ExpCnt = ExpCnt + 1;
                            }
                            if (i == 4)
                            {
                                Data.HiddenExpCompanyName5 = dsBrokerData.Tables[1].Rows[i]["CompanyName"].ToString();
                                if (dsBrokerData.Tables[1].Rows[i]["Logo"].ToString() != "")
                                {
                                    Data.HiddenExpLogo5 = dsBrokerData.Tables[1].Rows[i]["CompanyName"].ToString() + ".png";
                                }
                                Data.HiddenExpTitle5 = dsBrokerData.Tables[1].Rows[i]["Designation"].ToString();
                                Data.HiddenExpDurationFrom5 = dsBrokerData.Tables[1].Rows[i]["DurationFrom"].ToString();
                                Data.HiddenExpDurationTo5 = dsBrokerData.Tables[1].Rows[i]["DurationTo"].ToString();
                                Data.HiddenExpLogoPath5 = dsBrokerData.Tables[1].Rows[i]["Logo"].ToString();
                                Data.HiddenExpLogoIsChanged5 = "false";
                                ExpCnt = ExpCnt + 1;
                            }
                            if (i == 5)
                            {
                                Data.HiddenExpCompanyName6 = dsBrokerData.Tables[1].Rows[i]["CompanyName"].ToString();
                                if (dsBrokerData.Tables[1].Rows[i]["Logo"].ToString() != "")
                                {
                                    Data.HiddenExpLogo6 = dsBrokerData.Tables[1].Rows[i]["CompanyName"].ToString() + ".png";
                                }
                                Data.HiddenExpTitle6 = dsBrokerData.Tables[1].Rows[i]["Designation"].ToString();
                                Data.HiddenExpDurationFrom6 = dsBrokerData.Tables[1].Rows[i]["DurationFrom"].ToString();
                                Data.HiddenExpDurationTo6 = dsBrokerData.Tables[1].Rows[i]["DurationTo"].ToString();
                                Data.HiddenExpLogoPath6 = dsBrokerData.Tables[1].Rows[i]["Logo"].ToString();
                                Data.HiddenExpLogoIsChanged6 = "false";
                                ExpCnt = ExpCnt + 1;
                            }
                            if (i == 6)
                            {
                                Data.HiddenExpCompanyName7 = dsBrokerData.Tables[1].Rows[i]["CompanyName"].ToString();
                                if (dsBrokerData.Tables[1].Rows[i]["Logo"].ToString() != "")
                                {
                                    Data.HiddenExpLogo7 = dsBrokerData.Tables[1].Rows[i]["CompanyName"].ToString() + ".png";
                                }
                                Data.HiddenExpTitle7 = dsBrokerData.Tables[1].Rows[i]["Designation"].ToString();
                                Data.HiddenExpDurationFrom7 = dsBrokerData.Tables[1].Rows[i]["DurationFrom"].ToString();
                                Data.HiddenExpDurationTo7 = dsBrokerData.Tables[1].Rows[i]["DurationTo"].ToString();
                                Data.HiddenExpLogoPath7 = dsBrokerData.Tables[1].Rows[i]["Logo"].ToString();
                                Data.HiddenExpLogoIsChanged7 = "false";
                                ExpCnt = ExpCnt + 1;
                            }

                        }

                    }

                    ViewBag.TotalExpCount = ExpCnt;

                    Data.HiddenCompanyNameRpt = CompanyId;


                    Data.HiddenCompanyLogo = "false";
                    Data.HiddenProfilePhoto = "false";

                    ViewBag.CompanyNameRpt = CompanyId;
                    ViewBag.TotalEducationCount = cnt;
                    ViewBag.Languages = dsBrokerData.Tables[0].Rows[0]["Languages"].ToString();
                    ViewBag.cntZipCodes = cntZipCodes;

                    IndustryId = IndustryId.Replace(",,", ",");
                    ViewBag.IndustryId = IndustryId;
                    //ViewBag.SubIndustryId = SubIndustryId;

                    Data.IsVisibleIndustry1 = "false";
                    Data.IsVisibleIndustry2 = "false";
                    Data.IsVisibleIndustry3 = "false";

                    //Data.IsDeletedIndustry1 = "true";
                    //Data.IsDeletedIndustry2 = "true";
                    //Data.IsDeletedIndustry3 = "true";

                    if (dtIndustryId.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtIndustryId.Rows.Count; i++)
                        {
                            if (i == 0)
                            {
                                ViewBag.SubIndustryId1 = dtIndustryId.Rows[i][0].ToString();
                            }
                            else if (i == 1)
                            {
                                ViewBag.SubIndustryId2 = dtIndustryId.Rows[i][0].ToString();
                            }
                            else if (i == 2)
                            {
                                ViewBag.SubIndustryId3 = dtIndustryId.Rows[i][0].ToString();
                            }
                        }
                    }
                }

                var db = new BrokerDBEntities();
                ViewBag.CompanyMaster = new SelectList(db.CompanyMasters.ToList().OrderBy(x => x.CompanyName), "CompanyId", "CompanyName");

                ViewBag.IndustryMaster = new SelectList(db.IndustryMasters.ToList().OrderBy(x => x.IndustryName), "IndustryId", "IndustryName");
                int id = 1;

                ViewBag.SubIndustryMaster = new SelectList(db.SubIndustryMasters.Where(c => c.IndustryId == id).Select(c => new { Value = c.Id, Text = c.SICCode + " - " + c.SubIndustryName }).ToList().OrderBy(c => c.Text), "Id", "SubIndustryName");
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(Session["UserId"].ToString()), "Index_Wesite", Ex.Message.ToString(), "BrokerRegistrationController.cs_Index", "");
            }
            return View(Data);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult EditBroker(EditBrokerProfile data, IEnumerable<HttpPostedFileBase> file, FormCollection Form, string[] chkLanguages, string[] CompanyNameRpt, string Industry1, string[] chkSubIndustry1, string Industry2, string[] chkSubIndustry2, string Industry3, string[] chkSubIndustry3)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Request.Form["SUBMIT"] != null)
                    {
                        //string strDDLValue = Form["CompanyName1"].ToString();

                        #region Save Broker Details

                        List<uspSaveBrokerBasicDetails_Result> User = null;

                        DataSet dsGetBrokerDetails = null;

                        string FirstName = "", LastName = "", Phone = "", Email = "", ZipCode = "", Title = "",
                           Language = "", Specialities = "", Phone1 = "", TempPass = "", random = "", Area = "", Company = "",
                            ProfilePic = "", CompanyLogo = "", CompanyLogoPath = "", ProfilePicPath = "", Bio = "";

                        string Degree1 = "", Degree2 = "", Degree3 = "", Degree4 = "", Degree5 = "";
                        string School1 = "", School2 = "", School3 = "", School4 = "", School5 = "";
                        string Year1 = "", Year2 = "", Year3 = "", Year4 = "", Year5 = "";
                        string EducationLogo1 = "", EducationLogo2 = "", EducationLogo3 = "", EducationLogo4 = "", EducationLogo5 = "";

                        //30April18
                        string Company1 = "", Company2 = "", Company3 = "", Company4 = "", Company5 = "", Company6 = "", Company7 = "";
                        string Title1 = "", Title2 = "", Title3 = "", Title4 = "", Title5 = "", Title6 = "", Title7 = "";
                        string DurationFrom1 = "", DurationFrom2 = "", DurationFrom3 = "", DurationFrom4 = "", DurationFrom5 = "", DurationFrom6 = "", DurationFrom7 = "";
                        string DurationTo1 = "", DurationTo2 = "", DurationTo3 = "", DurationTo4 = "", DurationTo5 = "", DurationTo6 = "", DurationTo7 = "";
                        string ExpLogo1 = "", ExpLogo2 = "", ExpLogo3 = "", ExpLogo4 = "", ExpLogo5 = "", ExpLogo6 = "", ExpLogo7 = "";

                        bool E1 = false, E2 = false, E3 = false, E4 = false, E5 = false, E6 = false, E7 = false;
                        string ExpLogoPath1 = "", ExpLogoPath2 = "", ExpLogoPath3 = "", ExpLogoPath4 = "", ExpLogoPath5 = "", ExpLogoPath6 = "", ExpLogoPath7 = "";

                        string HomeValue = "", AutoType = "", Revenue = "", Employees = "", CoverageAmt = "";

                        string IndustryId = "", SubIndustryId = "", latitude = "", longitude = "", OnlyZipCode = "";
                        string ZipCode1 = "", ZipCode2 = "", ZipCode3 = "", ZipCode4 = "", ZipCode5 = "", MainZipCodes = "";
                        string Addr1 = "", Addr2 = "", Addr3 = "", Addr4 = "", Addr5 = "";


                        string IsProfilePhotoChanged = "false";
                        string IsCompanyLogoChanged = "false";

                        string IsProfilePhotoFileSaved = "false";
                        string IsCompanyLogoFileSaved = "false";

                        string IsEducationLogoChanged1 = "false";
                        string IsEducationLogoChanged2 = "false";
                        string IsEducationLogoChanged3 = "false";
                        string IsEducationLogoChanged4 = "false";
                        string IsEducationLogoChanged5 = "false";

                        int UserId = 0, Flag = 0;

                        if (data.UserId != null)
                        {
                            UserId = Convert.ToInt32(data.UserId.ToString());
                        }

                        if (data.FirstName != null)
                        {
                            FirstName = data.FirstName.ToString();
                        }
                        if (data.LastName != null)
                        {
                            LastName = data.LastName.ToString();
                        }

                        if (data.PhoneNo1 != null)
                        {
                            Phone1 = data.PhoneNo1.ToString();
                        }

                        Phone = Phone1;

                        if (data.Email != null)
                        {
                            Email = data.Email.ToString();
                        }
                        if (data.Area != null)
                        {
                            Area = data.Area.ToString();
                        }

                        if (data.Bio != null)
                        {
                            Bio = data.Bio.ToString().Trim();
                        }

                        //if (data.ZipCode1 != null)
                        //{
                        //    ZipCode = data.ZipCode1.ToString();
                        //    MainZipCode = ZipCode;
                        //}

                        if (data.ZipCode1 != null)
                        {
                            ZipCode1 = data.ZipCode1.ToString();
                            Addr1 = GetPositionFromZip(ZipCode1);

                            ZipCode = ZipCode1 + ":" + Addr1;
                            OnlyZipCode = ZipCode1;
                        }
                        if (data.ZipCode2 != null)
                        {
                            ZipCode2 = data.ZipCode2.ToString();
                            Addr2 = GetPositionFromZip(ZipCode2);

                            ZipCode = ZipCode + ";" + ZipCode2 + ":" + Addr2;
                            OnlyZipCode = OnlyZipCode + "," + ZipCode2;
                        }

                        if (data.ZipCode3 != null)
                        {
                            ZipCode3 = data.ZipCode3.ToString();
                            Addr3 = GetPositionFromZip(ZipCode3);

                            ZipCode = ZipCode + ";" + ZipCode3 + ":" + Addr3;
                            OnlyZipCode = OnlyZipCode + "," + ZipCode3;
                        }

                        if (data.ZipCode4 != null)
                        {
                            ZipCode4 = data.ZipCode4.ToString();
                            Addr4 = GetPositionFromZip(ZipCode4);

                            ZipCode = ZipCode + ";" + ZipCode4 + ":" + Addr4;
                            OnlyZipCode = OnlyZipCode + "," + ZipCode4;
                        }

                        if (data.ZipCode5 != null)
                        {
                            ZipCode5 = data.ZipCode5.ToString();
                            Addr5 = GetPositionFromZip(ZipCode5);

                            ZipCode = ZipCode + ";" + ZipCode5 + ":" + Addr5;
                            OnlyZipCode = OnlyZipCode + "," + ZipCode5;
                        }


                        //if (data.ZipCode1 != null)
                        //{
                        //    ZipCode = data.ZipCode1.ToString();
                        //    MainZipCode =MainZipCode+","+ ZipCode;
                        //}

                        /***************Get Longitude and Latitude from Zip Code***************/

                        //string address = "";
                        //address = "http://maps.googleapis.com/maps/api/geocode/xml?components=postal_code:" + ZipCode.Trim() + "&sensor=false";

                        //var result = new System.Net.WebClient().DownloadString(address);
                        //XmlDocument doc = new XmlDocument();
                        //doc.LoadXml(result);
                        //XmlNodeList parentNode = doc.GetElementsByTagName("location");
                        //var lat = "";
                        //var lng = "";
                        //foreach (XmlNode childrenNode in parentNode)
                        //{
                        //    lat = childrenNode.SelectSingleNode("lat").InnerText;
                        //    lng = childrenNode.SelectSingleNode("lng").InnerText;
                        //}

                        //if (lat != "")
                        //{
                        //    latitude = lat;
                        //}
                        //if (lng != "")
                        //{
                        //    longitude = lng;
                        //}

                        /***************End of Get Longitude and Latitude from Zip Code***************/

                        if (data.Title != null)
                        {
                            Title = data.Title.ToString();
                        }

                        if (data.Company != null)
                        {
                            Company = data.Company.ToString();
                        }


                        //
                        if (data.ProfilePhoto != null && data.HiddenProfilePhoto == "true")
                        {
                            ProfilePic = data.ProfilePhoto.ToString().Replace("C:\\fakepath\\", "");
                            IsProfilePhotoChanged = "true";
                        }

                        if (data.CompanyLogo != null && data.HiddenCompanyLogo == "true")
                        {
                            CompanyLogo = data.CompanyLogo.ToString().Replace("C:\\fakepath\\", "");
                            IsCompanyLogoChanged = "true";
                        }

                        //End of More to work

                        #region Save Company Logo and Profile Pic
                        /*Save Company Logo & Profile Picture */

                        if (file == null)
                        {

                        }
                        else
                        {
                            foreach (var f in file)
                            {
                                if (f != null)
                                {
                                    string base64String = "";
                                    if (IsCompanyLogoChanged == "true" && IsCompanyLogoFileSaved == "false")
                                    {
                                        if (f.ContentLength > 0)
                                        {
                                            if (Path.GetFileName(f.FileName) == CompanyLogo)
                                            {
                                                int MaxContentLength = 1024 * 1024 * 4; //Size = 4 MB
                                                string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".jpe", ".jpeg" };
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
                                                    CompanyLogoPath = Email + ".png";

                                                    byte[] binaryData;
                                                    binaryData = new Byte[f.InputStream.Length];
                                                    long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                                    f.InputStream.Close();
                                                    base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                                    byte[] imageBytes1 = Convert.FromBase64String(base64String);
                                                    MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                                    ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                                    System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                                    string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/UploadedCompanyLogo/" + data.Email.ToString() + ".png");
                                                    bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                                    //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/UploadedCompanyLogo/" + Email + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                                    System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                                    thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/UploadedCompanyLogo/" + Email + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                                    ms1.Flush();
                                                    IsCompanyLogoFileSaved = "true";

                                                }
                                            }
                                        }
                                    }

                                    if (IsProfilePhotoChanged == "true" && IsProfilePhotoFileSaved == "false")
                                    {
                                        if (f.ContentLength > 0)
                                        {
                                            if (Path.GetFileName(f.FileName) == ProfilePic)
                                            {

                                                int MaxContentLength = 1024 * 1024 * 4; //Size = 4 MB
                                                string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".jpe", ".jpeg" };
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
                                                    ProfilePicPath = Email + ".png";

                                                    byte[] binaryData;
                                                    binaryData = new Byte[f.InputStream.Length];
                                                    long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                                    f.InputStream.Close();
                                                    base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                                    byte[] imageBytes1 = Convert.FromBase64String(base64String);
                                                    MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                                    ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                                    System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                                    string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + data.Email.ToString() + ".png");
                                                    bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                                    //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Email + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                                    Size thumbnailSize = GetThumbnailSize(image1);

                                                    System.Drawing.Image thumbnail = image1.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                                    thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Email + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                                    ms1.Flush();
                                                    IsProfilePhotoFileSaved = "true";
                                                }
                                            }
                                        }
                                    }
                                    //}
                                }
                            }
                        }

                        /*End of Save Company Logo & Profile Picture*/

                        #endregion Save Company Logo and Profile Pic


                        if (chkLanguages != null)
                        {
                            foreach (var Lang in chkLanguages)
                            {
                                Language = Language + "," + Lang;
                            }
                            Language = Language.TrimStart(',');
                        }

                        if (data.Specialities != null)
                        {
                            Specialities = data.Specialities.ToString();
                        }

                        if (data.HomeValue != null)
                        {
                            HomeValue = data.HomeValue.ToString();
                        }

                        if (data.AutoType != null)
                        {
                            AutoType = data.AutoType.ToString();
                        }

                        if (data.Revenue != null)
                        {
                            Revenue = data.Revenue.ToString();
                        }

                        if (data.Employees != null)
                        {
                            Employees = data.Employees.ToString();
                        }

                        if (data.CoverageAmt != null)
                        {
                            CoverageAmt = data.CoverageAmt.ToString();
                        }


                        /*************** Regarding Industry and SubIndustry *********************/

                        string SubIndustry1 = "", SubIndustryIds1 = "";
                        string SubIndustry2 = "", SubIndustryIds2 = "";
                        string SubIndustry3 = "", SubIndustryIds3 = "";

                        string IndustryId1 = ""; string IndustryId2 = ""; string IndustryId3 = "";

                        if (data.IsVisibleIndustry1.ToString() == "true")
                        {
                            if (Industry1 != null)
                            {
                                if (chkSubIndustry1 != null)
                                {
                                    IndustryId1 = IndustryId1 + "," + Industry1;
                                    IndustryId1 = IndustryId1.TrimStart(',');

                                    foreach (var Id in chkSubIndustry1)
                                    {
                                        SubIndustry1 = SubIndustry1 + "," + Id;
                                    }
                                    SubIndustry1 = SubIndustry1.TrimStart(',');
                                    SubIndustryIds1 = IndustryId1 + ":" + SubIndustry1;
                                }
                            }
                        }

                        if (data.IsVisibleIndustry2.ToString() == "true")
                        {
                            if (Industry2 != null)
                            {
                                if (chkSubIndustry2 != null)
                                {
                                    IndustryId2 = IndustryId2 + "," + Industry2;
                                    IndustryId2 = IndustryId2.TrimStart(',');

                                    foreach (var Id in chkSubIndustry2)
                                    {
                                        SubIndustry2 = SubIndustry2 + "," + Id;
                                    }
                                    SubIndustry2 = SubIndustry2.TrimStart(',');
                                    SubIndustryIds2 = IndustryId2 + ":" + SubIndustry2;
                                }
                            }
                        }

                        if (data.IsVisibleIndustry3.ToString() == "true")
                        {
                            if (Industry3 != null)
                            {
                                if (chkSubIndustry3 != null)
                                {
                                    IndustryId3 = IndustryId3 + "," + Industry3;
                                    IndustryId3 = IndustryId3.TrimStart(',');

                                    foreach (var Id in chkSubIndustry3)
                                    {
                                        SubIndustry3 = SubIndustry3 + "," + Id;
                                    }
                                    SubIndustry3 = SubIndustry3.TrimStart(',');
                                    SubIndustryIds3 = IndustryId3 + ":" + SubIndustry3;
                                }
                            }
                        }

                        IndustryId = IndustryId1 + "," + IndustryId2 + "," + IndustryId3;
                        SubIndustryId = SubIndustryIds1 + ";" + SubIndustryIds2 + ";" + SubIndustryIds3;

                        IndustryId = IndustryId.Trim(',');
                        SubIndustryId = SubIndustryId.Trim(';');

                        /*************** End of Regarding Industry and SubIndustry *********************/


                        //ExpiryDate = Request["datepicker"].ToString();
                        //random = BrokerWSUtility.GetRandomNumber();
                        //string Encryptrandom = BrokerUtility.EncryptURL(random);
                        Session["random"] = "";

                        /***************************** Save Broker Basic Details ***************************************/
                        int Result = BrokerWebDB.BrokerWebDB.UpdateBrokerBasicDetails(UserId, FirstName, LastName, Phone, Area, OnlyZipCode, Title, Company, Language, Specialities, CompanyLogoPath, ProfilePicPath, HomeValue, AutoType, Revenue, Employees, CoverageAmt, IndustryId, SubIndustryId, IsProfilePhotoChanged, IsCompanyLogoChanged, longitude, latitude, Bio);

                        if (Result > 0)
                        {
                            bool f1 = false, f2 = false, f3 = false, f4 = false, f5 = false;
                            string EduLogoPath1 = "", EduLogoPath2 = "", EduLogoPath3 = "", EduLogoPath4 = "", EduLogoPath5 = "";

                            string EducationLogoPathPrefix = strDomainName + "" + strEducationLogoFolder;

                            //Session["UserId"] = UserId;
                            //Session["EmailId"] = data.Email.ToString();


                            /*For Saving ZipCode details*/

                            #region Save ZipCode
                            int Result2 = BrokerWebDB.BrokerWebDB.DeleteZipCode(UserId.ToString());

                            if (ZipCode1 != "" && Addr1 != "")
                            {
                                string[] strZipcode1 = Addr1.Split(',');

                                string lng = strZipcode1[0];
                                string lat = strZipcode1[1];

                                int Result1 = BrokerWebDB.BrokerWebDB.InsertUserZipCode(UserId, ZipCode1, lng, lat);
                            }

                            if (ZipCode2 != "" && Addr2 != "")
                            {
                                string[] strZipcode2 = Addr2.Split(',');

                                string lng = strZipcode2[0];
                                string lat = strZipcode2[1];

                                int Result1 = BrokerWebDB.BrokerWebDB.InsertUserZipCode(UserId, ZipCode2, lng, lat);
                            }

                            if (ZipCode3 != "" && Addr3 != "")
                            {
                                string[] strZipcode3 = Addr3.Split(',');

                                string lng = strZipcode3[0];
                                string lat = strZipcode3[1];

                                int Result1 = BrokerWebDB.BrokerWebDB.InsertUserZipCode(UserId, ZipCode3, lng, lat);
                            }

                            if (ZipCode4 != "" && Addr4 != "")
                            {
                                string[] strZipcode4 = Addr4.Split(',');

                                string lng = strZipcode4[0];
                                string lat = strZipcode4[1];

                                int Result1 = BrokerWebDB.BrokerWebDB.InsertUserZipCode(UserId, ZipCode4, lng, lat);
                            }

                            if (ZipCode5 != "" && Addr5 != "")
                            {
                                string[] strZipcode5 = Addr5.Split(',');

                                string lng = strZipcode5[0];
                                string lat = strZipcode5[1];

                                int Result1 = BrokerWebDB.BrokerWebDB.InsertUserZipCode(UserId, ZipCode5, lng, lat);
                            }

                            #endregion

                            /*End of For Saving ZipCode details*/

                            //DELETE old records of education details.

                            int User1 = BrokerWebDB.BrokerWebDB.DeleteBrokerSubDetailsWeb(UserId, "UserEducation");

                            /*For Education 1*/
                            #region Education 1

                            if (data.School1 != "" && data.School1 != null)
                            {
                                School1 = data.School1.ToString();
                                f1 = true;
                            }

                            if (data.Degree1 != "" && data.Degree1 != null)
                            {
                                Degree1 = data.Degree1.ToString();
                                f1 = true;
                            }

                            if (data.Year1 != "" && data.Year1 != null)
                            {
                                Year1 = data.Year1.ToString();
                                f1 = true;
                            }

                            if (data.HiddenEducationLogoIsChanged1 == "true" && f1 == true)
                            {
                                if (data.EducationLogo1 != "" && data.EducationLogo1 != null)
                                {
                                    EducationLogo1 = data.EducationLogo1.ToString().Replace("C:\\fakepath\\", "");

                                    foreach (var f in file)
                                    {
                                        if (f != null)
                                        {
                                            if (f.ContentLength > 0)
                                            {
                                                if (Path.GetFileName(f.FileName) == EducationLogo1)
                                                {
                                                    EduLogoPath1 = UserId + "_" + School1 + "_" + Degree1 + ".png";

                                                    byte[] binaryData;
                                                    binaryData = new Byte[f.InputStream.Length];
                                                    long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                                    f.InputStream.Close();
                                                    string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                                    byte[] imageBytes1 = Convert.FromBase64String(base64String);
                                                    MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                                    ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                                    System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                                    string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1.Replace(" ", "") + "_" + Degree1.Replace(" ", "") + ".png");
                                                    bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                                    // image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1 + "_" + Degree1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                                    System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                                    thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1.Replace(" ", "") + "_" + Degree1.Replace(" ", "") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                                    ms1.Flush();
                                                }
                                            }
                                        }
                                        //f1 = true;
                                    }
                                }
                            }
                            else if (data.HiddenEducationLogoIsChanged1 == "false" && f1 == true) //Get the old image for this Education details
                            {
                                if (data.HiddenEducationLogoPath1 != null && data.HiddenEducationLogoPath1 != "")
                                {
                                    string OldEducationLogo1 = data.HiddenEducationLogoPath1.ToString();
                                    EduLogoPath1 = OldEducationLogo1.Replace(EducationLogoPathPrefix, "");
                                    //string output = input.Substring(input.IndexOf('.') + 1);

                                    //EduLogoPath1 = EduLogoPath1.Substring(EduLogoPath1.IndexOf('?') + 1);
                                    EduLogoPath1 = EduLogoPath1.Substring(0, EduLogoPath1.LastIndexOf("?"));
                                }
                            }

                            if (f1 == true)
                            {
                                Flag = BrokerWebDB.BrokerWebDB.SaveBrokerEducationDetails(UserId, School1, Degree1, Year1, EduLogoPath1.Replace(" ", ""));
                            }

                            #endregion Education 1
                            /*For Education 1*/

                            /*For Education 2*/
                            #region Education 2

                            if (data.School2 != "" && data.School2 != null)
                            {
                                School2 = data.School2.ToString();
                                f2 = true;
                            }

                            if (data.Degree2 != "" && data.Degree2 != null)
                            {
                                Degree2 = data.Degree2.ToString();
                                f2 = true;
                            }

                            if (data.Year2 != "" && data.Year2 != null)
                            {
                                Year2 = data.Year2.ToString();
                                f2 = true;
                            }

                            if (data.HiddenEducationLogoIsChanged2 == "true" && f2 == true)
                            {
                                if (data.EducationLogo2 != "" && data.EducationLogo2 != null)
                                {
                                    EducationLogo2 = data.EducationLogo2.ToString().Replace("C:\\fakepath\\", "");

                                    foreach (var f in file)
                                    {
                                        if (f != null)
                                        {
                                            if (f.ContentLength > 0)
                                            {
                                                if (Path.GetFileName(f.FileName) == EducationLogo2)
                                                {
                                                    EduLogoPath2 = UserId + "_" + School2 + "_" + Degree2 + ".png";

                                                    byte[] binaryData;
                                                    binaryData = new Byte[f.InputStream.Length];
                                                    long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                                    f.InputStream.Close();
                                                    string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                                    byte[] imageBytes1 = Convert.FromBase64String(base64String);
                                                    MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                                    ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                                    System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                                    string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School2.Replace(" ", "") + "_" + Degree2.Replace(" ", "") + ".png");
                                                    bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                                    //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School2 + "_" + Degree2 + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                                    System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                                    thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School2.Replace(" ", "") + "_" + Degree2.Replace(" ", "") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                                    ms1.Flush();
                                                }
                                            }
                                        }
                                        //f2 = true;
                                    }
                                }
                            }
                            else if (data.HiddenEducationLogoIsChanged2 == "false" && f2 == true) //Get the old image for this Education details
                            {
                                if (data.HiddenEducationLogoPath2 != null && data.HiddenEducationLogoPath2 != "")
                                {
                                    string OldEducationLogo2 = data.HiddenEducationLogoPath2.ToString();
                                    EduLogoPath2 = OldEducationLogo2.Replace(EducationLogoPathPrefix, "");

                                    EduLogoPath2 = EduLogoPath2.Substring(0, EduLogoPath2.LastIndexOf("?"));
                                }
                            }

                            if (f2 == true)
                            {
                                Flag = BrokerWebDB.BrokerWebDB.SaveBrokerEducationDetails(UserId, School2, Degree2, Year1, EduLogoPath2.Replace(" ", ""));
                            }

                            #endregion Education 2
                            /*For Education 2*/

                            /*For Education 3*/
                            #region Education 3

                            if (data.School3 != "" && data.School3 != null)
                            {
                                School3 = data.School3.ToString();
                                f3 = true;
                            }

                            if (data.Degree3 != "" && data.Degree3 != null)
                            {
                                Degree3 = data.Degree3.ToString();
                                f3 = true;
                            }

                            if (data.Year3 != "" && data.Year3 != null)
                            {
                                Year3 = data.Year3.ToString();
                                f3 = true;
                            }

                            if (data.HiddenEducationLogoIsChanged3 == "true" && f3 == true)
                            {
                                if (data.EducationLogo3 != "" && data.EducationLogo3 != null)
                                {
                                    EducationLogo3 = data.EducationLogo3.ToString().Replace("C:\\fakepath\\", "");

                                    foreach (var f in file)
                                    {
                                        if (f != null)
                                        {
                                            if (f.ContentLength > 0)
                                            {
                                                if (Path.GetFileName(f.FileName) == EducationLogo3)
                                                {
                                                    EduLogoPath3 = UserId + "_" + School3 + "_" + Degree3 + ".png";

                                                    byte[] binaryData;
                                                    binaryData = new Byte[f.InputStream.Length];
                                                    long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                                    f.InputStream.Close();
                                                    string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                                    byte[] imageBytes1 = Convert.FromBase64String(base64String);
                                                    MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                                    ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                                    System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                                    string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School3.Replace(" ", "") + "_" + Degree3.Replace(" ", "") + ".png");
                                                    bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                                    //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School3 + "_" + Degree3 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                                    System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                                    thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School3.Replace(" ", "") + "_" + Degree3.Replace(" ", "") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                                    ms1.Flush();
                                                }
                                            }
                                        }
                                        //f3 = true;
                                    }
                                }
                            }
                            else if (data.HiddenEducationLogoIsChanged3 == "false" && f3 == true) //Get the old image for this Education details
                            {
                                if (data.HiddenEducationLogoPath3 != null && data.HiddenEducationLogoPath3 != "")
                                {
                                    string OldEducationLogo3 = data.HiddenEducationLogoPath3.ToString();
                                    EduLogoPath3 = OldEducationLogo3.Replace(EducationLogoPathPrefix, "");

                                    EduLogoPath3 = EduLogoPath3.Substring(0, EduLogoPath3.LastIndexOf("?"));
                                }
                            }

                            if (f3 == true)
                            {
                                Flag = BrokerWebDB.BrokerWebDB.SaveBrokerEducationDetails(UserId, School3, Degree3, Year3, EduLogoPath3.Replace(" ", ""));
                            }

                            #endregion Education 3
                            /*For Education 3*/

                            /*For Education 4*/
                            #region Education 4

                            if (data.School4 != "" && data.School4 != null)
                            {
                                School4 = data.School4.ToString();
                                f4 = true;
                            }

                            if (data.Degree4 != "" && data.Degree4 != null)
                            {
                                Degree4 = data.Degree4.ToString();
                                f4 = true;
                            }

                            if (data.Year4 != "" && data.Year4 != null)
                            {
                                Year4 = data.Year4.ToString();
                                f4 = true;
                            }

                            if (data.HiddenEducationLogoIsChanged4 == "true" && f4 == true)
                            {
                                if (data.EducationLogo4 != "" && data.EducationLogo4 != null)
                                {
                                    EducationLogo4 = data.EducationLogo4.ToString().Replace("C:\\fakepath\\", "");

                                    foreach (var f in file)
                                    {
                                        if (f != null)
                                        {
                                            if (f.ContentLength > 0)
                                            {
                                                if (Path.GetFileName(f.FileName) == EducationLogo4)
                                                {
                                                    EduLogoPath4 = UserId + "_" + School4 + "_" + Degree4 + ".png";

                                                    byte[] binaryData;
                                                    binaryData = new Byte[f.InputStream.Length];
                                                    long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                                    f.InputStream.Close();
                                                    string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                                    byte[] imageBytes1 = Convert.FromBase64String(base64String);
                                                    MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                                    ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                                    System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                                    string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School4.Replace(" ", "") + "_" + Degree4.Replace(" ", "") + ".png");
                                                    bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                                    //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School4 + "_" + Degree4 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                                    System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                                    thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School4.Replace(" ", "") + "_" + Degree4.Replace(" ", "") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                                    ms1.Flush();
                                                }
                                            }
                                        }
                                        //f4 = true;
                                    }
                                }
                            }
                            else if (data.HiddenEducationLogoIsChanged4 == "false" && f4 == true) //Get the old image for this Education details
                            {
                                if (data.HiddenEducationLogoPath4 != null && data.HiddenEducationLogoPath4 != "")
                                {
                                    string OldEducationLogo4 = data.HiddenEducationLogoPath4.ToString();
                                    EduLogoPath4 = OldEducationLogo4.Replace(EducationLogoPathPrefix, "");

                                    EduLogoPath4 = EduLogoPath4.Substring(0, EduLogoPath4.LastIndexOf("?"));
                                }
                            }

                            if (f4 == true)
                            {
                                Flag = BrokerWebDB.BrokerWebDB.SaveBrokerEducationDetails(UserId, School4, Degree4, Year4, EduLogoPath4.Replace(" ", ""));
                            }

                            #endregion Education 4
                            /*For Education 4*/

                            /*For Education 5*/
                            #region Education 5

                            if (data.School5 != "" && data.School5 != null)
                            {
                                School5 = data.School5.ToString();
                                f5 = true;
                            }

                            if (data.Degree5 != "" && data.Degree5 != null)
                            {
                                Degree5 = data.Degree5.ToString();
                                f5 = true;
                            }

                            if (data.Year5 != "" && data.Year5 != null)
                            {
                                Year5 = data.Year5.ToString();
                                f5 = true;
                            }

                            if (data.HiddenEducationLogoIsChanged5 == "true" && f5 == true)
                            {
                                if (data.EducationLogo5 != "" && data.EducationLogo5 != null)
                                {
                                    EducationLogo5 = data.EducationLogo5.ToString().Replace("C:\\fakepath\\", "");

                                    foreach (var f in file)
                                    {
                                        if (f != null)
                                        {
                                            if (f.ContentLength > 0)
                                            {
                                                if (Path.GetFileName(f.FileName) == EducationLogo5)
                                                {
                                                    EduLogoPath5 = UserId + "_" + School5 + "_" + Degree5 + ".png";

                                                    byte[] binaryData;
                                                    binaryData = new Byte[f.InputStream.Length];
                                                    long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                                    f.InputStream.Close();
                                                    string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                                    byte[] imageBytes1 = Convert.FromBase64String(base64String);
                                                    MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                                    ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                                    System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                                    string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School5.Replace(" ", "") + "_" + Degree5.Replace(" ", "") + ".png");
                                                    bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                                    //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School5 + "_" + Degree5 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                                    System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                                    thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School5.Replace(" ", "") + "_" + Degree5.Replace(" ", "") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                                    ms1.Flush();
                                                }
                                            }
                                        }
                                        //f5 = true;
                                    }
                                }
                            }
                            else if (data.HiddenEducationLogoIsChanged5 == "false" && f5 == true) //Get the old image for this Education details
                            {
                                if (data.HiddenEducationLogoPath5 != null && data.HiddenEducationLogoPath5 != "")
                                {
                                    string OldEducationLogo5 = data.HiddenEducationLogoPath5.ToString();
                                    EduLogoPath5 = OldEducationLogo5.Replace(EducationLogoPathPrefix, "");

                                    EduLogoPath5 = EduLogoPath5.Substring(0, EduLogoPath5.LastIndexOf("?"));
                                }
                            }

                            if (f5 == true)
                            {
                                Flag = BrokerWebDB.BrokerWebDB.SaveBrokerEducationDetails(UserId, School5, Degree5, Year5, EduLogoPath5.Replace(" ", ""));
                            }

                            #endregion Education 5
                            /*For Education 5*/

                            /*For Saving Company Details*/
                            #region For Experience Details

                            //DELETE old records of education details.

                            int User2 = BrokerWebDB.BrokerWebDB.DeleteBrokerSubDetailsWeb(UserId, "UserExperience");

                            //if (CompanyNameRpt != null)
                            //{
                            //    foreach (var Id in CompanyNameRpt)
                            //    {
                            //          Flag = BrokerWebDB.BrokerWebDB.SaveBrokerExperienceDetails(UserId, Id);
                            //    }
                            //}

                            string ExpLogoPathPrefix = strDomainName + "" + strCompanyLogoFolder;

                            #region Experience 1

                            if (data.ExpCompanyName1 != "" && data.ExpCompanyName1 != null)
                            {
                                Company1 = data.ExpCompanyName1.ToString();
                                E1 = true;
                            }

                            if (data.ExpTitle1 != "" && data.ExpTitle1 != null)
                            {
                                Title1 = data.ExpTitle1.ToString();
                                E1 = true;
                            }

                            if (data.ExpDurationFrom1 != "" && data.ExpDurationFrom1 != null)
                            {
                                DurationFrom1 = data.ExpDurationFrom1.ToString();
                                E1 = true;
                            }

                            if (data.ExpDurationTo1 != "" && data.ExpDurationTo1 != null)
                            {
                                DurationTo1 = data.ExpDurationTo1.ToString();
                                E1 = true;
                            }

                            if (data.HiddenExpLogoIsChanged1 == "true" && E1 == true)
                            {
                                if (data.ExpLogo1 != "" && data.ExpLogo1 != null)
                                {
                                    ExpLogo1 = data.ExpLogo1.ToString().Replace("C:\\fakepath\\", "");

                                    foreach (var f in file)
                                    {
                                        if (f != null)
                                        {
                                            if (f.ContentLength > 0)
                                            {
                                                if (Path.GetFileName(f.FileName) == ExpLogo1)
                                                {
                                                    ExpLogoPath1 = UserId + "_" + Company1.Replace(" ", "") + "_" + Title1.Replace(" ", "") + ".png";

                                                    byte[] binaryData;
                                                    binaryData = new Byte[f.InputStream.Length];
                                                    long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                                    f.InputStream.Close();
                                                    string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                                    byte[] imageBytes1 = Convert.FromBase64String(base64String);
                                                    MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                                    ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                                    System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                                    string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + Company1.Replace(" ", "") + "_" + Title1.Replace(" ", "") + ".png");
                                                    bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                                    // image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1 + "_" + Degree1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                                    System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                                    thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + Company1.Replace(" ", "") + "_" + Title1.Replace(" ", "") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                                    ms1.Flush();
                                                }
                                            }
                                        }
                                        //f1 = true;
                                    }
                                }
                            }
                            else if (data.HiddenExpLogoIsChanged1 == "false" && E1 == true) //Get the old image for this Education details
                            {
                                if (data.HiddenExpLogoPath1 != null && data.HiddenExpLogoPath1 != "")
                                {
                                    string OldExpLogo1 = data.HiddenExpLogoPath1.ToString();
                                    ExpLogoPath1 = OldExpLogo1.Replace(ExpLogoPathPrefix, "");
                                    //string output = input.Substring(input.IndexOf('.') + 1);

                                    //EduLogoPath1 = EduLogoPath1.Substring(EduLogoPath1.IndexOf('?') + 1);
                                    ExpLogoPath1 = ExpLogoPath1.Substring(0, ExpLogoPath1.LastIndexOf("?"));
                                }
                            }

                            if (E1 == true)
                            {
                                Flag = BrokerWebDB.BrokerWebDB.UpdateBrokerExperienceDetails(UserId, Company1, Title1, DurationFrom1, DurationTo1, ExpLogoPath1);
                            }

                            #endregion Experience 1


                            #region Experience 2

                            if (data.ExpCompanyName2 != "" && data.ExpCompanyName2 != null)
                            {
                                Company2 = data.ExpCompanyName2.ToString();
                                E2 = true;
                            }

                            if (data.ExpTitle2 != "" && data.ExpTitle2 != null)
                            {
                                Title2 = data.ExpTitle2.ToString();
                                E2 = true;
                            }

                            if (data.ExpDurationFrom2 != "" && data.ExpDurationFrom2 != null)
                            {
                                DurationFrom2 = data.ExpDurationFrom2.ToString();
                                E2 = true;
                            }

                            if (data.ExpDurationTo2 != "" && data.ExpDurationTo2 != null)
                            {
                                DurationTo2 = data.ExpDurationTo2.ToString();
                                E2 = true;
                            }

                            if (data.HiddenExpLogoIsChanged2 == "true" && E2 == true)
                            {
                                if (data.ExpLogo2 != "" && data.ExpLogo2 != null)
                                {
                                    ExpLogo2 = data.ExpLogo2.ToString().Replace("C:\\fakepath\\", "");

                                    foreach (var f in file)
                                    {
                                        if (f != null)
                                        {
                                            if (f.ContentLength > 0)
                                            {
                                                if (Path.GetFileName(f.FileName) == ExpLogo2)
                                                {
                                                    ExpLogoPath2 = UserId + "_" + Company2.Replace(" ", "") + "_" + Title2.Replace(" ", "") + ".png";

                                                    byte[] binaryData;
                                                    binaryData = new Byte[f.InputStream.Length];
                                                    long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                                    f.InputStream.Close();
                                                    string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                                    byte[] imageBytes1 = Convert.FromBase64String(base64String);
                                                    MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                                    ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                                    System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                                    string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + Company2.Replace(" ", "") + "_" + Title2.Replace(" ", "") + ".png");
                                                    bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                                    // image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1 + "_" + Degree1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                                    System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                                    thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + Company2.Replace(" ", "") + "_" + Title2.Replace(" ", "") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                                    ms1.Flush();
                                                }
                                            }
                                        }
                                        //f2 = true;
                                    }
                                }
                            }
                            else if (data.HiddenExpLogoIsChanged2 == "false" && E2 == true) //Get the old image for this Education details
                            {
                                if (data.HiddenExpLogoPath2 != null && data.HiddenExpLogoPath2 != "")
                                {
                                    string OldExpLogo2 = data.HiddenExpLogoPath2.ToString();
                                    ExpLogoPath2 = OldExpLogo2.Replace(ExpLogoPathPrefix, "");
                                    //string output = input.Substring(input.IndexOf('.') + 2);

                                    //EduLogoPath2 = EduLogoPath2.Substring(EduLogoPath2.IndexOf('?') + 2);
                                    ExpLogoPath2 = ExpLogoPath2.Substring(0, ExpLogoPath2.LastIndexOf("?"));
                                }
                            }

                            if (E2 == true)
                            {
                                Flag = BrokerWebDB.BrokerWebDB.UpdateBrokerExperienceDetails(UserId, Company2, Title2, DurationFrom2, DurationTo2, ExpLogoPath2);
                            }

                            #endregion Experience 2


                            #region Experience 3

                            if (data.ExpCompanyName3 != "" && data.ExpCompanyName3 != null)
                            {
                                Company3 = data.ExpCompanyName3.ToString();
                                E3 = true;
                            }

                            if (data.ExpTitle3 != "" && data.ExpTitle3 != null)
                            {
                                Title3 = data.ExpTitle3.ToString();
                                E3 = true;
                            }

                            if (data.ExpDurationFrom3 != "" && data.ExpDurationFrom3 != null)
                            {
                                DurationFrom3 = data.ExpDurationFrom3.ToString();
                                E3 = true;
                            }

                            if (data.ExpDurationTo3 != "" && data.ExpDurationTo3 != null)
                            {
                                DurationTo3 = data.ExpDurationTo3.ToString();
                                E3 = true;
                            }

                            if (data.HiddenExpLogoIsChanged3 == "true" && E3 == true)
                            {
                                if (data.ExpLogo3 != "" && data.ExpLogo3 != null)
                                {
                                    ExpLogo3 = data.ExpLogo3.ToString().Replace("C:\\fakepath\\", "");

                                    foreach (var f in file)
                                    {
                                        if (f != null)
                                        {
                                            if (f.ContentLength > 0)
                                            {
                                                if (Path.GetFileName(f.FileName) == ExpLogo3)
                                                {
                                                    ExpLogoPath3 = UserId + "_" + Company3.Replace(" ", "") + "_" + Title3.Replace(" ", "") + ".png";

                                                    byte[] binaryData;
                                                    binaryData = new Byte[f.InputStream.Length];
                                                    long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                                    f.InputStream.Close();
                                                    string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                                    byte[] imageBytes1 = Convert.FromBase64String(base64String);
                                                    MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                                    ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                                    System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                                    string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + Company3.Replace(" ", "") + "_" + Title3.Replace(" ", "") + ".png");
                                                    bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                                    // image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1 + "_" + Degree1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                                    System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                                    thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + Company3.Replace(" ", "") + "_" + Title3.Replace(" ", "") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                                    ms1.Flush();
                                                }
                                            }
                                        }
                                        //f2 = true;
                                    }
                                }
                            }
                            else if (data.HiddenExpLogoIsChanged3 == "false" && E3 == true) //Get the old image for this Education details
                            {
                                if (data.HiddenExpLogoPath3 != null && data.HiddenExpLogoPath3 != "")
                                {
                                    string OldExpLogo3 = data.HiddenExpLogoPath3.ToString();
                                    ExpLogoPath3 = OldExpLogo3.Replace(ExpLogoPathPrefix, "");
                                    //string output = input.Substring(input.IndexOf('.') + 3);

                                    //EduLogoPath3 = EduLogoPath3.Substring(EduLogoPath3.IndexOf('?') + 3);
                                    ExpLogoPath3 = ExpLogoPath3.Substring(0, ExpLogoPath3.LastIndexOf("?"));
                                }
                            }

                            if (E3 == true)
                            {
                                Flag = BrokerWebDB.BrokerWebDB.UpdateBrokerExperienceDetails(UserId, Company3, Title3, DurationFrom3, DurationTo3, ExpLogoPath3);
                            }

                            #endregion Experience 3


                            #region Experience 4

                            if (data.ExpCompanyName4 != "" && data.ExpCompanyName4 != null)
                            {
                                Company4 = data.ExpCompanyName4.ToString();
                                E4 = true;
                            }

                            if (data.ExpTitle4 != "" && data.ExpTitle4 != null)
                            {
                                Title4 = data.ExpTitle4.ToString();
                                E4 = true;
                            }

                            if (data.ExpDurationFrom4 != "" && data.ExpDurationFrom4 != null)
                            {
                                DurationFrom4 = data.ExpDurationFrom4.ToString();
                                E4 = true;
                            }

                            if (data.ExpDurationTo4 != "" && data.ExpDurationTo4 != null)
                            {
                                DurationTo4 = data.ExpDurationTo4.ToString();
                                E4 = true;
                            }

                            if (data.HiddenExpLogoIsChanged4 == "true" && E4 == true)
                            {
                                if (data.ExpLogo4 != "" && data.ExpLogo4 != null)
                                {
                                    ExpLogo4 = data.ExpLogo4.ToString().Replace("C:\\fakepath\\", "");

                                    foreach (var f in file)
                                    {
                                        if (f != null)
                                        {
                                            if (f.ContentLength > 0)
                                            {
                                                if (Path.GetFileName(f.FileName) == ExpLogo4)
                                                {
                                                    ExpLogoPath4 = UserId + "_" + Company4.Replace(" ", "") + "_" + Title4.Replace(" ", "") + ".png";

                                                    byte[] binaryData;
                                                    binaryData = new Byte[f.InputStream.Length];
                                                    long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                                    f.InputStream.Close();
                                                    string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                                    byte[] imageBytes1 = Convert.FromBase64String(base64String);
                                                    MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                                    ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                                    System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                                    string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + Company4.Replace(" ", "") + "_" + Title4.Replace(" ", "") + ".png");
                                                    bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                                    // image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1 + "_" + Degree1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                                    System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                                    thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + Company4.Replace(" ", "") + "_" + Title4.Replace(" ", "") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                                    ms1.Flush();
                                                }
                                            }
                                        }
                                        //f2 = true;
                                    }
                                }
                            }
                            else if (data.HiddenExpLogoIsChanged4 == "false" && E4 == true) //Get the old image for this Education details
                            {
                                if (data.HiddenExpLogoPath4 != null && data.HiddenExpLogoPath4 != "")
                                {
                                    string OldExpLogo4 = data.HiddenExpLogoPath4.ToString();
                                    ExpLogoPath4 = OldExpLogo4.Replace(ExpLogoPathPrefix, "");
                                    //string output = input.Substring(input.IndexOf('.') + 4);

                                    //EduLogoPath4 = EduLogoPath4.Substring(EduLogoPath4.IndexOf('?') + 4);
                                    ExpLogoPath4 = ExpLogoPath4.Substring(0, ExpLogoPath4.LastIndexOf("?"));
                                }
                            }

                            if (E4 == true)
                            {
                                Flag = BrokerWebDB.BrokerWebDB.UpdateBrokerExperienceDetails(UserId, Company4, Title4, DurationFrom4, DurationTo4, ExpLogoPath4);
                            }

                            #endregion Experience 4



                            #region Experience 5

                            if (data.ExpCompanyName5 != "" && data.ExpCompanyName5 != null)
                            {
                                Company5 = data.ExpCompanyName5.ToString();
                                E5 = true;
                            }

                            if (data.ExpTitle5 != "" && data.ExpTitle5 != null)
                            {
                                Title5 = data.ExpTitle5.ToString();
                                E5 = true;
                            }

                            if (data.ExpDurationFrom5 != "" && data.ExpDurationFrom5 != null)
                            {
                                DurationFrom5 = data.ExpDurationFrom5.ToString();
                                E5 = true;
                            }

                            if (data.ExpDurationTo5 != "" && data.ExpDurationTo5 != null)
                            {
                                DurationTo5 = data.ExpDurationTo5.ToString();
                                E5 = true;
                            }

                            if (data.HiddenExpLogoIsChanged5 == "true" && E5 == true)
                            {
                                if (data.ExpLogo5 != "" && data.ExpLogo5 != null)
                                {
                                    ExpLogo5 = data.ExpLogo5.ToString().Replace("C:\\fakepath\\", "");

                                    foreach (var f in file)
                                    {
                                        if (f != null)
                                        {
                                            if (f.ContentLength > 0)
                                            {
                                                if (Path.GetFileName(f.FileName) == ExpLogo5)
                                                {
                                                    ExpLogoPath5 = UserId + "_" + Company5.Replace(" ", "") + "_" + Title5.Replace(" ", "") + ".png";

                                                    byte[] binaryData;
                                                    binaryData = new Byte[f.InputStream.Length];
                                                    long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                                    f.InputStream.Close();
                                                    string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                                    byte[] imageBytes1 = Convert.FromBase64String(base64String);
                                                    MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                                    ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                                    System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                                    string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + Company5.Replace(" ", "") + "_" + Title5.Replace(" ", "") + ".png");
                                                    bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                                    // image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1 + "_" + Degree1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                                    System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                                    thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + Company5.Replace(" ", "") + "_" + Title5.Replace(" ", "") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                                    ms1.Flush();
                                                }
                                            }
                                        }
                                        //f2 = true;
                                    }
                                }
                            }
                            else if (data.HiddenExpLogoIsChanged5 == "false" && E5 == true) //Get the old image for this Education details
                            {
                                if (data.HiddenExpLogoPath5 != null && data.HiddenExpLogoPath5 != "")
                                {
                                    string OldExpLogo5 = data.HiddenExpLogoPath5.ToString();
                                    ExpLogoPath5 = OldExpLogo5.Replace(ExpLogoPathPrefix, "");
                                    //string output = input.Substring(input.IndexOf('.') + 5);

                                    //EduLogoPath5 = EduLogoPath5.Substring(EduLogoPath5.IndexOf('?') + 5);
                                    ExpLogoPath5 = ExpLogoPath5.Substring(0, ExpLogoPath5.LastIndexOf("?"));
                                }
                            }

                            if (E5 == true)
                            {
                                Flag = BrokerWebDB.BrokerWebDB.UpdateBrokerExperienceDetails(UserId, Company5, Title5, DurationFrom5, DurationTo5, ExpLogoPath5);
                            }

                            #endregion Experience 5

                            #region Experience 6

                            if (data.ExpCompanyName6 != "" && data.ExpCompanyName6 != null)
                            {
                                Company6 = data.ExpCompanyName6.ToString();
                                E6 = true;
                            }

                            if (data.ExpTitle6 != "" && data.ExpTitle6 != null)
                            {
                                Title6 = data.ExpTitle6.ToString();
                                E6 = true;
                            }

                            if (data.ExpDurationFrom6 != "" && data.ExpDurationFrom6 != null)
                            {
                                DurationFrom6 = data.ExpDurationFrom6.ToString();
                                E6 = true;
                            }

                            if (data.ExpDurationTo6 != "" && data.ExpDurationTo6 != null)
                            {
                                DurationTo6 = data.ExpDurationTo6.ToString();
                                E6 = true;
                            }

                            if (data.HiddenExpLogoIsChanged6 == "true" && E6 == true)
                            {
                                if (data.ExpLogo6 != "" && data.ExpLogo6 != null)
                                {
                                    ExpLogo6 = data.ExpLogo6.ToString().Replace("C:\\fakepath\\", "");

                                    foreach (var f in file)
                                    {
                                        if (f != null)
                                        {
                                            if (f.ContentLength > 0)
                                            {
                                                if (Path.GetFileName(f.FileName) == ExpLogo6)
                                                {
                                                    ExpLogoPath6 = UserId + "_" + Company6.Replace(" ", "") + "_" + Title6.Replace(" ", "") + ".png";

                                                    byte[] binaryData;
                                                    binaryData = new Byte[f.InputStream.Length];
                                                    long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                                    f.InputStream.Close();
                                                    string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                                    byte[] imageBytes1 = Convert.FromBase64String(base64String);
                                                    MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                                    ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                                    System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                                    string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + Company6.Replace(" ", "") + "_" + Title6.Replace(" ", "") + ".png");
                                                    bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                                    // image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1 + "_" + Degree1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                                    System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                                    thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + Company6.Replace(" ", "") + "_" + Title6.Replace(" ", "") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                                    ms1.Flush();
                                                }
                                            }
                                        }
                                        //f2 = true;
                                    }
                                }
                            }
                            else if (data.HiddenExpLogoIsChanged6 == "false" && E6 == true) //Get the old image for this Education details
                            {
                                if (data.HiddenExpLogoPath6 != null && data.HiddenExpLogoPath6 != "")
                                {
                                    string OldExpLogo6 = data.HiddenExpLogoPath6.ToString();
                                    ExpLogoPath6 = OldExpLogo6.Replace(ExpLogoPathPrefix, "");
                                    //string output = input.Substring(input.IndexOf('.') + 6);

                                    //EduLogoPath6 = EduLogoPath6.Substring(EduLogoPath6.IndexOf('?') + 6);
                                    ExpLogoPath6 = ExpLogoPath6.Substring(0, ExpLogoPath6.LastIndexOf("?"));
                                }
                            }

                            if (E6 == true)
                            {
                                Flag = BrokerWebDB.BrokerWebDB.UpdateBrokerExperienceDetails(UserId, Company6, Title6, DurationFrom6, DurationTo6, ExpLogoPath6);
                            }

                            #endregion Experience 6

                            #region Experience 7

                            if (data.ExpCompanyName7 != "" && data.ExpCompanyName7 != null)
                            {
                                Company7 = data.ExpCompanyName7.ToString();
                                E7 = true;
                            }

                            if (data.ExpTitle7 != "" && data.ExpTitle7 != null)
                            {
                                Title7 = data.ExpTitle7.ToString();
                                E7 = true;
                            }

                            if (data.ExpDurationFrom7 != "" && data.ExpDurationFrom7 != null)
                            {
                                DurationFrom7 = data.ExpDurationFrom7.ToString();
                                E7 = true;
                            }

                            if (data.ExpDurationTo7 != "" && data.ExpDurationTo7 != null)
                            {
                                DurationTo7 = data.ExpDurationTo7.ToString();
                                E7 = true;
                            }

                            if (data.HiddenExpLogoIsChanged7 == "true" && E7 == true)
                            {
                                if (data.ExpLogo7 != "" && data.ExpLogo7 != null)
                                {
                                    ExpLogo7 = data.ExpLogo7.ToString().Replace("C:\\fakepath\\", "");

                                    foreach (var f in file)
                                    {
                                        if (f != null)
                                        {
                                            if (f.ContentLength > 0)
                                            {
                                                if (Path.GetFileName(f.FileName) == ExpLogo7)
                                                {
                                                    ExpLogoPath7 = UserId + "_" + Company7.Replace(" ", "") + "_" + Title7.Replace(" ", "") + ".png";

                                                    byte[] binaryData;
                                                    binaryData = new Byte[f.InputStream.Length];
                                                    long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                                    f.InputStream.Close();
                                                    string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                                    byte[] imageBytes1 = Convert.FromBase64String(base64String);
                                                    MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                                    ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                                    System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                                    string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + Company7.Replace(" ", "") + "_" + Title7.Replace(" ", "") + ".png");
                                                    bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                                    // image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1 + "_" + Degree1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                                    System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                                    thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + Company7.Replace(" ", "") + "_" + Title7.Replace(" ", "") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                                    ms1.Flush();
                                                }
                                            }
                                        }
                                        //f2 = true;
                                    }
                                }
                            }
                            else if (data.HiddenExpLogoIsChanged7 == "false" && E7 == true) //Get the old image for this Education details
                            {
                                if (data.HiddenExpLogoPath7 != null && data.HiddenExpLogoPath7 != "")
                                {
                                    string OldExpLogo7 = data.HiddenExpLogoPath7.ToString();
                                    ExpLogoPath7 = OldExpLogo7.Replace(ExpLogoPathPrefix, "");
                                    //string output = input.Substring(input.IndexOf('.') + 7);

                                    //EduLogoPath7 = EduLogoPath7.Substring(EduLogoPath7.IndexOf('?') + 7);
                                    ExpLogoPath7 = ExpLogoPath7.Substring(0, ExpLogoPath7.LastIndexOf("?"));
                                }
                            }

                            if (E7 == true)
                            {
                                Flag = BrokerWebDB.BrokerWebDB.UpdateBrokerExperienceDetails(UserId, Company7, Title7, DurationFrom7, DurationTo7, ExpLogoPath7);
                            }

                            #endregion Experience 7
                            #endregion
                            /*End of For Saving Company Details*/

                            /*For Saving Industry Details*/
                            #region For Industry Details

                            //Delete Old Industry and SubIndustryId's

                            int User3 = BrokerWebDB.BrokerWebDB.DeleteBrokerSubDetailsWeb(UserId, "UserIndustry");

                            if (data.IsVisibleIndustry1.ToString() == "true")
                            {
                                if (Industry1 != null)
                                {
                                    if (chkSubIndustry1 != null)
                                    {
                                        foreach (var Ids in chkSubIndustry1)
                                        {
                                            Flag = BrokerWebDB.BrokerWebDB.SaveBrokerIndustryDetails(UserId, Industry1, Ids);
                                        }
                                    }
                                }
                            }


                            if (data.IsVisibleIndustry2.ToString() == "true")
                            {
                                if (Industry2 != null)
                                {
                                    if (chkSubIndustry2 != null)
                                    {
                                        foreach (var Ids in chkSubIndustry2)
                                        {
                                            Flag = BrokerWebDB.BrokerWebDB.SaveBrokerIndustryDetails(UserId, Industry2, Ids);
                                        }
                                    }
                                }
                            }

                            if (data.IsVisibleIndustry3.ToString() == "true")
                            {
                                if (Industry3 != null)
                                {
                                    if (chkSubIndustry3 != null)
                                    {
                                        foreach (var Ids in chkSubIndustry3)
                                        {
                                            Flag = BrokerWebDB.BrokerWebDB.SaveBrokerIndustryDetails(UserId, Industry3, Ids);
                                        }
                                    }
                                }
                            }
                            #endregion For Industry Details
                            /*End of For Saving Industry Details*/

                            //bool EmailFlag = false;

                            //EmailFlag = BrokerWSUtility.SendRegistrationEmail(Session["EmailId"].ToString(), Session["random"].ToString(), Session["UserId"].ToString(), "Broker");
                            //if (EmailFlag)
                            //{
                            //    ViewBag.VerificationMessage = "You are registered successfully but yet not activated. <br/> Please accept your verification email.";
                            //    return View("Success");
                            //}
                            //else
                            //{
                            //    //If User registerd successfully, but verification link has not
                            //    //been sent over EmailId
                            //    ViewBag.VerificationMessage = "You are registered successfully but yet not activated. <br/>Please accept your verification email.";
                            //    return View("Error");
                            //}

                            /*********************** Get the current changes from db***************************************/
                            dsGetBrokerDetails = BrokerWebDB.BrokerWebDB.GetBrokerDetails(Session["EmailId"].ToString());

                            if (dsGetBrokerDetails.Tables.Count > 0)
                            {
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

                                string CompanyLogo1 = dsGetBrokerDetails.Tables[0].Rows[0]["CompanyLogo"].ToString();
                                if (CompanyLogo1 != "")
                                {
                                    CompanyLogo1 = strDomainName + "" + strUploadedCompLogoFolder + "" + dsGetBrokerDetails.Tables[0].Rows[0]["CompanyLogo"].ToString();

                                    dsGetBrokerDetails.Tables[0].Rows[0]["CompanyLogo"] = CompanyLogo1 + "?" + DateTime.Now;
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
                            }

                            /***************************End of get current changes from DB***********************************/
                            return RedirectToAction("BrokerProfile", "Profile");

                            //return RedirectToAction("MakePayment", "BrokerRegistration", new { EmailId = BrokerUtility.EncryptURL(Email), RegistrationCode = BrokerUtility.EncryptURL(random) });
                        }
                        //return View();
                        //}
                        #endregion Save Broker Details

                        //return RedirectToAction("MakePayment", "BrokerRegistration", new { EmailId = BrokerUtility.EncryptURL("samplemail@gmail.com"), RegistrationCode = BrokerUtility.EncryptURL("123456789") });

                        return RedirectToAction("BrokerProfile", "Profile");
                    }
                    else if (Request.Form["CANCEL"] != null)
                    {
                        return RedirectToAction("BrokerProfile", "Profile");
                    }
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "EditBroker()_Website", Ex.Message.ToString(), "BrokerRegistrationController.cs_EditBroker()", "");
            }

            return View();
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
        [AllowAnonymous]
        public ActionResult BrokerSignUpOLD()
        {
            var db = new BrokerDBEntities();
            ViewBag.CompanyMaster = new SelectList(db.CompanyMasters.ToList(), "CompanyId", "CompanyName");

            ViewBag.IndustryMaster = new SelectList(db.IndustryMasters.ToList(), "IndustryId", "IndustryName").OrderBy(c => c.Text);
            int id = 1;

            ViewBag.SubIndustryMaster = new SelectList(db.SubIndustryMasters.Where(c => c.IndustryId == id).Select(c => new { Value = c.Id, Text = c.SICCode + " - " + c.SubIndustryName }).ToList().OrderBy(c => c.Text), "Id", "SubIndustryName");

            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult PaymentError()
        {
            return View();
        }

        public static string GetPositionFromZip(string ZipCode)
        {
            string address = "", latitude = "", longitude = "";

            address = "https://maps.googleapis.com/maps/api/geocode/xml?components=postal_code:" + ZipCode.Trim() + "&key="+strGoogleMapKey+"&sensor=false";

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

            if (lat != "")
            {
                latitude = lat;
            }
            if (lng != "")
            {
                longitude = lng;
            }

            return (longitude + "," + latitude);
        }

        public bool ThumbnailCallback()
        {
            return false;
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult IsEmailIdAlreadyExist(AllModels data)
        {
            try
            {
                DataSet dsCheckUserExist = BrokerUtility.CheckUSerExist(data.BrokerInfo[0].Email.ToString());

                if (dsCheckUserExist.Tables.Count > 0)
                {
                    return Json("Email Id already registered.", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "IsEmailIdAlreadyExist_Website", Ex.Message.ToString(), "BrokerRegistrationController.cs_IsEmailIdAlreadyExist()", "");
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public ActionResult MakePayment(string EmailId, string RegistrationCode)
        {
            int UserId = 0;
            bool IsPaymentDone = false;

            string DecyptedEmailId = BrokerUtility.DecryptURL(EmailId);

            DataSet dsCheckUserExist = BrokerUtility.CheckUSerExist(DecyptedEmailId);

            if (dsCheckUserExist.Tables.Count > 0)
            {
                UserId = Convert.ToInt32(dsCheckUserExist.Tables[0].Rows[0]["UserId"].ToString());
                IsPaymentDone = Convert.ToBoolean(dsCheckUserExist.Tables[0].Rows[0]["IsPaymentDone"].ToString());

                Session["UserId"] = UserId;
                Session["EmailId"] = DecyptedEmailId;

                if (IsPaymentDone == false)
                {

                    List<uspGetPaymentAmount_Result> oPaymentDetails = null;
                    BrokerPaymentModel BrokerPaymentModel = new BrokerPaymentModel();
                    oPaymentDetails = BrokerUtility.GetPaymentDetails();
                    BrokerPaymentModel.Amount = oPaymentDetails[0].Amount;

                    return View(BrokerPaymentModel);
                }
                else
                {
                    //Go to view if user has already paid the amount.
                    ViewBag.Message = "Your account is activated. Please Log In to applicaton. ";
                    return View("PaymentAlreadyDone");
                }
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult MakePayment(BrokerPaymentModel Broker)
        {
            //string redirectUrl = "";
            ////string UserId = Session["UserId"].ToString();


            ////Mention URL to redirect content to paypal site
            //redirectUrl += "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick&business=" + ConfigurationManager.AppSettings["paypalemail"].ToString();

            ////First name I assign static based on login details assign this value
            //redirectUrl += "&first_name=" + ConfigurationManager.AppSettings["FirstName"].ToString();

            ////Product Name
            ////redirectUrl += "&item_name=" + pName.Text;
            //redirectUrl += "&item_name=Membership Fee";

            ////Product Amount
            //redirectUrl += "&amount=" + Broker.Amount;

            ////Business contact paypal EmailID
            //redirectUrl += "&business=" + ConfigurationManager.AppSettings["paypalemail"].ToString();

            ////Shipping charges if any, or available or using shopping cart system
            //redirectUrl += "&shipping=0";

            ////Handling charges if any, or available or using shopping cart system
            //redirectUrl += "&handling=0";

            ////Tax charges if any, or available or using shopping cart system
            //redirectUrl += "&tax=0";

            ////Quantiy of product, Here statically added quantity 1
            //redirectUrl += "&quantity=1";

            ////If transactioin has been successfully performed, redirect SuccessURL page- this page will be designed by developer
            //redirectUrl += "&return=" + ConfigurationManager.AppSettings["SuccessURL"].ToString();

            ////If transactioin has been failed, redirect FailedURL page- this page will be designed by developer
            //redirectUrl += "&cancel_return=" + ConfigurationManager.AppSettings["FailedURL"].ToString();

            //Response.Redirect(redirectUrl);

            return RedirectToAction("Index", "BrokerRegistration");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult PaymentSuccess(string tx, string st, string amt)
        {
            List<uspInsertPaymentDetails_Result> user = null;
            bool status = false;
            //string UserId = Session["UserId"].ToString();

            try
            {
                if (st == "Completed")
                {
                    status = true;

                    if (Session["UserId"] != null)
                    {
                        string UserId = Session["UserId"].ToString();

                        //Make an User's payment entry into database.

                        user = BrokerUtility.InsertPaymentDetails(UserId, tx, "Membership Fee", "Membership Fee", amt, status, "Online");
                        if (user.Count > 0)
                        {
                            int Success = BrokerUtility.UpdateBrokerPaymentDoneFlag(UserId);

                            ViewBag.Status = st;
                            ViewBag.TransactionId = tx;
                            ViewBag.Amount = amt;
                        }
                    }

                    // Send Verification Link on Email Id

                    //string random = BrokerWSUtility.GetRandomNumber();

                    bool EmailFlag = false;

                    EmailFlag = BrokerWSUtility.SendRegistrationEmail(Session["EmailId"].ToString(), Session["random"].ToString(), Session["UserId"].ToString(), "Broker");
                    if (EmailFlag)
                    {
                        //ViewBag.VerificationMessage = "You are registered successfully but yet not activated. <br/>Please accept your verification email.";
                        return View();
                    }
                    else
                    {
                        //If User registerd successfully, Payment also done but verification link has not
                        //been sent over EmailId
                        //ViewBag.VerificationMessage = "Due to some circumstances Email is not sent.";
                        return View("PaymentError");
                    }
                }
                else
                {
                    return View();//Go to Payment Error View. 
                }

            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(1, "PaymentSuccess", ex.Message.ToString(), "BrokerRegistrationController.cs_PaymentSuccess()", "");
                return View();
            }
        }

        //[AllowAnonymous]
        //public JsonResult GetIndustryMaster()
        //{
        //    BrokerDBEntities Db = new BrokerDBEntities();

        //    var resultData = Db.IndustryMasters.Select(c => new { Value = c.IndustryId, Text = c.IndustryName }).ToList();
        //    return Json(new { result = resultData }, JsonRequestBehavior.AllowGet);
        //}

        [AllowAnonymous]
        public JsonResult GetSubIndustryMaster(int id)
        {
            BrokerDBEntities Db = new BrokerDBEntities();

            //var resultData = Db.SubIndustryMasters.Select(c => new { Value = c.IndustryId, Text = c.SubIndustryName }).ToList();
            var resultData = Db.SubIndustryMasters.Where(c => c.IndustryId == id).Select(c => new { Value = c.Id, Text = c.SICCode + " - " + c.SubIndustryName }).ToList();

            return Json(new { result = resultData }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetIndustryMaster()
        {
            BrokerDBEntities Db = new BrokerDBEntities();

            var resultData = Db.IndustryMasters.Where(c => c.CompanyId == 1).Select(c => new { Value = c.IndustryId, Text = c.IndustryName }).ToList().OrderBy(c => c.Text);

            return Json(new { result = resultData }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult BrokerSignUp()
        {
            ViewBag.iPhoneAppPath = striPhoneAppPath;
            ViewBag.AndroidAppPath = strAndroidAppPath;

            var db = new BrokerDBEntities();
            ViewBag.CompanyMaster = new SelectList(db.CompanyMasters.ToList(), "CompanyId", "CompanyName");

            ViewBag.IndustryMaster = new SelectList(db.IndustryMasters.ToList(), "IndustryId", "IndustryName").OrderBy(c => c.Text);
            int id = 0;

            ViewBag.SubIndustryMaster = new SelectList(db.SubIndustryMasters.Where(c => c.IndustryId == id).Select(c => new { Value = c.Id, Text = c.SICCode + " - " + c.SubIndustryName }).ToList().OrderBy(c => c.Text), "Id", "SubIndustryName");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult BrokerSignUp(AllModels data, IEnumerable<HttpPostedFileBase> file, FormCollection Form, string[] chkLanguages, string[] CompanyNameRpt, string Industry1, string[] chkSubIndustry1, string Industry2, string[] chkSubIndustry2, string Industry3, string[] chkSubIndustry3)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Request.Form["Save"] != null)
                    {
                        //string strDDLValue = Form["CompanyName1"].ToString();

                        #region Save Broker Details

                        List<uspSaveBrokerBasicDetails_Result> User = null;

                        string FirstName = "", LastName = "", Phone = "", Email = "", Title = "", Bio = "",
                           Language = "", Specialities = "", Phone1 = "", Password = "", TempPass = "", random = "", Area = "", Company = "",
                            ProfilePic = "", CompanyLogo = "", CompanyLogoPath = "", ProfilePicPath = "";

                        string Degree1 = "", Degree2 = "", Degree3 = "", Degree4 = "", Degree5 = "";
                        string School1 = "", School2 = "", School3 = "", School4 = "", School5 = "";
                        string Year1 = "", Year2 = "", Year3 = "", Year4 = "", Year5 = "";
                        string EducationLogo1 = "", EducationLogo2 = "", EducationLogo3 = "", EducationLogo4 = "", EducationLogo5 = "";

                        string HomeValue = "", AutoType = "", Revenue = "", Employees = "", CoverageAmt = "";

                        string IndustryId = "", SubIndustryId = "", longitude = "", latitude = "";

                        string Addr1 = "", Addr2 = "", Addr3 = "", Addr4 = "", Addr5 = "";
                        string ZipCode = "", ZipCode1 = "", ZipCode2 = "", ZipCode3 = "", ZipCode4 = "", ZipCode5 = "", OnlyZipCodes = "";

                        string ExpLogo1 = "", ExpLogo2 = "", ExpLogo3 = "", ExpLogo4 = "", ExpLogo5 = "", ExpLogo6 = "", ExpLogo7 = "";
                        string ExpComp1 = "", ExpComp2 = "", ExpComp3 = "", ExpComp4 = "", ExpComp5 = "", ExpComp6 = "", ExpComp7 = "";
                        string ExpDesig1 = "", ExpDesig2 = "", ExpDesig3 = "", ExpDesig4 = "", ExpDesig5 = "", ExpDesig6 = "", ExpDesig7 = "";
                        string ExpFromMonth1 = "", ExpFromMonth2 = "", ExpFromMonth3 = "", ExpFromMonth4 = "", ExpFromMonth5 = "", ExpFromMonth6 = "", ExpFromMonth7 = "";
                        string ExpFromYear1 = "", ExpFromYear2 = "", ExpFromYear3 = "", ExpFromYear4 = "", ExpFromYear5 = "", ExpFromYear6 = "", ExpFromYear7 = "";
                        string ExpToMonth1 = "", ExpToMonth2 = "", ExpToMonth3 = "", ExpToMonth4 = "", ExpToMonth5 = "", ExpToMonth6 = "", ExpToMonth7 = "";
                        string ExpToYear1 = "", ExpToYear2 = "", ExpToYear3 = "", ExpToYear4 = "", ExpToYear5 = "", ExpToYear6 = "", ExpToYear7 = "";


                        int UserId = 0, Flag = 0;

                        if (data.BrokerInfo[0].FirstName != null)
                        {
                            FirstName = data.BrokerInfo[0].FirstName.ToString();
                        }
                        if (data.BrokerInfo[0].LastName != null)
                        {
                            LastName = data.BrokerInfo[0].LastName.ToString();
                        }

                        if (data.BrokerInfo[0].PhoneNo1 != null)
                        {
                            Phone1 = data.BrokerInfo[0].PhoneNo1.ToString();
                        }

                        Phone = Phone1;

                        if (data.BrokerInfo[0].Email != null)
                        {
                            Email = data.BrokerInfo[0].Email.ToString();
                        }

                        if (data.BrokerInfo[0].Bio != null)
                        {
                            Bio = data.BrokerInfo[0].Bio.ToString().Trim();
                        }

                        if (data.BrokerInfo[0].Password != null)
                        {
                            Password = data.BrokerInfo[0].Password.ToString();
                            TempPass = BrokerUtility.EncryptURL(Password);
                        }

                        if (data.BrokerInfo[0].Area != null)
                        {
                            Area = data.BrokerInfo[0].Area.ToString();
                        }

                        if (data.BrokerInfo[0].ZipCode != null)
                        {
                            ZipCode1 = data.BrokerInfo[0].ZipCode.ToString();
                            Addr1 = GetPositionFromZip(ZipCode1);

                            ZipCode = ZipCode1 + ":" + Addr1;
                            OnlyZipCodes = ZipCode1;
                        }

                        if (data.BrokerInfo[0].ZipCode2 != null)
                        {
                            ZipCode2 = data.BrokerInfo[0].ZipCode2.ToString();
                            Addr2 = GetPositionFromZip(ZipCode2);

                            ZipCode = ZipCode + ";" + ZipCode2 + ":" + Addr2;
                            OnlyZipCodes = OnlyZipCodes + "," + ZipCode2;
                        }

                        if (data.BrokerInfo[0].ZipCode3 != null)
                        {
                            ZipCode3 = data.BrokerInfo[0].ZipCode3.ToString();
                            Addr3 = GetPositionFromZip(ZipCode3);

                            ZipCode = ZipCode + ";" + ZipCode3 + ":" + Addr3;
                            OnlyZipCodes = OnlyZipCodes + "," + ZipCode3;
                        }

                        if (data.BrokerInfo[0].ZipCode4 != null)
                        {
                            ZipCode4 = data.BrokerInfo[0].ZipCode4.ToString();
                            Addr4 = GetPositionFromZip(ZipCode4);

                            ZipCode = ZipCode + ";" + ZipCode4 + ":" + Addr4;
                            OnlyZipCodes = OnlyZipCodes + "," + ZipCode4;
                        }

                        if (data.BrokerInfo[0].ZipCode5 != null)
                        {
                            ZipCode5 = data.BrokerInfo[0].ZipCode5.ToString();
                            Addr5 = GetPositionFromZip(ZipCode5);

                            ZipCode = ZipCode + ";" + ZipCode5 + ":" + Addr5;
                            OnlyZipCodes = OnlyZipCodes + "," + ZipCode5;
                        }

                        /***************Get Longitude and Latitude from Zip Code***************/
                        /***************End of Get Longitude and Latitude from Zip Code***************/


                        if (data.BrokerInfo[0].Title != null)
                        {
                            Title = data.BrokerInfo[0].Title.ToString();
                        }

                        if (data.BrokerInfo[0].Company != null)
                        {
                            Company = data.BrokerInfo[0].Company.ToString();
                        }

                        if (data.BrokerInfo[0].ProfilePhoto != null)
                        {
                            ProfilePic = data.BrokerInfo[0].ProfilePhoto.ToString().Replace("C:\\fakepath\\", "");
                        }

                        if (data.BrokerInfo[0].CompanyLogo != null)
                        {
                            CompanyLogo = data.BrokerInfo[0].CompanyLogo.ToString().Replace("C:\\fakepath\\", ""); ;
                        }

                        #region Save Profile Pic--New Logic
                        if (ProfilePic != "")
                        {
                            //if (data.BrokerInfo[0].HiddenProfilePhoto.ToString() != "" && data.BrokerInfo[0].HiddenProfilePhoto.ToString() != null)
                            //{
                            foreach (var f in file)
                            {
                                if (f != null)
                                {
                                    if (f.ContentLength > 0)
                                    {
                                        if (Path.GetFileName(f.FileName) == ProfilePic.Replace("C:\\fakepath\\", ""))
                                        {
                                            ProfilePicPath = Email + ".png";
                                            byte[] binaryData;
                                            binaryData = new Byte[f.InputStream.Length];
                                            long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                            f.InputStream.Close();
                                            //string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                            MemoryStream ms1 = new MemoryStream(binaryData, 0, binaryData.Length);

                                            ms1.Write(binaryData, 0, binaryData.Length);
                                            System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                            string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + data.BrokerInfo[0].Email.ToString() + ".png");
                                            bool CheckFile = BrokerUtility.CheckFile(FileName1);
                                            Size thumbnailSize = GetThumbnailSize(image1);
                                            System.Drawing.Image thumbnail = image1.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                            thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Email + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                        }
                                    }
                                }
                            }


                            //ProfilePicPath = Email + ".png";

                            ////byte[] binaryData;
                            ////binaryData = new Byte[f.InputStream.Length];
                            ////long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                            ////f.InputStream.Close();
                            ////base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                            //byte[] imageBytes1 = Convert.FromBase64String(data.BrokerInfo[0].HiddenProfilePhoto.ToString());
                            //MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                            //ms1.Write(imageBytes1, 0, imageBytes1.Length);
                            //System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                            //string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + data.BrokerInfo[0].Email.ToString() + ".png");
                            //bool CheckFile = BrokerUtility.CheckFile(FileName1);

                            //System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                            //thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Email + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                            //}
                        }
                        #endregion Save Profile Pic--New Logic

                        #region Save Company Logo--New Logic
                        if (CompanyLogo != "")
                        {
                            if (data.BrokerInfo[0].HiddenCompanyLogo.ToString() != "" && data.BrokerInfo[0].HiddenCompanyLogo.ToString() != null)
                            {

                                CompanyLogoPath = Email + ".png";

                                //byte[] binaryData;
                                //binaryData = new Byte[f.InputStream.Length];
                                //long bytesRead = f.InputStream.Read(binaryData, 0, (int)f.InputStream.Length);
                                //f.InputStream.Close();
                                //base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

                                byte[] imageBytes1 = Convert.FromBase64String(data.BrokerInfo[0].HiddenCompanyLogo.ToString());
                                MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/UploadedCompanyLogo/" + data.BrokerInfo[0].Email.ToString() + ".png");
                                bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/UploadedCompanyLogo/" + Email + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                            }
                        }
                        #endregion  Save Company Logo--New Logic

                        if (chkLanguages != null)
                        {
                            foreach (var Lang in chkLanguages)
                            {
                                Language = Language + "," + Lang;
                            }
                            Language = Language.TrimStart(',');
                        }

                        if (data.BrokerInfo[0].Specialities != null)
                        {
                            Specialities = data.BrokerInfo[0].Specialities.ToString();
                        }

                        if (data.BrokerInfo[0].HomeValue != null)
                        {
                            HomeValue = data.BrokerInfo[0].HomeValue.ToString();
                        }

                        if (data.BrokerInfo[0].AutoType != null)
                        {
                            AutoType = data.BrokerInfo[0].AutoType.ToString();
                        }

                        if (data.BrokerInfo[0].Revenue != null)
                        {
                            Revenue = data.BrokerInfo[0].Revenue.ToString();
                        }

                        if (data.BrokerInfo[0].Employees != null)
                        {
                            Employees = data.BrokerInfo[0].Employees.ToString();
                        }

                        if (data.BrokerInfo[0].CoverageAmt != null)
                        {
                            CoverageAmt = data.BrokerInfo[0].CoverageAmt.ToString();
                        }


                        /*************** Regarding Industry and SubIndustry *********************/

                        string SubIndustry1 = "", SubIndustryIds1 = "";
                        string SubIndustry2 = "", SubIndustryIds2 = "";
                        string SubIndustry3 = "", SubIndustryIds3 = "";

                        string IndustryId1 = ""; string IndustryId2 = ""; string IndustryId3 = "";
                        if (Industry1 != null)
                        {
                            if (chkSubIndustry1 != null)
                            {
                                IndustryId1 = IndustryId1 + "," + Industry1;
                                IndustryId1 = IndustryId1.TrimStart(',');

                                foreach (var Id in chkSubIndustry1)
                                {
                                    SubIndustry1 = SubIndustry1 + "," + Id;
                                }
                                SubIndustry1 = SubIndustry1.TrimStart(',');
                                SubIndustryIds1 = IndustryId1 + ":" + SubIndustry1;
                            }
                        }

                        if (Industry2 != null)
                        {
                            if (chkSubIndustry2 != null)
                            {
                                IndustryId2 = IndustryId2 + "," + Industry2;
                                IndustryId2 = IndustryId2.TrimStart(',');

                                foreach (var Id in chkSubIndustry2)
                                {
                                    SubIndustry2 = SubIndustry2 + "," + Id;
                                }
                                SubIndustry2 = SubIndustry2.TrimStart(',');
                                SubIndustryIds2 = IndustryId2 + ":" + SubIndustry2;
                            }
                        }

                        if (Industry3 != null)
                        {
                            if (chkSubIndustry3 != null)
                            {
                                IndustryId3 = IndustryId3 + "," + Industry3;
                                IndustryId3 = IndustryId3.TrimStart(',');

                                foreach (var Id in chkSubIndustry3)
                                {
                                    SubIndustry3 = SubIndustry3 + "," + Id;
                                }
                                SubIndustry3 = SubIndustry3.TrimStart(',');
                                SubIndustryIds3 = IndustryId3 + ":" + SubIndustry3;
                            }
                        }

                        IndustryId = IndustryId1 + "," + IndustryId2 + "," + IndustryId3;
                        SubIndustryId = SubIndustryIds1 + ";" + SubIndustryIds2 + ";" + SubIndustryIds3;

                        IndustryId = IndustryId.Trim(',');
                        SubIndustryId = SubIndustryId.Trim(';');

                        /*************** End of Regarding Industry and SubIndustry *********************/


                        //ExpiryDate = Request["datepicker"].ToString();
                        random = BrokerWSUtility.GetRandomNumber();
                        string Encryptrandom = BrokerUtility.EncryptURL(random);
                        Session["random"] = Encryptrandom;

                        /***************************** Save Broker Basic Details ***************************************/
                        User = BrokerWebDB.BrokerWebDB.SaveBrokerBasicDetails(FirstName, LastName, Phone, Email, Area, OnlyZipCodes, Title, Company, Language, Specialities, TempPass, Encryptrandom, CompanyLogoPath, ProfilePicPath, HomeValue, AutoType, Revenue, Employees, CoverageAmt, IndustryId, SubIndustryId, longitude, latitude, Bio);

                        if (User.Count > 0)
                        {
                            #region AfterSaveBasicInfo
                            bool f1 = false, f2 = false, f3 = false, f4 = false, f5 = false;
                            string EduLogoPath1 = "", EduLogoPath2 = "", EduLogoPath3 = "", EduLogoPath4 = "", EduLogoPath5 = "";

                            string ExpLogoPath1 = "", ExpLogoPath2 = "", ExpLogoPath3 = "", ExpLogoPath4 = "", ExpLogoPath5 = "", ExpLogoPath6 = "", ExpLogoPath7 = "";

                            UserId = Convert.ToInt32(User[0].UserId.ToString());
                            Session["UserId"] = UserId;
                            Session["EmailId"] = User[0].EmailId.ToString();

                            /*For Saving ZipCode details*/

                            #region Save ZipCode
                            int Result2 = BrokerWebDB.BrokerWebDB.DeleteZipCode(UserId.ToString());

                            if (ZipCode1 != "" && Addr1 != "")
                            {
                                string[] strZipcode1 = Addr1.Split(',');

                                string lng = strZipcode1[0];
                                string lat = strZipcode1[1];

                                int Result = BrokerWebDB.BrokerWebDB.InsertUserZipCode(UserId, ZipCode1, lng, lat);
                            }

                            if (ZipCode2 != "" && Addr2 != "")
                            {
                                string[] strZipcode2 = Addr2.Split(',');

                                string lng = strZipcode2[0];
                                string lat = strZipcode2[1];

                                int Result = BrokerWebDB.BrokerWebDB.InsertUserZipCode(UserId, ZipCode2, lng, lat);
                            }

                            if (ZipCode3 != "" && Addr3 != "")
                            {
                                string[] strZipcode3 = Addr3.Split(',');

                                string lng = strZipcode3[0];
                                string lat = strZipcode3[1];

                                int Result = BrokerWebDB.BrokerWebDB.InsertUserZipCode(UserId, ZipCode3, lng, lat);
                            }

                            if (ZipCode4 != "" && Addr4 != "")
                            {
                                string[] strZipcode4 = Addr4.Split(',');

                                string lng = strZipcode4[0];
                                string lat = strZipcode4[1];

                                int Result = BrokerWebDB.BrokerWebDB.InsertUserZipCode(UserId, ZipCode4, lng, lat);
                            }

                            if (ZipCode5 != "" && Addr5 != "")
                            {
                                string[] strZipcode5 = Addr5.Split(',');

                                string lng = strZipcode5[0];
                                string lat = strZipcode5[1];

                                int Result = BrokerWebDB.BrokerWebDB.InsertUserZipCode(UserId, ZipCode5, lng, lat);
                            }

                            #endregion

                            if (data.BrokerEduction != null)
                            {
                                /*For Education 1*/
                                #region Education 1


                                if (data.BrokerEduction[0].School1 != "" && data.BrokerEduction[0].School1 != null)
                                {
                                    School1 = data.BrokerEduction[0].School1.ToString();
                                    f1 = true;
                                }

                                if (data.BrokerEduction[0].Degree1 != "" && data.BrokerEduction[0].Degree1 != null)
                                {
                                    Degree1 = data.BrokerEduction[0].Degree1.ToString();
                                    f1 = true;
                                }

                                if (data.BrokerEduction[0].Year1 != "" && data.BrokerEduction[0].Year1 != null)
                                {
                                    Year1 = data.BrokerEduction[0].Year1.ToString();
                                    f1 = true;
                                }

                                if (data.BrokerEduction[0].EducationLogo1 != "" && data.BrokerEduction[0].EducationLogo1 != null)
                                {
                                    EducationLogo1 = data.BrokerEduction[0].EducationLogo1.ToString().Replace("C:\\fakepath\\", "");

                                    //foreach (var f in file)
                                    //{
                                    //    if (f != null)
                                    //    {
                                    //        if (f.ContentLength > 0)
                                    //        {
                                    //            if (Path.GetFileName(f.FileName) == EducationLogo1)
                                    //            {

                                    if (data.BrokerEduction[0].HiddenEducationLogo1 != "" && data.BrokerEduction[0].HiddenEducationLogo1 != null)
                                    {
                                        EduLogoPath1 = UserId + "_" + School1.Replace(" ", "_") + "_" + Degree1.Replace(" ", "_") + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerEduction[0].HiddenEducationLogo1.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1.Replace(" ", "_") + "_" + Degree1.Replace(" ", "_") + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1 + "_" + Degree1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1.Replace(" ", "_") + "_" + Degree1.Replace(" ", "_") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }

                                    //            }
                                    //        }
                                    //    }

                                    //}
                                }

                                if (f1 == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerEducationDetails(UserId, School1, Degree1, Year1, EduLogoPath1);
                                }

                                #endregion Education 1
                                /*For Education 1*/

                                /*For Education 2*/
                                #region Education 2

                                if (data.BrokerEduction[0].School2 != "" && data.BrokerEduction[0].School2 != null)
                                {
                                    School2 = data.BrokerEduction[0].School2.ToString();
                                    f2 = true;
                                }

                                if (data.BrokerEduction[0].Degree2 != "" && data.BrokerEduction[0].Degree2 != null)
                                {
                                    Degree2 = data.BrokerEduction[0].Degree2.ToString();
                                    f2 = true;
                                }

                                if (data.BrokerEduction[0].Year2 != "" && data.BrokerEduction[0].Year2 != null)
                                {
                                    Year2 = data.BrokerEduction[0].Year2.ToString();
                                    f2 = true;
                                }

                                if (data.BrokerEduction[0].EducationLogo2 != "" && data.BrokerEduction[0].EducationLogo2 != null)
                                {
                                    EducationLogo2 = data.BrokerEduction[0].EducationLogo2.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerEduction[0].HiddenEducationLogo2 != "" && data.BrokerEduction[0].HiddenEducationLogo2 != null)
                                    {
                                        EduLogoPath2 = UserId + "_" + School2.Replace(" ", "_") + "_" + Degree2.Replace(" ", "_") + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerEduction[0].HiddenEducationLogo2.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School2.Replace(" ", "_") + "_" + Degree2.Replace(" ", "_") + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1 + "_" + Degree1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School2.Replace(" ", "_") + "_" + Degree2.Replace(" ", "_") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (f2 == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerEducationDetails(UserId, School2, Degree2, Year2, EduLogoPath2);
                                }

                                #endregion Education 2
                                /*For Education 2*/

                                /*For Education 3*/
                                #region Education 3

                                if (data.BrokerEduction[0].School3 != "" && data.BrokerEduction[0].School3 != null)
                                {
                                    School3 = data.BrokerEduction[0].School3.ToString();
                                    f3 = true;
                                }

                                if (data.BrokerEduction[0].Degree3 != "" && data.BrokerEduction[0].Degree3 != null)
                                {
                                    Degree3 = data.BrokerEduction[0].Degree3.ToString();
                                    f3 = true;
                                }

                                if (data.BrokerEduction[0].Year3 != "" && data.BrokerEduction[0].Year3 != null)
                                {
                                    Year3 = data.BrokerEduction[0].Year3.ToString();
                                    f3 = true;
                                }

                                if (data.BrokerEduction[0].EducationLogo3 != "" && data.BrokerEduction[0].EducationLogo3 != null)
                                {
                                    EducationLogo3 = data.BrokerEduction[0].EducationLogo3.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerEduction[0].HiddenEducationLogo3 != "" && data.BrokerEduction[0].HiddenEducationLogo3 != null)
                                    {
                                        EduLogoPath3 = UserId + "_" + School3.Replace(" ", "_") + "_" + Degree3.Replace(" ", "_") + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerEduction[0].HiddenEducationLogo3.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School3.Replace(" ", "_") + "_" + Degree3.Replace(" ", "_") + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1 + "_" + Degree1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School3.Replace(" ", "_") + "_" + Degree3.Replace(" ", "_") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (f3 == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerEducationDetails(UserId, School3, Degree3, Year3, EduLogoPath3);
                                }

                                #endregion Education 2
                                /*For Education 3*/

                                /*For Education 4*/
                                #region Education 4

                                if (data.BrokerEduction[0].School4 != "" && data.BrokerEduction[0].School4 != null)
                                {
                                    School4 = data.BrokerEduction[0].School4.ToString();
                                    f4 = true;
                                }

                                if (data.BrokerEduction[0].Degree4 != "" && data.BrokerEduction[0].Degree4 != null)
                                {
                                    Degree4 = data.BrokerEduction[0].Degree4.ToString();
                                    f4 = true;
                                }

                                if (data.BrokerEduction[0].Year4 != "" && data.BrokerEduction[0].Year4 != null)
                                {
                                    Year4 = data.BrokerEduction[0].Year4.ToString();
                                    f4 = true;
                                }

                                if (data.BrokerEduction[0].EducationLogo4 != "" && data.BrokerEduction[0].EducationLogo4 != null)
                                {
                                    EducationLogo4 = data.BrokerEduction[0].EducationLogo4.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerEduction[0].HiddenEducationLogo4 != "" && data.BrokerEduction[0].HiddenEducationLogo4 != null)
                                    {
                                        EduLogoPath4 = UserId + "_" + School4.Replace(" ", "_") + "_" + Degree4.Replace(" ", "_") + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerEduction[0].HiddenEducationLogo4.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School4.Replace(" ", "_") + "_" + Degree4.Replace(" ", "_") + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1 + "_" + Degree1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School4.Replace(" ", "_") + "_" + Degree4.Replace(" ", "_") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (f4 == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerEducationDetails(UserId, School4, Degree4, Year4, EduLogoPath4);
                                }

                                #endregion Education 2
                                /*For Education 4*/

                                /*For Education 5*/
                                #region Education 5

                                if (data.BrokerEduction[0].School5 != "" && data.BrokerEduction[0].School5 != null)
                                {
                                    School5 = data.BrokerEduction[0].School5.ToString();
                                    f5 = true;
                                }

                                if (data.BrokerEduction[0].Degree5 != "" && data.BrokerEduction[0].Degree5 != null)
                                {
                                    Degree5 = data.BrokerEduction[0].Degree5.ToString();
                                    f5 = true;
                                }

                                if (data.BrokerEduction[0].Year5 != "" && data.BrokerEduction[0].Year5 != null)
                                {
                                    Year5 = data.BrokerEduction[0].Year5.ToString();
                                    f5 = true;
                                }

                                if (data.BrokerEduction[0].EducationLogo5 != "" && data.BrokerEduction[0].EducationLogo5 != null)
                                {
                                    EducationLogo5 = data.BrokerEduction[0].EducationLogo5.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerEduction[0].HiddenEducationLogo5 != "" && data.BrokerEduction[0].HiddenEducationLogo5 != null)
                                    {
                                        EduLogoPath5 = UserId + "_" + School5.Replace(" ", "_") + "_" + Degree5.Replace(" ", "_") + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerEduction[0].HiddenEducationLogo5.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School5.Replace(" ", "_") + "_" + Degree5.Replace(" ", "_") + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        //image1.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School1 + "_" + Degree1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + UserId + "_" + School5.Replace(" ", "_") + "_" + Degree5.Replace(" ", "_") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (f5 == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerEducationDetails(UserId, School5, Degree5, Year5, EduLogoPath5);
                                }

                                #endregion Education 2
                                /*For Education 5*/
                            }

                            /*For Saving Company Details*/
                            #region For Experience Details

                            if (data.BrokerPriorEmployment != null)
                            {
                                bool ExpFlag = false;
                                /*For Company 1*/
                                #region Company 1

                                if (data.BrokerPriorEmployment[0].CmpName1 != "" && data.BrokerPriorEmployment[0].CmpName1 != null)
                                {
                                    ExpComp1 = data.BrokerPriorEmployment[0].CmpName1.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].Desig1 != "" && data.BrokerPriorEmployment[0].Desig1 != null)
                                {
                                    ExpDesig1 = data.BrokerPriorEmployment[0].Desig1.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthFrom1 != "" && data.BrokerPriorEmployment[0].DurMonthFrom1 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthFrom1 != "Select")
                                    {
                                        ExpFromMonth1 = data.BrokerPriorEmployment[0].DurMonthFrom1.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearFrom1 != "" && data.BrokerPriorEmployment[0].DurYearFrom1 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearFrom1 != "Select")
                                    {
                                        ExpFromYear1 = data.BrokerPriorEmployment[0].DurYearFrom1.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthTo1 != "" && data.BrokerPriorEmployment[0].DurMonthTo1 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthTo1 != "Select")
                                    {
                                        ExpToMonth1 = data.BrokerPriorEmployment[0].DurMonthTo1.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearTo1 != "" && data.BrokerPriorEmployment[0].DurYearTo1 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearTo1 != "Select")
                                    {
                                        ExpToYear1 = data.BrokerPriorEmployment[0].DurYearTo1.ToString();
                                        ExpFlag = true;
                                    }
                                }

                                if (data.BrokerPriorEmployment[0].CmpLogo1 != "" && data.BrokerPriorEmployment[0].CmpLogo1 != null)
                                {
                                    ExpLogo1 = data.BrokerPriorEmployment[0].CmpLogo1.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerPriorEmployment[0].HiddenCmpLogo1 != "" && data.BrokerPriorEmployment[0].HiddenCmpLogo1 != null)
                                    {
                                        ExpLogoPath1 = UserId + "_" + ExpComp1.Replace(" ", "_") + "_" + ExpFromYear1.Replace(" ", "_") + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerPriorEmployment[0].HiddenCmpLogo1.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp1.Replace(" ", "_") + "_" + ExpFromYear1.Replace(" ", "_") + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp1.Replace(" ", "_") + "_" + ExpFromYear1.Replace(" ", "_") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (ExpFlag == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerExperienceDetails(UserId, ExpComp1, ExpDesig1, ExpFromMonth1, ExpFromYear1, ExpToMonth1, ExpToYear1, ExpLogoPath1);
                                }
                                #endregion Company 1
                                /*End For Company 1*/

                                /*For Company 2*/
                                #region Company 2
                                ExpFlag = false;
                                if (data.BrokerPriorEmployment[0].CmpName2 != "" && data.BrokerPriorEmployment[0].CmpName2 != null)
                                {
                                    ExpComp2 = data.BrokerPriorEmployment[0].CmpName2.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].Desig2 != "" && data.BrokerPriorEmployment[0].Desig2 != null)
                                {
                                    ExpDesig2 = data.BrokerPriorEmployment[0].Desig2.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthFrom2 != "" && data.BrokerPriorEmployment[0].DurMonthFrom2 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthFrom2 != "Select")
                                    {
                                        ExpFromMonth2 = data.BrokerPriorEmployment[0].DurMonthFrom2.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearFrom2 != "" && data.BrokerPriorEmployment[0].DurYearFrom2 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearFrom2 != "Select")
                                    {
                                        ExpFromYear2 = data.BrokerPriorEmployment[0].DurYearFrom2.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthTo2 != "" && data.BrokerPriorEmployment[0].DurMonthTo2 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthTo2 != "Select")
                                    {
                                        ExpToMonth2 = data.BrokerPriorEmployment[0].DurMonthTo2.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearTo2 != "" && data.BrokerPriorEmployment[0].DurYearTo2 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearTo2 != "Select")
                                    {
                                        ExpToYear2 = data.BrokerPriorEmployment[0].DurYearTo2.ToString();
                                        ExpFlag = true;
                                    }
                                }

                                if (data.BrokerPriorEmployment[0].CmpLogo2 != "" && data.BrokerPriorEmployment[0].CmpLogo2 != null)
                                {
                                    ExpLogo2 = data.BrokerPriorEmployment[0].CmpLogo2.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerPriorEmployment[0].HiddenCmpLogo2 != "" && data.BrokerPriorEmployment[0].HiddenCmpLogo2 != null)
                                    {
                                        ExpLogoPath2 = UserId + "_" + ExpComp2.Replace(" ", "_") + "_" + ExpFromYear2.Replace(" ", "_") + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerPriorEmployment[0].HiddenCmpLogo2.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp2.Replace(" ", "_") + "_" + ExpFromYear2.Replace(" ", "_") + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp2.Replace(" ", "_") + "_" + ExpFromYear2.Replace(" ", "_") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (ExpFlag == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerExperienceDetails(UserId, ExpComp2, ExpDesig2, ExpFromMonth2, ExpFromYear2, ExpToMonth2, ExpToYear2, ExpLogoPath2);
                                }
                                #endregion Company 2
                                /*End For Company 2*/

                                /*For Company 3*/
                                #region Company 3
                                ExpFlag = false;
                                if (data.BrokerPriorEmployment[0].CmpName3 != "" && data.BrokerPriorEmployment[0].CmpName3 != null)
                                {
                                    ExpComp3 = data.BrokerPriorEmployment[0].CmpName3.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].Desig3 != "" && data.BrokerPriorEmployment[0].Desig3 != null)
                                {
                                    ExpDesig3 = data.BrokerPriorEmployment[0].Desig3.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthFrom3 != "" && data.BrokerPriorEmployment[0].DurMonthFrom3 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthFrom3 != "Select")
                                    {
                                        ExpFromMonth3 = data.BrokerPriorEmployment[0].DurMonthFrom3.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearFrom3 != "" && data.BrokerPriorEmployment[0].DurYearFrom3 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearFrom3 != "Select")
                                    {
                                        ExpFromYear3 = data.BrokerPriorEmployment[0].DurYearFrom3.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthTo3 != "" && data.BrokerPriorEmployment[0].DurMonthTo3 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthTo3 != "Select")
                                    {
                                        ExpToMonth3 = data.BrokerPriorEmployment[0].DurMonthTo3.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearTo3 != "" && data.BrokerPriorEmployment[0].DurYearTo3 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearTo3 != "Select")
                                    {
                                        ExpToYear3 = data.BrokerPriorEmployment[0].DurYearTo3.ToString();
                                        ExpFlag = true;
                                    }
                                }

                                if (data.BrokerPriorEmployment[0].CmpLogo3 != "" && data.BrokerPriorEmployment[0].CmpLogo3 != null)
                                {
                                    ExpLogo3 = data.BrokerPriorEmployment[0].CmpLogo3.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerPriorEmployment[0].HiddenCmpLogo3 != "" && data.BrokerPriorEmployment[0].HiddenCmpLogo3 != null)
                                    {
                                        ExpLogoPath3 = UserId + "_" + ExpComp3.Replace(" ", "_") + "_" + ExpFromYear3.Replace(" ", "_") + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerPriorEmployment[0].HiddenCmpLogo3.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp3.Replace(" ", "_") + "_" + ExpFromYear3.Replace(" ", "_") + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp3.Replace(" ", "_") + "_" + ExpFromYear3.Replace(" ", "_") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (ExpFlag == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerExperienceDetails(UserId, ExpComp3, ExpDesig3, ExpFromMonth3, ExpFromYear3, ExpToMonth3, ExpToYear3, ExpLogoPath3);
                                }
                                #endregion Company 3
                                /*End For Company 3*/

                                /*For Company4*/
                                #region Company 4
                                ExpFlag = false;
                                if (data.BrokerPriorEmployment[0].CmpName4 != "" && data.BrokerPriorEmployment[0].CmpName4 != null)
                                {
                                    ExpComp4 = data.BrokerPriorEmployment[0].CmpName4.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].Desig4 != "" && data.BrokerPriorEmployment[0].Desig4 != null)
                                {
                                    ExpDesig4 = data.BrokerPriorEmployment[0].Desig4.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthFrom4 != "" && data.BrokerPriorEmployment[0].DurMonthFrom4 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthFrom4 != "Select")
                                    {
                                        ExpFromMonth4 = data.BrokerPriorEmployment[0].DurMonthFrom4.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearFrom4 != "" && data.BrokerPriorEmployment[0].DurYearFrom4 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearFrom4 != "Select")
                                    {
                                        ExpFromYear4 = data.BrokerPriorEmployment[0].DurYearFrom4.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthTo4 != "" && data.BrokerPriorEmployment[0].DurMonthTo4 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthTo4 != "Select")
                                    {
                                        ExpToMonth4 = data.BrokerPriorEmployment[0].DurMonthTo4.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearTo4 != "" && data.BrokerPriorEmployment[0].DurYearTo4 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearTo4 != "Select")
                                    {
                                        ExpToYear4 = data.BrokerPriorEmployment[0].DurYearTo4.ToString();
                                        ExpFlag = true;
                                    }
                                }

                                if (data.BrokerPriorEmployment[0].CmpLogo4 != "" && data.BrokerPriorEmployment[0].CmpLogo4 != null)
                                {
                                    ExpLogo4 = data.BrokerPriorEmployment[0].CmpLogo4.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerPriorEmployment[0].HiddenCmpLogo4 != "" && data.BrokerPriorEmployment[0].HiddenCmpLogo4 != null)
                                    {
                                        ExpLogoPath4 = UserId + "_" + ExpComp4.Replace(" ", "_") + "_" + ExpFromYear4.Replace(" ", "_") + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerPriorEmployment[0].HiddenCmpLogo4.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp4.Replace(" ", "_") + "_" + ExpFromYear4.Replace(" ", "_") + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp4.Replace(" ", "_") + "_" + ExpFromYear4.Replace(" ", "_") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (ExpFlag == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerExperienceDetails(UserId, ExpComp4, ExpDesig4, ExpFromMonth4, ExpFromYear4, ExpToMonth4, ExpToYear4, ExpLogoPath4);
                                }
                                #endregion Company 4
                                /*End For Company 4*/

                                /*For Company5*/
                                #region Company 5
                                ExpFlag = false;
                                if (data.BrokerPriorEmployment[0].CmpName5 != "" && data.BrokerPriorEmployment[0].CmpName5 != null)
                                {
                                    ExpComp5 = data.BrokerPriorEmployment[0].CmpName5.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].Desig5 != "" && data.BrokerPriorEmployment[0].Desig5 != null)
                                {
                                    ExpDesig5 = data.BrokerPriorEmployment[0].Desig5.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthFrom5 != "" && data.BrokerPriorEmployment[0].DurMonthFrom5 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthFrom5 != "Select")
                                    {
                                        ExpFromMonth5 = data.BrokerPriorEmployment[0].DurMonthFrom5.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearFrom5 != "" && data.BrokerPriorEmployment[0].DurYearFrom5 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearFrom5 != "Select")
                                    {
                                        ExpFromYear5 = data.BrokerPriorEmployment[0].DurYearFrom5.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthTo5 != "" && data.BrokerPriorEmployment[0].DurMonthTo5 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthTo5 != "Select")
                                    {
                                        ExpToMonth5 = data.BrokerPriorEmployment[0].DurMonthTo5.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearTo5 != "" && data.BrokerPriorEmployment[0].DurYearTo5 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearTo5 != "Select")
                                    {
                                        ExpToYear5 = data.BrokerPriorEmployment[0].DurYearTo5.ToString();
                                        ExpFlag = true;
                                    }
                                }

                                if (data.BrokerPriorEmployment[0].CmpLogo5 != "" && data.BrokerPriorEmployment[0].CmpLogo5 != null)
                                {
                                    ExpLogo5 = data.BrokerPriorEmployment[0].CmpLogo5.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerPriorEmployment[0].HiddenCmpLogo5 != "" && data.BrokerPriorEmployment[0].HiddenCmpLogo5 != null)
                                    {
                                        ExpLogoPath5 = UserId + "_" + ExpComp5.Replace(" ", "_") + "_" + ExpFromYear5.Replace(" ", "_") + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerPriorEmployment[0].HiddenCmpLogo5.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp5.Replace(" ", "_") + "_" + ExpFromYear5.Replace(" ", "_") + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp5.Replace(" ", "_") + "_" + ExpFromYear5.Replace(" ", "_") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (ExpFlag == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerExperienceDetails(UserId, ExpComp5, ExpDesig5, ExpFromMonth5, ExpFromYear5, ExpToMonth5, ExpToYear5, ExpLogoPath5);
                                }
                                #endregion Company 5
                                /*End For Company 5*/

                                /*For Company6*/
                                #region Company 6
                                ExpFlag = false;
                                if (data.BrokerPriorEmployment[0].CmpName6 != "" && data.BrokerPriorEmployment[0].CmpName6 != null)
                                {
                                    ExpComp6 = data.BrokerPriorEmployment[0].CmpName6.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].Desig6 != "" && data.BrokerPriorEmployment[0].Desig6 != null)
                                {
                                    ExpDesig6 = data.BrokerPriorEmployment[0].Desig6.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthFrom6 != "" && data.BrokerPriorEmployment[0].DurMonthFrom6 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthFrom6 != "Select")
                                    {
                                        ExpFromMonth6 = data.BrokerPriorEmployment[0].DurMonthFrom6.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearFrom6 != "" && data.BrokerPriorEmployment[0].DurYearFrom6 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearFrom6 != "Select")
                                    {
                                        ExpFromYear6 = data.BrokerPriorEmployment[0].DurYearFrom6.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthTo6 != "" && data.BrokerPriorEmployment[0].DurMonthTo6 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthTo6 != "Select")
                                    {
                                        ExpToMonth6 = data.BrokerPriorEmployment[0].DurMonthTo6.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearTo6 != "" && data.BrokerPriorEmployment[0].DurYearTo6 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearTo6 != "Select")
                                    {
                                        ExpToYear6 = data.BrokerPriorEmployment[0].DurYearTo6.ToString();
                                        ExpFlag = true;
                                    }
                                }

                                if (data.BrokerPriorEmployment[0].CmpLogo6 != "" && data.BrokerPriorEmployment[0].CmpLogo6 != null)
                                {
                                    ExpLogo6 = data.BrokerPriorEmployment[0].CmpLogo6.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerPriorEmployment[0].HiddenCmpLogo6 != "" && data.BrokerPriorEmployment[0].HiddenCmpLogo6 != null)
                                    {
                                        ExpLogoPath6 = UserId + "_" + ExpComp6.Replace(" ", "_") + "_" + ExpFromYear6.Replace(" ", "_") + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerPriorEmployment[0].HiddenCmpLogo6.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp6.Replace(" ", "_") + "_" + ExpFromYear6.Replace(" ", "_") + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp6.Replace(" ", "_") + "_" + ExpFromYear6.Replace(" ", "_") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (ExpFlag == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerExperienceDetails(UserId, ExpComp6, ExpDesig6, ExpFromMonth6, ExpFromYear6, ExpToMonth6, ExpToYear6, ExpLogoPath6);
                                }
                                #endregion Company 6
                                /*End For Company 6*/

                                /*For Company7*/
                                #region Company 7
                                ExpFlag = false;
                                if (data.BrokerPriorEmployment[0].CmpName7 != "" && data.BrokerPriorEmployment[0].CmpName7 != null)
                                {
                                    ExpComp7 = data.BrokerPriorEmployment[0].CmpName7.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].Desig7 != "" && data.BrokerPriorEmployment[0].Desig7 != null)
                                {
                                    ExpDesig7 = data.BrokerPriorEmployment[0].Desig7.ToString();
                                    ExpFlag = true;
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthFrom7 != "" && data.BrokerPriorEmployment[0].DurMonthFrom7 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthFrom7 != "Select")
                                    {
                                        ExpFromMonth7 = data.BrokerPriorEmployment[0].DurMonthFrom7.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearFrom7 != "" && data.BrokerPriorEmployment[0].DurYearFrom7 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearFrom7 != "Select")
                                    {
                                        ExpFromYear7 = data.BrokerPriorEmployment[0].DurYearFrom7.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurMonthTo7 != "" && data.BrokerPriorEmployment[0].DurMonthTo7 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurMonthTo7 != "Select")
                                    {
                                        ExpToMonth7 = data.BrokerPriorEmployment[0].DurMonthTo7.ToString();
                                        ExpFlag = true;
                                    }
                                }
                                if (data.BrokerPriorEmployment[0].DurYearTo7 != "" && data.BrokerPriorEmployment[0].DurYearTo7 != null)
                                {
                                    if (data.BrokerPriorEmployment[0].DurYearTo7 != "Select")
                                    {
                                        ExpToYear7 = data.BrokerPriorEmployment[0].DurYearTo7.ToString();
                                        ExpFlag = true;
                                    }
                                }

                                if (data.BrokerPriorEmployment[0].CmpLogo7 != "" && data.BrokerPriorEmployment[0].CmpLogo7 != null)
                                {
                                    ExpLogo7 = data.BrokerPriorEmployment[0].CmpLogo7.ToString().Replace("C:\\fakepath\\", "");

                                    if (data.BrokerPriorEmployment[0].HiddenCmpLogo7 != "" && data.BrokerPriorEmployment[0].HiddenCmpLogo7 != null)
                                    {
                                        ExpLogoPath7 = UserId + "_" + ExpComp7.Replace(" ", "_") + "_" + ExpFromYear7.Replace(" ", "_") + ".png";

                                        byte[] imageBytes1 = Convert.FromBase64String(data.BrokerPriorEmployment[0].HiddenCmpLogo7.ToString());
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp7.Replace(" ", "_") + "_" + ExpFromYear7.Replace(" ", "_") + ".png");
                                        bool CheckFile = BrokerUtility.CheckFile(FileName1);

                                        System.Drawing.Image thumbnail = image1.GetThumbnailImage(200, 200, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                                        thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + UserId + "_" + ExpComp7.Replace(" ", "_") + "_" + ExpFromYear7.Replace(" ", "_") + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                }

                                if (ExpFlag == true)
                                {
                                    Flag = BrokerWebDB.BrokerWebDB.SaveBrokerExperienceDetails(UserId, ExpComp7, ExpDesig7, ExpFromMonth7, ExpFromYear7, ExpToMonth7, ExpToYear7, ExpLogoPath7);
                                }
                                #endregion Company 7
                                /*End For Company 7*/
                            }
                            #endregion For Experience Details


                            /*End of For Saving Company Details*/


                            /*For Saving Industry Details*/
                            #region For Industry Details
                            if (Industry1 != null)
                            {
                                if (chkSubIndustry1 != null)
                                {
                                    foreach (var Ids in chkSubIndustry1)
                                    {
                                        Flag = BrokerWebDB.BrokerWebDB.SaveBrokerIndustryDetails(UserId, Industry1, Ids);
                                    }
                                }
                            }

                            if (Industry2 != null)
                            {
                                if (chkSubIndustry2 != null)
                                {
                                    foreach (var Ids in chkSubIndustry2)
                                    {
                                        Flag = BrokerWebDB.BrokerWebDB.SaveBrokerIndustryDetails(UserId, Industry2, Ids);
                                    }
                                }
                            }

                            if (Industry3 != null)
                            {
                                if (chkSubIndustry3 != null)
                                {
                                    foreach (var Ids in chkSubIndustry3)
                                    {
                                        Flag = BrokerWebDB.BrokerWebDB.SaveBrokerIndustryDetails(UserId, Industry3, Ids);
                                    }
                                }
                            }
                            #endregion For Industry Details
                            /*End of For Saving Industry Details*/

                            bool EmailFlag = false;

                            EmailFlag = BrokerWSUtility.SendRegistrationEmail(Session["EmailId"].ToString(), Session["random"].ToString(), Session["UserId"].ToString(), "Broker");
                            //EmailFlag = true;
                            if (EmailFlag)
                            {
                                //ViewBag.VerificationMessage = "You are registered successfully but yet not activated. ";
                                //ViewBag.VerificationMessage1 = "Please accept your verification email.";
                                return View("BrokerSuccess");
                            }
                            else
                            {
                                //If User registerd successfully, but verification link has not
                                //been sent over EmailId
                                //ViewBag.VerificationMessage = "You are registered successfully but yet not activated. <br/>Please accept your verification email.";

                                ViewBag.VerificationMessage = "You are registered successfully but can't send mail to you.";
                                ViewBag.VerificationMessage1 = "Please contact to admin.";
                                return View("BrokerError");
                            }

                            //return RedirectToAction("MakePayment", "BrokerRegistration", new { EmailId = BrokerUtility.EncryptURL(Email), RegistrationCode = BrokerUtility.EncryptURL(random) });

                            #endregion AfterSaveBasicInfo
                        }
                        ViewBag.VerificationMessage = "Error occured while saving details. ";
                        ViewBag.VerificationMessage1 = "Please try again.";
                        return View("BrokerError");
                        //}
                        #endregion Save Broker Details

                        //return RedirectToAction("MakePayment", "BrokerRegistration", new { EmailId = BrokerUtility.EncryptURL("samplemail@gmail.com"), RegistrationCode = BrokerUtility.EncryptURL("123456789") });
                    }
                    //ViewBag.VerificationMessage = "Error occured while saving details. ";
                    //ViewBag.VerificationMessage1 = "Please try again.";
                    //return View("BrokerError");
                }
                //ViewBag.VerificationMessage = "Error occured while saving details. ";
                //ViewBag.VerificationMessage1 = "Please try again.";
                //return View("BrokerError");
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "BrokerSignUp_POST_Website", Ex.Message.ToString(), "BrokerRegistrationController.cs_BrokerSignUp()", "");
                ViewBag.VerificationMessage = "Error occured while saving details. ";
                ViewBag.VerificationMessage1 = "Please try again.";
                return View("BrokerError");
            }
            ViewBag.VerificationMessage = "Error occured while saving details. ";
            ViewBag.VerificationMessage1 = "Please try again.";
            return View("BrokerError");
        }
    }
}