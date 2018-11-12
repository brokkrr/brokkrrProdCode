using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using BrokerMVC.BrokerService;
using BrokerMVC.App_Code;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using Owin.Security.Providers.EveOnline;
using System.Text;
using System.Configuration;
using System.Drawing;
using System.Net;
using PushSharp;
using PushSharp.Apple;
using PushSharp.Core;
using Newtonsoft.Json.Linq;
using PushSharp.Google;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Xml;



namespace BrokerMVC.BrokerService
{
    public class BrokerWSDB
    {
        public static string strDomainName = ConfigurationManager.AppSettings["DomainName"].ToString();
        public static string strProfilePicForlderName = ConfigurationManager.AppSettings["ProfilePicForlderName"].ToString();
        public static string strResumeForlderName = ConfigurationManager.AppSettings["ResumeForlderName"].ToString();

        public static string strProfilePicImageFolder = ConfigurationManager.AppSettings["ProfilePicImageFolder"].ToString();
        public static string strResumeImageFolder = ConfigurationManager.AppSettings["ResumeImageFolder"].ToString();
        public static string strUploadedCompLogoFolder = ConfigurationManager.AppSettings["UploadedCompLogoFolder"].ToString();
        public static string strEducationLogoFolder = ConfigurationManager.AppSettings["EducationLogo"].ToString();

        public static string strCompanyLogoFolder = ConfigurationManager.AppSettings["CompanyLogoFolder"].ToString();
        public static string strExperienceCompLogoFolder = ConfigurationManager.AppSettings["ExperienceCompLogoFolder"].ToString();

        public static string strGoogleAppID = ConfigurationManager.AppSettings["GoogleAppID"].ToString();
        public static string strSENDER_ID = ConfigurationManager.AppSettings["SENDER_ID"].ToString();

        public static string strUsersToShowByDefaultInSearchList = ConfigurationManager.AppSettings["UsersToShowByDefaultInSearchList"].ToString();
        public static string strSentMailOnChatMessage = ConfigurationManager.AppSettings["SentMailOnChatMessage"].ToString();

        public static string strDeclarationDocumentFolder = ConfigurationManager.AppSettings["DeclarationDocumentFolder"].ToString();
        
        public static string strGoogleMapKey = ConfigurationManager.AppSettings["GoogleMapKey"].ToString();

        #region Common Actions

        public static void DoGetDocBase64FromMessageId(string WSFlag)
        {
            DataSet dsResult = new DataSet();
            int MainMessageId;
            try
            {
                if (WSFlag == "Application")
                {
                    MainMessageId = Convert.ToInt32(HttpContext.Current.Request.Form["MainMessageId"]);
                }
                else
                {
                    MainMessageId = Convert.ToInt32(HttpContext.Current.Request.QueryString["MainMessageId"]);
                }

                dsResult = BrokerWSUtility.GetDocBase64String(MainMessageId);

                if (dsResult.Tables.Count > 0)
                {
                    dsResult.Tables[0].TableName = "DocBase64String";
                    dsResult.AcceptChanges();

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsResult, "DoGetDocBase64FromMessageId", "true", "null"));
                }
            }
            catch (Exception Ex)
            {
                string UsersList = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(UsersList);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetDocBase64FromMessageId", "false", "Error occured, please try again."));
            }
        }

        public static void DoGetUsersList(string WSFlag)
        {
            DataSet dsResult = new DataSet();
            try
            {
                dsResult = BrokerWSUtility.GetUsersList();

                if (dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                        {
                            if (dsResult.Tables[0].Rows[i]["Password"].ToString() != "")
                            {
                                string DecryptedPass = BrokerUtility.DecryptURL(dsResult.Tables[0].Rows[i]["Password"].ToString()); //dsResult.Tables[0].Rows[i]["Password"].ToString();
                                dsResult.Tables[0].Rows[i]["Password"] = DecryptedPass;
                            }
                        }

                        dsResult.Tables[0].TableName = "UsersList";
                        dsResult.AcceptChanges();

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsResult, "DoCheckUserExist", "true", "null"));
                    }
                }
            }
            catch (Exception Ex)
            {
                string UsersList = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(UsersList);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetUsersList", "false", "Error occured, please try again."));
            }
        }

        public static void DoCheckUserExist(string WSFlag)
        {
            int UserId;
            string data = "";
            DataSet dtResult = null;
            try
            {
                if (WSFlag == "Application")
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                }
                else
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                }

                dtResult = BrokerWSUtility.CheckUserExistInDB(UserId);

                if (dtResult.Tables.Count > 0)
                {
                    //data = dtResult.Tables[0].Rows[0][0].ToString();
                    //if (data == "1")
                    //{
                    //    data = "true";
                    //}
                    //else
                    //{
                    //    data = "false";
                    //}
                    //DataTable dtDetails = new DataTable();
                    //DataSet dsDetails =null;

                    //dtDetails.Columns.Add("IsExists");

                    //dtDetails.Rows.Add(data);

                    //dsDetails.Tables.Add(dtDetails);
                    //dsDetails.Tables[0].TableName = "Response";
                    //dsDetails.AcceptChanges();

                    dtResult.Tables[0].TableName = "Response";
                    dtResult.AcceptChanges();
                    
                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dtResult, "DoCheckUserExist", "true", "null"));
                }
                else
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoCheckUserExist", "false", "Error Occured"));


                }

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoCheckUserExist", Ex.Message.ToString(), "BrokerWSDB.cs_DoCheckUserExist()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoCheckUserExist", "false", "Error occured, please try again."));
            }
        }

        //Do Regular Login Using Username and password
        public static void DoLogin(string WSFlag)
        {
            try
            {

                string UserName = "", Password = "", UserType = "";
                if (WSFlag == "Application")
                {
                    UserName = HttpContext.Current.Request.Form["UserName"].ToString();
                    Password = HttpContext.Current.Request.Form["Password"].ToString();
                    UserType = HttpContext.Current.Request.Form["UserType"].ToString();
                }
                else
                {
                    UserName = HttpContext.Current.Request.QueryString["UserName"].ToString();
                    Password = HttpContext.Current.Request.QueryString["Password"].ToString();
                    UserType = HttpContext.Current.Request.QueryString["UserType"].ToString();
                }
                HttpContext.Current.Response.Write(BrokerWSUtility.CheckLogin(UserName, Password, UserType));
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoLogin", Ex.Message.ToString(), "BrokerWSDB.cs_DoLogin()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        //Do external Login with facebook and linkedin
        public static void DoExternalLogin(string WSFlag)
        {
            try
            {
                DataSet dsLoginuser = null;
                string FirstName = "", LastName = "", EmailId = "", Provider = "", UserType = "", RegisteredFor = "";
                if (WSFlag == "Application")
                {
                    FirstName = HttpContext.Current.Request.Form["FirstName"].ToString();
                    LastName = HttpContext.Current.Request.Form["LastName"].ToString();
                    EmailId = HttpContext.Current.Request.Form["EmailId"].ToString();
                    Provider = HttpContext.Current.Request.Form["Provider"].ToString();
                    UserType = HttpContext.Current.Request.Form["UserType"].ToString();
                    RegisteredFor = HttpContext.Current.Request.Form["RegisteredFor"].ToString();//22Feb18
                }
                else
                {
                    FirstName = HttpContext.Current.Request.QueryString["FirstName"].ToString();
                    LastName = HttpContext.Current.Request.QueryString["LastName"].ToString();
                    EmailId = HttpContext.Current.Request.QueryString["EmailId"].ToString();
                    Provider = HttpContext.Current.Request.QueryString["Provider"].ToString();
                    UserType = HttpContext.Current.Request.QueryString["UserType"].ToString();
                    RegisteredFor = HttpContext.Current.Request.QueryString["RegisteredFor"].ToString();//22Feb18
                }
                dsLoginuser = BrokerUtility.LoginExternal(Provider, FirstName, LastName, EmailId, UserType, "0", RegisteredFor);//22Feb18

                if (dsLoginuser.Tables.Count > 0)
                {
                    if (dsLoginuser.Tables[0].Columns.Count == 1)
                    {
                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsLoginuser, "DoExternalLogin", "false", "Invalid User Type"));
                    }
                    else
                    {
                        string IsNewRegister = dsLoginuser.Tables["RegisterFlag"].Rows[0][0].ToString();

                        dsLoginuser.Tables.Remove("RegisterFlag");
                        dsLoginuser.AcceptChanges();

                        string binData = dsLoginuser.Tables[0].Rows[0]["ProfilePicture"].ToString();
                        if (binData != "")
                        {
                            binData = strDomainName + "" + strProfilePicForlderName + "" + dsLoginuser.Tables[0].Rows[0]["ProfilePicture"].ToString(); ;

                            dsLoginuser.Tables[0].Rows[0]["ProfilePicture"] = binData;
                        }

                        string ProfilePicImg = dsLoginuser.Tables[0].Rows[0]["ProfilePictureImg"].ToString();
                        if (ProfilePicImg != "")
                        {
                            ProfilePicImg = strDomainName + "" + strProfilePicImageFolder + "" + dsLoginuser.Tables[0].Rows[0]["ProfilePictureImg"].ToString(); ;

                            dsLoginuser.Tables[0].Rows[0]["ProfilePictureImg"] = ProfilePicImg;
                        }

                        if (dsLoginuser.Tables[0].Rows[0]["UserType"].ToString() == "Broker")
                        {
                            string ResumeData = dsLoginuser.Tables[0].Rows[0]["Resume"].ToString();
                            if (ResumeData != "")
                            {
                                ResumeData = strDomainName + "" + strResumeForlderName + "" + dsLoginuser.Tables[0].Rows[0]["Resume"].ToString(); ;

                                dsLoginuser.Tables[0].Rows[0]["Resume"] = ResumeData;
                            }

                            string ResumeImg = dsLoginuser.Tables[0].Rows[0]["ResumeDoc"].ToString();
                            if (ResumeImg != "")
                            {
                                ResumeImg = strDomainName + "" + strResumeImageFolder + "" + dsLoginuser.Tables[0].Rows[0]["ResumeDoc"].ToString(); ;

                                dsLoginuser.Tables[0].Rows[0]["ResumeDoc"] = ResumeImg;
                            }

                            string CompanyLogo = dsLoginuser.Tables[0].Rows[0]["CompanyLogo"].ToString();
                            if (CompanyLogo != "")
                            {
                                CompanyLogo = strDomainName + "" + strUploadedCompLogoFolder + "" + dsLoginuser.Tables[0].Rows[0]["CompanyLogo"].ToString();

                                dsLoginuser.Tables[0].Rows[0]["CompanyLogo"] = CompanyLogo;
                            }


                            //Set Server address to logos

                            if (dsLoginuser.Tables["ExperienceDetails"].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsLoginuser.Tables["ExperienceDetails"].Rows.Count; i++)
                                {
                                    string Logo = dsLoginuser.Tables["ExperienceDetails"].Rows[i]["Logo"].ToString();
                                    if (Logo != "")
                                    {
                                        Logo = strDomainName + "" + strExperienceCompLogoFolder + "" + dsLoginuser.Tables["ExperienceDetails"].Rows[i]["Logo"].ToString(); ;

                                        dsLoginuser.Tables["ExperienceDetails"].Rows[i]["Logo"] = Logo;
                                    }
                                }
                            }

                            /******************************Add Server path to School logo ********************************/
                            if (dsLoginuser.Tables["EducationDetails"].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsLoginuser.Tables["EducationDetails"].Rows.Count; i++)
                                {
                                    string Logo = dsLoginuser.Tables["EducationDetails"].Rows[i]["EducationLogo"].ToString();
                                    if (Logo != "")
                                    {
                                        Logo = strDomainName + "" + strEducationLogoFolder + "" + dsLoginuser.Tables["EducationDetails"].Rows[i]["EducationLogo"].ToString(); ;

                                        dsLoginuser.Tables["EducationDetails"].Rows[i]["EducationLogo"] = Logo;
                                    }
                                }
                            }
                            /******************************End of Add Server path to School logo ********************************/


                        }


                        //HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsLoginuser, "DoExternalLogin", "true", "null"));

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDatasetNew(dsLoginuser, "DoExternalLogin", "true", "null", IsNewRegister));
                    }
                }
                else
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", "null"));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoExternalLogin", "false", "Error Occured"));

                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoExternalLogin", Ex.Message.ToString(), "BrokerWSDB.cs_DoExternalLogin()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        //Update Profile after singup  
        public static void DoUpdateProfileBroker(string WSFlag)
        {
            string UserId = "", FirstName = "", LastName = "", Address = "", City = "", PinCode = "", MobNo = "", State = "", Country = "",
            CompanyName = "", Title = "", Resume = "", Designation = "", Languages = "", Specialities = "",
            Exp_Designation = "", Exp_CompanyName = "", Exp_DurationFrom = "", Exp_DurationTo = "", Edu_UniversityName = "",
            Edu_CourseName = "", Edu_DurationFrom = "", Edu_DurationTo = "", UpdateTable = "", DOB = "", ProfilePicture = "", PhoneNo = "",
            Awards = "", Skills = "", Recomendations = "", License = "", ExpiryDate = "";

            string ProfilePicImagePath = "", ResumeImagePath = "";


            int User = 0;
            string TempPass = "";
            DataSet dsUserDetails = null, dsUserExperienceDetails = null, dsUserEducationDetails = null, dsUserDetails1 = null;
            string EmailId = "";


            try
            {
                string JSonResult = "";
                if (WSFlag == "Application")
                {
                    JSonResult = HttpContext.Current.Request.Form["Result"].ToString();
                }
                else
                {
                    JSonResult = HttpContext.Current.Request.QueryString["Result"].ToString();
                }

                if (JSonResult != "")
                {
                    dsUserDetails = JsonConvert.DeserializeObject<DataSet>(JSonResult);

                    if (dsUserDetails.Tables.Count > 0)
                    {
                        if (dsUserDetails.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsUserDetails.Tables[0].Rows.Count; i++)
                            {
                                UserId = dsUserDetails.Tables[0].Rows[i]["UserId"].ToString();
                                FirstName = dsUserDetails.Tables[0].Rows[i]["FirstName"].ToString();
                                LastName = dsUserDetails.Tables[0].Rows[i]["LastName"].ToString();
                                Address = dsUserDetails.Tables[0].Rows[i]["Address"].ToString();
                                City = dsUserDetails.Tables[0].Rows[i]["City"].ToString();
                                PinCode = dsUserDetails.Tables[0].Rows[i]["PinCode"].ToString();
                                MobNo = dsUserDetails.Tables[0].Rows[i]["MobNo"].ToString();
                                State = dsUserDetails.Tables[0].Rows[i]["State"].ToString();
                                Country = dsUserDetails.Tables[0].Rows[i]["Country"].ToString();
                                CompanyName = dsUserDetails.Tables[0].Rows[i]["CompanyName"].ToString();
                                Title = dsUserDetails.Tables[0].Rows[i]["Title"].ToString();
                                Designation = dsUserDetails.Tables[0].Rows[i]["Designation"].ToString();
                                Languages = dsUserDetails.Tables[0].Rows[i]["Languages"].ToString();
                                Specialities = dsUserDetails.Tables[0].Rows[i]["Specialities"].ToString();

                                DOB = dsUserDetails.Tables[0].Rows[i]["DOB"].ToString();

                                ProfilePicture = dsUserDetails.Tables[0].Rows[i]["ProfilePicture"].ToString();
                                ProfilePicture = ProfilePicture.Replace(" ", "+");

                                Resume = dsUserDetails.Tables[0].Rows[i]["Resume"].ToString();
                                Resume = Resume.Replace(" ", "+");

                                PhoneNo = dsUserDetails.Tables[0].Rows[i]["PhoneNo"].ToString();

                                Awards = dsUserDetails.Tables[0].Rows[i]["Awards"].ToString();
                                Skills = dsUserDetails.Tables[0].Rows[i]["Skills"].ToString();
                                Recomendations = dsUserDetails.Tables[0].Rows[i]["Recommendations"].ToString();
                                License = dsUserDetails.Tables[0].Rows[i]["License"].ToString();
                                ExpiryDate = dsUserDetails.Tables[0].Rows[i]["ExpiryDate"].ToString();

                                Exp_Designation = "";
                                Exp_CompanyName = "";
                                Exp_DurationFrom = "";
                                Exp_DurationTo = "";
                                Edu_UniversityName = "";
                                Edu_CourseName = "";
                                Edu_DurationFrom = "";
                                Edu_DurationTo = "";
                                UpdateTable = "User";

                                List<spGetUserDetails_Result> oUserDetails = null;

                                oUserDetails = BrokerUtility.GetUserDetails(Convert.ToInt32(UserId));
                                string Email = oUserDetails[0].EmailId;

                                /*****************************Create Image file for Profile Picture***************/
                                // Delete file if already exists.

                                ProfilePicImagePath = Email + "_" + UserId + ".png";
                                string ProfilePicImageFullPath = HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + ProfilePicImagePath);

                                if (File.Exists(HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + ProfilePicImagePath)))
                                {
                                    File.Delete(ProfilePicImageFullPath);
                                }

                                // Convert Base64 String to byte[]
                                if (ProfilePicture.ToString().Trim() != "")
                                {
                                    byte[] imageBytes = Convert.FromBase64String(ProfilePicture);
                                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                                    // Convert byte[] to Image
                                    ms.Write(imageBytes, 0, imageBytes.Length);
                                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                                    //ProfilePicImagePath = Email + "_" + UserId + ".png";
                                    //ProfilePicImageFullPath = HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + ProfilePicImagePath);

                                    //if (File.Exists(HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + ProfilePicImagePath)))
                                    //{
                                    //    File.Delete(ProfilePicImageFullPath);
                                    //}

                                    image.Save(HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Email + "_" + UserId + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                }
                                else
                                {
                                    ProfilePicImagePath = "";

                                }
                                /*****************************End of Create Image file for Profile Picture*********/

                                /*****************************Create Image file for Resume ***************/

                                //Delete filr if already exists.

                                ResumeImagePath = Email + ".png";
                                string ResumeImageFullPath = HttpContext.Current.Server.MapPath("~/UploadedImages/Resume/" + ResumeImagePath);

                                if (File.Exists(HttpContext.Current.Server.MapPath("~/UploadedImages/Resume/" + ResumeImagePath)))
                                {
                                    File.Delete(ResumeImageFullPath);
                                }

                                // Convert Base64 String to byte[]

                                if (Resume.ToString().Trim() != "")
                                {
                                    byte[] imageBytes1 = Convert.FromBase64String(Resume);
                                    MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                    // Convert byte[] to Image
                                    ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                    System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                    image1.Save(HttpContext.Current.Server.MapPath("~/UploadedImages/Resume/" + Email + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                }
                                else
                                {
                                    ResumeImagePath = "";
                                }
                                /*****************************End of Create Image file for Resume *********/


                                /****************Create text file for Profile Picture****************/


                                string FileName1 = Email + "_" + UserId + ".txt";
                                string FileName = HttpContext.Current.Server.MapPath("~/ProfilePicture/" + Email + "_" + UserId + ".txt").ToString();
                                if (File.Exists(FileName))
                                {
                                    File.Delete(FileName);
                                }

                                // Create a new file 
                                if (ProfilePicture.ToString().Trim() != "")
                                {
                                    using (FileStream fs = File.Create(FileName))
                                    {
                                        // Add text to file
                                        Byte[] title = new UTF8Encoding(true).GetBytes(ProfilePicture);
                                        fs.Write(title, 0, title.Length);
                                    }
                                }
                                else
                                {
                                    FileName1 = "";
                                }

                                /***************End of Create text file for Profile Picture*****************/

                                /****************Create text file for Resume****************/

                                string ResumeFileName1 = Email + ".txt";
                                string ResumeFileName = HttpContext.Current.Server.MapPath("~/Resume/" + Email + ".txt").ToString();
                                if (File.Exists(ResumeFileName))
                                {
                                    File.Delete(ResumeFileName);
                                }

                                // Create a new file 
                                if (Resume.ToString().Trim() != "")
                                {
                                    using (FileStream fs = File.Create(ResumeFileName))
                                    {
                                        // Add text to file
                                        Byte[] title = new UTF8Encoding(true).GetBytes(Resume);
                                        fs.Write(title, 0, title.Length);
                                    }
                                }
                                else
                                {
                                    ResumeFileName1 = "";
                                }

                                /***************End of Create text file for Resume*****************/

                                dsUserDetails1 = BrokerWSUtility.UpdateBroker(UserId, FirstName, LastName, Address, City, State, Country, PinCode, MobNo, CompanyName, Title, Designation, Languages, Specialities, Exp_Designation, Exp_CompanyName, Exp_DurationFrom, Exp_DurationTo, Edu_UniversityName, Edu_CourseName, Edu_DurationFrom, Edu_DurationTo, DOB, FileName1, PhoneNo, UpdateTable, Awards, Skills, Recomendations, License, ExpiryDate, ResumeFileName1, ProfilePicImagePath, ResumeImagePath);

                                if (dsUserDetails1.Tables[0].Rows.Count > 0)
                                {
                                    EmailId = dsUserDetails1.Tables[0].Rows[0]["EmailId"].ToString();

                                    if (EmailId != "")
                                    {
                                        int User1 = 0;

                                        //User1 = BrokerWSUtility.DeleteBrokerDetails(EmailId);

                                        if (dsUserDetails.Tables[1].Rows.Count > 0)
                                        {
                                            for (int j = 0; j < dsUserDetails.Tables[1].Rows.Count; j++)
                                            {
                                                UserId = dsUserDetails.Tables[1].Rows[j]["UserId"].ToString();
                                                FirstName = "";
                                                LastName = "";
                                                Address = "";
                                                City = "";
                                                PinCode = "";
                                                MobNo = "";
                                                State = "";
                                                Country = "";
                                                CompanyName = "";
                                                Title = "";
                                                Designation = "";
                                                Languages = "";
                                                Specialities = "";

                                                DOB = "";
                                                ProfilePicture = "";
                                                PhoneNo = "";

                                                Awards = "";
                                                Skills = "";
                                                Recomendations = "";
                                                License = "";
                                                ExpiryDate = "";

                                                Exp_Designation = "";
                                                Exp_CompanyName = "";
                                                Exp_DurationFrom = "";
                                                Exp_DurationTo = "";

                                                bool Flag = false;

                                                if (dsUserDetails.Tables[1].Rows[j]["Designation"].ToString() != "")
                                                {
                                                    Exp_Designation = dsUserDetails.Tables[1].Rows[j]["Designation"].ToString();
                                                    Flag = true;
                                                }

                                                if (dsUserDetails.Tables[1].Rows[j]["CompanyName"].ToString() != "")
                                                {
                                                    Exp_CompanyName = dsUserDetails.Tables[1].Rows[j]["CompanyName"].ToString();
                                                    Flag = true;
                                                }

                                                if (dsUserDetails.Tables[1].Rows[j]["DurationFrom"].ToString() != "")
                                                {
                                                    Exp_DurationFrom = dsUserDetails.Tables[1].Rows[j]["DurationFrom"].ToString();
                                                    Flag = true;
                                                }

                                                if (dsUserDetails.Tables[1].Rows[j]["DurationTo"].ToString() != "")
                                                {
                                                    Exp_DurationTo = dsUserDetails.Tables[1].Rows[j]["DurationTo"].ToString();
                                                    Flag = true;
                                                }
                                                Edu_UniversityName = "";
                                                Edu_CourseName = "";
                                                Edu_DurationFrom = "";
                                                Edu_DurationTo = "";
                                                UpdateTable = "UserExperience";

                                                if (Flag == true)
                                                {
                                                    dsUserExperienceDetails = BrokerWSUtility.UpdateBroker(UserId, FirstName, LastName, Address, City, State, Country, PinCode, MobNo, CompanyName, Title, Designation, Languages, Specialities, Exp_Designation, Exp_CompanyName, Exp_DurationFrom, Exp_DurationTo, Edu_UniversityName, Edu_CourseName, Edu_DurationFrom, Edu_DurationTo, DOB, ProfilePicture, PhoneNo, UpdateTable, Awards, Skills, Recomendations, License, ExpiryDate, Resume, ProfilePicImagePath, ResumeImagePath);
                                                }
                                            }
                                        }



                                        if (dsUserDetails.Tables[2].Rows.Count > 0)
                                        {
                                            for (int k = 0; k < dsUserDetails.Tables[2].Rows.Count; k++)
                                            {
                                                UserId = dsUserDetails.Tables[2].Rows[k]["UserId"].ToString();
                                                FirstName = "";
                                                LastName = "";
                                                Address = "";
                                                City = "";
                                                PinCode = "";
                                                MobNo = "";
                                                State = "";
                                                Country = "";
                                                CompanyName = "";
                                                Title = "";
                                                Designation = "";
                                                Languages = "";
                                                Specialities = "";

                                                DOB = "";
                                                ProfilePicture = "";
                                                PhoneNo = "";

                                                Awards = "";
                                                Skills = "";
                                                Recomendations = "";
                                                License = "";
                                                ExpiryDate = "";

                                                Exp_Designation = "";
                                                Exp_CompanyName = "";
                                                Exp_DurationFrom = "";
                                                Exp_DurationTo = "";

                                                bool Flag = false;

                                                Edu_UniversityName = "";
                                                Edu_CourseName = "";
                                                Edu_DurationFrom = "";
                                                Edu_DurationTo = "";

                                                if (dsUserDetails.Tables[2].Rows[k]["UniversityName"].ToString() != "")
                                                {
                                                    Edu_UniversityName = dsUserDetails.Tables[2].Rows[k]["UniversityName"].ToString();
                                                    Flag = true;
                                                }
                                                if (dsUserDetails.Tables[2].Rows[k]["CourseName"].ToString() != "")
                                                {
                                                    Edu_CourseName = dsUserDetails.Tables[2].Rows[k]["CourseName"].ToString();
                                                    Flag = true;
                                                }

                                                if (dsUserDetails.Tables[2].Rows[k]["DurationFrom"].ToString() != "")
                                                {
                                                    Edu_DurationFrom = dsUserDetails.Tables[2].Rows[k]["DurationFrom"].ToString();
                                                    Flag = true;
                                                }

                                                if (dsUserDetails.Tables[2].Rows[k]["DurationTo"].ToString() != "")
                                                {
                                                    Edu_DurationTo = dsUserDetails.Tables[2].Rows[k]["DurationTo"].ToString();
                                                    Flag = true;
                                                }
                                                UpdateTable = "UserEducation";

                                                if (Flag == true)
                                                {
                                                    dsUserEducationDetails = BrokerWSUtility.UpdateBroker(UserId, FirstName, LastName, Address, City, State, Country, PinCode, MobNo, CompanyName, Title, Designation, Languages, Specialities, Exp_Designation, Exp_CompanyName, Exp_DurationFrom, Exp_DurationTo, Edu_UniversityName, Edu_CourseName, Edu_DurationFrom, Edu_DurationTo, DOB, ProfilePicture, PhoneNo, UpdateTable, Awards, Skills, Recomendations, License, ExpiryDate, Resume, ProfilePicImagePath, ResumeImagePath);
                                                }
                                            }
                                        }
                                    }

                                    dsUserDetails = BrokerWSUtility.GetBrokerDetails(EmailId);
                                    if (dsUserDetails.Tables.Count > 0)
                                    {
                                        string binData = "", ResumeData = "", ProfilePicImg = "", ResumeImg = "";

                                        dsUserDetails.Tables[0].TableName = "UserDetails";
                                        dsUserDetails.Tables[1].TableName = "ExperienceDetails";
                                        dsUserDetails.Tables[2].TableName = "EducationDetails";
                                        dsUserDetails.AcceptChanges();

                                        binData = dsUserDetails.Tables[0].Rows[0]["ProfilePicture"].ToString();
                                        if (binData != "")
                                        {
                                            binData = strDomainName + "" + strProfilePicForlderName + "" + dsUserDetails.Tables[0].Rows[0]["ProfilePicture"].ToString(); ;

                                            dsUserDetails.Tables[0].Rows[0]["ProfilePicture"] = binData;
                                        }

                                        ResumeData = dsUserDetails.Tables[0].Rows[0]["Resume"].ToString();
                                        if (ResumeData != "")
                                        {
                                            ResumeData = strDomainName + "" + strResumeForlderName + "" + dsUserDetails.Tables[0].Rows[0]["Resume"].ToString(); ;

                                            dsUserDetails.Tables[0].Rows[0]["Resume"] = ResumeData;
                                        }

                                        ProfilePicImg = dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString();
                                        if (ProfilePicImg != "")
                                        {
                                            ProfilePicImg = strDomainName + "" + strProfilePicImageFolder + "" + dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString(); ;

                                            dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"] = ProfilePicImg;
                                        }

                                        ResumeImg = dsUserDetails.Tables[0].Rows[0]["ResumeDoc"].ToString();
                                        if (ResumeImg != "")
                                        {
                                            ResumeImg = strDomainName + "" + strResumeImageFolder + "" + dsUserDetails.Tables[0].Rows[0]["ResumeDoc"].ToString(); ;

                                            dsUserDetails.Tables[0].Rows[0]["ResumeDoc"] = ResumeImg;
                                        }

                                        if (dsUserDetails.Tables["ExperienceDetails"].Rows.Count > 0)
                                        {
                                            for (int j = 0; j < dsUserDetails.Tables["ExperienceDetails"].Rows.Count; j++)
                                            {
                                                string Logo = dsUserDetails.Tables["ExperienceDetails"].Rows[j]["Logo"].ToString();
                                                if (Logo != "")
                                                {
                                                    Logo = strDomainName + "" + strCompanyLogoFolder + "" + dsUserDetails.Tables["ExperienceDetails"].Rows[j]["Logo"].ToString(); ;

                                                    dsUserDetails.Tables["ExperienceDetails"].Rows[j]["Logo"] = Logo;
                                                }
                                            }
                                        }

                                        //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForSuccess("Success", "null"));
                                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsUserDetails, "DoUpdateProfileBroker", "true", "null"));
                                    }
                                    else
                                    {
                                        //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", "null"));
                                        string Message = "";
                                        DataSet dsDetails = new DataSet();
                                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoExternalLogin", "false", "Error Occured"));

                                    }

                                }
                                else
                                {
                                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", "null"));
                                    string Message = "";
                                    DataSet dsDetails = new DataSet();
                                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoExternalLogin", "false", "Error Occured"));

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoUpdateProfileBroker", Ex.Message.ToString(), "BrokerWSDB.cs_DoUpdateProfileBroker()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }

        }

        //Update Profile after singup for Cross Platform Application.
        public static void DoUpdateProfileBrokerForAndroid(string WSFlag)
        {
            //BrokerUtility.ErrorLog(0, "DoUpdateProfileBrokerForAndroid", "START SAVING", "BrokerWSDB.cs_DoUpdateProfileBrokerForAndroid()", "");

            string UserId = "", FirstName = "", LastName = "", Address = "", City = "", PinCode = "", MobNo = "", State = "", Country = "",
            CompanyName = "", Title = "", Resume = "", Designation = "", Languages = "", Specialities = "",
            Exp_Designation = "", Exp_CompanyName = "", Exp_DurationFrom = "", Exp_DurationTo = "", Exp_Bio = "", Edu_UniversityName = "",
            Edu_CourseName = "", Edu_DurationFrom = "", Edu_DurationTo = "", Edu_Logo = "", Exp_Logo = "", UpdateTable = "", DOB = "", ProfilePicture = "", PhoneNo = "",
            Awards = "", Skills = "", Recomendations = "", License = "", ExpiryDate = "", Email = "", ResumeFileName1 = "", FileName1 = "", latitude = "", longitude = "",
            HomeValue = "", AutoType = "", Revenue = "", Employees = "", CoverageAmt = "", CompanyLogo = "", IndustryId = "", SubIndustryId = "";// PinDetails = "";

            string ProfilePicImagePath = "", ResumeImagePath = "", IsProfilePicUpdated = "", CompanyLogoPath = "", IsCompanyLogoUpdated = "", IsEducationLogoUpdated = "";


            int User = 0;
            string TempPass = "";
            DataSet dsUserDetails = null, dsUserExperienceDetails = null, dsUserEducationDetails = null, dsUserDetails1 = null;
            string EmailId = "", DeletedEduList = "", IsUpdated = "", UserBio = "", DeletedExpList = "";

            try
            {
                string JSonResult = "";
                if (WSFlag == "Application")
                {
                    IsProfilePicUpdated = HttpContext.Current.Request.Form["IsProfilePicUpdated"].ToString();
                    IsCompanyLogoUpdated = HttpContext.Current.Request.Form["IsCompanyLogoUpdated"].ToString();
                    DeletedEduList = HttpContext.Current.Request.Form["DeletedEduList"].ToString();
                    DeletedExpList = HttpContext.Current.Request.Form["DeletedExpList"].ToString();
                    //IsEducationLogoUpdated = HttpContext.Current.Request.Form["IsCompanyLogoUpdated"].ToString();
                    JSonResult = HttpContext.Current.Request.Form["Result"].ToString();
                }
                else
                {
                    IsProfilePicUpdated = HttpContext.Current.Request.QueryString["IsProfilePicUpdated"].ToString();
                    IsCompanyLogoUpdated = HttpContext.Current.Request.QueryString["IsCompanyLogoUpdated"].ToString();
                    //IsEducationLogoUpdated = HttpContext.Current.Request.QueryString["IsCompanyLogoUpdated"].ToString();
                    DeletedEduList = HttpContext.Current.Request.QueryString["DeletedEduList"].ToString();
                    DeletedExpList = HttpContext.Current.Request.QueryString["DeletedExpList"].ToString();
                    JSonResult = HttpContext.Current.Request.QueryString["Result"].ToString();
                }

                DeletedEduList = DeletedEduList.TrimStart('[');
                DeletedEduList = DeletedEduList.TrimEnd(']');

                DeletedExpList = DeletedExpList.TrimStart('[');
                DeletedExpList = DeletedExpList.TrimEnd(']');

                if (JSonResult != "")
                {
                    dsUserDetails = JsonConvert.DeserializeObject<DataSet>(JSonResult);

                    if (dsUserDetails.Tables.Count > 0)
                    {
                        if (dsUserDetails.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsUserDetails.Tables[0].Rows.Count; i++)
                            {
                                UserId = dsUserDetails.Tables[0].Rows[i]["UserId"].ToString();
                                FirstName = dsUserDetails.Tables[0].Rows[i]["FirstName"].ToString();
                                LastName = dsUserDetails.Tables[0].Rows[i]["LastName"].ToString();
                                Address = dsUserDetails.Tables[0].Rows[i]["Address"].ToString();
                                City = dsUserDetails.Tables[0].Rows[i]["City"].ToString();
                                PinCode = dsUserDetails.Tables[0].Rows[i]["PinCode"].ToString();
                                MobNo = dsUserDetails.Tables[0].Rows[i]["MobNo"].ToString();
                                State = dsUserDetails.Tables[0].Rows[i]["State"].ToString();
                                Country = dsUserDetails.Tables[0].Rows[i]["Country"].ToString();
                                CompanyName = dsUserDetails.Tables[0].Rows[i]["CompanyName"].ToString();
                                Title = dsUserDetails.Tables[0].Rows[i]["Title"].ToString();
                                Designation = dsUserDetails.Tables[0].Rows[i]["Designation"].ToString();
                                Languages = dsUserDetails.Tables[0].Rows[i]["Languages"].ToString();
                                Specialities = dsUserDetails.Tables[0].Rows[i]["Specialities"].ToString();

                                DOB = dsUserDetails.Tables[0].Rows[i]["DOB"].ToString();

                                ProfilePicture = dsUserDetails.Tables[0].Rows[i]["ProfilePicture"].ToString();
                                ProfilePicture = ProfilePicture.Replace(" ", "+");

                                Resume = dsUserDetails.Tables[0].Rows[i]["Resume"].ToString();
                                Resume = Resume.Replace(" ", "+");

                                CompanyLogo = dsUserDetails.Tables[0].Rows[i]["CompanyLogo"].ToString();
                                CompanyLogo = CompanyLogo.Replace(" ", "+");

                                PhoneNo = dsUserDetails.Tables[0].Rows[i]["PhoneNo"].ToString();

                                Awards = dsUserDetails.Tables[0].Rows[i]["Awards"].ToString();
                                Skills = dsUserDetails.Tables[0].Rows[i]["Skills"].ToString();
                                Recomendations = dsUserDetails.Tables[0].Rows[i]["Recommendations"].ToString();
                                License = dsUserDetails.Tables[0].Rows[i]["License"].ToString();
                                ExpiryDate = dsUserDetails.Tables[0].Rows[i]["ExpiryDate"].ToString();

                                latitude = dsUserDetails.Tables[0].Rows[i]["latitude"].ToString();
                                longitude = dsUserDetails.Tables[0].Rows[i]["longitude"].ToString();

                                HomeValue = dsUserDetails.Tables[0].Rows[i]["HomeValue"].ToString();
                                AutoType = dsUserDetails.Tables[0].Rows[i]["AutoType"].ToString();
                                Revenue = dsUserDetails.Tables[0].Rows[i]["Revenue"].ToString();
                                Employees = dsUserDetails.Tables[0].Rows[i]["Employees"].ToString();
                                CoverageAmt = dsUserDetails.Tables[0].Rows[i]["CoverageAmt"].ToString();

                                IndustryId = dsUserDetails.Tables[0].Rows[i]["IndustryId"].ToString();
                                SubIndustryId = dsUserDetails.Tables[0].Rows[i]["SubIndustryId"].ToString();

                                UserBio = dsUserDetails.Tables[0].Rows[i]["Bio"].ToString().Trim();//09Oct17

                                //Start to split Industry Ids
                                #region Split Industry Ids

                                string[] IndustryIds = null;
                                if (IndustryId != "")
                                {
                                    IndustryIds = IndustryId.Split(',');
                                }

                                #endregion Split Industry Ids
                                //End of split Industry Ids   

                                //Split ZipCodes

                                #region Split ZipCodes

                                //string[] ZipCodes = null;
                                //string[] SubCodeZipcodes = null;
                                //if (PinCode != "")
                                //{
                                //    ZipCodes = PinCode.Split(';');
                                //    foreach (string Id in ZipCodes)
                                //    {
                                //        SubCodeZipcodes = Id.Split(':');
                                //        PinDetails = PinDetails + "," + SubCodeZipcodes[0];
                                //    }
                                //}
                                //PinDetails = PinDetails.TrimStart(',');

                                #endregion Split ZipCodes
                                //End of Split ZipCodes

                                Exp_Designation = "";
                                Exp_CompanyName = "";
                                Exp_DurationFrom = "";
                                Exp_DurationTo = "";
                                Edu_UniversityName = "";
                                Edu_CourseName = "";
                                Edu_DurationFrom = "";
                                Edu_DurationTo = "";
                                Edu_Logo = "";
                                UpdateTable = "User";

                                List<spGetUserDetails_Result> oUserDetails = null;

                                oUserDetails = BrokerUtility.GetUserDetails(Convert.ToInt32(UserId));
                                Email = oUserDetails[0].EmailId;

                                #region Regarding Uploading Images

                                if (IsProfilePicUpdated == "true")
                                {

                                    /*****************************Create Image file for Profile Picture***************/
                                    // Delete file if already exists.

                                    ProfilePicImagePath = Email + "_" + UserId + ".png";
                                    string ProfilePicImageFullPath = HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + ProfilePicImagePath);

                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + ProfilePicImagePath)))
                                    {
                                        File.Delete(ProfilePicImageFullPath);
                                    }

                                    // Convert Base64 String to byte[]
                                    if (ProfilePicture.ToString().Trim() != "")
                                    {
                                        byte[] imageBytes = Convert.FromBase64String(ProfilePicture);
                                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                                        // Convert byte[] to Image
                                        ms.Write(imageBytes, 0, imageBytes.Length);
                                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                                        //ProfilePicImagePath = Email + "_" + UserId + ".png";
                                        //ProfilePicImageFullPath = HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + ProfilePicImagePath);

                                        //if (File.Exists(HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + ProfilePicImagePath)))
                                        //{
                                        //    File.Delete(ProfilePicImageFullPath);
                                        //}

                                        image.Save(HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Email + "_" + UserId + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                    else
                                    {
                                        ProfilePicImagePath = "";

                                    }

                                    /*****************************End of Create Image file for Profile Picture*********/

                                    /*****************************Create Image file for Resume ***************/

                                    //Delete file if already exists.

                                    ResumeImagePath = Email + ".png";
                                    string ResumeImageFullPath = HttpContext.Current.Server.MapPath("~/UploadedImages/Resume/" + ResumeImagePath);

                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/UploadedImages/Resume/" + ResumeImagePath)))
                                    {
                                        File.Delete(ResumeImageFullPath);
                                    }

                                    // Convert Base64 String to byte[]

                                    if (Resume.ToString().Trim() != "")
                                    {
                                        byte[] imageBytes1 = Convert.FromBase64String(Resume);
                                        MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);

                                        // Convert byte[] to Image
                                        ms1.Write(imageBytes1, 0, imageBytes1.Length);
                                        System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                                        image1.Save(HttpContext.Current.Server.MapPath("~/UploadedImages/Resume/" + Email + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                    else
                                    {
                                        ResumeImagePath = "";
                                    }
                                    /*****************************End of Create Image file for Resume *********/

                                    /****************Create text file for Profile Picture****************/


                                    FileName1 = Email + "_" + UserId + ".txt";
                                    string FileName = HttpContext.Current.Server.MapPath("~/ProfilePicture/" + Email + "_" + UserId + ".txt").ToString();
                                    if (File.Exists(FileName))
                                    {
                                        File.Delete(FileName);
                                    }

                                    // Create a new file 
                                    if (ProfilePicture.ToString().Trim() != "")
                                    {
                                        using (FileStream fs = File.Create(FileName))
                                        {
                                            // Add text to file
                                            Byte[] title = new UTF8Encoding(true).GetBytes(ProfilePicture);
                                            fs.Write(title, 0, title.Length);
                                        }
                                    }
                                    else
                                    {
                                        FileName1 = "";
                                    }

                                    /***************End of Create text file for Profile Picture*****************/

                                    /****************Create text file for Resume****************/

                                    ResumeFileName1 = Email + ".txt";
                                    string ResumeFileName = HttpContext.Current.Server.MapPath("~/Resume/" + Email + ".txt").ToString();
                                    if (File.Exists(ResumeFileName))
                                    {
                                        File.Delete(ResumeFileName);
                                    }

                                    // Create a new file 
                                    if (Resume.ToString().Trim() != "")
                                    {
                                        using (FileStream fs = File.Create(ResumeFileName))
                                        {
                                            // Add text to file
                                            Byte[] title = new UTF8Encoding(true).GetBytes(Resume);
                                            fs.Write(title, 0, title.Length);
                                        }
                                    }
                                    else
                                    {
                                        ResumeFileName1 = "";
                                    }

                                    /***************End of Create text file for Resume*****************/
                                }

                                if (IsCompanyLogoUpdated == "true")
                                {
                                    /*****************************Create Image file for Company Logo***************/
                                    // Delete file if already exists.

                                    CompanyLogoPath = Email + "_" + UserId + ".png";
                                    string CompanyLogoFullPath = HttpContext.Current.Server.MapPath("~/UploadedImages/UploadedCompanyLogo/" + CompanyLogoPath);

                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/UploadedImages/UploadedCompanyLogo/" + CompanyLogoPath)))
                                    {
                                        File.Delete(CompanyLogoFullPath);
                                    }

                                    // Convert Base64 String to byte[]
                                    if (CompanyLogo.ToString().Trim() != "")
                                    {
                                        byte[] imageBytes = Convert.FromBase64String(CompanyLogo);
                                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                                        // Convert byte[] to Image
                                        ms.Write(imageBytes, 0, imageBytes.Length);
                                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                                        image.Save(HttpContext.Current.Server.MapPath("~/UploadedImages/UploadedCompanyLogo/" + Email + "_" + UserId + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                    else
                                    {
                                        CompanyLogoPath = "";

                                    }

                                    /*****************************End of Create Image file for Company Logo*********/
                                }

                                #endregion Regarding Uploading Images


                                dsUserDetails1 = BrokerWSUtility.UpdateBrokerForAndroid(UserId, FirstName, LastName, Address, City, State, Country, PinCode, MobNo, CompanyName, Title, Designation, Languages, Specialities, Exp_Designation, Exp_CompanyName, Exp_DurationFrom, Exp_DurationTo, Edu_UniversityName, Edu_CourseName, Edu_DurationFrom, Edu_DurationTo, DOB, FileName1, PhoneNo, UpdateTable, Awards, Skills, Recomendations, License, ExpiryDate, ResumeFileName1, ProfilePicImagePath, ResumeImagePath, IsProfilePicUpdated, longitude, latitude, HomeValue, AutoType, Revenue, Employees, CoverageAmt, IsCompanyLogoUpdated, CompanyLogoPath, Edu_Logo, IndustryId, SubIndustryId, Exp_Bio, UserBio, Exp_Logo);// 09Oct17
                                //BrokerUtility.ErrorLog(0, "DoUpdateProfileBrokerForAndroid", "User Basic Details Saved...", "BrokerWSDB.cs_DoUpdateProfileBrokerForAndroid()", "");
                                if (dsUserDetails1.Tables[0].Rows.Count > 0)
                                {
                                    EmailId = dsUserDetails1.Tables[0].Rows[0]["EmailId"].ToString();

                                    if (EmailId != "")
                                    {

                                        //Start to split Sub Industry Ids
                                        #region Split Sub Industry Ids

                                        int Result1 = BrokerWSUtility.DeleteIndustryId(UserId);

                                        if (SubIndustryId != "")
                                        {
                                            DataTable dtIndustryId = new DataTable();

                                            dtIndustryId.Columns.Add("IndustryId");
                                            dtIndustryId.Columns.Add("SubIndustryId");

                                            string[] SubIndustryIds1 = null;
                                            string[] SubIndustryIds2 = null;

                                            SubIndustryIds1 = SubIndustryId.Split(';');

                                            foreach (string Id in SubIndustryIds1)
                                            {
                                                SubIndustryIds2 = Id.Split(':');

                                                string x = SubIndustryIds2[0];
                                                string y = SubIndustryIds2[1];

                                                string[] u = y.Split(',');

                                                foreach (string Id1 in u)
                                                {
                                                    dtIndustryId.Rows.Add(x, Id1);
                                                }
                                            }

                                            for (int k = 0; k < dtIndustryId.Rows.Count; k++)
                                            {
                                                int Result = BrokerWSUtility.InsertIndustryId(UserId, dtIndustryId.Rows[k][0].ToString(), dtIndustryId.Rows[k][1].ToString());
                                            }

                                        }

                                        #endregion Split Industry Ids
                                        //End of split Sub Industry Ids

                                        //Start to split ZipCode and Inser into DB
                                        #region Insert ZipCodes
                                        int Result2 = BrokerWSUtility.DeleteZipCode(UserId);

                                        if (PinCode != "")
                                        {
                                            DataTable dtZipCodes = new DataTable();

                                            dtZipCodes.Columns.Add("ZipCode", typeof(string));
                                            dtZipCodes.Columns.Add("Logitude", typeof(string));
                                            dtZipCodes.Columns.Add("Latitude", typeof(string));

                                            string[] ZipMainPart = null;
                                            string[] ZipSubPart = null;

                                            ZipMainPart = PinCode.Split(',');

                                            foreach (string Id in ZipMainPart)
                                            {
                                                string address = "";
                                                try
                                                {
                                                    address = "https://maps.googleapis.com/maps/api/geocode/xml?components=postal_code:" + Id.Trim() + "&key="+strGoogleMapKey+"&sensor=false";

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

                                                    //if (lat != "" && lng != "")
                                                    //{
                                                    dtZipCodes.Rows.Add(Id.Trim(), lng, lat);
                                                    //}
                                                    //dtZipCodes.Rows.Add(x, Long1, Lat1);
                                                }
                                                catch (Exception Ex)
                                                {

                                                }
                                            }

                                            for (int s = 0; s < dtZipCodes.Rows.Count; s++)
                                            {
                                                int Result = BrokerWSUtility.InsertUserZipCode(UserId, dtZipCodes.Rows[s][0].ToString(), dtZipCodes.Rows[s][1].ToString(), dtZipCodes.Rows[s][2].ToString());
                                            }
                                        }

                                        #endregion Insert ZipCodes
                                        //End of Start to split ZipCode and Inser into DB

                                        int User1 = 0;

                                        //For Saving Experience details
                                        // BrokerUtility.ErrorLog(0, "DoUpdateProfileBrokerForAndroid", "User Experience Details Saving Start...", "BrokerWSDB.cs_DoUpdateProfileBrokerForAndroid()", "");
                                        #region For Experience details

                                        #region Old Experience Deails Saving
                                        //User1 = BrokerWSUtility.DeleteBrokerDetails(EmailId, "UserExperience", "");

                                        //if (dsUserDetails.Tables[1].Rows.Count > 0)
                                        //{
                                        //    for (int j = 0; j < dsUserDetails.Tables[1].Rows.Count; j++)
                                        //    {
                                        //        UserId = dsUserDetails.Tables[1].Rows[j]["UserId"].ToString();
                                        //        FirstName = "";
                                        //        LastName = "";
                                        //        Address = "";
                                        //        City = "";
                                        //        PinCode = "";
                                        //        MobNo = "";
                                        //        State = "";
                                        //        Country = "";
                                        //        CompanyName = "";
                                        //        Title = "";
                                        //        Designation = "";
                                        //        Languages = "";
                                        //        Specialities = "";

                                        //        DOB = "";
                                        //        ProfilePicture = "";
                                        //        PhoneNo = "";

                                        //        Awards = "";
                                        //        Skills = "";
                                        //        Recomendations = "";
                                        //        License = "";
                                        //        ExpiryDate = "";

                                        //        latitude = "";
                                        //        longitude = "";

                                        //        HomeValue = "";
                                        //        AutoType = "";
                                        //        Revenue = "";
                                        //        Employees = "";
                                        //        CoverageAmt = "";

                                        //        IndustryId = "";
                                        //        SubIndustryId = "";

                                        //        Exp_Designation = "";
                                        //        Exp_CompanyName = "";
                                        //        Exp_DurationFrom = "";
                                        //        Exp_DurationTo = "";
                                        //        Exp_Bio = "";
                                        //        UserBio = "";

                                        //        bool Flag = false;

                                        //        if (dsUserDetails.Tables[1].Rows[j]["Designation"].ToString() != "")
                                        //        {
                                        //            Exp_Designation = dsUserDetails.Tables[1].Rows[j]["Designation"].ToString();
                                        //            Flag = true;
                                        //        }

                                        //        if (dsUserDetails.Tables[1].Rows[j]["CompanyName"].ToString() != "")
                                        //        {
                                        //            Exp_CompanyName = dsUserDetails.Tables[1].Rows[j]["CompanyName"].ToString();
                                        //            Flag = true;
                                        //        }

                                        //        if (dsUserDetails.Tables[1].Rows[j]["DurationFrom"].ToString() != "")
                                        //        {
                                        //            Exp_DurationFrom = dsUserDetails.Tables[1].Rows[j]["DurationFrom"].ToString();
                                        //            Flag = true;
                                        //        }

                                        //        if (dsUserDetails.Tables[1].Rows[j]["DurationTo"].ToString() != "")
                                        //        {
                                        //            Exp_DurationTo = dsUserDetails.Tables[1].Rows[j]["DurationTo"].ToString();
                                        //            Flag = true;
                                        //        }

                                        //        Edu_UniversityName = "";
                                        //        Edu_CourseName = "";
                                        //        Edu_DurationFrom = "";
                                        //        Edu_DurationTo = "";
                                        //        Edu_Logo = "";
                                        //        UpdateTable = "UserExperience";

                                        //        if (Flag == true)
                                        //        {
                                        //            dsUserExperienceDetails = BrokerWSUtility.UpdateBrokerForAndroid(UserId, FirstName, LastName, Address, City, State, Country, PinCode, MobNo, CompanyName, Title, Designation, Languages, Specialities, Exp_Designation, Exp_CompanyName, Exp_DurationFrom, Exp_DurationTo, Edu_UniversityName, Edu_CourseName, Edu_DurationFrom, Edu_DurationTo, DOB, ProfilePicture, PhoneNo, UpdateTable, Awards, Skills, Recomendations, License, ExpiryDate, Resume, ProfilePicImagePath, ResumeImagePath, IsProfilePicUpdated, longitude, latitude, HomeValue, AutoType, Revenue, Employees, CoverageAmt, IsCompanyLogoUpdated, CompanyLogoPath, Edu_Logo, IndustryId, SubIndustryId, Exp_Bio, UserBio);
                                        //        }
                                        //    }
                                        //}

                                        #endregion Old Experience Deails Saving

                                        if (DeletedExpList != "")
                                        {
                                            User1 = BrokerWSUtility.DeleteBrokerDetails(EmailId, "UserExperience", DeletedExpList);
                                        }

                                        if (dsUserDetails.Tables[1].Rows.Count > 0)
                                        {
                                            for (int j = 0; j < dsUserDetails.Tables[1].Rows.Count; j++)
                                            {
                                                if (dsUserDetails.Tables[1].Rows[j]["ExpId"].ToString() != "") //Update Old Record
                                                {
                                                    UserId = dsUserDetails.Tables[1].Rows[j]["UserId"].ToString();

                                                    Exp_Designation = "";
                                                    Exp_CompanyName = "";
                                                    Exp_DurationFrom = "";
                                                    Exp_DurationTo = "";
                                                    Exp_Bio = "";
                                                    IsUpdated = "";

                                                    bool Flag = false;
                                                    string ExpId = "";

                                                    ExpId = dsUserDetails.Tables[1].Rows[j]["ExpId"].ToString();

                                                    if (dsUserDetails.Tables[1].Rows[j]["IsUpdated"].ToString() != "")
                                                    {
                                                        IsUpdated = dsUserDetails.Tables[1].Rows[j]["IsUpdated"].ToString();

                                                        if (dsUserDetails.Tables[1].Rows[j]["Designation"].ToString() != "")
                                                        {
                                                            Exp_Designation = dsUserDetails.Tables[1].Rows[j]["Designation"].ToString();
                                                            Flag = true;
                                                        }

                                                        if (dsUserDetails.Tables[1].Rows[j]["CompanyName"].ToString() != "")
                                                        {
                                                            Exp_CompanyName = dsUserDetails.Tables[1].Rows[j]["CompanyName"].ToString();
                                                            Flag = true;
                                                        }

                                                        if (dsUserDetails.Tables[1].Rows[j]["DurationFrom"].ToString() != "")
                                                        {
                                                            Exp_DurationFrom = dsUserDetails.Tables[1].Rows[j]["DurationFrom"].ToString();
                                                            Flag = true;
                                                        }

                                                        if (dsUserDetails.Tables[1].Rows[j]["DurationTo"].ToString() != "")
                                                        {
                                                            Exp_DurationTo = dsUserDetails.Tables[1].Rows[j]["DurationTo"].ToString();
                                                            Flag = true;
                                                        }

                                                        if (IsUpdated == "true")
                                                        {
                                                            if (dsUserDetails.Tables[1].Rows[j]["ExpCompLogo"].ToString() != "")
                                                            {
                                                                /*****************************Create Image file for Experience Company Logo***************/

                                                                Exp_Logo = UserId + "_" + (j + 1) + Regex.Replace(Exp_CompanyName, @"\s+", "") + ".png";
                                                                string Exp_LogoPath = HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + Exp_Logo);

                                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + Exp_Logo)))
                                                                {
                                                                    File.Delete(Exp_LogoPath);
                                                                }

                                                                string Exp_Logo1 = dsUserDetails.Tables[1].Rows[j]["ExpCompLogo"].ToString();
                                                                Exp_Logo1 = Exp_Logo1.Replace(" ", "+");
                                                                if (Exp_Logo1.ToString().Trim() != "")
                                                                {
                                                                    byte[] imageBytes = Convert.FromBase64String(Exp_Logo1);
                                                                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                                                                    ms.Write(imageBytes, 0, imageBytes.Length);
                                                                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                                                                    image.Save(HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + Exp_Logo), System.Drawing.Imaging.ImageFormat.Png);
                                                                }
                                                                else
                                                                {
                                                                    Exp_Logo = "";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Exp_Logo = "";

                                                            }
                                                        }
                                                        else
                                                        {
                                                            Exp_Logo = "";
                                                        }

                                                        if (Flag == true)
                                                        {
                                                            //dsUserExperienceDetails = BrokerWSUtility.UpdateBrokerForAndroid(UserId, FirstName, LastName, Address, City, State, Country, PinCode, MobNo, CompanyName, Title, Designation, Languages, Specialities, Exp_Designation, Exp_CompanyName, Exp_DurationFrom, Exp_DurationTo, Edu_UniversityName, Edu_CourseName, Edu_DurationFrom, Edu_DurationTo, DOB, ProfilePicture, PhoneNo, UpdateTable, Awards, Skills, Recomendations, License, ExpiryDate, Resume, ProfilePicImagePath, ResumeImagePath, IsProfilePicUpdated, longitude, latitude, HomeValue, AutoType, Revenue, Employees, CoverageAmt, IsCompanyLogoUpdated, CompanyLogoPath, Edu_Logo, IndustryId, SubIndustryId, Exp_Bio, UserBio);
                                                            dsUserExperienceDetails = BrokerWSUtility.UpdateExperienceDetails(ExpId, UserId, Exp_Designation, Exp_CompanyName, Exp_DurationFrom, Exp_DurationTo, Exp_Logo, IsUpdated);
                                                        }
                                                    }
                                                    else
                                                    {

                                                    }
                                                }
                                                else //Insert new entry
                                                {
                                                    UserId = dsUserDetails.Tables[1].Rows[j]["UserId"].ToString();
                                                    FirstName = "";
                                                    LastName = "";
                                                    Address = "";
                                                    City = "";
                                                    PinCode = "";
                                                    MobNo = "";
                                                    State = "";
                                                    Country = "";
                                                    CompanyName = "";
                                                    Title = "";
                                                    Designation = "";
                                                    Languages = "";
                                                    Specialities = "";

                                                    DOB = "";
                                                    ProfilePicture = "";
                                                    PhoneNo = "";

                                                    Awards = "";
                                                    Skills = "";
                                                    Recomendations = "";
                                                    License = "";
                                                    ExpiryDate = "";

                                                    latitude = "";
                                                    longitude = "";

                                                    HomeValue = "";
                                                    AutoType = "";
                                                    Revenue = "";
                                                    Employees = "";
                                                    CoverageAmt = "";

                                                    IndustryId = "";
                                                    SubIndustryId = "";

                                                    Exp_Designation = "";
                                                    Exp_CompanyName = "";
                                                    Exp_DurationFrom = "";
                                                    Exp_DurationTo = "";
                                                    Exp_Bio = "";
                                                    Exp_Logo = "";
                                                    UserBio = "";

                                                    bool Flag = false;

                                                    if (dsUserDetails.Tables[1].Rows[j]["Designation"].ToString() != "")
                                                    {
                                                        Exp_Designation = dsUserDetails.Tables[1].Rows[j]["Designation"].ToString();
                                                        Flag = true;
                                                    }

                                                    if (dsUserDetails.Tables[1].Rows[j]["CompanyName"].ToString() != "")
                                                    {
                                                        Exp_CompanyName = dsUserDetails.Tables[1].Rows[j]["CompanyName"].ToString();
                                                        Flag = true;
                                                    }

                                                    if (dsUserDetails.Tables[1].Rows[j]["DurationFrom"].ToString() != "")
                                                    {
                                                        Exp_DurationFrom = dsUserDetails.Tables[1].Rows[j]["DurationFrom"].ToString();
                                                        Flag = true;
                                                    }

                                                    if (dsUserDetails.Tables[1].Rows[j]["DurationTo"].ToString() != "")
                                                    {
                                                        Exp_DurationTo = dsUserDetails.Tables[1].Rows[j]["DurationTo"].ToString();
                                                        Flag = true;
                                                    }

                                                    if (dsUserDetails.Tables[1].Rows[j]["ExpCompLogo"].ToString() != "")
                                                    {
                                                        /*****************************Create Image file for Experience Company Logo***************/

                                                        Exp_Logo = UserId + "_" + (j + 1) + Regex.Replace(Exp_CompanyName, @"\s+", "") + ".png";
                                                        string Exp_LogoPath = HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + Exp_Logo);

                                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + Exp_Logo)))
                                                        {
                                                            File.Delete(Exp_LogoPath);
                                                        }

                                                        string Exp_Logo1 = dsUserDetails.Tables[1].Rows[j]["ExpCompLogo"].ToString();
                                                        Exp_Logo1 = Exp_Logo1.Replace(" ", "+");
                                                        if (Exp_Logo1.ToString().Trim() != "")
                                                        {
                                                            byte[] imageBytes = Convert.FromBase64String(Exp_Logo1);
                                                            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                                                            ms.Write(imageBytes, 0, imageBytes.Length);
                                                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                                                            image.Save(HttpContext.Current.Server.MapPath("~/UploadedImages/ExperienceCompLogo/" + Exp_Logo), System.Drawing.Imaging.ImageFormat.Png);
                                                        }
                                                        else
                                                        {
                                                            Exp_Logo = "";
                                                        }
                                                        Flag = true;
                                                    }
                                                    else
                                                    {
                                                        Exp_Logo = "";
                                                        // Flag = false;

                                                    }

                                                    Edu_UniversityName = "";
                                                    Edu_CourseName = "";
                                                    Edu_DurationFrom = "";
                                                    Edu_DurationTo = "";
                                                    Edu_Logo = "";
                                                    UpdateTable = "UserExperience";

                                                    if (Flag == true)
                                                    {
                                                        dsUserExperienceDetails = BrokerWSUtility.UpdateBrokerForAndroid(UserId, FirstName, LastName, Address, City, State, Country, PinCode, MobNo, CompanyName, Title, Designation, Languages, Specialities, Exp_Designation, Exp_CompanyName, Exp_DurationFrom, Exp_DurationTo, Edu_UniversityName, Edu_CourseName, Edu_DurationFrom, Edu_DurationTo, DOB, ProfilePicture, PhoneNo, UpdateTable, Awards, Skills, Recomendations, License, ExpiryDate, Resume, ProfilePicImagePath, ResumeImagePath, IsProfilePicUpdated, longitude, latitude, HomeValue, AutoType, Revenue, Employees, CoverageAmt, IsCompanyLogoUpdated, CompanyLogoPath, Edu_Logo, IndustryId, SubIndustryId, Exp_Bio, UserBio, Exp_Logo);
                                                    }
                                                }
                                            }
                                        }

                                        #endregion
                                        // BrokerUtility.ErrorLog(0, "DoUpdateProfileBrokerForAndroid", "User Experience Details Saving End...", "BrokerWSDB.cs_DoUpdateProfileBrokerForAndroid()", "");
                                        //For Saving Education Details
                                        //  BrokerUtility.ErrorLog(0, "DoUpdateProfileBrokerForAndroid", "User Education Details Saving Start...", "BrokerWSDB.cs_DoUpdateProfileBrokerForAndroid()", "");
                                        #region Education Details

                                        if (DeletedEduList != "")
                                        {
                                            User1 = BrokerWSUtility.DeleteBrokerDetails(EmailId, "UserEducation", DeletedEduList);
                                        }

                                        if (dsUserDetails.Tables[2].Rows.Count > 0)
                                        {
                                            for (int k = 0; k < dsUserDetails.Tables[2].Rows.Count; k++)
                                            {
                                                if (dsUserDetails.Tables[2].Rows[k]["EduId"].ToString() != "") //Update Old Record
                                                {
                                                    bool Flag = false;
                                                    string EduId = "";

                                                    Edu_UniversityName = "";
                                                    Edu_CourseName = "";
                                                    Edu_DurationFrom = "";
                                                    Edu_DurationTo = "";
                                                    Edu_Logo = "";
                                                    IsUpdated = "";

                                                    EduId = dsUserDetails.Tables[2].Rows[k]["EduId"].ToString();

                                                    if (dsUserDetails.Tables[2].Rows[k]["IsUpdated"].ToString() != "")
                                                    {
                                                        IsUpdated = dsUserDetails.Tables[2].Rows[k]["IsUpdated"].ToString();

                                                        if (dsUserDetails.Tables[2].Rows[k]["UserId"].ToString() != "")
                                                        {
                                                            UserId = dsUserDetails.Tables[2].Rows[k]["UserId"].ToString();
                                                        }

                                                        if (dsUserDetails.Tables[2].Rows[k]["UniversityName"].ToString() != "")
                                                        {
                                                            Edu_UniversityName = dsUserDetails.Tables[2].Rows[k]["UniversityName"].ToString();
                                                            Flag = true;
                                                        }
                                                        if (dsUserDetails.Tables[2].Rows[k]["CourseName"].ToString() != "")
                                                        {
                                                            Edu_CourseName = dsUserDetails.Tables[2].Rows[k]["CourseName"].ToString();
                                                            Flag = true;
                                                        }

                                                        if (dsUserDetails.Tables[2].Rows[k]["DurationFrom"].ToString() != "")
                                                        {
                                                            Edu_DurationFrom = dsUserDetails.Tables[2].Rows[k]["DurationFrom"].ToString();
                                                            Flag = true;
                                                        }

                                                        if (dsUserDetails.Tables[2].Rows[k]["DurationTo"].ToString() != "")
                                                        {
                                                            Edu_DurationTo = dsUserDetails.Tables[2].Rows[k]["DurationTo"].ToString();
                                                            Flag = true;
                                                        }

                                                        if (IsUpdated == "true")
                                                        {
                                                            if (dsUserDetails.Tables[2].Rows[k]["EducationLogo"].ToString() != "")
                                                            {
                                                                /*****************************Create Image file for Education Logo***************/

                                                                Edu_Logo = UserId + "_" + (k + 1) + Regex.Replace(Edu_UniversityName, @"\s+", "") + "_" + Regex.Replace(Edu_CourseName, @"\s+", "") + ".png";
                                                                string Edu_LogoPath = HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + Edu_Logo);

                                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + Edu_Logo)))
                                                                {
                                                                    File.Delete(Edu_LogoPath);
                                                                }

                                                                string Edu_Logo1 = dsUserDetails.Tables[2].Rows[k]["EducationLogo"].ToString();
                                                                Edu_Logo1 = Edu_Logo1.Replace(" ", "+");
                                                                if (Edu_Logo1.ToString().Trim() != "")
                                                                {
                                                                    byte[] imageBytes = Convert.FromBase64String(Edu_Logo1);
                                                                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                                                                    ms.Write(imageBytes, 0, imageBytes.Length);
                                                                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                                                                    image.Save(HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + Edu_Logo), System.Drawing.Imaging.ImageFormat.Png);
                                                                }
                                                                else
                                                                {
                                                                    Edu_Logo = "";

                                                                }
                                                            }
                                                            else
                                                            {
                                                                Edu_Logo = "";

                                                            }
                                                        }
                                                        else
                                                        {
                                                            Edu_Logo = "";
                                                        }

                                                        //Update Education details
                                                        if (Flag == true)
                                                        {
                                                            dsUserEducationDetails = BrokerWSUtility.UpdateEducationDetails(EduId, UserId, Edu_UniversityName, Edu_CourseName, Edu_DurationFrom, Edu_DurationTo, Edu_Logo, IsUpdated);
                                                            //dsUserEducationDetails = BrokerWSUtility.UpdateBrokerForAndroid(UserId, FirstName, LastName, Address, City, State, Country, PinCode, MobNo, CompanyName, Title, Designation, Languages, Specialities, Exp_Designation, Exp_CompanyName, Exp_DurationFrom, Exp_DurationTo, Edu_UniversityName, Edu_CourseName, Edu_DurationFrom, Edu_DurationTo, DOB, ProfilePicture, PhoneNo, UpdateTable, Awards, Skills, Recomendations, License, ExpiryDate, Resume, ProfilePicImagePath, ResumeImagePath, IsProfilePicUpdated, longitude, latitude, HomeValue, AutoType, Revenue, Employees, CoverageAmt, IsCompanyLogoUpdated, CompanyLogoPath, Edu_Logo);
                                                        }
                                                    }

                                                    else
                                                    {

                                                    }
                                                }
                                                else //Insert new entry
                                                {
                                                    #region Save Education Details

                                                    UserId = dsUserDetails.Tables[2].Rows[k]["UserId"].ToString();
                                                    FirstName = "";
                                                    LastName = "";
                                                    Address = "";
                                                    City = "";
                                                    PinCode = "";
                                                    MobNo = "";
                                                    State = "";
                                                    Country = "";
                                                    CompanyName = "";
                                                    Title = "";
                                                    Designation = "";
                                                    Languages = "";
                                                    Specialities = "";

                                                    DOB = "";
                                                    ProfilePicture = "";
                                                    PhoneNo = "";

                                                    Awards = "";
                                                    Skills = "";
                                                    Recomendations = "";
                                                    License = "";
                                                    ExpiryDate = "";

                                                    latitude = "";
                                                    longitude = "";

                                                    HomeValue = "";
                                                    AutoType = "";
                                                    Revenue = "";
                                                    Employees = "";
                                                    CoverageAmt = "";

                                                    IndustryId = "";
                                                    SubIndustryId = "";

                                                    Exp_Designation = "";
                                                    Exp_CompanyName = "";
                                                    Exp_DurationFrom = "";
                                                    Exp_DurationTo = "";
                                                    Exp_Bio = "";
                                                    UserBio = "";

                                                    bool Flag = false;

                                                    Edu_UniversityName = "";
                                                    Edu_CourseName = "";
                                                    Edu_DurationFrom = "";
                                                    Edu_DurationTo = "";
                                                    Edu_Logo = "";

                                                    if (dsUserDetails.Tables[2].Rows[k]["UniversityName"].ToString() != "")
                                                    {
                                                        Edu_UniversityName = dsUserDetails.Tables[2].Rows[k]["UniversityName"].ToString();
                                                        Flag = true;
                                                    }
                                                    if (dsUserDetails.Tables[2].Rows[k]["CourseName"].ToString() != "")
                                                    {
                                                        Edu_CourseName = dsUserDetails.Tables[2].Rows[k]["CourseName"].ToString();
                                                        Flag = true;
                                                    }

                                                    if (dsUserDetails.Tables[2].Rows[k]["DurationFrom"].ToString() != "")
                                                    {
                                                        Edu_DurationFrom = dsUserDetails.Tables[2].Rows[k]["DurationFrom"].ToString();
                                                        Flag = true;
                                                    }

                                                    if (dsUserDetails.Tables[2].Rows[k]["DurationTo"].ToString() != "")
                                                    {
                                                        Edu_DurationTo = dsUserDetails.Tables[2].Rows[k]["DurationTo"].ToString();
                                                        Flag = true;
                                                    }
                                                    if (dsUserDetails.Tables[2].Rows[k]["EducationLogo"].ToString() != "")
                                                    {
                                                        /*****************************Create Image file for Education Logo***************/

                                                        Edu_Logo = UserId + "_" + (k + 1) + Regex.Replace(Edu_UniversityName, @"\s+", "") + "_" + Regex.Replace(Edu_CourseName, @"\s+", "") + ".png";
                                                        string Edu_LogoPath = HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + Edu_Logo);

                                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + Edu_Logo)))
                                                        {
                                                            File.Delete(Edu_LogoPath);
                                                        }

                                                        string Edu_Logo1 = dsUserDetails.Tables[2].Rows[k]["EducationLogo"].ToString();
                                                        Edu_Logo1 = Edu_Logo1.Replace(" ", "+");

                                                        // Convert Base64 String to byte[]
                                                        if (Edu_Logo1.ToString().Trim() != "")
                                                        {
                                                            byte[] imageBytes = Convert.FromBase64String(Edu_Logo1);
                                                            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                                                            // Convert byte[] to Image
                                                            ms.Write(imageBytes, 0, imageBytes.Length);
                                                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                                                            image.Save(HttpContext.Current.Server.MapPath("~/UploadedImages/EducationLogo/" + Edu_Logo), System.Drawing.Imaging.ImageFormat.Png);
                                                        }
                                                        else
                                                        {
                                                            Edu_Logo = "";

                                                        }

                                                        Flag = true;
                                                    }
                                                    else
                                                    {
                                                        Edu_Logo = "";

                                                    }
                                                    UpdateTable = "UserEducation";

                                                    if (Flag == true)
                                                    {
                                                        dsUserEducationDetails = BrokerWSUtility.UpdateBrokerForAndroid(UserId, FirstName, LastName, Address, City, State, Country, PinCode, MobNo, CompanyName, Title, Designation, Languages, Specialities, Exp_Designation, Exp_CompanyName, Exp_DurationFrom, Exp_DurationTo, Edu_UniversityName, Edu_CourseName, Edu_DurationFrom, Edu_DurationTo, DOB, ProfilePicture, PhoneNo, UpdateTable, Awards, Skills, Recomendations, License, ExpiryDate, Resume, ProfilePicImagePath, ResumeImagePath, IsProfilePicUpdated, longitude, latitude, HomeValue, AutoType, Revenue, Employees, CoverageAmt, IsCompanyLogoUpdated, CompanyLogoPath, Edu_Logo, IndustryId, SubIndustryId, Exp_Bio, UserBio, Exp_Logo);
                                                    }
                                                    #endregion Save Education Details
                                                }
                                            }
                                        }

                                        #endregion
                                        // BrokerUtility.ErrorLog(0, "DoUpdateProfileBrokerForAndroid", "User Education Details Saving End...", "BrokerWSDB.cs_DoUpdateProfileBrokerForAndroid()", "");
                                    }

                                    // BrokerUtility.ErrorLog(0, "DoUpdateProfileBrokerForAndroid", "Getting Saved Details for Broker...", "BrokerWSDB.cs_DoUpdateProfileBrokerForAndroid()", "");
                                    dsUserDetails = BrokerWSUtility.GetBrokerDetails(EmailId);
                                    if (dsUserDetails.Tables.Count > 0)
                                    {
                                        string binData = "", ResumeData = "", ProfilePicImg = "", ResumeImg = "", CompLogoImg = "";

                                        dsUserDetails.Tables[0].TableName = "UserDetails";
                                        dsUserDetails.Tables[1].TableName = "ExperienceDetails";
                                        dsUserDetails.Tables[2].TableName = "EducationDetails";
                                        dsUserDetails.AcceptChanges();

                                        binData = dsUserDetails.Tables[0].Rows[0]["ProfilePicture"].ToString();
                                        if (binData != "")
                                        {
                                            binData = strDomainName + "" + strProfilePicForlderName + "" + dsUserDetails.Tables[0].Rows[0]["ProfilePicture"].ToString(); ;

                                            dsUserDetails.Tables[0].Rows[0]["ProfilePicture"] = binData;
                                        }

                                        ResumeData = dsUserDetails.Tables[0].Rows[0]["Resume"].ToString();
                                        if (ResumeData != "")
                                        {
                                            ResumeData = strDomainName + "" + strResumeForlderName + "" + dsUserDetails.Tables[0].Rows[0]["Resume"].ToString(); ;

                                            dsUserDetails.Tables[0].Rows[0]["Resume"] = ResumeData;
                                        }

                                        ProfilePicImg = dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString();
                                        if (ProfilePicImg != "")
                                        {
                                            ProfilePicImg = strDomainName + "" + strProfilePicImageFolder + "" + dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString(); ;

                                            dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"] = ProfilePicImg;
                                        }

                                        ResumeImg = dsUserDetails.Tables[0].Rows[0]["ResumeDoc"].ToString();
                                        if (ResumeImg != "")
                                        {
                                            ResumeImg = strDomainName + "" + strResumeImageFolder + "" + dsUserDetails.Tables[0].Rows[0]["ResumeDoc"].ToString(); ;

                                            dsUserDetails.Tables[0].Rows[0]["ResumeDoc"] = ResumeImg;
                                        }


                                        CompLogoImg = dsUserDetails.Tables[0].Rows[0]["CompanyLogo"].ToString();
                                        if (CompLogoImg != "")
                                        {
                                            CompLogoImg = strDomainName + "" + strUploadedCompLogoFolder + "" + dsUserDetails.Tables[0].Rows[0]["CompanyLogo"].ToString(); ;

                                            dsUserDetails.Tables[0].Rows[0]["CompanyLogo"] = CompLogoImg;
                                        }


                                        if (dsUserDetails.Tables["ExperienceDetails"].Rows.Count > 0)
                                        {
                                            for (int j = 0; j < dsUserDetails.Tables["ExperienceDetails"].Rows.Count; j++)
                                            {
                                                string Logo = dsUserDetails.Tables["ExperienceDetails"].Rows[j]["Logo"].ToString();
                                                if (Logo != "")
                                                {
                                                    Logo = strDomainName + "" + strExperienceCompLogoFolder + "" + dsUserDetails.Tables["ExperienceDetails"].Rows[j]["Logo"].ToString(); ;

                                                    dsUserDetails.Tables["ExperienceDetails"].Rows[j]["Logo"] = Logo;
                                                }
                                            }
                                        }

                                        if (dsUserDetails.Tables["EducationDetails"].Rows.Count > 0)
                                        {
                                            for (int j = 0; j < dsUserDetails.Tables["EducationDetails"].Rows.Count; j++)
                                            {
                                                string EduLogo = dsUserDetails.Tables["EducationDetails"].Rows[j]["EducationLogo"].ToString();
                                                if (EduLogo != "")
                                                {
                                                    EduLogo = strDomainName + "" + strEducationLogoFolder + "" + dsUserDetails.Tables["EducationDetails"].Rows[j]["EducationLogo"].ToString(); ;

                                                    dsUserDetails.Tables["EducationDetails"].Rows[j]["EducationLogo"] = EduLogo;
                                                }
                                            }
                                        }

                                        //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForSuccess("Success", "null"));
                                        // BrokerUtility.ErrorLog(0, "DoUpdateProfileBrokerForAndroid", "END SAVING", "BrokerWSDB.cs_DoUpdateProfileBrokerForAndroid()", "");
                                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsUserDetails, "DoUpdateProfileBroker", "true", "null"));

                                    }
                                    else
                                    {
                                        //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", "null"));
                                        string Message = "";
                                        DataSet dsDetails = new DataSet();
                                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);
                                        // BrokerUtility.ErrorLog(0, "DoUpdateProfileBrokerForAndroid", "END SAVING With ERROR", "BrokerWSDB.cs_DoUpdateProfileBrokerForAndroid()", "");
                                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoExternalLogin", "false", "Error Occured"));

                                    }

                                }
                                else
                                {
                                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", "null"));
                                    string Message = "";
                                    DataSet dsDetails = new DataSet();
                                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);
                                    //   BrokerUtility.ErrorLog(0, "DoUpdateProfileBrokerForAndroid", "END SAVING With ERROR", "BrokerWSDB.cs_DoUpdateProfileBrokerForAndroid()", "");
                                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoExternalLogin", "false", "Error Occured"));

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoUpdateProfileBrokerForAndroid", Ex.Message.ToString(), "BrokerWSDB.cs_DoUpdateProfileBrokerForAndroid()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);
                BrokerUtility.ErrorLog(0, "DoUpdateProfileBrokerForAndroid", "END SAVING With ERROR-In catch block", "BrokerWSDB.cs_DoUpdateProfileBrokerForAndroid()", "");
                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoUpdateProfileBrokerForAndroid", "false", "Error occured, please try again."));
            }

        }

        // Regular Signup without facebook and linkedin
        public static void DoRegularSignUp(string WSFlag)
        {
            DataSet dsUserDetails = null;
            string UserName = "", Password = "", UserType = "", TempPass = "", RegisteredFor = "";

            try
            {
                if (WSFlag == "Application")
                {
                    UserName = HttpContext.Current.Request.Form["UserName"].ToString();
                    Password = HttpContext.Current.Request.Form["Password"].ToString();
                    UserType = HttpContext.Current.Request.Form["UserType"].ToString();
                    RegisteredFor = HttpContext.Current.Request.Form["RegisteredFor"].ToString();
                }
                else
                {
                    UserName = HttpContext.Current.Request.QueryString["UserName"].ToString();
                    Password = HttpContext.Current.Request.QueryString["Password"].ToString();
                    UserType = HttpContext.Current.Request.QueryString["UserType"].ToString();
                    RegisteredFor = HttpContext.Current.Request.QueryString["RegisteredFor"].ToString();
                }
                //HttpContext.Current.Response.Write(BrokerWSUtility.CheckLogin(UserName, Password));

                DataSet dsCheckUserExist = BrokerUtility.CheckUSerExist(UserName);

                if (dsCheckUserExist.Tables.Count > 0)
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("User Already Exist", "null"));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoRegularSignUp", "false", "User Already Exist"));

                }
                else
                {

                    string random = BrokerWSUtility.GetRandomNumber();

                    string Encryptrandom = BrokerUtility.EncryptURL(random);

                    TempPass = BrokerUtility.EncryptURL(Password);
                    dsUserDetails = BrokerUtility.RegisterUser("", "", UserName, TempPass, "", "", "", "", "", "", "0", UserType, "Regular", "0", "", Encryptrandom.ToString(), RegisteredFor);//22Feb18

                    if (dsUserDetails.Tables[0].Rows.Count > 0)
                    {
                        bool EmailFlag = false;

                        //send activation link to user.
                        EmailFlag = BrokerWSUtility.SendRegistrationEmail(dsUserDetails.Tables[0].Rows[0]["EmailId"].ToString(), Encryptrandom.ToString(), dsUserDetails.Tables[0].Rows[0]["UserId"].ToString(), UserType);

                        DataTable dtDetails = new DataTable();
                        DataSet dsDetails = new DataSet();

                        dtDetails.Columns.Add("Message");
                        dtDetails.Columns.Add("EmailId");
                        dtDetails.Columns.Add("EmailSuccess");

                        dtDetails.Rows.Add("Success", dsUserDetails.Tables[0].Rows[0]["EmailId"].ToString(), EmailFlag);

                        dsDetails.Tables.Add(dtDetails);
                        dsDetails.Tables[0].TableName = "Response";
                        dsDetails.AcceptChanges();

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoRegularSignUp", "true", "null"));
                    }
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoRegularSignUp", Ex.Message.ToString(), "BrokerWSDB.cs_DoRegularSignUp()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }

        }

        //Verify emailid from activation link which is sent after successfull regular signup.
        public static void DoVerifyEmailID(string WSFlag)
        {

            string EmailID = "", RegistrationCode = "";
            DataSet dsResult = null;

            try
            {
                if (WSFlag == "Application")
                {
                    EmailID = HttpContext.Current.Request.Form["EmailID"].ToString();
                    RegistrationCode = HttpContext.Current.Request.Form["RegistrationCode"].ToString();
                }
                else
                {
                    EmailID = HttpContext.Current.Request.QueryString["EmailID"].ToString();
                    RegistrationCode = HttpContext.Current.Request.QueryString["RegistrationCode"].ToString();
                }

                dsResult = BrokerWSUtility.VerifyUserDetails(EmailID, RegistrationCode);

                if (dsResult.Tables.Count > 0)
                {
                    dsResult.Tables[0].TableName = "UserDetails";
                    dsResult.AcceptChanges();

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsResult, "DoVerifyEmailID", "true", "null"));
                }
                else
                {
                    HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoVerifyEmailID", Ex.Message.ToString(), "BrokerWSDB.cs_DoVerifyEmailID()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }
        //Update Profile after singup for Android application.
        public static void DoUpdateProfileCustomerForAndroid(string WSFlag)
        {
            DataSet dsUserDetails = null;
            string UserId = "", FirstName = "", LastName = "", Password = "", Address = "", City = "", PinCode = "", MobNo = "", IsActive = "", UserType = "", IsUpdateProfile = "", State = "", Country = "",
                   HouseType = "", IsHavingCar = "", NoOfCars = "", TypeOfEmployment = "", CompanyName = "", AddressOfHouse = "";

            string ProfilePicImagePath = "", IsProfilePicUpdated = "", FileName1 = "", Email = "";

            string binData = "", DOB = "", PhoneNo = "", p1 = "", ProfilePicImg = "";

            try
            {
                string JSonResult = "";
                if (WSFlag == "Application")
                {
                    IsProfilePicUpdated = HttpContext.Current.Request.Form["IsProfilePicUpdated"].ToString();
                    JSonResult = HttpContext.Current.Request.Form["Result"].ToString();
                }
                else
                {
                    IsProfilePicUpdated = HttpContext.Current.Request.QueryString["IsProfilePicUpdated"].ToString();
                    JSonResult = HttpContext.Current.Request.QueryString["Result"].ToString();
                }

                if (JSonResult != "")
                {
                    dsUserDetails = JsonConvert.DeserializeObject<DataSet>(JSonResult);

                    if (dsUserDetails.Tables.Count > 0)
                    {
                        if (dsUserDetails.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsUserDetails.Tables[0].Rows.Count; i++)
                            {
                                UserId = dsUserDetails.Tables[0].Rows[i]["UserId"].ToString();
                                FirstName = dsUserDetails.Tables[0].Rows[i]["FirstName"].ToString();
                                LastName = dsUserDetails.Tables[0].Rows[i]["LastName"].ToString();
                                Address = dsUserDetails.Tables[0].Rows[i]["Address"].ToString();
                                City = dsUserDetails.Tables[0].Rows[i]["City"].ToString();
                                PinCode = dsUserDetails.Tables[0].Rows[i]["PinCode"].ToString();
                                MobNo = dsUserDetails.Tables[0].Rows[i]["MobNo"].ToString();
                                State = dsUserDetails.Tables[0].Rows[i]["State"].ToString();
                                Country = dsUserDetails.Tables[0].Rows[i]["Country"].ToString();
                                HouseType = dsUserDetails.Tables[0].Rows[i]["HouseType"].ToString();
                                AddressOfHouse = dsUserDetails.Tables[0].Rows[i]["AddressOfHouse"].ToString();
                                IsHavingCar = dsUserDetails.Tables[0].Rows[i]["IsHavingCar"].ToString();
                                NoOfCars = dsUserDetails.Tables[0].Rows[i]["NoOfCars"].ToString();
                                TypeOfEmployment = dsUserDetails.Tables[0].Rows[i]["TypeOfEmployment"].ToString();
                                CompanyName = dsUserDetails.Tables[0].Rows[i]["CompanyName"].ToString();

                                //p1 = dsUserDetails.Tables[0].Rows[i]["ProfilePicture"].ToString();

                                DOB = dsUserDetails.Tables[0].Rows[i]["DOB"].ToString();
                                string ProfilePicture = dsUserDetails.Tables[0].Rows[i]["ProfilePicture"].ToString();
                                ProfilePicture = ProfilePicture.Replace(" ", "+");
                                PhoneNo = dsUserDetails.Tables[0].Rows[i]["PhoneNo"].ToString();

                                /********************************/


                                if (IsProfilePicUpdated == "true")
                                {
                                    List<spGetUserDetails_Result> oUserDetails = null;

                                    oUserDetails = BrokerUtility.GetUserDetails(Convert.ToInt32(UserId));
                                    Email = oUserDetails[0].EmailId;

                                    FileName1 = Email + "_" + UserId + ".txt";
                                    string FileName = HttpContext.Current.Server.MapPath("~/ProfilePicture/" + Email + "_" + UserId + ".txt").ToString();
                                    if (File.Exists(FileName))
                                    {
                                        File.Delete(FileName);
                                    }
                                    // Create a new file 
                                    if (ProfilePicture.ToString().Trim() != "")
                                    {
                                        using (FileStream fs = File.Create(FileName))
                                        {
                                            // Add some text to file
                                            Byte[] title = new UTF8Encoding(true).GetBytes(ProfilePicture);
                                            fs.Write(title, 0, title.Length);
                                        }
                                    }
                                    else
                                    {
                                        FileName1 = "";
                                    }


                                    /********************************/

                                    /*****************************Create Image file for Profile Picture***************/

                                    ProfilePicImagePath = Email + "_" + UserId + ".png";
                                    string ProfilePicImageFullPath = HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + ProfilePicImagePath);

                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + ProfilePicImagePath)))
                                    {
                                        File.Delete(ProfilePicImageFullPath);
                                    }

                                    // Convert Base64 String to byte[]
                                    if (ProfilePicture.ToString().Trim() != "")
                                    {
                                        byte[] imageBytes = Convert.FromBase64String(ProfilePicture);
                                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                                        // Convert byte[] to Image
                                        ms.Write(imageBytes, 0, imageBytes.Length);
                                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                                        image.Save(HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Email + "_" + UserId + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                    else
                                    {
                                        ProfilePicImagePath = "";
                                    }
                                }

                                /*****************************End of Create Image file for Profile Picture*********/

                                dsUserDetails = BrokerWSUtility.UpdateCustomerForAndroid(UserId, FirstName, LastName, Address, City, State, Country, PinCode, MobNo, HouseType, IsHavingCar, NoOfCars, TypeOfEmployment, CompanyName, DOB, FileName1, PhoneNo, AddressOfHouse, ProfilePicImagePath, IsProfilePicUpdated, "", "", "");

                                if (dsUserDetails.Tables[0].Rows.Count > 0)
                                {
                                    string EmailId = dsUserDetails.Tables[0].Rows[0]["EmailId"].ToString();

                                    dsUserDetails = BrokerWSUtility.GetCustomerDetails(EmailId);

                                    if (dsUserDetails.Tables[0].Rows.Count > 0)
                                    {
                                        dsUserDetails.Tables[0].TableName = "UserDetails";
                                        dsUserDetails.AcceptChanges();

                                        //Set ProfilePicture path
                                        binData = dsUserDetails.Tables[0].Rows[0]["ProfilePicture"].ToString();
                                        if (binData != "")
                                        {
                                            binData = strDomainName + "" + strProfilePicForlderName + "" + dsUserDetails.Tables[0].Rows[0]["ProfilePicture"].ToString(); ;

                                            dsUserDetails.Tables[0].Rows[0]["ProfilePicture"] = binData;
                                        }

                                        //Set ProfilePicImg path
                                        ProfilePicImg = dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString();
                                        if (ProfilePicImg != "")
                                        {
                                            ProfilePicImg = strDomainName + "" + strProfilePicImageFolder + "" + dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString(); ;

                                            dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"] = ProfilePicImg;
                                        }

                                        //dsUserDetails.Tables[0].Columns.Remove("ProfilePicture");
                                        //dsUserDetails.AcceptChanges();

                                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsUserDetails, "DoUpdateProfileCustomer", "true", "null"));
                                    }
                                    else
                                    {
                                        //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", "null"));
                                        string Message = "";
                                        DataSet dsDetails = new DataSet();
                                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoUpdateProfileCustomer", "false", "Error Occured"));

                                    }
                                }
                                else
                                {
                                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", "null"));
                                    string Message = "";
                                    DataSet dsDetails = new DataSet();
                                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoUpdateProfileCustomer", "false", "Error Occured"));

                                }
                            }

                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoUpdateProfileCustomerForAndroid", Ex.Message.ToString(), "BrokerWSDB.cs_DoUpdateProfileCustomerForAndroid()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }

        }

        //Update Profile after singup  



        //Function for getting the details of Customer for specific UserId
        public static void DoViewProfileCustomer(string WSFlag)
        {
            string UserId = "";
            DataSet dsUserDetails = null;

            try
            {
                if (WSFlag == "Application")
                {
                    UserId = HttpContext.Current.Request.Form["UserId"].ToString();
                }
                else
                {
                    UserId = HttpContext.Current.Request.QueryString["UserId"].ToString();
                }

                dsUserDetails = BrokerWSUtility.CheckValidUser(UserId);

                if (dsUserDetails.Tables.Count > 0)
                {
                    if (dsUserDetails.Tables[0].Rows[0]["UserType"].ToString() == "Customer")
                    {
                        string binData = "";

                        dsUserDetails.Tables[0].TableName = "UserDetails";
                        dsUserDetails.AcceptChanges();


                        /***************************************/
                        binData = dsUserDetails.Tables[0].Rows[0]["ProfilePicture"].ToString();
                        if (binData != "")
                        {
                            binData = strDomainName + "" + strProfilePicForlderName + "" + dsUserDetails.Tables[0].Rows[0]["ProfilePicture"].ToString(); ;

                            dsUserDetails.Tables[0].Rows[0]["ProfilePicture"] = binData;
                        }

                        string ProfilePicImg = dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString();
                        if (ProfilePicImg != "")
                        {
                            ProfilePicImg = strDomainName + "" + strProfilePicImageFolder + "" + dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString(); ;

                            dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"] = ProfilePicImg;
                        }
                        /***************************************/

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsUserDetails, "DoViewProfileCustomer", "true", "null"));
                    }
                    else
                    {
                        //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));
                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoViewProfileCustomer", "false", "Error Occured"));

                    }
                }
                else
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoViewProfileCustomer", "false", "Error Occured"));

                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoViewProfileCustomer", Ex.Message.ToString(), "BrokerWSDB.cs_DoViewProfileCustomer()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        //Function for getting the details of Broker for specific UserId
        public static void DoViewProfileBroker(string WSFlag)
        {
            string UserId = "";
            DataSet dsUserDetails = null;
            try
            {
                if (WSFlag == "Application")
                {
                    UserId = HttpContext.Current.Request.Form["UserId"].ToString();
                }
                else
                {
                    UserId = HttpContext.Current.Request.QueryString["UserId"].ToString();
                }

                dsUserDetails = BrokerWSUtility.CheckValidUser(UserId);

                if (dsUserDetails.Tables.Count > 0)
                {
                    string binData = "", ResumeData = "";

                    if (dsUserDetails.Tables[0].Rows[0]["UserType"].ToString() == "Broker")
                    {
                        dsUserDetails.Tables[0].TableName = "UserDetails";
                        dsUserDetails.Tables[1].TableName = "ExperienceDetails";
                        dsUserDetails.Tables[2].TableName = "EducationDetails";
                        dsUserDetails.AcceptChanges();

                        binData = dsUserDetails.Tables[0].Rows[0]["ProfilePicture"].ToString();
                        if (binData != "")
                        {
                            binData = strDomainName + "" + strProfilePicForlderName + "" + dsUserDetails.Tables[0].Rows[0]["ProfilePicture"].ToString(); ;

                            dsUserDetails.Tables[0].Rows[0]["ProfilePicture"] = binData;
                        }

                        ResumeData = dsUserDetails.Tables[0].Rows[0]["Resume"].ToString();
                        if (ResumeData != "")
                        {
                            ResumeData = strDomainName + "" + strResumeForlderName + "" + dsUserDetails.Tables[0].Rows[0]["Resume"].ToString(); ;

                            dsUserDetails.Tables[0].Rows[0]["Resume"] = ResumeData;
                        }

                        string ProfilePicImg = dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString();
                        if (ProfilePicImg != "")
                        {
                            ProfilePicImg = strDomainName + "" + strProfilePicImageFolder + "" + dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString(); ;

                            dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"] = ProfilePicImg;
                        }

                        string ResumeImg = dsUserDetails.Tables[0].Rows[0]["ResumeDoc"].ToString();
                        if (ResumeImg != "")
                        {
                            ResumeImg = strDomainName + "" + strResumeImageFolder + "" + dsUserDetails.Tables[0].Rows[0]["ResumeDoc"].ToString(); ;

                            dsUserDetails.Tables[0].Rows[0]["ResumeDoc"] = ResumeImg;
                        }

                        if (dsUserDetails.Tables["ExperienceDetails"].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsUserDetails.Tables["ExperienceDetails"].Rows.Count; i++)
                            {
                                string Logo = dsUserDetails.Tables["ExperienceDetails"].Rows[i]["Logo"].ToString();
                                if (Logo != "")
                                {
                                    Logo = strDomainName + "" + strCompanyLogoFolder + "" + dsUserDetails.Tables["ExperienceDetails"].Rows[i]["Logo"].ToString(); ;

                                    dsUserDetails.Tables["ExperienceDetails"].Rows[i]["Logo"] = Logo;
                                }
                            }
                        }


                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsUserDetails, "DoViewProfileBroker", "true", "null"));
                    }
                    else
                    {
                        //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));
                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoViewProfileBroker", "false", "This user is not a broker"));

                    }
                }
                else
                {
                    // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoViewProfileBroker", "false", "Error Occured"));

                }

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoViewProfileBroker", Ex.Message.ToString(), "BrokerWSDB.cs_DoViewProfileBroker()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }


        //used to saved Customer's Business Insurace Details.

        public static void DoSaveBusinessInsuranceDetails(string WSFlag)
        {
            string ZipCode = "", City = "", IsInsured = "", InsuranceCompany = "", SICCode = "",
                Revenue = "", CoverageExpires = "", Language = "", Notes = "", Longitude = "", Latitude = "",
                IndustryId = "", SubIndustryId = "", DeclarationDoc = "", DocName = "", DocPath = "";//29May18;

            int UserId;

            int User = 0;
            DataSet dsBrokerDetails = null;

            try
            {
                if (WSFlag == "Application")
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"].ToString());
                    ZipCode = HttpContext.Current.Request.Form["ZipCode"].ToString();
                    City = HttpContext.Current.Request.Form["City"].ToString();
                    IsInsured = HttpContext.Current.Request.Form["IsInsured"].ToString();
                    InsuranceCompany = HttpContext.Current.Request.Form["InsuranceCompany"].ToString();
                    SICCode = HttpContext.Current.Request.Form["SICCode"].ToString();
                    Revenue = HttpContext.Current.Request.Form["Revenue"].ToString();
                    CoverageExpires = HttpContext.Current.Request.Form["CoverageExpires"].ToString();
                    Language = HttpContext.Current.Request.Form["Language"].ToString();
                    Notes = HttpContext.Current.Request.Form["Notes"].ToString();

                    Longitude = HttpContext.Current.Request.Form["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.Form["Latitude"].ToString();

                    IndustryId = HttpContext.Current.Request.Form["IndustryId"].ToString();
                    SubIndustryId = HttpContext.Current.Request.Form["SubIndustryId"].ToString();

                    DeclarationDoc = HttpContext.Current.Request.Form["DeclarationDoc"].ToString();//29May18
                    DocName = HttpContext.Current.Request.Form["DocName"].ToString();//29May18

                }
                else
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"].ToString());
                    ZipCode = HttpContext.Current.Request.QueryString["ZipCode"].ToString();
                    City = HttpContext.Current.Request.QueryString["City"].ToString();
                    IsInsured = HttpContext.Current.Request.QueryString["IsInsured"].ToString();
                    InsuranceCompany = HttpContext.Current.Request.QueryString["InsuranceCompany"].ToString();
                    SICCode = HttpContext.Current.Request.QueryString["SICCode"].ToString();
                    Revenue = HttpContext.Current.Request.QueryString["Revenue"].ToString();
                    CoverageExpires = HttpContext.Current.Request.QueryString["CoverageExpires"].ToString();
                    Language = HttpContext.Current.Request.QueryString["Language"].ToString();
                    Notes = HttpContext.Current.Request.QueryString["Notes"].ToString();

                    Longitude = HttpContext.Current.Request.QueryString["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.QueryString["Latitude"].ToString();

                    IndustryId = HttpContext.Current.Request.QueryString["IndustryId"].ToString();
                    SubIndustryId = HttpContext.Current.Request.QueryString["SubIndustryId"].ToString();

                    DeclarationDoc = HttpContext.Current.Request.QueryString["DeclarationDoc"].ToString();//29May18
                    DocName = HttpContext.Current.Request.QueryString["DocName"].ToString();//29May18
                }

                //29May18
                if (DocName != "" && DeclarationDoc != "")
                {
                    DeclarationDoc = DeclarationDoc.Replace(" ", "+");
                    DocPath = BrokerUtility.SaveDeclarationDocument(DocName, DeclarationDoc, "BusinessApp", UserId.ToString());
                }

                User = BrokerWSUtility.SaveBusinessInsuranceDetails(UserId, ZipCode, City, IsInsured, InsuranceCompany, SICCode, Revenue, CoverageExpires, Language, Notes, Longitude, Latitude, IndustryId, SubIndustryId, DocPath, DeclarationDoc);

                if (User != 0)
                {
                    dsBrokerDetails = BrokerWSUtility.GetBrokersList(UserId, ZipCode, City, Language, "Business", Longitude, Latitude, Revenue, IndustryId, SubIndustryId);

                    if (dsBrokerDetails.Tables.Count > 0)
                    {
                        if (dsBrokerDetails.Tables[0].Rows.Count > 0)
                        {
                            string SerchForUserId = "";
                            if (strUsersToShowByDefaultInSearchList != "")
                            {
                                SerchForUserId = CheckUserIdInExistingResult(strUsersToShowByDefaultInSearchList, dsBrokerDetails);
                            }

                            if (SerchForUserId != "")
                            {
                                dsBrokerDetails = MergeTwoResults(dsBrokerDetails, SerchForUserId, UserId);
                            }

                            SendUserDetailsForHomeInsurance(dsBrokerDetails);
                        }
                        else
                        {
                            dsBrokerDetails = BrokerWSUtility.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsBrokerDetails.Tables.Count > 0)
                            {
                                if (dsBrokerDetails.Tables[0].Rows.Count > 0)
                                {
                                    SendUserDetailsForHomeInsurance(dsBrokerDetails);
                                }
                                else
                                {
                                    dsBrokerDetails.Tables[0].TableName = "UserDetails";
                                    dsBrokerDetails.Tables[1].TableName = "ExperienceDetails";
                                    dsBrokerDetails.Tables[2].TableName = "EducationDetails";
                                    dsBrokerDetails.Tables[3].TableName = "BrokerContactList";
                                    dsBrokerDetails.AcceptChanges();

                                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsBrokerDetails, "DoSavBusinessInsuranceDetails", "true", "null"));
                                }
                            }
                            else
                            {
                                string Message = "";
                                DataSet dsDetails = new DataSet();
                                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));
                            }
                        }
                    }
                    else
                    {
                        //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));

                    }
                }
                else
                {
                    // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));

                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSaveBusinessInsuranceDetails", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveBusinessInsuranceDetails()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        //used to saved Customer's Benefits Insurace Details.

        public static void DoSaveBenefitInsuranceDetails(string WSFlag)
        {
            string ZipCode = "", City = "", IsInsured = "", InsuranceCompany = "", EmployeeStrength = "",
                 CoverageExpires = "", Language = "", Notes = "", Longitude = "", Latitude = "", IndustryId = "",
                 SubIndustryId = "", DeclarationDoc = "", DocName = "", DocPath = "";//29May18;

            int UserId;

            int User = 0;
            DataSet dsBrokerDetails = null;

            try
            {
                if (WSFlag == "Application")
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"].ToString());
                    ZipCode = HttpContext.Current.Request.Form["ZipCode"].ToString();
                    City = HttpContext.Current.Request.Form["City"].ToString();
                    IsInsured = HttpContext.Current.Request.Form["IsInsured"].ToString();
                    InsuranceCompany = HttpContext.Current.Request.Form["InsuranceCompany"].ToString();
                    EmployeeStrength = HttpContext.Current.Request.Form["EmployeeStrength"].ToString();
                    //Revenue = HttpContext.Current.Request.Form["Revenue"].ToString();
                    CoverageExpires = HttpContext.Current.Request.Form["CoverageExpires"].ToString();
                    Language = HttpContext.Current.Request.Form["Language"].ToString();
                    Notes = HttpContext.Current.Request.Form["Notes"].ToString();

                    Longitude = HttpContext.Current.Request.Form["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.Form["Latitude"].ToString();

                    IndustryId = HttpContext.Current.Request.Form["IndustryId"].ToString();
                    SubIndustryId = HttpContext.Current.Request.Form["SubIndustryId"].ToString();

                    DeclarationDoc = HttpContext.Current.Request.Form["DeclarationDoc"].ToString();//29May18
                    DocName = HttpContext.Current.Request.Form["DocName"].ToString();//29May18

                }
                else
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"].ToString());
                    ZipCode = HttpContext.Current.Request.QueryString["ZipCode"].ToString();
                    City = HttpContext.Current.Request.QueryString["City"].ToString();
                    IsInsured = HttpContext.Current.Request.QueryString["IsInsured"].ToString();
                    InsuranceCompany = HttpContext.Current.Request.QueryString["InsuranceCompany"].ToString();
                    EmployeeStrength = HttpContext.Current.Request.QueryString["EmployeeStrength"].ToString();
                    //Revenue = HttpContext.Current.Request.QueryString["Revenue"].ToString();
                    CoverageExpires = HttpContext.Current.Request.QueryString["CoverageExpires"].ToString();
                    Language = HttpContext.Current.Request.QueryString["Language"].ToString();
                    Notes = HttpContext.Current.Request.QueryString["Notes"].ToString();

                    Longitude = HttpContext.Current.Request.QueryString["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.QueryString["Latitude"].ToString();

                    IndustryId = HttpContext.Current.Request.Form["IndustryId"].ToString();
                    SubIndustryId = HttpContext.Current.Request.Form["SubIndustryId"].ToString();

                    DeclarationDoc = HttpContext.Current.Request.QueryString["DeclarationDoc"].ToString();//29May18
                    DocName = HttpContext.Current.Request.QueryString["DocName"].ToString();//29May18
                }

                //29May18
                if (DocName != "" && DeclarationDoc != "")
                {
                    DeclarationDoc = DeclarationDoc.Replace(" ", "+");
                    DocPath = BrokerUtility.SaveDeclarationDocument(DocName, DeclarationDoc, "BebefitsApp", UserId.ToString());
                }

                User = BrokerWSUtility.SaveBenefitInsuranceDetails(UserId, ZipCode, City, IsInsured, InsuranceCompany, EmployeeStrength, CoverageExpires, Language, Notes, Longitude, Latitude, IndustryId, SubIndustryId, DocPath, DeclarationDoc);

                if (User != 0)
                {
                    dsBrokerDetails = BrokerWSUtility.GetBrokersList(UserId, ZipCode, City, Language, "Benefit", Longitude, Latitude, EmployeeStrength, IndustryId, SubIndustryId);

                    if (dsBrokerDetails.Tables.Count > 0)
                    {
                        if (dsBrokerDetails.Tables[0].Rows.Count > 0)
                        {
                            string SerchForUserId = "";
                            if (strUsersToShowByDefaultInSearchList != "")
                            {
                                SerchForUserId = CheckUserIdInExistingResult(strUsersToShowByDefaultInSearchList, dsBrokerDetails);
                            }

                            if (SerchForUserId != "")
                            {
                                dsBrokerDetails = MergeTwoResults(dsBrokerDetails, SerchForUserId, UserId);
                            }

                            SendUserDetailsForHomeInsurance(dsBrokerDetails);
                        }
                        else
                        {
                            dsBrokerDetails = BrokerWSUtility.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsBrokerDetails.Tables.Count > 0)
                            {
                                if (dsBrokerDetails.Tables[0].Rows.Count > 0)
                                {
                                    SendUserDetailsForHomeInsurance(dsBrokerDetails);
                                }
                                else
                                {
                                    dsBrokerDetails.Tables[0].TableName = "UserDetails";
                                    dsBrokerDetails.Tables[1].TableName = "ExperienceDetails";
                                    dsBrokerDetails.Tables[2].TableName = "EducationDetails";
                                    dsBrokerDetails.Tables[3].TableName = "BrokerContactList";
                                    dsBrokerDetails.AcceptChanges();

                                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsBrokerDetails, "DoSaveBenefitsInsuranceDetails", "true", "null"));
                                }
                            }
                            else
                            {
                                string Message = "";
                                DataSet dsDetails = new DataSet();
                                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBenefitsInsuranceDetails", "false", "Error Occured"));
                            }
                        }
                    }
                    else
                    {
                        // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBenefitsInsuranceDetails", "false", "Error Occured"));

                    }
                }
                else
                {
                    // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBenefitsInsuranceDetails", "false", "Error Occured"));

                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSaveBenefitInsuranceDetails", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveBenefitInsuranceDetails()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static void DoSaveLifeInsuranceDetails(string WSFlag)
        {
            DataSet dsUserDetails = null;
            int User, UserId = 0;
            string ZipCode = "", City = "", IsInsured = "", InsuranceCompany = "",
                 Language = "", FaceValue = "", CoverageExpires = "", Notes = "", Longitude = "", Latitude = "",
                 DeclarationDoc = "", DocName = "", DocPath = "";//29May18;
            try
            {
                if (WSFlag == "Application")
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                    ZipCode = HttpContext.Current.Request.Form["ZipCode"].ToString();
                    City = HttpContext.Current.Request.Form["City"].ToString();
                    IsInsured = HttpContext.Current.Request.Form["IsInsured"].ToString();
                    InsuranceCompany = HttpContext.Current.Request.Form["InsuranceCompany"].ToString();
                    Language = HttpContext.Current.Request.Form["Language"].ToString();
                    FaceValue = HttpContext.Current.Request.Form["FaceValue"].ToString();
                    CoverageExpires = HttpContext.Current.Request.Form["CoverageExpires"].ToString();
                    Notes = HttpContext.Current.Request.Form["Notes"].ToString();

                    Longitude = HttpContext.Current.Request.Form["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.Form["Latitude"].ToString();

                    DeclarationDoc = HttpContext.Current.Request.Form["DeclarationDoc"].ToString();//29May18
                    DocName = HttpContext.Current.Request.Form["DocName"].ToString();//29May18
                }
                else
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                    ZipCode = HttpContext.Current.Request.QueryString["ZipCode"].ToString();
                    City = HttpContext.Current.Request.QueryString["City"].ToString();
                    IsInsured = HttpContext.Current.Request.QueryString["IsInsured"].ToString();
                    InsuranceCompany = HttpContext.Current.Request.QueryString["InsuranceCompany"].ToString();
                    Language = HttpContext.Current.Request.QueryString["Language"].ToString();
                    FaceValue = HttpContext.Current.Request.QueryString["FaceValue"].ToString();
                    CoverageExpires = HttpContext.Current.Request.QueryString["CoverageExpires"].ToString();
                    Notes = HttpContext.Current.Request.QueryString["Notes"].ToString();

                    Longitude = HttpContext.Current.Request.QueryString["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.QueryString["Latitude"].ToString();

                    DeclarationDoc = HttpContext.Current.Request.QueryString["DeclarationDoc"].ToString();//29May18
                    DocName = HttpContext.Current.Request.QueryString["DocName"].ToString();//29May18
                }

                //29May18
                if (DocName != "" && DeclarationDoc != "")
                {
                    DeclarationDoc = DeclarationDoc.Replace(" ", "+");
                    DocPath = BrokerUtility.SaveDeclarationDocument(DocName, DeclarationDoc, "LifeApp", UserId.ToString());
                }

                User = BrokerWSUtility.SaveLifeInsuranceDetails(UserId, ZipCode, City, IsInsured, InsuranceCompany, Language, FaceValue, CoverageExpires, Notes, Longitude, Latitude, DocPath, DeclarationDoc);

                if (User != 0)
                {
                    dsUserDetails = BrokerWSUtility.GetBrokersList(UserId, ZipCode, City, Language, "Life", Longitude, Latitude, FaceValue, "0", "0");

                    if (dsUserDetails.Tables.Count > 0)
                    {
                        if (dsUserDetails.Tables[0].Rows.Count > 0)
                        {

                            string SerchForUserId = "";
                            if (strUsersToShowByDefaultInSearchList != "")
                            {
                                SerchForUserId = CheckUserIdInExistingResult(strUsersToShowByDefaultInSearchList, dsUserDetails);
                            }

                            if (SerchForUserId != "")
                            {
                                dsUserDetails = MergeTwoResults(dsUserDetails, SerchForUserId, UserId);
                            }

                            SendUserDetailsForHomeInsurance(dsUserDetails);
                        }
                        else
                        {
                            dsUserDetails = BrokerWSUtility.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsUserDetails.Tables.Count > 0)
                            {
                                if (dsUserDetails.Tables[0].Rows.Count > 0)
                                {
                                    SendUserDetailsForHomeInsurance(dsUserDetails);
                                }
                                else
                                {
                                    dsUserDetails.Tables[0].TableName = "UserDetails";
                                    dsUserDetails.Tables[1].TableName = "ExperienceDetails";
                                    dsUserDetails.Tables[2].TableName = "EducationDetails";
                                    dsUserDetails.Tables[3].TableName = "BrokerContactList";
                                    dsUserDetails.AcceptChanges();

                                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsUserDetails, "DoSaveLifeInsuranceDetails", "true", "null"));
                                }
                            }
                            else
                            {
                                string Message = "";
                                DataSet dsDetails = new DataSet();
                                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveLifeInsuranceDetails", "false", "Error Occured"));
                            }
                        }
                    }
                    else
                    {
                        // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveLifeInsuranceDetails", "false", "Error Occured"));

                    }
                }
                else
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveLifeInsuranceDetails", "false", "Error Occured"));

                }

            }

            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSaveLifeInsuranceDetails", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveLifeInsuranceDetails()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveLifeInsuranceDetails", "false", "Error occured, please try again."));
            }
        }

        public static void DoSaveAutoInsuranceDetails(string WSFlag)
        {
            DataSet dsUserDetails = null;

            int User, UserId = 0;
            string ZipCode = "", City = "", VehicleType = "", IsInsured = "", InsuranceCompany = "",
                CoverageExpires = "", Language = "", Notes = "", Longitude = "", Latitude = "", DeclarationDoc = "", DocName = "", DocPath = "";//29May18;
            try
            {
                if (WSFlag == "Application")
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                    ZipCode = HttpContext.Current.Request.Form["ZipCode"].ToString();
                    City = HttpContext.Current.Request.Form["City"].ToString();
                    VehicleType = HttpContext.Current.Request.Form["VehicleType"].ToString();
                    IsInsured = HttpContext.Current.Request.Form["IsInsured"].ToString();
                    InsuranceCompany = HttpContext.Current.Request.Form["InsuranceCompany"].ToString();
                    CoverageExpires = HttpContext.Current.Request.Form["CoverageExpires"].ToString();
                    Language = HttpContext.Current.Request.Form["Language"].ToString();
                    Notes = HttpContext.Current.Request.Form["Notes"].ToString();

                    Longitude = HttpContext.Current.Request.Form["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.Form["Latitude"].ToString();

                    DeclarationDoc = HttpContext.Current.Request.Form["DeclarationDoc"].ToString();//29May18
                    DocName = HttpContext.Current.Request.Form["DocName"].ToString();//29May18

                }
                else
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                    ZipCode = HttpContext.Current.Request.QueryString["ZipCode"].ToString();
                    City = HttpContext.Current.Request.QueryString["City"].ToString();
                    VehicleType = HttpContext.Current.Request.QueryString["VehicleType"].ToString();
                    IsInsured = HttpContext.Current.Request.QueryString["IsInsured"].ToString();
                    InsuranceCompany = HttpContext.Current.Request.QueryString["InsuranceCompany"].ToString();
                    CoverageExpires = HttpContext.Current.Request.QueryString["CoverageExpires"].ToString();
                    Language = HttpContext.Current.Request.QueryString["Language"].ToString();
                    Notes = HttpContext.Current.Request.QueryString["Notes"].ToString();

                    Longitude = HttpContext.Current.Request.QueryString["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.QueryString["Latitude"].ToString();

                    DeclarationDoc = HttpContext.Current.Request.QueryString["DeclarationDoc"].ToString();//29May18
                    DocName = HttpContext.Current.Request.QueryString["DocName"].ToString();//29May18
                }

                //29May18
                if (DocName != "" && DeclarationDoc != "")
                {
                    DeclarationDoc = DeclarationDoc.Replace(" ", "+");
                    DocPath = BrokerUtility.SaveDeclarationDocument(DocName, DeclarationDoc, "AutoApp", UserId.ToString());
                }

                User = BrokerWSUtility.SaveAutoInsuranceDetails(UserId, ZipCode, City, VehicleType, IsInsured, InsuranceCompany, CoverageExpires, Language, Notes, Longitude, Latitude, DocPath, DeclarationDoc);

                if (User != 0)
                {
                    dsUserDetails = BrokerWSUtility.GetBrokersList(UserId, ZipCode, City, Language, "Auto", Longitude, Latitude, VehicleType, "0", "0");

                    if (dsUserDetails.Tables.Count > 0)
                    {
                        if (dsUserDetails.Tables[0].Rows.Count > 0)
                        {
                            string SerchForUserId = "";
                            if (strUsersToShowByDefaultInSearchList != "")
                            {
                                SerchForUserId = CheckUserIdInExistingResult(strUsersToShowByDefaultInSearchList, dsUserDetails);
                            }

                            if (SerchForUserId != "")
                            {
                                dsUserDetails = MergeTwoResults(dsUserDetails, SerchForUserId, UserId);
                            }

                            SendUserDetailsForHomeInsurance(dsUserDetails);
                        }
                        else
                        {
                            dsUserDetails = BrokerWSUtility.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsUserDetails.Tables.Count > 0)
                            {
                                if (dsUserDetails.Tables[0].Rows.Count > 0)
                                {
                                    SendUserDetailsForHomeInsurance(dsUserDetails);
                                }
                                else
                                {
                                    dsUserDetails.Tables[0].TableName = "UserDetails";
                                    dsUserDetails.Tables[1].TableName = "ExperienceDetails";
                                    dsUserDetails.Tables[2].TableName = "EducationDetails";
                                    dsUserDetails.Tables[3].TableName = "BrokerContactList";
                                    dsUserDetails.AcceptChanges();

                                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsUserDetails, "DoSaveAutoInsuranceDetails", "true", "null"));
                                }
                            }
                            else
                            {
                                string Message = "";
                                DataSet dsDetails = new DataSet();
                                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveAutoInsuranceDetails", "false", "Error Occured"));
                            }
                        }
                    }
                    else
                    {
                        // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));
                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveAutoInsuranceDetails", "false", "Error Occured"));

                    }
                }
                else
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveAutoInsuranceDetails", "false", "Error Occured"));

                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSaveAutoInsuranceDetails", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveAutoInsuranceDetails()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static DataSet MergeTwoResults(DataSet dsBrokerDetails, string SerchForUserId, int UserId)
        {
            DataSet dsUserDetailsByDefaultList = null;

            SerchForUserId = SerchForUserId.TrimStart(',');
            dsUserDetailsByDefaultList = BrokerWebDB.BrokerWebDB.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

            if (dsUserDetailsByDefaultList.Tables.Count > 0)
            {
                if (dsUserDetailsByDefaultList.Tables[0].Rows.Count > 0)
                {
                    dsBrokerDetails.Tables[0].Merge(dsUserDetailsByDefaultList.Tables[0]);
                    dsBrokerDetails.AcceptChanges();
                }
                if (dsUserDetailsByDefaultList.Tables[1].Rows.Count > 0)
                {
                    dsBrokerDetails.Tables[1].Merge(dsUserDetailsByDefaultList.Tables[1]);
                    dsBrokerDetails.AcceptChanges();
                }
                if (dsUserDetailsByDefaultList.Tables[2].Rows.Count > 0)
                {
                    dsBrokerDetails.Tables[2].Merge(dsUserDetailsByDefaultList.Tables[2]);
                    dsBrokerDetails.AcceptChanges();
                }
            }

            return dsBrokerDetails;
        }

        public static string CheckUserIdInExistingResult(string strUsersToShowByDefaultInSearchList, DataSet dsBrokerDetails)
        {
            string SerchForUserId = "";

            string[] defaultUserList = strUsersToShowByDefaultInSearchList.Split(',');

            for (int i = 0; i < defaultUserList.Length; i++)
            {
                bool flag = false;
                for (int j = 0; j < dsBrokerDetails.Tables[0].Rows.Count; j++)
                {
                    if (defaultUserList[i].ToString() == dsBrokerDetails.Tables[0].Rows[j]["UserId"].ToString())
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag == false)
                {
                    SerchForUserId = SerchForUserId + "," + defaultUserList[i].ToString();
                }
            }

            return SerchForUserId;
        }

        public static void DoSaveHomeInsuranceDetails(string WSFlag)
        {
            DataSet dsUserDetails = null;

            int User, UserId = 0;
            string ZipCode = "", City = "", EstimatedValue = "", IsInsured = "", CompanyName = "",
                CoverageExpires = "", Language = "", Notes = "", Longitude = "", Latitude = "",
                DeclarationDoc = "", DocName = "", DocPath = "";//29May18

            try
            {
                if (WSFlag == "Application")
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                    ZipCode = HttpContext.Current.Request.Form["ZipCode"].ToString();
                    City = HttpContext.Current.Request.Form["City"].ToString();
                    EstimatedValue = HttpContext.Current.Request.Form["EstimatedValue"].ToString();
                    IsInsured = HttpContext.Current.Request.Form["IsInsured"].ToString();
                    CompanyName = HttpContext.Current.Request.Form["CompanyName"].ToString();
                    CoverageExpires = HttpContext.Current.Request.Form["CoverageExpires"].ToString();
                    Language = HttpContext.Current.Request.Form["Language"].ToString();
                    Notes = HttpContext.Current.Request.Form["Notes"].ToString();

                    Longitude = HttpContext.Current.Request.Form["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.Form["Latitude"].ToString();

                    DeclarationDoc = HttpContext.Current.Request.Form["DeclarationDoc"].ToString();//29May18
                    DocName = HttpContext.Current.Request.Form["DocName"].ToString();//29May18
                }
                else
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                    ZipCode = HttpContext.Current.Request.QueryString["ZipCode"].ToString();
                    City = HttpContext.Current.Request.QueryString["City"].ToString();
                    EstimatedValue = HttpContext.Current.Request.QueryString["EstimatedValue"].ToString();
                    IsInsured = HttpContext.Current.Request.QueryString["IsInsured"].ToString();
                    CompanyName = HttpContext.Current.Request.QueryString["CompanyName"].ToString();
                    CoverageExpires = HttpContext.Current.Request.QueryString["CoverageExpires"].ToString();
                    Language = HttpContext.Current.Request.QueryString["Language"].ToString();
                    Notes = HttpContext.Current.Request.QueryString["Notes"].ToString();

                    DeclarationDoc = HttpContext.Current.Request.QueryString["DeclarationDoc"].ToString();//29May18
                    DocName = HttpContext.Current.Request.QueryString["DocName"].ToString();//29May18
                }

                //29May18
                if (DocName != "" && DeclarationDoc != "")
                {
                    DeclarationDoc = DeclarationDoc.Replace(" ", "+");
                    DocPath = BrokerUtility.SaveDeclarationDocument(DocName, DeclarationDoc, "HomeApp", UserId.ToString());
                }

                User = BrokerWSUtility.SaveHomeInsuranceDetails(UserId, ZipCode, City, EstimatedValue, IsInsured, CompanyName, CoverageExpires, Language, Notes, Longitude, Latitude, DocPath, DeclarationDoc);

                if (User != 0)
                {
                    dsUserDetails = BrokerWSUtility.GetBrokersList(UserId, ZipCode, City, Language, "Home", Longitude, Latitude, EstimatedValue, "0", "0");

                    if (dsUserDetails.Tables.Count > 0)
                    {
                        if (dsUserDetails.Tables[0].Rows.Count > 0)
                        {
                            string SerchForUserId = "";
                            if (strUsersToShowByDefaultInSearchList != "")
                            {
                                SerchForUserId = CheckUserIdInExistingResult(strUsersToShowByDefaultInSearchList, dsUserDetails);
                            }

                            if (SerchForUserId != "")
                            {
                                dsUserDetails = MergeTwoResults(dsUserDetails, SerchForUserId, UserId);
                            }

                            SendUserDetailsForHomeInsurance(dsUserDetails);
                        }
                        else
                        {
                            dsUserDetails = BrokerWSUtility.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsUserDetails.Tables.Count > 0)
                            {
                                if (dsUserDetails.Tables[0].Rows.Count > 0)
                                {
                                    SendUserDetailsForHomeInsurance(dsUserDetails);
                                }
                                else
                                {
                                    dsUserDetails.Tables[0].TableName = "UserDetails";
                                    dsUserDetails.Tables[1].TableName = "ExperienceDetails";
                                    dsUserDetails.Tables[2].TableName = "EducationDetails";
                                    dsUserDetails.Tables[3].TableName = "BrokerContactList";
                                    dsUserDetails.AcceptChanges();

                                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsUserDetails, "DoSaveHomeInsuranceDetails", "true", "null"));
                                }
                            }
                            else
                            {
                                string Message = "";
                                DataSet dsDetails = new DataSet();
                                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveHomeInsuranceDetails", "false", "Error Occured"));
                            }
                        }
                    }
                    else
                    {
                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveHomeInsuranceDetails", "false", "Error Occured"));
                    }
                }
                else
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveHomeInsuranceDetails", "false", "Error Occured"));

                }
            }

            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSaveHomeInsuranceDetails", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveHomeInsuranceDetails()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static void SendUserDetailsForHomeInsurance(DataSet dsUserDetails)
        {
            dsUserDetails.Tables[0].TableName = "UserDetails";
            dsUserDetails.Tables[1].TableName = "ExperienceDetails";
            dsUserDetails.Tables[2].TableName = "EducationDetails";
            dsUserDetails.Tables[3].TableName = "BrokerContactList";
            dsUserDetails.AcceptChanges();

            for (int i = 0; i < dsUserDetails.Tables[0].Rows.Count; i++)
            {
                string binData = dsUserDetails.Tables[0].Rows[i]["ProfilePicture"].ToString();
                if (binData != "")
                {
                    binData = strDomainName + "" + strProfilePicForlderName + "" + dsUserDetails.Tables[0].Rows[i]["ProfilePicture"].ToString();

                    dsUserDetails.Tables[0].Rows[i]["ProfilePicture"] = binData;
                }

                string ProfilePicImg = dsUserDetails.Tables[0].Rows[i]["ProfilePictureImg"].ToString();
                if (ProfilePicImg != "")
                {
                    ProfilePicImg = strDomainName + "" + strProfilePicImageFolder + "" + dsUserDetails.Tables[0].Rows[i]["ProfilePictureImg"].ToString();

                    dsUserDetails.Tables[0].Rows[i]["ProfilePictureImg"] = ProfilePicImg;
                }

                string CompanyLogo = dsUserDetails.Tables[0].Rows[i]["CompanyLogo"].ToString();
                if (CompanyLogo != "")
                {
                    CompanyLogo = strDomainName + "" + strUploadedCompLogoFolder + "" + dsUserDetails.Tables[0].Rows[i]["CompanyLogo"].ToString();

                    dsUserDetails.Tables[0].Rows[i]["CompanyLogo"] = CompanyLogo;
                }
            }

            if (dsUserDetails.Tables["ExperienceDetails"].Rows.Count > 0)
            {
                for (int i = 0; i < dsUserDetails.Tables["ExperienceDetails"].Rows.Count; i++)
                {
                    string Logo = dsUserDetails.Tables["ExperienceDetails"].Rows[i]["Logo"].ToString();
                    if (Logo != "")
                    {
                        Logo = strDomainName + "" + strExperienceCompLogoFolder + "" + dsUserDetails.Tables["ExperienceDetails"].Rows[i]["Logo"].ToString(); ;

                        dsUserDetails.Tables["ExperienceDetails"].Rows[i]["Logo"] = Logo;
                    }
                    else
                    {

                    }
                }
            }

            /******************************Add Server path to School logo ********************************/
            if (dsUserDetails.Tables["EducationDetails"].Rows.Count > 0)
            {
                for (int i = 0; i < dsUserDetails.Tables["EducationDetails"].Rows.Count; i++)
                {
                    string Logo = dsUserDetails.Tables["EducationDetails"].Rows[i]["EducationLogo"].ToString();
                    if (Logo != "")
                    {
                        Logo = strDomainName + "" + strEducationLogoFolder + "" + dsUserDetails.Tables["EducationDetails"].Rows[i]["EducationLogo"].ToString(); ;

                        dsUserDetails.Tables["EducationDetails"].Rows[i]["EducationLogo"] = Logo;
                    }
                }
            }
            /******************************End of Add Server path to School logo ********************************/

            HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsUserDetails, "DoSaveHomeInsuranceDetails", "true", "null"));
        }


        public static void DoGetBrokerAvailabilityStatus(string WSFlag)
        {
            int UserId;
            DataSet dsBrokerAvialability = null;
            try
            {
                if (WSFlag == "Application")
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                }
                else
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                }

                dsBrokerAvialability = BrokerWSUtility.GetBrokerAvailabilityStatus(UserId);

                if (dsBrokerAvialability.Tables.Count > 0)
                {
                    if (dsBrokerAvialability.Tables[0].Rows.Count > 0)
                    {

                        dsBrokerAvialability.Tables[0].TableName = "Response";
                        dsBrokerAvialability.AcceptChanges();

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsBrokerAvialability, "DoGetBrokerAvailabilityStatus", "true", "null"));
                    }
                    else
                    {
                        //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));
                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetBrokerAvailabilityStatus", "false", "Error Occured"));

                    }
                }
                else
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetBrokerAvailabilityStatus", "false", "Error Occured"));

                }

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoGetBrokerAvailabilityStatus", Ex.Message.ToString(), "BrokerWSDB.cs_DoGetBrokerAvailabilityStatus()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static void DoSetBrokerAvailabilityStatus(string WSFlag)
        {
            int UserId;
            bool Availability = false;
            string longitude = "", latitude = "";

            DataSet dsBrokerAvialability = null;
            int Status = 0;
            try
            {
                if (WSFlag == "Application")
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                    Availability = Convert.ToBoolean(HttpContext.Current.Request.Form["Availability"]);

                    //Change on 19Sep2016
                    /*
                    if (Availability == true)
                    {
                        longitude = HttpContext.Current.Request.Form["longitude"];
                        latitude = HttpContext.Current.Request.Form["latitude"];
                    }*/


                }
                else
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                    Availability = Convert.ToBoolean(HttpContext.Current.Request.QueryString["Availability"]);

                    //Change on 19Sep2016
                    /*
                    if (Availability == true)
                    {
                        longitude = HttpContext.Current.Request.QueryString["longitude"];
                        latitude = HttpContext.Current.Request.QueryString["latitude"];
                    }*/
                }

                Status = BrokerWSUtility.SetBrokerAvailabilityStatus(UserId, Availability, longitude, latitude);

                if (Status > 0)
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForSuccess("true", "Availability Updated"));

                    DataTable dtDetails = new DataTable();
                    DataSet dsDetails = new DataSet();

                    dtDetails.Columns.Add("Message");

                    dtDetails.Rows.Add("Availability Updated");

                    dsDetails.Tables.Add(dtDetails);
                    dsDetails.Tables[0].TableName = "Response";
                    dsDetails.AcceptChanges();

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSetBrokerAvailabilityStatus", "true", "null"));
                }
                else
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSetBrokerAvailabilityStatus", "false", "Error Occured"));


                }

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSetBrokerAvailabilityStatus", Ex.Message.ToString(), "BrokerWSDB.cs_DoSetBrokerAvailabilityStatus()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static void DoSetBrokerAvailabilityWithZipCode(string WSFlag)
        {
            int UserId;
            bool Availability = false;
            string longitude = "", latitude = "", ZipCode = "";

            DataSet dsBrokerAvialability = null;
            DataSet dsUserDetails = null;
            int Status = 0;
            try
            {
                if (WSFlag == "Application")
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                    Availability = Convert.ToBoolean(HttpContext.Current.Request.Form["Availability"]);

                    //Change on 19Sep2016
                    /*
                    if (Availability == true)
                    {
                        longitude = HttpContext.Current.Request.Form["longitude"];
                        latitude = HttpContext.Current.Request.Form["latitude"];
                        ZipCode = HttpContext.Current.Request.Form["ZipCode"];
                    }*/


                }
                else
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                    Availability = Convert.ToBoolean(HttpContext.Current.Request.QueryString["Availability"]);

                    //Change on 19Sep2016
                    /*
                    if (Availability == true)
                    {
                        longitude = HttpContext.Current.Request.QueryString["longitude"];
                        latitude = HttpContext.Current.Request.QueryString["latitude"];
                        ZipCode = HttpContext.Current.Request.QueryString["ZipCode"];
                    }*/
                }

                dsUserDetails = BrokerWSUtility.SetBrokerAvailabilityWithZipCode(UserId, Availability, longitude, latitude, ZipCode);

                if (dsUserDetails.Tables.Count > 0)
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForSuccess("true", "Availability Updated"));

                    //DataTable dtDetails = new DataTable();
                    //DataSet dsDetails = new DataSet();

                    //dtDetails.Columns.Add("Message");

                    //dtDetails.Rows.Add("Availability Updated");

                    //dsDetails.Tables.Add(dtDetails);
                    //dsDetails.Tables[0].TableName = "Response";
                    //dsDetails.AcceptChanges();

                    //HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSetBrokerAvailabilityStatus", "true", "null"));

                    dsUserDetails.Tables[0].TableName = "UserDetails";
                    dsUserDetails.Tables[1].TableName = "ExperienceDetails";
                    dsUserDetails.Tables[2].TableName = "EducationDetails";
                    dsUserDetails.AcceptChanges();

                    string binData = dsUserDetails.Tables[0].Rows[0]["ProfilePicture"].ToString();
                    if (binData != "")
                    {
                        binData = strDomainName + "" + strProfilePicForlderName + "" + dsUserDetails.Tables[0].Rows[0]["ProfilePicture"].ToString();

                        dsUserDetails.Tables[0].Rows[0]["ProfilePicture"] = binData;
                    }

                    string ResumeData = dsUserDetails.Tables[0].Rows[0]["Resume"].ToString();
                    if (ResumeData != "")
                    {
                        ResumeData = strDomainName + "" + strResumeForlderName + "" + dsUserDetails.Tables[0].Rows[0]["Resume"].ToString(); ;

                        dsUserDetails.Tables[0].Rows[0]["Resume"] = ResumeData;
                    }

                    ////////////////////////////////////////////////////////////
                    string ProfilePicImg = dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString();
                    if (ProfilePicImg != "")
                    {
                        ProfilePicImg = strDomainName + "" + strProfilePicImageFolder + "" + dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString(); ;

                        dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"] = ProfilePicImg;
                    }

                    string ResumeImg = dsUserDetails.Tables[0].Rows[0]["ResumeDoc"].ToString();
                    if (ResumeImg != "")
                    {
                        ResumeImg = strDomainName + "" + strResumeImageFolder + "" + dsUserDetails.Tables[0].Rows[0]["ResumeDoc"].ToString(); ;

                        dsUserDetails.Tables[0].Rows[0]["ResumeDoc"] = ResumeImg;
                    }

                    if (dsUserDetails.Tables["ExperienceDetails"].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsUserDetails.Tables["ExperienceDetails"].Rows.Count; i++)
                        {
                            string Logo = dsUserDetails.Tables["ExperienceDetails"].Rows[i]["Logo"].ToString();
                            if (Logo != "")
                            {
                                Logo = strDomainName + "" + strCompanyLogoFolder + "" + dsUserDetails.Tables["ExperienceDetails"].Rows[i]["Logo"].ToString(); ;

                                dsUserDetails.Tables["ExperienceDetails"].Rows[i]["Logo"] = Logo;
                            }
                        }
                    }

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsUserDetails, "SetBrokerAvailabilityWithZipCode", "true", "null"));

                }
                else
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSetBrokerAvailabilityStatus", "false", "Error Occured"));


                }

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSetBrokerAvailabilityWithZipCode", Ex.Message.ToString(), "BrokerWSDB.cs_DoSetBrokerAvailabilityWithZipCode()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static void DoContactBroker(string WSFlag)
        {

            int User, count = 0, UserId, BrokerId;
            string InsuranceType = "", Note = "", LocalDateTime = "", Company = "", LineType = "";
            DataSet dsBrokerList = null;
            DataSet dsMsgDetails = null;

            try
            {
                if (WSFlag == "Application")
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                    BrokerId = Convert.ToInt32(HttpContext.Current.Request.Form["BrokerId"]);
                    InsuranceType = HttpContext.Current.Request.Form["InsuranceType"];
                    Note = HttpContext.Current.Request.Form["Note"];
                    LocalDateTime = HttpContext.Current.Request.Form["LocalDateTime"];
                    Company = HttpContext.Current.Request.Form["RegisteredFor"];
                    LineType = HttpContext.Current.Request.Form["LineType"];

                }
                else
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                    BrokerId = Convert.ToInt32(HttpContext.Current.Request.QueryString["BrokerId"]);
                    InsuranceType = HttpContext.Current.Request.QueryString["InsuranceType"];
                    Note = HttpContext.Current.Request.QueryString["Note"];
                    LocalDateTime = HttpContext.Current.Request.QueryString["LocalDateTime"];
                    Company = HttpContext.Current.Request.QueryString["RegisteredFor"];
                    LineType = HttpContext.Current.Request.QueryString["LineType"];
                }

                //Insert Data in UserBroker table for storing which customer contacted which broker for
                //what type of Insurance


                User = BrokerWSUtility.ContactBroker(UserId, BrokerId, InsuranceType);


                if (User != 0)
                {
                    //Send contact messages to both Customer and Broker.
                    //Enter message details in BrokerMessages and CustomerMessages tables

                    if (Company == "Meineke" || Company == "APSP")
                    {
                        if (LineType == "Personal")
                        {
                            count = BrokerWSUtility.SendMessagesForMeinekePersonal(UserId, BrokerId, InsuranceType, Note, LocalDateTime);
                        }
                        else if (LineType == "Commercial")
                        {
                            count = BrokerWSUtility.SendMessagesForMeineke(UserId, BrokerId, InsuranceType, Note, LocalDateTime);
                        }
                        else if (LineType == "401k")
                        {
                            count = BrokerWSUtility.SendMessagesForMeineke(UserId, BrokerId, InsuranceType, Note, LocalDateTime);
                        }

                    }
                    else if (Company == "Brokkrr")
                    {
                        if (LineType == "401k")
                        {
                            count = BrokerWSUtility.SendMessagesForMeineke(UserId, BrokerId, InsuranceType, Note, LocalDateTime);
                        }
                        else if (LineType == "Commercial" && InsuranceType == "Workers compensation")
                        {
                            count = BrokerWSUtility.SendMessagesForMeineke(UserId, BrokerId, InsuranceType, Note, LocalDateTime);
                        }
                        else
                        {
                            count = BrokerWSUtility.SendMessages(UserId, BrokerId, InsuranceType, Note, LocalDateTime);
                        }
                    }

                    // count = BrokerWSUtility.SendMessages(UserId, BrokerId, InsuranceType, Note, LocalDateTime);

                    if (count != 0)
                    {
                        dsMsgDetails = BrokerWebDB.BrokerWebDB.getCustomerMessageDetails(count);
                        if (dsMsgDetails.Tables.Count > 0)
                        {
                            if (dsMsgDetails.Tables[0].Rows.Count > 0)
                            {
                                DataSet dt = BrokerWebDB.BrokerWebDB.GetMessageDetails(dsMsgDetails.Tables[0].Rows[0]["CustMsgId"].ToString(), dsMsgDetails.Tables[0].Rows[0]["BrokerMsgId"].ToString());

                                string BrokerMessage = dt.Tables[0].Rows[0]["Message"].ToString();
                                string DeclarationDocPath = dt.Tables[0].Rows[0]["DeclarationDocPath"].ToString();

                                if (DeclarationDocPath != "" && DeclarationDocPath != null)
                                {
                                    string DocPath = strDomainName + strDeclarationDocumentFolder + DeclarationDocPath;
                                    BrokerMessage = BrokerMessage.Replace("Please reply back if you are interested.", "Declaration Document - <a style='text-decoration: underline;' href=" + DocPath + " download>Click here to download</a><br/>Please reply back from brokkrr app if you are interested.");
                                }
                                else
                                {
                                    BrokerMessage = BrokerMessage.Replace("Please reply back if you are interested.", "Please reply back from brokkrr app if you are interested.");
                                }

                                //BrokerMessage = BrokerMessage.Replace("Please reply back if you are interested.", "Please reply back from brokkrr app if you are interested.");

                                List<spGetUserDetails_Result> oUserDetails = BrokerUtility.GetUserDetails(Convert.ToInt32(BrokerId));

                                if (oUserDetails.Count > 0)
                                {
                                    bool flag = BrokerWSUtility.SendContactMessageToBrokerOnEmail(Convert.ToInt32(BrokerId), InsuranceType, "", oUserDetails[0].EmailId, BrokerMessage);
                                }
                            }
                        }

                        //Get already contacted broker list.
                        dsBrokerList = BrokerWSUtility.GetContactedBrokerList(UserId);

                        if (dsBrokerList.Tables.Count > 0)
                        {
                            dsBrokerList.Tables[0].TableName = "BrokerContactList";
                            dsBrokerList.AcceptChanges();

                            HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsBrokerList, "DoContactBroker", "true", "null"));
                        }
                        else
                        {
                            //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                            string Message = "";
                            DataSet dsDetails = new DataSet();
                            dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                            HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoContactBroker", "false", "Error Occured"));

                        }

                        //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForSuccess("true", "Contacted"));
                    }
                    else
                    {
                        //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoContactBroker", "false", "Error Occured"));

                    }
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoContactBroker", Ex.Message.ToString(), "BrokerWSDB.cs_DoContactBroker()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }

        }

        public static void DoGetMessages(string WSFlag)
        {

            int User, UserId;
            DataSet dsContactList = null;
            string TimeStamp = "", UserType = "";
            try
            {
                if (WSFlag == "Application")
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                    TimeStamp = HttpContext.Current.Request.Form["TimeStamp"];
                }
                else
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                    TimeStamp = HttpContext.Current.Request.QueryString["TimeStamp"];
                }

                //Get the Contacted list for both Broker and Customer.
                //Data come from BrokerMessages and CustomerMessages tables.
                dsContactList = BrokerWSUtility.GetContactedList(UserId, TimeStamp);

                if (dsContactList.Tables.Count > 0)
                {
                    if (dsContactList.Tables[0].Rows.Count > 0)
                    {
                        dsContactList.Tables[0].TableName = "ContactedMessageList";
                        dsContactList.AcceptChanges();

                        List<spGetUserDetails_Result> oUserDetails = null;
                        oUserDetails = BrokerUtility.GetUserDetails(Convert.ToInt32(UserId));

                        UserType = oUserDetails[0].UserType;

                        for (int i = 0; i < dsContactList.Tables[0].Rows.Count; i++)
                        {
                            string binData = dsContactList.Tables[0].Rows[i]["ProfilePicture"].ToString();
                            if (binData != "")
                            {
                                binData = strDomainName + "" + strProfilePicForlderName + "" + dsContactList.Tables[0].Rows[i]["ProfilePicture"].ToString(); ;

                                dsContactList.Tables[0].Rows[i]["ProfilePicture"] = binData;
                            }

                            string ProfilePicImg = dsContactList.Tables[0].Rows[i]["ProfilePictureImg"].ToString();
                            if (ProfilePicImg != "")
                            {
                                ProfilePicImg = strDomainName + "" + strProfilePicImageFolder + "" + dsContactList.Tables[0].Rows[i]["ProfilePictureImg"].ToString(); ;

                                dsContactList.Tables[0].Rows[i]["ProfilePictureImg"] = ProfilePicImg;
                            }

                            if (UserType == "Broker")
                            {
                                if (dsContactList.Tables[0].Rows[i]["Message"].ToString() != "" && dsContactList.Tables[0].Rows[i]["DeclarationDocPath"].ToString() != "")
                                {
                                    string Message = dsContactList.Tables[0].Rows[i]["Message"].ToString();
                                    string DocPath = strDomainName + strDeclarationDocumentFolder + dsContactList.Tables[0].Rows[i]["DeclarationDocPath"].ToString();

                                    Message = Message.Replace("Please reply back if you are interested.", "Declaration Document - <span id='DeclarationDoc' style='text-decoration: underline;cursor:pointer;' >Click here to view</span><br/>Please reply back if you are interested.");

                                    dsContactList.Tables[0].Rows[i]["DeclarationDocPath"] = DocPath;
                                    dsContactList.Tables[0].Rows[i]["Message"] = Message;
                                    dsContactList.AcceptChanges();
                                }
                            }
                        }

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsContactList, "DoReadMessages", "true", "null"));
                    }
                    else
                    {
                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        //dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);
                        dsContactList.Tables[0].TableName = "ContactedMessageList";
                        dsContactList.AcceptChanges();

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsContactList, "DoReadMessages", "true", "null"));
                    }
                }
                else
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);
                    //dsDetails.Tables[0].TableName = "ContactedMessageList";
                    //dsDetails.AcceptChanges();

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoReadMessages", "false", "Error Occured"));

                }

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoGetMessages", Ex.Message.ToString(), "BrokerWSDB.cs_DoGetMessages()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);
                //dsDetails.Tables[0].TableName = "ContactedMessageList";
                //dsDetails.AcceptChanges();

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static void DoSaveBrokerChat(string WSFlag)
        {
            int BrokerMsgId, BrokerId, CustomerId, OldMessageId, CustomerMsgId;
            List<uspSaveBrokerChat_Result> user = null;
            string BrokerMessage = "", LocalDateTime = "";
            DataSet dsBrokerList = null;

            try
            {
                if (WSFlag == "Application")
                {
                    BrokerMsgId = Convert.ToInt32(HttpContext.Current.Request.Form["BrokerMsgId"]);
                    CustomerMsgId = Convert.ToInt32(HttpContext.Current.Request.Form["CustomerMsgId"]);
                    BrokerId = Convert.ToInt32(HttpContext.Current.Request.Form["BrokerId"]);
                    CustomerId = Convert.ToInt32(HttpContext.Current.Request.Form["CustomerId"]);
                    BrokerMessage = HttpContext.Current.Request.Form["BrokerMessage"];
                    OldMessageId = Convert.ToInt32(HttpContext.Current.Request.Form["OldMessageId"]);
                    LocalDateTime = HttpContext.Current.Request.Form["LocalDateTime"];
                }
                else
                {
                    BrokerMsgId = Convert.ToInt32(HttpContext.Current.Request.QueryString["BrokerMsgId"]);
                    CustomerMsgId = Convert.ToInt32(HttpContext.Current.Request.QueryString["CustomerMsgId"]);
                    BrokerId = Convert.ToInt32(HttpContext.Current.Request.QueryString["BrokerId"]);
                    CustomerId = Convert.ToInt32(HttpContext.Current.Request.QueryString["CustomerId"]);
                    BrokerMessage = HttpContext.Current.Request.QueryString["BrokerMessage"];
                    OldMessageId = Convert.ToInt32(HttpContext.Current.Request.QueryString["OldMessageId"]);
                    LocalDateTime = HttpContext.Current.Request.QueryString["LocalDateTime"];
                }

                //Insert Data in CustomerBrokerChat table - i.e. message send from Broker to Customer
                //int user1 = 0;
                user = BrokerWSUtility.SaveBrokerChat(BrokerMsgId, BrokerId, CustomerId, BrokerMessage, CustomerMsgId, LocalDateTime);
                if (user.Count > 0)
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForSuccess("true", ""));

                    //Send Email of Chat Message to broker

                    if (strSentMailOnChatMessage == "true")
                    {
                        List<spGetUserDetails_Result> oBrokerDetails = BrokerUtility.GetUserDetails(Convert.ToInt32(BrokerId));

                        List<spGetUserDetails_Result> oCustDetails = BrokerUtility.GetUserDetails(Convert.ToInt32(CustomerId));

                        if (oCustDetails.Count > 0)
                        {
                            if (oBrokerDetails.Count > 0)
                            {
                                string BrokerName = oBrokerDetails[0].FirstName + " " + oBrokerDetails[0].LastName;
                                bool flag = BrokerWSUtility.SendChatMessageOnEmail(BrokerName, oCustDetails[0].EmailId, BrokerMessage);
                            }

                        }
                    }

                    DataTable dtDetails = new DataTable();
                    DataSet dsDetails = new DataSet();

                    dtDetails.Columns.Add("Message");
                    dtDetails.Columns.Add("OldMessageId");
                    dtDetails.Columns.Add("NewMessageId");
                    dtDetails.Columns.Add("MessageDate");
                    dtDetails.Columns.Add("LocalDateTime");

                    dtDetails.Rows.Add("Saved", OldMessageId, user[0].Id.ToString(), user[0].MessageDate.ToString(), LocalDateTime);

                    dsDetails.Tables.Add(dtDetails);
                    dsDetails.Tables[0].TableName = "Response";
                    dsDetails.AcceptChanges();

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBrokerChat", "true", "null"));
                }
                else
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBrokerChat", "false", "Error Occured"));
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSaveBrokerChat", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveBrokerChat()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static void DoSaveCustomerChat(string WSFlag)
        {
            int CustMsgId, CustomerId, BrokerId, OldMessageId, BrokerMsgId;
            string CustomerMessage = "", LocalDateTime = ""; ;
            DataSet dsBrokerList = null;

            List<uspSaveCustomerChat_Result> User = null;

            try
            {
                if (WSFlag == "Application")
                {
                    CustMsgId = Convert.ToInt32(HttpContext.Current.Request.Form["CustMsgId"]);
                    BrokerMsgId = Convert.ToInt32(HttpContext.Current.Request.Form["BrokerMsgId"]);
                    CustomerId = Convert.ToInt32(HttpContext.Current.Request.Form["CustomerId"]);
                    BrokerId = Convert.ToInt32(HttpContext.Current.Request.Form["BrokerId"]);
                    CustomerMessage = HttpContext.Current.Request.Form["CustomerMessage"];
                    OldMessageId = Convert.ToInt32(HttpContext.Current.Request.Form["OldMessageId"]);
                    LocalDateTime = HttpContext.Current.Request.Form["LocalDateTime"];
                }
                else
                {
                    CustMsgId = Convert.ToInt32(HttpContext.Current.Request.QueryString["CustMsgId"]);
                    BrokerMsgId = Convert.ToInt32(HttpContext.Current.Request.QueryString["BrokerMsgId"]);
                    CustomerId = Convert.ToInt32(HttpContext.Current.Request.QueryString["CustomerId"]);
                    BrokerId = Convert.ToInt32(HttpContext.Current.Request.QueryString["BrokerId"]);
                    CustomerMessage = HttpContext.Current.Request.QueryString["CustomerMessage"];
                    OldMessageId = Convert.ToInt32(HttpContext.Current.Request.QueryString["OldMessageId"]);
                    LocalDateTime = HttpContext.Current.Request.QueryString["LocalDateTime"];
                }

                //Insert Data in CustomerBrokerChat table - i.e. message send from Customer to Broker 

                User = BrokerWSUtility.SaveCustomerChat(CustMsgId, CustomerId, BrokerId, CustomerMessage, BrokerMsgId, LocalDateTime);
                if (User.Count > 0)
                {
                    //Send Email of Chat Message to broker

                    if (strSentMailOnChatMessage == "true")
                    {
                        List<spGetUserDetails_Result> oBrokerDetails = BrokerUtility.GetUserDetails(Convert.ToInt32(BrokerId));

                        List<spGetUserDetails_Result> oCustDetails = BrokerUtility.GetUserDetails(Convert.ToInt32(CustomerId));

                        if (oBrokerDetails.Count > 0)
                        {
                            if (oCustDetails.Count > 0)
                            {
                                string CustomerName = oCustDetails[0].FirstName + " " + oCustDetails[0].LastName;
                                bool flag = BrokerWSUtility.SendChatMessageOnEmail(CustomerName, oBrokerDetails[0].EmailId, CustomerMessage);
                            }

                        }
                    }

                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForSuccess("true", ""));
                    DataTable dtDetails = new DataTable();
                    DataSet dsDetails = new DataSet();

                    dtDetails.Columns.Add("Message");
                    dtDetails.Columns.Add("OldMessageId");
                    dtDetails.Columns.Add("NewMessageId");
                    dtDetails.Columns.Add("MessageDate");
                    dtDetails.Columns.Add("LocalDateTime");

                    dtDetails.Rows.Add("Saved", OldMessageId, User[0].Id.ToString(), User[0].MessageDate.ToString(), LocalDateTime);

                    dsDetails.Tables.Add(dtDetails);
                    dsDetails.Tables[0].TableName = "Response";
                    dsDetails.AcceptChanges();

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveCustomerChat", "true", "null"));

                }
                else
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveCustomerChat", "false", "Error Occured"));

                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSaveCustomerChat", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveCustomerChat()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        //public static void DoGetChatMessages(string WSFlag)
        //{
        //    int MessageId, UserId;
        //    string IsRead;
        //    DataSet dsChatDetails = null;

        //    try
        //    {
        //        if (WSFlag == "Application")
        //        {
        //            MessageId = Convert.ToInt32(HttpContext.Current.Request.Form["MessageId"]);
        //            UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
        //            IsRead = HttpContext.Current.Request.Form["IsRead"];
        //        }
        //        else
        //        {
        //            MessageId = Convert.ToInt32(HttpContext.Current.Request.QueryString["MessageId"]);
        //            UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
        //            IsRead = HttpContext.Current.Request.QueryString["IsRead"];
        //        }

        //        dsChatDetails = BrokerWSUtility.GetChatMessages(MessageId, UserId, IsRead.ToLower());
        //        //dsChatDetails = BrokerWSUtility.GetChatMessages(UserId, IsRead.ToLower());

        //        if (dsChatDetails.Tables.Count > 0)
        //        {
        //            if (dsChatDetails.Tables[0].Rows.Count > 0)
        //            {
        //                dsChatDetails.Tables[0].TableName = "ChatMessages";
        //                dsChatDetails.AcceptChanges();

        //                for (int i = 0; i < dsChatDetails.Tables[0].Rows.Count; i++)
        //                {
        //                    string binData = dsChatDetails.Tables[0].Rows[i]["ProfilePicture"].ToString();
        //                    if (binData != "")
        //                    {
        //                        binData = strDomainName + "" + strProfilePicForlderName + "" + dsChatDetails.Tables[0].Rows[i]["ProfilePicture"].ToString(); ;

        //                        dsChatDetails.Tables[0].Rows[i]["ProfilePicture"] = binData;
        //                    }
        //                }

        //                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsChatDetails, "DoGetChatMessages", "true", "null"));
        //            }
        //            else
        //            {
        //                dsChatDetails.Tables[0].TableName = "ChatMessages";
        //                dsChatDetails.AcceptChanges();

        //                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsChatDetails, "DoGetChatMessages", "true", "null"));
        //            }
        //        }

        //        else
        //        {
        //            HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsChatDetails, "DoGetChatMessages", "true", "null"));
        //        }

        //    }
        //    catch (Exception Ex)
        //    {
        //        BrokerUtility.ErrorLog(0, "DoGetChatMessages", Ex.Message.ToString(), "BrokerWSDB.cs_DoGetChatMessages()", "");
        //    }
        //}

        public static void DoGetChatMessages(string WSFlag)
        {
            int MessageId, UserId;
            string IsRead = "", TimeSpan = "";
            DataSet dsChatDetails = null;

            try
            {
                if (WSFlag == "Application")
                {
                    //MessageId = Convert.ToInt32(HttpContext.Current.Request.Form["MessageId"]);
                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                    TimeSpan = HttpContext.Current.Request.Form["TimeSpan"];
                }
                else
                {
                    //MessageId = Convert.ToInt32(HttpContext.Current.Request.QueryString["MessageId"]);
                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                    TimeSpan = HttpContext.Current.Request.QueryString["TimeSpan"];
                }

                //dsChatDetails = BrokerWSUtility.GetChatMessages(MessageId, UserId, IsRead.ToLower());
                dsChatDetails = BrokerWSUtility.GetChatMessages(UserId, TimeSpan);

                if (dsChatDetails.Tables.Count > 0)
                {
                    if (dsChatDetails.Tables[0].Rows.Count > 0)
                    {
                        dsChatDetails.Tables[0].TableName = "ChatMessages";
                        dsChatDetails.AcceptChanges();

                        //for (int i = 0; i < dsChatDetails.Tables[0].Rows.Count; i++)
                        //{
                        //    string binData = dsChatDetails.Tables[0].Rows[i]["ProfilePicture"].ToString();
                        //    if (binData != "")
                        //    {
                        //        binData = strDomainName + "" + strProfilePicForlderName + "" + dsChatDetails.Tables[0].Rows[i]["ProfilePicture"].ToString(); ;

                        //        dsChatDetails.Tables[0].Rows[i]["ProfilePicture"] = binData;
                        //    }

                        //    string ProfilePicImg = dsChatDetails.Tables[0].Rows[i]["ProfilePictureImg"].ToString();
                        //    if (ProfilePicImg != "")
                        //    {
                        //        ProfilePicImg = strDomainName + "" + strProfilePicImageFolder + "" + dsChatDetails.Tables[0].Rows[i]["ProfilePictureImg"].ToString(); ;

                        //        dsChatDetails.Tables[0].Rows[i]["ProfilePictureImg"] = ProfilePicImg;
                        //    }
                        //}

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsChatDetails, "DoGetChatMessages", "true", "null"));
                    }
                    else
                    {
                        dsChatDetails.Tables[0].TableName = "ChatMessages";
                        dsChatDetails.AcceptChanges();

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsChatDetails, "DoGetChatMessages", "true", "null"));
                    }
                }

                else
                {
                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsChatDetails, "DoGetChatMessages", "true", "null"));
                }

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoGetChatMessages", Ex.Message.ToString(), "BrokerWSDB.cs_DoGetChatMessages()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static void DoForgetPassword(string WSFlag)
        {
            string EmailId = "";
            bool EmailFlag = false;
            int Count;
            DataSet dsValidEmail = null;

            try
            {
                if (WSFlag == "Application")
                {
                    EmailId = HttpContext.Current.Request.Form["EmailId"];
                }
                else
                {
                    EmailId = HttpContext.Current.Request.QueryString["EmailId"];
                }

                dsValidEmail = BrokerWSUtility.CheckForValidEmailId(EmailId);

                if (dsValidEmail.Tables.Count > 0)
                {
                    if (dsValidEmail.Tables[0].Rows.Count > 0)
                    {
                        if (dsValidEmail.Tables[0].Rows[0][0].ToString() != "0")
                        {
                            //send Reset Password link to user.

                            string Random = BrokerWSUtility.GetRandomNumber();
                            string EncryptRandom = BrokerUtility.EncryptURL(Random);

                            int Flag = BrokerWSUtility.ForgetPasswordRanNum(EmailId, EncryptRandom);

                            EmailFlag = BrokerWSUtility.SendForgetPasswordEmail(EmailId, EncryptRandom);
                            if (EmailFlag)
                            {
                                //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForSuccess("true", "Mail Sent"));

                                DataTable dtDetails = new DataTable();
                                DataSet dsDetails = new DataSet();

                                dtDetails.Columns.Add("Message");

                                dtDetails.Rows.Add("Mail Sent");

                                dsDetails.Tables.Add(dtDetails);
                                dsDetails.Tables[0].TableName = "Response";
                                dsDetails.AcceptChanges();

                                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoForgetPassword", "true", "null"));

                            }
                            else
                            {
                                //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", "Error For Sending Mail"));

                                string Message = "";
                                DataSet dsDetails = new DataSet();
                                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoForgetPassword", "false", "Error For Sending Mail"));

                            }
                        }
                        else
                        {
                            //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", "Invalid EmailId"));

                            string Message = "";
                            DataSet dsDetails = new DataSet();
                            dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                            HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoForgetPassword", "false", "Invalid EmailId"));

                        }
                    }
                }
                else
                {
                    HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", "Invalid EmailId"));
                }

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoForgetPassword", Ex.Message.ToString(), "BrokerWSDB.cs_DoForgetPassword()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static void DoResetPassword(string WSFlag)
        {
            string EmailId = "", Password = "", TempPass = "";

            int Count;

            try
            {
                if (WSFlag == "Application")
                {
                    EmailId = HttpContext.Current.Request.Form["EmailId"];
                    Password = HttpContext.Current.Request.Form["Password"];
                }
                else
                {
                    EmailId = HttpContext.Current.Request.QueryString["EmailId"];
                    Password = HttpContext.Current.Request.QueryString["Password"];
                }

                TempPass = BrokerUtility.EncryptURL(Password);

                Count = BrokerWSUtility.ResetPassword(EmailId, TempPass);

                if (Count != 0)
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForSuccess("true", "Password Reset"));

                    DataTable dtDetails = new DataTable();
                    DataSet dsDetails = new DataSet();

                    dtDetails.Columns.Add("Message");

                    dtDetails.Rows.Add("Password Reset");

                    dsDetails.Tables.Add(dtDetails);
                    dsDetails.Tables[0].TableName = "Response";
                    dsDetails.AcceptChanges();

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoResetPassword", "true", "null"));
                }
                else
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", "Error"));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoResetPassword", "false", "Error Occured"));

                }

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoResetPassword", Ex.Message.ToString(), "BrokerWSDB.cs_DoResetPassword()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static void DoSaveImage(string WSFlag)
        {
            string UserId = "", Password = "", TempPass = "";

            int Count;

            try
            {
                string imageId = null, data = null;
                HttpContext Context = HttpContext.Current;
                //tsTeam oteam = new tsTeam();
                if (WSFlag == "browser")
                {
                    imageId = HttpContext.Current.Request.QueryString["LogoId"].ToString();
                    data = HttpContext.Current.Request.QueryString["data"].ToString();

                }
                else
                {
                    imageId = HttpContext.Current.Request.Form["LogoId"].ToString();
                    data = HttpContext.Current.Request.Form["data"].ToString();

                }
                string imagename = HttpContext.Current.Server.MapPath("~/Images/").ToString() + imageId.ToString() + ".jpg";
                if (File.Exists(imagename))
                {
                    //oteam.Res = "Already Exists";
                }
                else
                {
                    File.WriteAllBytes(imagename, Convert.FromBase64String(data));
                    bool res = saveImageinDB(imageId);

                    if (res == true)
                    {

                        //oteam.Res = "Success";
                        //Context.Response.Write(tsData.createjsonforImageUpload(oteam));
                    }
                    else
                    {
                        // oteam.Res = "failed";
                        // Context.Response.Write(tsData.createjsonforImageUpload(oteam));
                    }
                }


            }
            catch (Exception ex)
            {
                //throw ex;

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static bool saveImageinDB(string oImage)
        {
            bool response = false;
            try
            {
                //if (strCountry == "US")
                //{
                //    tsData.DBLink = ConfigurationManager.ConnectionStrings["DBUS"].Name;
                //}
                //else
                //{
                //    tsData.DBLink = ConfigurationManager.ConnectionStrings["DBIndia"].Name;
                //}
                //Database db = DatabaseFactory.CreateDatabase(DBLink);
                //DbCommand dbcmd = db.GetStoredProcCommand("spUpdateIntoTeam");
                //db.AddInParameter(dbcmd, "imagepath", DbType.String, oImage.fname);
                //db.AddInParameter(dbcmd, "id", DbType.Int32, Convert.ToInt32(oImage.id));
                //int i = (int)db.ExecuteNonQuery(dbcmd);
                //if (i > 0)
                //{
                //    response = true;
                //}
                //else
                //{
                //    response = false;
                //}




            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public static void DoSetIsRead(string WSFlag)
        {
            try
            {
                string MessageId = "", MainMessageId = "";
                int Result = 0, UserId = 0;
                bool Res = false;

                if (WSFlag == "Application")
                {
                    MessageId = HttpContext.Current.Request.Form["MessageId"].ToString();
                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"].ToString());
                    MainMessageId = HttpContext.Current.Request.Form["MainMessageId"].ToString();
                }
                else
                {
                    MessageId = HttpContext.Current.Request.QueryString["MessageId"].ToString();
                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"].ToString());
                    MainMessageId = HttpContext.Current.Request.QueryString["MainMessageId"].ToString();

                }
                MessageId = MessageId.TrimStart('[');
                MessageId = MessageId.TrimEnd(']');

                //string[] Ids = MessageId.Split(',');

                Result = BrokerWSUtility.SetIsRead(MessageId, UserId, MainMessageId);
                if (Result != 0)
                {
                    Res = true;
                }
                else
                {
                    Res = false;
                }

                if (Res == true)
                {
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("IsRead Flag Set to true");

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSetIsRead", "true", "null"));
                }
                else
                {
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("");

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSetIsRead", "false", "Error Occured"));
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSetIsRead", Ex.Message.ToString(), "BrokerWSDB.cs_DoSetIsRead()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }


        public static void DoGetCurrentTimeSpan(string WSFlag)
        {
            try
            {
                //string TimeSpan = "";
                List<uspGetCurrentTimeSpan_Result> TimeSpan = null;

                TimeSpan = BrokerWSUtility.GetCurrentTimeSpan();

                if (TimeSpan.Count > 0)
                {
                    DataTable dtDetails = new DataTable();
                    DataSet dsDetails = new DataSet();

                    dtDetails.Columns.Add("CurrentTimeSpan");

                    dtDetails.Rows.Add(TimeSpan[0].TimeSpan.ToString());

                    dsDetails.Tables.Add(dtDetails);
                    dsDetails.Tables[0].TableName = "Response";
                    dsDetails.AcceptChanges();

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetCurrentTimeSpan", "true", "null"));
                }
                else
                {
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetCurrentTimeSpan", "false", "Error occured, please try again."));
                }

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoGetCurrentTimeSpan", Ex.Message.ToString(), "BrokerWSDB.cs_DoGetCurrentTimeSpan()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetCurrentTimeSpan", "false", "Error occured, please try again."));
            }
        }


        //For Android Application
        public static void DoGetChatMessagesByMessageId(string WSFlag)
        {
            int MessageId, UserId;
            string IsRead = "", TimeStamp = "";
            DataSet dsChatDetails = null;

            try
            {
                if (WSFlag == "Application")
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                    MessageId = Convert.ToInt32(HttpContext.Current.Request.Form["MessageId"]);
                    TimeStamp = HttpContext.Current.Request.Form["TimeStamp"];

                }
                else
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                    MessageId = Convert.ToInt32(HttpContext.Current.Request.QueryString["MessageId"]);
                    TimeStamp = HttpContext.Current.Request.QueryString["TimeStamp"];
                }

                dsChatDetails = BrokerWSUtility.GetChatMessagesByMessageId(UserId, MessageId, TimeStamp);

                if (dsChatDetails.Tables.Count > 0)
                {
                    if (dsChatDetails.Tables[0].Rows.Count > 0)
                    {
                        dsChatDetails.Tables[0].TableName = "ChatMessages";
                        dsChatDetails.Tables[1].TableName = "IsMessageDeleted";
                        dsChatDetails.AcceptChanges();

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsChatDetails, "DoGetChatMessages", "true", "null"));
                    }
                    else
                    {
                        dsChatDetails.Tables[0].TableName = "ChatMessages";
                        dsChatDetails.Tables[1].TableName = "IsMessageDeleted";
                        dsChatDetails.AcceptChanges();

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsChatDetails, "DoGetChatMessages", "true", "null"));
                    }
                }

                else
                {
                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsChatDetails, "DoGetChatMessages", "true", "null"));
                }

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoGetChatMessagesByMessageId", Ex.Message.ToString(), "BrokerWSDB.cs_DoGetChatMessagesByMessageId()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetChatMessagesByMessageId", "false", "Error occured, please try again."));
            }
        }


        //For Android Application
        public static void DoGetUnreadChatMessages(string WSFlag)
        {
            int MessageId, UserId;
            string IsRead = "", TimeSpan = "";
            DataSet dsChatDetails = null;

            try
            {
                if (WSFlag == "Application")
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                    MessageId = Convert.ToInt32(HttpContext.Current.Request.Form["MessageId"]);

                }
                else
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                    MessageId = Convert.ToInt32(HttpContext.Current.Request.QueryString["MessageId"]);
                }

                dsChatDetails = BrokerWSUtility.GetUnreadChatMessages(UserId, MessageId);

                if (dsChatDetails.Tables.Count > 0)
                {
                    if (dsChatDetails.Tables[0].Rows.Count > 0)
                    {
                        dsChatDetails.Tables[0].TableName = "ChatMessages";
                        dsChatDetails.Tables[1].TableName = "IsMessageDeleted";

                        if (dsChatDetails.Tables[1].Rows.Count == 0)
                        {
                            dsChatDetails.Tables[1].Rows.Add("True");
                        }

                        dsChatDetails.AcceptChanges();



                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsChatDetails, "DoGetChatMessages", "true", "null"));
                    }
                    else
                    {
                        dsChatDetails.Tables[0].TableName = "ChatMessages";
                        dsChatDetails.Tables[1].TableName = "IsMessageDeleted";

                        if (dsChatDetails.Tables[1].Rows.Count == 0)
                        {
                            dsChatDetails.Tables[1].Rows.Add("True");
                        }

                        dsChatDetails.AcceptChanges();

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsChatDetails, "DoGetChatMessages", "true", "null"));
                    }
                }

                else
                {
                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsChatDetails, "DoGetChatMessages", "true", "null"));
                }

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoGetUnreadChatMessages", Ex.Message.ToString(), "BrokerWSDB.cs_DoGetUnreadChatMessages()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static void DoGetCompanyMaster(string WSFlag)
        {
            DataSet dsCompanyMaster = null;
            try
            {
                dsCompanyMaster = BrokerWSUtility.GetCompanyMaster();

                if (dsCompanyMaster.Tables.Count > 0)
                {
                    if (dsCompanyMaster.Tables[0].Rows.Count > 0)
                    {
                        dsCompanyMaster.Tables[0].TableName = "CompanyList";
                        dsCompanyMaster.AcceptChanges();

                        for (int i = 0; i < dsCompanyMaster.Tables["CompanyList"].Rows.Count; i++)
                        {
                            string Logo = dsCompanyMaster.Tables["CompanyList"].Rows[i]["Logo"].ToString();
                            if (Logo != "")
                            {
                                Logo = strDomainName + "" + strCompanyLogoFolder + "" + dsCompanyMaster.Tables["CompanyList"].Rows[i]["Logo"].ToString(); ;

                                dsCompanyMaster.Tables["CompanyList"].Rows[i]["Logo"] = Logo;
                            }
                        }

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsCompanyMaster, "DoGetCompanyMaster", "true", "null"));
                    }
                    else
                    {
                        dsCompanyMaster.Tables[0].TableName = "CompanyList";
                        dsCompanyMaster.AcceptChanges();

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsCompanyMaster, "DoGetCompanyMaster", "true", "null"));
                    }
                }

                else
                {
                    DataTable dtCompanyMaster = new DataTable();
                    dsCompanyMaster.Tables.Add(dtCompanyMaster);
                    dsCompanyMaster.Tables[0].TableName = "CompanyList";
                    dsCompanyMaster.AcceptChanges();

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsCompanyMaster, "DoGetCompanyMaster", "true", "null"));

                    //HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsChatDetails, "DoGetChatMessages", "true", "null"));
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoGetCompanyMaster", Ex.Message.ToString(), "BrokerWSDB.cs_DoGetCompanyMaster()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetCompanyMaster", "false", "Error occured, please try again."));
            }
        }

        public static void DoDeleteMessage(string WSFlag)
        {
            DataSet dsDeleteMessage = null;
            int UserId, MessageId, Result = 0;
            try
            {
                if (WSFlag == "Application")
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                    MessageId = Convert.ToInt32(HttpContext.Current.Request.Form["MessageId"]);

                }
                else
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                    MessageId = Convert.ToInt32(HttpContext.Current.Request.QueryString["MessageId"]);
                }

                Result = BrokerWSUtility.DeleteMessage(UserId, MessageId);

                if (Result != 0)
                {
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("Message Deleted");

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoDeleteMessage", "true", "null"));
                }
                else
                {
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("");

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoDeleteMessage", "false", "Error Occured"));
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoDeleteMessage", Ex.Message.ToString(), "BrokerWSDB.cs_DoDeleteMessage()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoDeleteMessage", "false", "Error occured, please try again."));
            }
        }

        public static void DoDeleteMultipleMessage(string WSFlag)
        {
            DataSet dsDeleteMessage = null;
            int UserId, Result = 0;
            //string UserId1 = "";
            string MessageId = "";
            try
            {
                if (WSFlag == "Application")
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                    //UserId1 = HttpContext.Current.Request.Form["UserId"];
                    //UserId = Convert.ToInt32(UserId1);
                    MessageId = HttpContext.Current.Request.Form["MessageId"];

                }
                else
                {
                    //UserId1 = HttpContext.Current.Request.QueryString["UserId"];
                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                    MessageId = HttpContext.Current.Request.QueryString["MessageId"];
                }

                MessageId = MessageId.TrimStart('[');
                MessageId = MessageId.TrimEnd(']');

                Result = BrokerWSUtility.DeleteMultipleMessage(UserId, MessageId);

                if (Result != 0)
                {
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("Messages Deleted");

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoDeleteMultipleMessage", "true", "null"));
                }
                else
                {
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("");

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoDeleteMultipleMessage", "false", "Error Occured"));
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoDeleteMultipleMessage", Ex.Message.ToString(), "BrokerWSDB.cs_DoDeleteMultipleMessage", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoDeleteMultipleMessage", "false", "Error occured, please try again."));
            }
        }

        public static void DoSetDeviceId(string WSFlag)
        {
            //DataSet dsDeleteMessage = null;
            int UserId, Result = 0;
            string DeviceId = "", DeviceType = "";
            try
            {
                if (WSFlag == "Application")
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                    DeviceId = HttpContext.Current.Request.Form["DeviceId"];

                }
                else
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                    DeviceId = HttpContext.Current.Request.QueryString["DeviceId"];
                }

                Result = BrokerWSUtility.DoSetDeviceId(UserId, DeviceId);

                if (Result != 0)
                {
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("DeviceId set Successfully.");

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSetDeviceId", "true", "null"));
                }
                else
                {
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("");

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSetDeviceId", "false", "Error Occured"));
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSetDeviceId", Ex.Message.ToString(), "BrokerWSDB.cs_DoSetDeviceId", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSetDeviceId", "false", "Error occured, please try again."));
            }
        }

        public static void DoClearDeviceId(string WSFlag)
        {
            int UserId, Result = 0;
            string DeviceId = "", DeviceType = "";
            try
            {
                if (WSFlag == "Application")
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                    DeviceId = HttpContext.Current.Request.Form["DeviceId"];

                }
                else
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                    DeviceId = HttpContext.Current.Request.QueryString["DeviceId"];
                }

                Result = BrokerWSUtility.DoClearDeviceId(UserId, DeviceId);

                if (Result != 0)
                {
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("DeviceId Cleared Successfully.");

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSetDeviceId", "true", "null"));
                }
                else
                {
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("");

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoClearDeviceId", "false", "Error Occured"));
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSetDeviceId", Ex.Message.ToString(), "BrokerWSDB.cs_DoSetDeviceId", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSetDeviceId", "false", "Error occured, please try again."));
            }
        }

        public static void DoPushNotification(string DeviceId, string message, string title, string msgcnt, int UserId)
        {
            //string deviceId = "", message = "", title = "", msgcnt = "", UserId = "", 
            string Notification = "";
            DataSet dsData = new DataSet();
            DataSet dsUnwatchedVideoCount = new DataSet();
            int badge = 0, UnWatchedVideoCnt = 0;
            //List<spGetUserDetails_Result> oUserDetails = null;
            DataSet dsDeviceId = null;

            try
            {
                //if (WSFlag == "Application")
                //{
                //    deviceId = HttpContext.Current.Request.Form["DeviceId"];
                //    message = HttpContext.Current.Request.Form["Message"];
                //    title = HttpContext.Current.Request.Form["title"];
                //    msgcnt = HttpContext.Current.Request.Form["msgcnt"];
                //    UserId = HttpContext.Current.Request.Form["UserId"];
                //}
                //else
                //{
                //    deviceId = HttpContext.Current.Request.QueryString["DeviceId"];
                //    message = HttpContext.Current.Request.QueryString["Message"];
                //    title = HttpContext.Current.Request.QueryString["title"];
                //    msgcnt = HttpContext.Current.Request.QueryString["msgcnt"];
                //    UserId = HttpContext.Current.Request.QueryString["UserId"];
                //}

                dsData = BrokerWSUtility.getUnreadMsgCountByDeviceid(UserId.ToString());

                if (dsData != null)
                {
                    if (dsData.Tables.Count == 3)
                    {
                        if (dsData.Tables[0].Rows.Count > 0)
                        {
                            badge = badge + dsData.Tables[0].Rows.Count;
                        }

                        if (dsData.Tables[1].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsData.Tables[1].Rows.Count; i++)
                            {
                                badge = badge + Convert.ToInt32(dsData.Tables[1].Rows[i]["Cnt"].ToString());
                            }
                        }


                        if (dsData.Tables[2].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsData.Tables[2].Rows.Count; i++)
                            {
                                string message1 = "", msg = "";
                                message1 = dsData.Tables[2].Rows[i]["Message"].ToString().Replace("<br />", "");

                                if (message1.Length > 20)
                                {
                                    msg = message1.Substring(0, 20) + "....";
                                }
                                else
                                {
                                    msg = message1;
                                }

                                Notification += dsData.Tables[2].Rows[i]["UserName"].ToString() + ": " + msg + "\n";

                                if (i == 12)
                                    break;
                            }
                        }
                    }
                }

                dsUnwatchedVideoCount = BrokerWSUtility.GetUnWatchedVideoCount(Convert.ToInt32(UserId));

                if (dsUnwatchedVideoCount.Tables.Count > 0)
                {
                    if (dsUnwatchedVideoCount.Tables[0].Rows.Count > 0)
                    {
                        UnWatchedVideoCnt = Convert.ToInt32(dsUnwatchedVideoCount.Tables[0].Rows[0][0].ToString());
                    }
                }

                badge = badge + UnWatchedVideoCnt;

                //if (dsDeviceId.Tables.Count > 0)
                //{
                //    if (dsDeviceId.Tables[0].Rows.Count > 0)
                //    {
                //        for (int i = 0; i < dsDeviceId.Tables[0].Rows.Count; i++)
                //        {
                WebRequest tRequest;
                tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", strGoogleAppID));

                tRequest.Headers.Add(string.Format("Sender: id={0}", strSENDER_ID));

                //string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + Notification + "&data.title=Brokkrr&data.msgcnt=" + badge + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + deviceId + "";
                string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + Notification + "&data.title=Brokkrr&data.msgcnt=" + badge + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + DeviceId + "";
                Console.WriteLine(postData);
                Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                tRequest.ContentLength = byteArray.Length;

                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse tResponse = tRequest.GetResponse();

                dataStream = tResponse.GetResponseStream();

                StreamReader tReader = new StreamReader(dataStream);

                String sResponseFromServer = tReader.ReadToEnd();

                tReader.Close();
                dataStream.Close();
                tResponse.Close();

                //        }
                //    }
                //}

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("Notification Sent");

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoPushNotification", "true", "null"));

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoPushNotification", Ex.Message.ToString(), "BrokerWSDB.cs_DoPushNotification", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSetDeviceId", "false", "Error occured, please try again."));
            }
        }

        public static void DoGetDeviceId(string WSFlag)
        {
            DataSet dsDeviceId = null;
            int UserId, Result = 0;
            try
            {
                if (WSFlag == "Application")
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                }
                else
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                }

                dsDeviceId = BrokerWSUtility.GetDeviceId(UserId);

                if (dsDeviceId.Tables.Count > 0)
                {
                    if (dsDeviceId.Tables[0].Rows.Count > 0)
                    {

                        //string Message = "";
                        //DataSet dsDetails = new DataSet();
                        //dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("DeviceId set Successfully.");

                        //HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSetDeviceId", "true", "null"));

                        dsDeviceId.Tables[0].TableName = "DeviceId";
                        dsDeviceId.AcceptChanges();

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDeviceId, "DoGetDeviceId", "true", "null"));
                    }
                    else
                    {
                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("");

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetDeviceId", "false", "Error Occured"));
                    }
                }
                else
                {
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("");

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetDeviceId", "false", "Error Occured"));
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoGetDeviceId", Ex.Message.ToString(), "BrokerWSDB.cs_DoGetDeviceId", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetDeviceId", "false", "Error occured, please try again."));
            }
        }

        public static void DoDeleteMultipleChatMessage(string WSFlag)
        {
            DataSet dsDeleteMessage = null;
            int UserId, Result = 0;
            string MessageId = "";
            try
            {
                if (WSFlag == "Application")
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                    MessageId = HttpContext.Current.Request.Form["MessageId"];

                }
                else
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                    MessageId = HttpContext.Current.Request.QueryString["MessageId"];
                }

                MessageId = MessageId.TrimStart('[');
                MessageId = MessageId.TrimEnd(']');

                Result = BrokerWSUtility.DeleteMultipleChatMessage(UserId, MessageId);

                if (Result != 0)
                {
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("Messages Deleted");

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoDeleteMultipleChatMessage", "true", "null"));
                }
                else
                {
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("");

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoDeleteMultipleChatMessage", "false", "Error Occured"));
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoDeleteMultipleChatMessage", Ex.Message.ToString(), "BrokerWSDB.cs_DoDeleteMultipleChatMessage", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoDeleteMultipleMessage", "false", "Error occured, please try again."));
            }
        }

        public static void DoPushNotificationForiOS(string DeviceId, string message, string title, string msgcnt, int UserId)
        {
            string sound = "", deviceId1 = "";
            int badge = 0, UnWatchedVideoCnt = 0; ;
            DataSet dsData = null;
            DataSet dsUnwatchedVideoCount = null;
            DataSet dsDeviceId = null;
            //List<spGetUserDetails_Result> oUserDetails = null;

            sound = "Sound/pop.m4a";

            try
            {
                //if (WSFlag == "Application")
                //{

                //    deviceId = HttpContext.Current.Request.Form["DeviceId"];
                //    message = HttpContext.Current.Request.Form["Message"];
                //    title = HttpContext.Current.Request.Form["title"];
                //    msgcnt = HttpContext.Current.Request.Form["msgcnt"];
                //    UserId = HttpContext.Current.Request.Form["UserId"];

                //}
                //else
                //{

                //    deviceId = HttpContext.Current.Request.QueryString["DeviceId"];
                //    message = HttpContext.Current.Request.QueryString["Message"];
                //    title = HttpContext.Current.Request.QueryString["title"];
                //    msgcnt = HttpContext.Current.Request.QueryString["msgcnt"];
                //    UserId = HttpContext.Current.Request.QueryString["UserId"];
                //}

                //deviceId1 = "iOS" + deviceId;

                dsData = BrokerWSUtility.getUnreadMsgCountByDeviceid(UserId.ToString());

                //oUserDetails = BrokerUtility.GetUserDetails(Convert.ToInt32(UserId));

                //dsDeviceId = BrokerWSUtility.GetDeviceId(Convert.ToInt32(UserId));

                //if (oUserDetails.Count > 0)
                //{
                //    if (!string.IsNullOrEmpty(oUserDetails[0].DeviceId))
                //    {
                //        deviceId = oUserDetails[0].DeviceId.ToString().Replace("iOS", "").Replace("Android", "");
                //    }
                //    else
                //    {
                //        deviceId = "";
                //    }
                //}

                if (dsData != null)
                {
                    if (dsData.Tables.Count == 3)
                    {
                        if (dsData.Tables[0].Rows.Count > 0)
                        {
                            badge = badge + dsData.Tables[0].Rows.Count;
                        }

                        if (dsData.Tables[1].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsData.Tables[1].Rows.Count; i++)
                            {
                                badge = badge + Convert.ToInt32(dsData.Tables[1].Rows[i]["Cnt"].ToString());
                            }
                        }
                    }
                }

                dsUnwatchedVideoCount = BrokerWSUtility.GetUnWatchedVideoCount(Convert.ToInt32(UserId));

                if (dsUnwatchedVideoCount.Tables.Count > 0)
                {
                    if (dsUnwatchedVideoCount.Tables[0].Rows.Count > 0)
                    {
                        UnWatchedVideoCnt = Convert.ToInt32(dsUnwatchedVideoCount.Tables[0].Rows[0][0].ToString());
                    }
                }

                badge = badge + UnWatchedVideoCnt;

                var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Production, AppDomain.CurrentDomain.BaseDirectory + "Certificates/key.p12", "rao123");
                var apnsBroker = new ApnsServiceBroker(config);

                apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
                {
                    aggregateEx.Handle(ex =>
                    {
                        if (ex is ApnsNotificationException)
                        {
                            var notificationException = (ApnsNotificationException)ex;

                            var apnsNotification = notificationException.Notification;
                            var statusCode = notificationException.ErrorStatusCode;

                            //jsonString.stringValue = $"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}";

                            Console.WriteLine("Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");
                        }
                        else
                        {
                            //jsonString.stringValue = $"Apple Notification Failed for some unknown reason : {ex.InnerException}";

                            Console.WriteLine("Apple Notification Failed for some unknown reason : {ex.InnerException}");
                        }

                        return true;
                    });
                };

                apnsBroker.OnNotificationSucceeded += (notification) =>
                {
                    //jsonString.stringValue = "Apple Notification Sent!";
                    Console.WriteLine("Apple Notification Sent!");
                };

                //if (dsDeviceId.Tables.Count > 0)
                //{
                //    if (dsDeviceId.Tables[0].Rows.Count > 0)
                //    {
                //        for (int i = 0; i < dsDeviceId.Tables[0].Rows.Count; i++)
                //        {
                apnsBroker.Start();

                string appleJsonFormat = "{\"aps\" : { \"alert\":\'" + title + " - " + message + "',\"badge\" : \'" + badge.ToString() + "',\"content-available\":\"1\",\"sound\" : \'" + sound + "' }}";

                apnsBroker.QueueNotification(new ApnsNotification
                {
                    DeviceToken = DeviceId,
                    Payload = JObject.Parse(appleJsonFormat)
                });

                apnsBroker.Stop();
                //        }
                //    }
                //}

                /************************************************************************/

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("Notification Sent");

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoPushNotification", "true", "null"));

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoPushNotificationForiOS", Ex.Message.ToString(), "BrokerWSDB.cs_DoPushNotificationForiOS", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoPushNotificationForiOS", "false", "Error occured, please try again."));
            }
        }

        public static void DoGetUnreadMsgCount(string WSFlag)
        {
            DataSet dsUnreadMessages = null;
            DataTable dtUnreadCount = new DataTable();

            dtUnreadCount.Columns.Add("UnreadMsgCnt");
            //dsUnreadMessages = new DataSet();
            int UserId, Result = 0, UnreadMsgCnt = 0;
            string MessageId = "";
            try
            {
                if (WSFlag == "Application")
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                }
                else
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                }

                dsUnreadMessages = BrokerWSUtility.GetUnreadMsgCount(UserId);

                if (dsUnreadMessages.Tables.Count > 0)
                {
                    if (dsUnreadMessages.Tables[0].Rows.Count > 0 || dsUnreadMessages.Tables[1].Rows.Count > 0)
                    {
                        if (dsUnreadMessages.Tables[0].Rows.Count > 0)
                        {
                            UnreadMsgCnt = UnreadMsgCnt + dsUnreadMessages.Tables[0].Rows.Count;
                        }

                        if (dsUnreadMessages.Tables[1].Rows.Count > 0)
                        {
                            for (int z = 0; z < dsUnreadMessages.Tables[1].Rows.Count; z++)
                            {
                                UnreadMsgCnt = UnreadMsgCnt + Convert.ToInt32(dsUnreadMessages.Tables[1].Rows[z]["Cnt"].ToString());
                            }
                        }

                        dtUnreadCount.Rows.Add(UnreadMsgCnt);
                        dsUnreadMessages.Tables.Add(dtUnreadCount);

                        dsUnreadMessages.Tables[0].TableName = "ContactedMessageList";
                        dsUnreadMessages.Tables[1].TableName = "MessageDetails";
                        dsUnreadMessages.Tables[2].TableName = "UnreadNotification";
                        dsUnreadMessages.Tables[3].TableName = "UnreadMsgCnt";

                        dsUnreadMessages.AcceptChanges();

                        for (int i = 0; i < dsUnreadMessages.Tables[0].Rows.Count; i++)
                        {
                            string binData = dsUnreadMessages.Tables[0].Rows[i]["ProfilePicture"].ToString();
                            if (binData != "")
                            {
                                binData = strDomainName + "" + strProfilePicForlderName + "" + dsUnreadMessages.Tables[0].Rows[i]["ProfilePicture"].ToString(); ;

                                dsUnreadMessages.Tables[0].Rows[i]["ProfilePicture"] = binData;
                            }

                            string ProfilePicImg = dsUnreadMessages.Tables[0].Rows[i]["ProfilePictureImg"].ToString();
                            if (ProfilePicImg != "")
                            {
                                ProfilePicImg = strDomainName + "" + strProfilePicImageFolder + "" + dsUnreadMessages.Tables[0].Rows[i]["ProfilePictureImg"].ToString(); ;

                                dsUnreadMessages.Tables[0].Rows[i]["ProfilePictureImg"] = ProfilePicImg;
                            }
                        }

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsUnreadMessages, "DoGetUnreadMsgCount", "true", "null"));
                    }
                    else
                    {
                        DataSet dsMessages = new DataSet();
                        DataTable dtContactList = new DataTable();
                        DataTable dtMessage = new DataTable();
                        DataTable dtUnreadNotification = new DataTable();
                        DataTable dtMessageCnt = new DataTable();

                        dsMessages.Tables.Add(dtContactList);
                        dsMessages.Tables.Add(dtMessage);
                        dsMessages.Tables.Add(dtUnreadNotification);
                        dsMessages.Tables.Add(dtMessageCnt);

                        dsMessages.Tables[0].TableName = "ContactedMessageList";
                        dsMessages.Tables[1].TableName = "MessageDetails";
                        dsMessages.Tables[2].TableName = "UnreadNotification";
                        dsMessages.Tables[3].TableName = "UnreadMsgCnt";
                        dsMessages.AcceptChanges();

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsMessages, "DoGetUnreadMsgCount", "true", "null"));


                    }
                }
                else
                {
                    DataSet dsMessages = new DataSet();
                    DataTable dtContactList = new DataTable();
                    DataTable dtMessage = new DataTable();
                    DataTable dtUnreadNotification = new DataTable();
                    DataTable dtMessageCnt = new DataTable();

                    dsMessages.Tables.Add(dtContactList);
                    dsMessages.Tables.Add(dtMessage);
                    dsMessages.Tables.Add(dtUnreadNotification);
                    dsMessages.Tables.Add(dtMessageCnt);

                    dsMessages.Tables[0].TableName = "ContactedMessageList";
                    dsMessages.Tables[1].TableName = "MessageDetails";
                    dsMessages.Tables[2].TableName = "UnreadNotification";
                    dsMessages.Tables[3].TableName = "UnreadMsgCnt";
                    dsMessages.AcceptChanges();

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsMessages, "DoGetUnreadMsgCount", "true", "null"));
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoGetUnreadMsgCount", Ex.Message.ToString(), "BrokerWSDB.cs_DoGetUnreadMsgCount", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetUnreadMsgCount", "false", "Error occured, please try again."));
            }
        }

        public static void DoGetIndustryMaster(string WSFlag)
        {
            DataSet dsIndustryMaster = null;
            int CompanyId;

            try
            {
                if (WSFlag == "Application")
                {

                    CompanyId = Convert.ToInt32(HttpContext.Current.Request.Form["CompanyId"]);
                }
                else
                {

                    CompanyId = Convert.ToInt32(HttpContext.Current.Request.QueryString["CompanyId"]);
                }

                dsIndustryMaster = BrokerWSUtility.GetIndustryMaster(CompanyId);

                if (dsIndustryMaster.Tables.Count > 0)
                {
                    if (dsIndustryMaster.Tables[0].Rows.Count > 0)
                    {
                        TextInfo convert = new CultureInfo("en-US", false).TextInfo;
                        for (int i = 0; i < dsIndustryMaster.Tables[0].Rows.Count; i++)
                        {
                            string IndustryName = convert.ToLower(dsIndustryMaster.Tables[0].Rows[i]["IndustryName"].ToString());
                            IndustryName = convert.ToTitleCase(IndustryName);
                            dsIndustryMaster.Tables[0].Rows[i]["IndustryName"] = IndustryName;
                        }

                        dsIndustryMaster.Tables[0].TableName = "IndustryMaster";
                        dsIndustryMaster.AcceptChanges();

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsIndustryMaster, "DoGetIndustryMaster", "true", "null"));
                    }
                    else
                    {
                        DataSet dsMessages = new DataSet();
                        DataTable dtContactList = new DataTable();
                        //DataTable dtMessage = new DataTable();

                        dsMessages.Tables.Add(dtContactList);
                        //dsMessages.Tables.Add(dtMessage);

                        dsMessages.Tables[0].TableName = "IndustryMaster";
                        //dsMessages.Tables[1].TableName = "MessageDetails";
                        dsMessages.AcceptChanges();

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsMessages, "DoGetIndustryMaster", "true", "null"));


                    }
                }
                else
                {
                    DataSet dsMessages = new DataSet();
                    DataTable dtContactList = new DataTable();
                    //DataTable dtMessage = new DataTable();

                    dsMessages.Tables.Add(dtContactList);
                    //dsMessages.Tables.Add(dtMessage);

                    dsMessages.Tables[0].TableName = "IndustryMaster";
                    //dsMessages.Tables[1].TableName = "MessageDetails";
                    dsMessages.AcceptChanges();

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsMessages, "DoGetIndustryMaster", "true", "null"));
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoGetIndustryMaster", Ex.Message.ToString(), "BrokerWSDB.cs_DoGetIndustryMaster", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetUnreadMsgCount", "false", "Error occured, please try again."));
            }
        }

        public static void DoGetSubIndustryMaster(string WSFlag)
        {
            DataSet dsSubIndustryMaster = null;

            int IndustryId = 0;
            try
            {
                if (WSFlag == "Application")
                {
                    if (HttpContext.Current.Request.Form["IndustryId"] != "")
                    {
                        IndustryId = Convert.ToInt32(HttpContext.Current.Request.Form["IndustryId"]);
                    }
                }
                else
                {
                    if (HttpContext.Current.Request.QueryString["IndustryId"] != "")
                    {
                        IndustryId = Convert.ToInt32(HttpContext.Current.Request.QueryString["IndustryId"]);
                    }
                }

                dsSubIndustryMaster = BrokerWSUtility.GetSubIndustryMaster(IndustryId);

                if (dsSubIndustryMaster.Tables.Count > 0)
                {
                    if (dsSubIndustryMaster.Tables[0].Rows.Count > 0)
                    {
                        TextInfo convert = new CultureInfo("en-US", false).TextInfo;
                        for (int i = 0; i < dsSubIndustryMaster.Tables[0].Rows.Count; i++)
                        {
                            string SubIndustryName = convert.ToLower(dsSubIndustryMaster.Tables[0].Rows[i]["SubIndustryName"].ToString());
                            SubIndustryName = convert.ToTitleCase(SubIndustryName);
                            dsSubIndustryMaster.Tables[0].Rows[i]["SubIndustryName"] = SubIndustryName;
                        }

                        dsSubIndustryMaster.Tables[0].TableName = "SubIndustryMaster";
                        dsSubIndustryMaster.AcceptChanges();

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsSubIndustryMaster, "DoGetSubIndustryMaster", "true", "null"));
                    }
                    else
                    {
                        DataSet dsMessages = new DataSet();
                        DataTable dtContactList = new DataTable();
                        //DataTable dtMessage = new DataTable();

                        dsMessages.Tables.Add(dtContactList);
                        //dsMessages.Tables.Add(dtMessage);

                        dsMessages.Tables[0].TableName = "SubIndustryMaster";
                        //dsMessages.Tables[1].TableName = "MessageDetails";
                        dsMessages.AcceptChanges();

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsMessages, "DoGetIndustryMaster", "true", "null"));


                    }
                }
                else
                {
                    DataSet dsMessages = new DataSet();
                    DataTable dtContactList = new DataTable();
                    //DataTable dtMessage = new DataTable();

                    dsMessages.Tables.Add(dtContactList);
                    //dsMessages.Tables.Add(dtMessage);

                    dsMessages.Tables[0].TableName = "SubIndustryMaster";
                    //dsMessages.Tables[1].TableName = "MessageDetails";
                    dsMessages.AcceptChanges();

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsMessages, "DoGetIndustryMaster", "true", "null"));
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoGetSubIndustryMaster", Ex.Message.ToString(), "BrokerWSDB.cs_DoGetSubIndustryMaster", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetUnreadMsgCount", "false", "Error occured, please try again."));
            }
        }

        public static void DoSampleAction(string WSFlag)
        {
            try
            {
                string SubIndustryId = "", UserId = "";
                if (WSFlag == "Application")
                {
                    UserId = HttpContext.Current.Request.Form["UserId"];
                    SubIndustryId = HttpContext.Current.Request.Form["SubIndustryId"];
                }
                else
                {
                    UserId = HttpContext.Current.Request.Form["UserId"];
                    SubIndustryId = HttpContext.Current.Request.QueryString["SubIndustryId"];
                }

                if (SubIndustryId != "")
                {
                    //Start to split Sub Industry Ids
                    #region Split Sub Industry Ids

                    int Result1 = BrokerWSUtility.DeleteIndustryId(UserId);

                    if (SubIndustryId != "")
                    {
                        DataTable dtIndustryId = new DataTable();

                        dtIndustryId.Columns.Add("IndustryId");
                        dtIndustryId.Columns.Add("SubIndustryId");

                        string[] SubIndustryIds1 = null;
                        string[] SubIndustryIds2 = null;

                        SubIndustryIds1 = SubIndustryId.Split(';');

                        foreach (string Id in SubIndustryIds1)
                        {
                            SubIndustryIds2 = Id.Split(':');
                            string a = "", b = "", c = "";

                            string x = SubIndustryIds2[0];
                            string y = SubIndustryIds2[1];

                            string[] u = y.Split(',');

                            foreach (string Id1 in u)
                            {
                                dtIndustryId.Rows.Add(x, Id1);
                            }
                        }

                        for (int k = 0; k < dtIndustryId.Rows.Count; k++)
                        {
                            int Result = BrokerWSUtility.InsertIndustryId(UserId, dtIndustryId.Rows[k][0].ToString(), dtIndustryId.Rows[k][1].ToString());
                        }

                    }

                    #endregion Split Industry Ids
                    //End of split Sub Industry Ids
                }
            }
            catch (Exception Ex)
            {

            }
        }

        public static void DoGetMainMessageofCustomerForBroker(string WSFlag)
        {
            try
            {
                DataSet dsMainMsgDetails = null;
                string BrokerId = "", CustomerId = "", BrokerMessageId = "", CustomerMessageId = "", TableName = "";

                if (WSFlag == "Application")
                {
                    BrokerId = HttpContext.Current.Request.Form["BrokerId"].ToString();
                    CustomerId = HttpContext.Current.Request.Form["CustomerId"].ToString();
                    BrokerMessageId = HttpContext.Current.Request.Form["BrokerMessageId"].ToString();
                    CustomerMessageId = HttpContext.Current.Request.Form["CustomerMessageId"].ToString();
                    TableName = HttpContext.Current.Request.Form["TableName"].ToString();
                }
                else
                {
                    BrokerId = HttpContext.Current.Request.QueryString["BrokerId"].ToString();
                    CustomerId = HttpContext.Current.Request.QueryString["CustomerId"].ToString();
                    BrokerMessageId = HttpContext.Current.Request.QueryString["BrokerMessageId"].ToString();
                    CustomerMessageId = HttpContext.Current.Request.QueryString["CustomerMessageId"].ToString();
                    TableName = HttpContext.Current.Request.QueryString["TableName"].ToString();
                }
                //HttpContext.Current.Response.Write(BrokerWSUtility.CheckLogin(UserName, Password, UserType));

                dsMainMsgDetails = BrokerWebDB.BrokerWebDB.GetMainMessage(BrokerId, CustomerId, BrokerMessageId, CustomerMessageId, TableName);

                if (dsMainMsgDetails.Tables.Count > 0)
                {
                    if (TableName == "BrokerMessages")
                    {
                        if (dsMainMsgDetails.Tables[0].Rows.Count > 0)
                        {
                            if (dsMainMsgDetails.Tables[0].Rows[0]["Message"].ToString() != "" && dsMainMsgDetails.Tables[0].Rows[0]["DeclarationDocPath"].ToString() != "")
                            {
                                string Message = dsMainMsgDetails.Tables[0].Rows[0]["Message"].ToString();
                                string DocPath = strDomainName + strDeclarationDocumentFolder + dsMainMsgDetails.Tables[0].Rows[0]["DeclarationDocPath"].ToString();

                                Message = Message.Replace("Please reply back if you are interested.", "Declaration Document - <a style='color: white;text-decoration: underline;' href=" + DocPath + " download>Click here to download</a><br/>Please reply back if you are interested.");
                                dsMainMsgDetails.Tables[0].Rows[0]["Message"] = Message;
                                dsMainMsgDetails.AcceptChanges();
                            }
                        }
                    }

                    dsMainMsgDetails.Tables[0].TableName = "MainMessageDetails";

                    dsMainMsgDetails.AcceptChanges();
                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsMainMsgDetails, "DoGetMainMessageofCustomerForBroker", "true", "null"));
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoGetMainMessageofCustomerForBroker", Ex.Message.ToString(), "BrokerWSDB.cs_DoGetMainMessageofCustomerForBroker()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetMainMessageofCustomerForBroker", "false", "Error occured, please try again."));
            }
        }

        #endregion Common Actions

        #region Meineke Insurance

        //Changes 16Mar18
        public static void DoSaveCommercialAutoInsuranceDetails(string WSFlag)
        {
            DataSet dsUserDetails = null;

            int User, UserId = 0;
            string ZipCode = "", City = "", Longitude = "", Latitude = "", NoOfUnits,
                DeductibleIfAny = "", CurrentLimit = "", NoOfStalls = "", NoOfLocations = "", GrossRevenue = ""
                , DeclarationDoc = "", DocName = "", DocPath = "";//29May18;
            try
            {
                if (WSFlag == "Application")
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                    ZipCode = HttpContext.Current.Request.Form["ZipCode"].ToString();
                    City = HttpContext.Current.Request.Form["City"].ToString();

                    NoOfStalls = HttpContext.Current.Request.Form["NoOfStalls"].ToString();
                    NoOfLocations = HttpContext.Current.Request.Form["NoOfLocations"].ToString();
                    GrossRevenue = HttpContext.Current.Request.Form["GrossRevenue"].ToString();

                    CurrentLimit = HttpContext.Current.Request.Form["CurrentLimit"].ToString();

                    Longitude = HttpContext.Current.Request.Form["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.Form["Latitude"].ToString();

                    DeclarationDoc = HttpContext.Current.Request.Form["DeclarationDoc"].ToString();//29May18
                    DocName = HttpContext.Current.Request.Form["DocName"].ToString();//29May18

                }
                else
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                    ZipCode = HttpContext.Current.Request.QueryString["ZipCode"].ToString();
                    City = HttpContext.Current.Request.QueryString["City"].ToString();

                    NoOfStalls = HttpContext.Current.Request.QueryString["NoOfStalls"].ToString();
                    NoOfLocations = HttpContext.Current.Request.QueryString["NoOfLocations"].ToString();
                    GrossRevenue = HttpContext.Current.Request.QueryString["GrossRevenue"].ToString();

                    CurrentLimit = HttpContext.Current.Request.QueryString["CurrentLimit"].ToString();

                    Longitude = HttpContext.Current.Request.QueryString["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.QueryString["Latitude"].ToString();

                    DeclarationDoc = HttpContext.Current.Request.QueryString["DeclarationDoc"].ToString();//29May18
                    DocName = HttpContext.Current.Request.QueryString["DocName"].ToString();//29May18

                }

                //29May18
                if (DocName != "" && DeclarationDoc != "")
                {
                    DeclarationDoc = DeclarationDoc.Replace(" ", "+");
                    DocPath = BrokerUtility.SaveDeclarationDocument(DocName, DeclarationDoc, "GarageApp", UserId.ToString());
                }


                User = BrokerWSUtility.SaveCommercialAutoInsuranceDetails(UserId, ZipCode, City, NoOfStalls, NoOfLocations, GrossRevenue, CurrentLimit, Longitude, Latitude, DocPath, DeclarationDoc);

                if (User != 0)
                {
                    dsUserDetails = BrokerWSUtility.GetBrokersListForMeineke(UserId, ZipCode, City, "CommercialAuto", Longitude, Latitude, "0", "0");

                    if (dsUserDetails.Tables.Count > 0)
                    {
                        if (dsUserDetails.Tables[0].Rows.Count > 0)
                        {
                            string SerchForUserId = "";
                            if (strUsersToShowByDefaultInSearchList != "")
                            {
                                SerchForUserId = CheckUserIdInExistingResult(strUsersToShowByDefaultInSearchList, dsUserDetails);
                            }

                            if (SerchForUserId != "")
                            {
                                dsUserDetails = MergeTwoResults(dsUserDetails, SerchForUserId, UserId);
                            }

                            SendUserDetailsForHomeInsurance(dsUserDetails);
                        }
                        else
                        {
                            dsUserDetails = BrokerWSUtility.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsUserDetails.Tables.Count > 0)
                            {
                                if (dsUserDetails.Tables[0].Rows.Count > 0)
                                {
                                    SendUserDetailsForHomeInsurance(dsUserDetails);
                                }
                                else
                                {
                                    dsUserDetails.Tables[0].TableName = "UserDetails";
                                    dsUserDetails.Tables[1].TableName = "ExperienceDetails";
                                    dsUserDetails.Tables[2].TableName = "EducationDetails";
                                    dsUserDetails.Tables[3].TableName = "BrokerContactList";
                                    dsUserDetails.AcceptChanges();

                                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsUserDetails, "DoSaveAutoInsuranceDetails", "true", "null"));
                                }
                            }
                            else
                            {
                                string Message = "";
                                DataSet dsDetails = new DataSet();
                                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveAutoInsuranceDetails", "false", "Error Occured"));
                            }
                        }
                    }
                    else
                    {
                        // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));
                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveAutoInsuranceDetails", "false", "Error Occured"));

                    }
                }
                else
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveAutoInsuranceDetails", "false", "Error Occured"));

                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSaveCommercialAutoInsuranceDetails", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveCommercialAutoInsuranceDetails()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        //public static void DoSaveCommercialAutoInsuranceDetails(string WSFlag)
        //{
        //    DataSet dsUserDetails = null;

        //    int User, UserId = 0;
        //    string ZipCode = "", City = "", Longitude = "", Latitude = "", NoOfUnits,
        //        DeductibleIfAny = "", CurrentLimit = "";
        //    try
        //    {
        //        if (WSFlag == "Application")
        //        {

        //            UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
        //            ZipCode = HttpContext.Current.Request.Form["ZipCode"].ToString();
        //            City = HttpContext.Current.Request.Form["City"].ToString();

        //            NoOfUnits = HttpContext.Current.Request.Form["NoOfUnits"].ToString();
        //            DeductibleIfAny = HttpContext.Current.Request.Form["DeductibleIfAny"].ToString();
        //            CurrentLimit = HttpContext.Current.Request.Form["CurrentLimit"].ToString();


        //            Longitude = HttpContext.Current.Request.Form["Longitude"].ToString();
        //            Latitude = HttpContext.Current.Request.Form["Latitude"].ToString();

        //        }
        //        else
        //        {

        //            UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
        //            ZipCode = HttpContext.Current.Request.QueryString["ZipCode"].ToString();
        //            City = HttpContext.Current.Request.QueryString["City"].ToString();

        //            NoOfUnits = HttpContext.Current.Request.QueryString["NoOfUnits"].ToString();
        //            DeductibleIfAny = HttpContext.Current.Request.QueryString["DeductibleIfAny"].ToString();
        //            CurrentLimit = HttpContext.Current.Request.QueryString["CurrentLimit"].ToString();

        //            Longitude = HttpContext.Current.Request.QueryString["Longitude"].ToString();
        //            Latitude = HttpContext.Current.Request.QueryString["Latitude"].ToString();

        //        }

        //        User = BrokerWSUtility.SaveCommercialAutoInsuranceDetails(UserId, ZipCode, City, NoOfUnits, DeductibleIfAny, CurrentLimit, Longitude, Latitude);

        //        if (User != 0)
        //        {
        //            dsUserDetails = BrokerWSUtility.GetBrokersListForMeineke(UserId, ZipCode, City, "CommercialAuto", Longitude, Latitude, "0", "0");

        //            if (dsUserDetails.Tables.Count > 0)
        //            {
        //                if (dsUserDetails.Tables[0].Rows.Count > 0)
        //                {
        //                    SendUserDetailsForHomeInsurance(dsUserDetails);
        //                }
        //                else
        //                {
        //                    dsUserDetails = BrokerWSUtility.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

        //                    if (dsUserDetails.Tables.Count > 0)
        //                    {
        //                        if (dsUserDetails.Tables[0].Rows.Count > 0)
        //                        {
        //                            SendUserDetailsForHomeInsurance(dsUserDetails);
        //                        }
        //                        else
        //                        {
        //                            dsUserDetails.Tables[0].TableName = "UserDetails";
        //                            dsUserDetails.Tables[1].TableName = "ExperienceDetails";
        //                            dsUserDetails.Tables[2].TableName = "EducationDetails";
        //                            dsUserDetails.Tables[3].TableName = "BrokerContactList";
        //                            dsUserDetails.AcceptChanges();

        //                            HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsUserDetails, "DoSaveAutoInsuranceDetails", "true", "null"));
        //                        }
        //                    }
        //                    else
        //                    {
        //                        string Message = "";
        //                        DataSet dsDetails = new DataSet();
        //                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

        //                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveAutoInsuranceDetails", "false", "Error Occured"));
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                string Message = "";
        //                DataSet dsDetails = new DataSet();
        //                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

        //                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveAutoInsuranceDetails", "false", "Error Occured"));

        //            }
        //        }
        //        else
        //        {
        //            string Message = "";
        //            DataSet dsDetails = new DataSet();
        //            dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

        //            HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveAutoInsuranceDetails", "false", "Error Occured"));

        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        BrokerUtility.ErrorLog(0, "DoSaveCommercialAutoInsuranceDetails", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveCommercialAutoInsuranceDetails()", "");

        //        string Message = "";
        //        DataSet dsDetails = new DataSet();
        //        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

        //        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
        //    }
        //}

        public static void DoSaveWorkersCompensationDetails(string WSFlag)
        {
            DataSet dsUserDetails = null;

            int User, UserId = 0;
            string ZipCode = "", City = "", Longitude = "", Latitude = "", NoOfEmployees,
                GrossPayroll = "", DeclarationDoc = "", DocName = "", DocPath = "";//29May18;
            try
            {
                if (WSFlag == "Application")
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                    ZipCode = HttpContext.Current.Request.Form["ZipCode"].ToString();
                    City = HttpContext.Current.Request.Form["City"].ToString();

                    NoOfEmployees = HttpContext.Current.Request.Form["NoOfEmployees"].ToString();
                    GrossPayroll = HttpContext.Current.Request.Form["GrossPayroll"].ToString();

                    Longitude = HttpContext.Current.Request.Form["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.Form["Latitude"].ToString();

                    DeclarationDoc = HttpContext.Current.Request.Form["DeclarationDoc"].ToString();//29May18
                    DocName = HttpContext.Current.Request.Form["DocName"].ToString();//29May18

                }
                else
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                    ZipCode = HttpContext.Current.Request.QueryString["ZipCode"].ToString();
                    City = HttpContext.Current.Request.QueryString["City"].ToString();

                    NoOfEmployees = HttpContext.Current.Request.QueryString["NoOfEmployees"].ToString();
                    GrossPayroll = HttpContext.Current.Request.QueryString["GrossPayroll"].ToString();

                    Longitude = HttpContext.Current.Request.QueryString["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.QueryString["Latitude"].ToString();

                    DeclarationDoc = HttpContext.Current.Request.QueryString["DeclarationDoc"].ToString();//29May18
                    DocName = HttpContext.Current.Request.QueryString["DocName"].ToString();//29May18

                }

                //29May18
                if (DocName != "" && DeclarationDoc != "")
                {
                    DeclarationDoc = DeclarationDoc.Replace(" ", "+");
                    DocPath = BrokerUtility.SaveDeclarationDocument(DocName, DeclarationDoc, "WorkersCompApp", UserId.ToString());
                }

                User = BrokerWSUtility.SaveWorkersCompensationDetails(UserId, ZipCode, City, Longitude, Latitude, NoOfEmployees, GrossPayroll, DocPath, DeclarationDoc);

                if (User != 0)
                {
                    dsUserDetails = BrokerWSUtility.GetBrokersListForMeineke(UserId, ZipCode, City, "WorkComp", Longitude, Latitude, "0", "0");

                    if (dsUserDetails.Tables.Count > 0)
                    {
                        if (dsUserDetails.Tables[0].Rows.Count > 0)
                        {
                            string SerchForUserId = "";
                            if (strUsersToShowByDefaultInSearchList != "")
                            {
                                SerchForUserId = CheckUserIdInExistingResult(strUsersToShowByDefaultInSearchList, dsUserDetails);
                            }

                            if (SerchForUserId != "")
                            {
                                dsUserDetails = MergeTwoResults(dsUserDetails, SerchForUserId, UserId);
                            }

                            SendUserDetailsForHomeInsurance(dsUserDetails);
                        }
                        else
                        {
                            dsUserDetails = BrokerWSUtility.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsUserDetails.Tables.Count > 0)
                            {
                                if (dsUserDetails.Tables[0].Rows.Count > 0)
                                {
                                    SendUserDetailsForHomeInsurance(dsUserDetails);
                                }
                                else
                                {
                                    dsUserDetails.Tables[0].TableName = "UserDetails";
                                    dsUserDetails.Tables[1].TableName = "ExperienceDetails";
                                    dsUserDetails.Tables[2].TableName = "EducationDetails";
                                    dsUserDetails.Tables[3].TableName = "BrokerContactList";
                                    dsUserDetails.AcceptChanges();

                                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsUserDetails, "DoSaveWorkersCompensationDetails", "true", "null"));
                                }
                            }
                            else
                            {
                                string Message = "";
                                DataSet dsDetails = new DataSet();
                                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveWorkersCompensationDetails", "false", "Error Occured"));
                            }
                        }
                    }
                    else
                    {
                        // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));
                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveWorkersCompensationDetails", "false", "Error Occured"));

                    }
                }
                else
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveAutoInsuranceDetails", "false", "Error Occured"));

                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSaveWorkersCompensationDetails", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveWorkersCompensationDetails()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static void DoSaveLiabilityInsuranceDetails(string WSFlag)
        {
            DataSet dsUserDetails = null;

            int User, UserId = 0;
            string ZipCode = "", City = "", Longitude = "", Latitude = "", GrossSale = "", IndustryId = "", SubIndustryId = "",
                DeductibleIfAny = "";
            try
            {
                if (WSFlag == "Application")
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                    ZipCode = HttpContext.Current.Request.Form["ZipCode"].ToString();
                    City = HttpContext.Current.Request.Form["City"].ToString();

                    GrossSale = HttpContext.Current.Request.Form["GrossSale"].ToString();
                    DeductibleIfAny = HttpContext.Current.Request.Form["DeductibleIfAny"].ToString();
                    IndustryId = HttpContext.Current.Request.Form["IndustryId"].ToString();
                    SubIndustryId = HttpContext.Current.Request.Form["SubIndustryId"].ToString();

                    Longitude = HttpContext.Current.Request.Form["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.Form["Latitude"].ToString();

                }
                else
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                    ZipCode = HttpContext.Current.Request.QueryString["ZipCode"].ToString();
                    City = HttpContext.Current.Request.QueryString["City"].ToString();

                    GrossSale = HttpContext.Current.Request.QueryString["GrossSale"].ToString();
                    DeductibleIfAny = HttpContext.Current.Request.QueryString["DeductibleIfAny"].ToString();
                    IndustryId = HttpContext.Current.Request.QueryString["IndustryId"].ToString();
                    SubIndustryId = HttpContext.Current.Request.QueryString["SubIndustryId"].ToString();

                    Longitude = HttpContext.Current.Request.QueryString["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.QueryString["Latitude"].ToString();

                }

                User = BrokerWSUtility.SaveLiabilityInsuranceDetails(UserId, ZipCode, City, Longitude, Latitude, GrossSale, DeductibleIfAny, IndustryId, SubIndustryId);

                if (User != 0)
                {
                    dsUserDetails = BrokerWSUtility.GetBrokersListForMeineke(UserId, ZipCode, City, "Liability", Longitude, Latitude, IndustryId, SubIndustryId);

                    if (dsUserDetails.Tables.Count > 0)
                    {
                        if (dsUserDetails.Tables[0].Rows.Count > 0)
                        {
                            string SerchForUserId = "";
                            if (strUsersToShowByDefaultInSearchList != "")
                            {
                                SerchForUserId = CheckUserIdInExistingResult(strUsersToShowByDefaultInSearchList, dsUserDetails);
                            }

                            if (SerchForUserId != "")
                            {
                                dsUserDetails = MergeTwoResults(dsUserDetails, SerchForUserId, UserId);
                            }

                            SendUserDetailsForHomeInsurance(dsUserDetails);
                        }
                        else
                        {
                            dsUserDetails = BrokerWSUtility.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsUserDetails.Tables.Count > 0)
                            {
                                if (dsUserDetails.Tables[0].Rows.Count > 0)
                                {
                                    SendUserDetailsForHomeInsurance(dsUserDetails);
                                }
                                else
                                {
                                    dsUserDetails.Tables[0].TableName = "UserDetails";
                                    dsUserDetails.Tables[1].TableName = "ExperienceDetails";
                                    dsUserDetails.Tables[2].TableName = "EducationDetails";
                                    dsUserDetails.Tables[3].TableName = "BrokerContactList";
                                    dsUserDetails.AcceptChanges();

                                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsUserDetails, "DoSaveWorkersCompensationDetails", "true", "null"));
                                }
                            }
                            else
                            {
                                string Message = "";
                                DataSet dsDetails = new DataSet();
                                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveWorkersCompensationDetails", "false", "Error Occured"));
                            }
                        }
                    }
                    else
                    {
                        // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));
                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveLiabilityInsuranceDetails", "false", "Error Occured"));

                    }
                }
                else
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveAutoInsuranceDetails", "false", "Error Occured"));

                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSaveLiabilityInsuranceDetails", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveLiabilityInsuranceDetails()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static void DoSaveMeinekeBenefitInsuranceDetails(string WSFlag)
        {
            string ZipCode = "", City = "", IsInsured = "", InsuranceCompany = "", EmployeeStrength = "",
                 CoverageExpires = "", Language = "", Notes = "", Longitude = "", Latitude = "", IndustryId = "",
                 SubIndustryId = "", DeclarationDoc = "", DocName = "", DocPath = "";//29May18;

            int UserId;

            int User = 0;
            DataSet dsBrokerDetails = null;

            try
            {
                if (WSFlag == "Application")
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"].ToString());
                    ZipCode = HttpContext.Current.Request.Form["ZipCode"].ToString();
                    City = HttpContext.Current.Request.Form["City"].ToString();
                    IsInsured = HttpContext.Current.Request.Form["IsInsured"].ToString();
                    InsuranceCompany = HttpContext.Current.Request.Form["InsuranceCompany"].ToString();
                    EmployeeStrength = HttpContext.Current.Request.Form["EmployeeStrength"].ToString();

                    CoverageExpires = HttpContext.Current.Request.Form["CoverageExpires"].ToString();
                    Language = HttpContext.Current.Request.Form["Language"].ToString();
                    Notes = HttpContext.Current.Request.Form["Notes"].ToString();

                    Longitude = HttpContext.Current.Request.Form["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.Form["Latitude"].ToString();

                    IndustryId = HttpContext.Current.Request.Form["IndustryId"].ToString();
                    SubIndustryId = HttpContext.Current.Request.Form["SubIndustryId"].ToString();

                    DeclarationDoc = HttpContext.Current.Request.Form["DeclarationDoc"].ToString();//29May18
                    DocName = HttpContext.Current.Request.Form["DocName"].ToString();//29May18

                }
                else
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"].ToString());
                    ZipCode = HttpContext.Current.Request.QueryString["ZipCode"].ToString();
                    City = HttpContext.Current.Request.QueryString["City"].ToString();
                    IsInsured = HttpContext.Current.Request.QueryString["IsInsured"].ToString();
                    InsuranceCompany = HttpContext.Current.Request.QueryString["InsuranceCompany"].ToString();
                    EmployeeStrength = HttpContext.Current.Request.QueryString["EmployeeStrength"].ToString();

                    CoverageExpires = HttpContext.Current.Request.QueryString["CoverageExpires"].ToString();
                    Language = HttpContext.Current.Request.QueryString["Language"].ToString();
                    Notes = HttpContext.Current.Request.QueryString["Notes"].ToString();

                    Longitude = HttpContext.Current.Request.QueryString["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.QueryString["Latitude"].ToString();

                    IndustryId = HttpContext.Current.Request.QueryString["IndustryId"].ToString();
                    SubIndustryId = HttpContext.Current.Request.QueryString["SubIndustryId"].ToString();

                    DeclarationDoc = HttpContext.Current.Request.QueryString["DeclarationDoc"].ToString();//29May18
                    DocName = HttpContext.Current.Request.QueryString["DocName"].ToString();//29May18
                }

                //29May18
                if (DocName != "" && DeclarationDoc != "")
                {
                    DeclarationDoc = DeclarationDoc.Replace(" ", "+");
                    DocPath = BrokerUtility.SaveDeclarationDocument(DocName, DeclarationDoc, "MeinekeBenefitApp", UserId.ToString());
                }

                User = BrokerWSUtility.SaveMeinekeBenefitInsuranceDetails(UserId, ZipCode, City, IsInsured, InsuranceCompany, EmployeeStrength, CoverageExpires, Language, Notes, Longitude, Latitude, IndustryId, SubIndustryId, DocPath, DeclarationDoc);

                if (User != 0)
                {
                    dsBrokerDetails = BrokerWSUtility.GetBrokersListForMeineke(UserId, ZipCode, City, "MeinekeBenefit", Longitude, Latitude, IndustryId, SubIndustryId);

                    if (dsBrokerDetails.Tables.Count > 0)
                    {
                        if (dsBrokerDetails.Tables[0].Rows.Count > 0)
                        {
                            string SerchForUserId = "";
                            if (strUsersToShowByDefaultInSearchList != "")
                            {
                                SerchForUserId = CheckUserIdInExistingResult(strUsersToShowByDefaultInSearchList, dsBrokerDetails);
                            }

                            if (SerchForUserId != "")
                            {
                                dsBrokerDetails = MergeTwoResults(dsBrokerDetails, SerchForUserId, UserId);
                            }

                            SendUserDetailsForHomeInsurance(dsBrokerDetails);
                        }
                        else
                        {
                            dsBrokerDetails = BrokerWSUtility.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsBrokerDetails.Tables.Count > 0)
                            {
                                if (dsBrokerDetails.Tables[0].Rows.Count > 0)
                                {
                                    SendUserDetailsForHomeInsurance(dsBrokerDetails);
                                }
                                else
                                {
                                    dsBrokerDetails.Tables[0].TableName = "UserDetails";
                                    dsBrokerDetails.Tables[1].TableName = "ExperienceDetails";
                                    dsBrokerDetails.Tables[2].TableName = "EducationDetails";
                                    dsBrokerDetails.Tables[3].TableName = "BrokerContactList";
                                    dsBrokerDetails.AcceptChanges();

                                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsBrokerDetails, "DoSaveBenefitsInsuranceDetails", "true", "null"));
                                }
                            }
                            else
                            {
                                string Message = "";
                                DataSet dsDetails = new DataSet();
                                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBenefitsInsuranceDetails", "false", "Error Occured"));
                            }
                        }
                    }
                    else
                    {
                        // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBenefitsInsuranceDetails", "false", "Error Occured"));

                    }
                }
                else
                {
                    // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBenefitsInsuranceDetails", "false", "Error Occured"));

                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSaveMeinekeBenefitInsuranceDetails", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveMeinekeBenefitInsuranceDetails()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static void DoUpdateProfileCustomerForMeineke(string WSFlag)
        {
            DataSet dsUserDetails = null;
            string UserId = "", FirstName = "", LastName = "", Password = "", Address = "", City = "", PinCode = "", MobNo = "", IsActive = "", UserType = "", IsUpdateProfile = "", State = "", Country = "",
                   HouseType = "", IsHavingCar = "", NoOfCars = "", TypeOfEmployment = "", CompanyName = "", AddressOfHouse = "",
                   NoOfEmp = "", Website = "", EstPremium = "";

            string ProfilePicImagePath = "", IsProfilePicUpdated = "", FileName1 = "", Email = "";

            string binData = "", DOB = "", PhoneNo = "", p1 = "", ProfilePicImg = "";

            try
            {
                string JSonResult = "";
                if (WSFlag == "Application")
                {
                    IsProfilePicUpdated = HttpContext.Current.Request.Form["IsProfilePicUpdated"].ToString();
                    JSonResult = HttpContext.Current.Request.Form["Result"].ToString();
                }
                else
                {
                    IsProfilePicUpdated = HttpContext.Current.Request.QueryString["IsProfilePicUpdated"].ToString();
                    JSonResult = HttpContext.Current.Request.QueryString["Result"].ToString();
                }

                if (JSonResult != "")
                {
                    dsUserDetails = JsonConvert.DeserializeObject<DataSet>(JSonResult);

                    if (dsUserDetails.Tables.Count > 0)
                    {
                        if (dsUserDetails.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsUserDetails.Tables[0].Rows.Count; i++)
                            {
                                UserId = dsUserDetails.Tables[0].Rows[i]["UserId"].ToString();
                                FirstName = dsUserDetails.Tables[0].Rows[i]["FirstName"].ToString();
                                LastName = dsUserDetails.Tables[0].Rows[i]["LastName"].ToString();
                                Address = dsUserDetails.Tables[0].Rows[i]["Address"].ToString();
                                City = dsUserDetails.Tables[0].Rows[i]["City"].ToString();
                                PinCode = dsUserDetails.Tables[0].Rows[i]["PinCode"].ToString();
                                MobNo = dsUserDetails.Tables[0].Rows[i]["MobNo"].ToString();
                                State = dsUserDetails.Tables[0].Rows[i]["State"].ToString();
                                Country = dsUserDetails.Tables[0].Rows[i]["Country"].ToString();
                                HouseType = dsUserDetails.Tables[0].Rows[i]["HouseType"].ToString();
                                AddressOfHouse = dsUserDetails.Tables[0].Rows[i]["AddressOfHouse"].ToString();
                                IsHavingCar = dsUserDetails.Tables[0].Rows[i]["IsHavingCar"].ToString();
                                NoOfCars = dsUserDetails.Tables[0].Rows[i]["NoOfCars"].ToString();
                                TypeOfEmployment = dsUserDetails.Tables[0].Rows[i]["TypeOfEmployment"].ToString();
                                CompanyName = dsUserDetails.Tables[0].Rows[i]["CompanyName"].ToString();

                                //p1 = dsUserDetails.Tables[0].Rows[i]["ProfilePicture"].ToString();

                                DOB = dsUserDetails.Tables[0].Rows[i]["DOB"].ToString();
                                string ProfilePicture = dsUserDetails.Tables[0].Rows[i]["ProfilePicture"].ToString();
                                ProfilePicture = ProfilePicture.Replace(" ", "+");
                                PhoneNo = dsUserDetails.Tables[0].Rows[i]["PhoneNo"].ToString();

                                Website = dsUserDetails.Tables[0].Rows[i]["Website"].ToString();
                                NoOfEmp = dsUserDetails.Tables[0].Rows[i]["NoOfEmp"].ToString();
                                EstPremium = dsUserDetails.Tables[0].Rows[i]["EstPremium"].ToString();

                                /********************************/


                                if (IsProfilePicUpdated == "true")
                                {
                                    List<spGetUserDetails_Result> oUserDetails = null;

                                    oUserDetails = BrokerUtility.GetUserDetails(Convert.ToInt32(UserId));
                                    Email = oUserDetails[0].EmailId;

                                    FileName1 = Email + "_" + UserId + ".txt";
                                    string FileName = HttpContext.Current.Server.MapPath("~/ProfilePicture/" + Email + "_" + UserId + ".txt").ToString();
                                    if (File.Exists(FileName))
                                    {
                                        File.Delete(FileName);
                                    }
                                    // Create a new file 
                                    if (ProfilePicture.ToString().Trim() != "")
                                    {
                                        using (FileStream fs = File.Create(FileName))
                                        {
                                            // Add some text to file
                                            Byte[] title = new UTF8Encoding(true).GetBytes(ProfilePicture);
                                            fs.Write(title, 0, title.Length);
                                        }
                                    }
                                    else
                                    {
                                        FileName1 = "";
                                    }


                                    /********************************/

                                    /*****************************Create Image file for Profile Picture***************/

                                    ProfilePicImagePath = Email + "_" + UserId + ".png";
                                    string ProfilePicImageFullPath = HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + ProfilePicImagePath);

                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + ProfilePicImagePath)))
                                    {
                                        File.Delete(ProfilePicImageFullPath);
                                    }

                                    // Convert Base64 String to byte[]
                                    if (ProfilePicture.ToString().Trim() != "")
                                    {
                                        byte[] imageBytes = Convert.FromBase64String(ProfilePicture);
                                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                                        // Convert byte[] to Image
                                        ms.Write(imageBytes, 0, imageBytes.Length);
                                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                                        image.Save(HttpContext.Current.Server.MapPath("~/UploadedImages/ProfilePicture/" + Email + "_" + UserId + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                    else
                                    {
                                        ProfilePicImagePath = "";
                                    }
                                }

                                /*****************************End of Create Image file for Profile Picture*********/

                                dsUserDetails = BrokerWSUtility.UpdateCustomerForAndroid(UserId, FirstName, LastName, Address, City, State, Country, PinCode, MobNo, HouseType, IsHavingCar, NoOfCars, TypeOfEmployment, CompanyName, DOB, FileName1, PhoneNo, AddressOfHouse, ProfilePicImagePath, IsProfilePicUpdated, NoOfEmp, Website, EstPremium);

                                if (dsUserDetails.Tables[0].Rows.Count > 0)
                                {
                                    string EmailId = dsUserDetails.Tables[0].Rows[0]["EmailId"].ToString();

                                    dsUserDetails = BrokerWSUtility.GetCustomerDetails(EmailId);

                                    if (dsUserDetails.Tables[0].Rows.Count > 0)
                                    {
                                        dsUserDetails.Tables[0].TableName = "UserDetails";
                                        dsUserDetails.AcceptChanges();

                                        //Set ProfilePicture path
                                        binData = dsUserDetails.Tables[0].Rows[0]["ProfilePicture"].ToString();
                                        if (binData != "")
                                        {
                                            binData = strDomainName + "" + strProfilePicForlderName + "" + dsUserDetails.Tables[0].Rows[0]["ProfilePicture"].ToString(); ;

                                            dsUserDetails.Tables[0].Rows[0]["ProfilePicture"] = binData;
                                        }

                                        //Set ProfilePicImg path
                                        ProfilePicImg = dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString();
                                        if (ProfilePicImg != "")
                                        {
                                            ProfilePicImg = strDomainName + "" + strProfilePicImageFolder + "" + dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"].ToString(); ;

                                            dsUserDetails.Tables[0].Rows[0]["ProfilePictureImg"] = ProfilePicImg;
                                        }

                                        //dsUserDetails.Tables[0].Columns.Remove("ProfilePicture");
                                        //dsUserDetails.AcceptChanges();

                                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsUserDetails, "DoUpdateProfileCustomer", "true", "null"));
                                    }
                                    else
                                    {
                                        //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", "null"));
                                        string Message = "";
                                        DataSet dsDetails = new DataSet();
                                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoUpdateProfileCustomer", "false", "Error Occured"));

                                    }
                                }
                                else
                                {
                                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", "null"));
                                    string Message = "";
                                    DataSet dsDetails = new DataSet();
                                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoUpdateProfileCustomer", "false", "Error Occured"));

                                }
                            }

                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoUpdateProfileCustomerForMeineke", Ex.Message.ToString(), "BrokerWSDB.cs_DoUpdateProfileCustomerForMeineke()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }

        }

        public static void DoSave401kInsuranceDetails(string WSFlag)
        {
            DataSet dsUserDetails = null;

            int User, UserId = 0;
            string ZipCode = "", City = "", Longitude = "", Latitude = "", CurrentPlan = "", NoOfEmp = "", PlanSize = "",
                DeclarationDoc = "", DocName = "", DocPath = "";//29May18
            try
            {
                if (WSFlag == "Application")
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"]);
                    ZipCode = HttpContext.Current.Request.Form["ZipCode"].ToString();
                    City = HttpContext.Current.Request.Form["City"].ToString();

                    CurrentPlan = HttpContext.Current.Request.Form["CurrentPlan"].ToString();
                    NoOfEmp = HttpContext.Current.Request.Form["NoOfEmp"].ToString();
                    PlanSize = HttpContext.Current.Request.Form["PlanSize"].ToString();

                    Longitude = HttpContext.Current.Request.Form["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.Form["Latitude"].ToString();

                    DeclarationDoc = HttpContext.Current.Request.Form["DeclarationDoc"].ToString();//29May18
                    DocName = HttpContext.Current.Request.Form["DocName"].ToString();//29May18
                }
                else
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"]);
                    ZipCode = HttpContext.Current.Request.QueryString["ZipCode"].ToString();
                    City = HttpContext.Current.Request.QueryString["City"].ToString();

                    CurrentPlan = HttpContext.Current.Request.QueryString["CurrentPlan"].ToString();
                    NoOfEmp = HttpContext.Current.Request.QueryString["NoOfEmp"].ToString();
                    PlanSize = HttpContext.Current.Request.QueryString["PlanSize"].ToString();

                    Longitude = HttpContext.Current.Request.QueryString["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.QueryString["Latitude"].ToString();

                    DeclarationDoc = HttpContext.Current.Request.QueryString["DeclarationDoc"].ToString();//29May18
                    DocName = HttpContext.Current.Request.QueryString["DocName"].ToString();//29May18
                }

                //29May18
                if (DocName != "" && DeclarationDoc != "")
                {
                    DeclarationDoc = DeclarationDoc.Replace(" ", "+");
                    DocPath = BrokerUtility.SaveDeclarationDocument(DocName, DeclarationDoc, "401kApp", UserId.ToString());
                }

                User = BrokerWSUtility.Save401kInsuranceDetails(UserId, ZipCode, City, Longitude, Latitude, CurrentPlan, NoOfEmp, PlanSize, DocPath, DeclarationDoc); //29May18

                if (User != 0)
                {
                    dsUserDetails = BrokerWSUtility.GetBrokersListForMeineke(UserId, ZipCode, City, "401k", Longitude, Latitude, "0", "0");

                    if (dsUserDetails.Tables.Count > 0)
                    {
                        if (dsUserDetails.Tables[0].Rows.Count > 0)
                        {
                            string SerchForUserId = "";
                            if (strUsersToShowByDefaultInSearchList != "")
                            {
                                SerchForUserId = CheckUserIdInExistingResult(strUsersToShowByDefaultInSearchList, dsUserDetails);
                            }

                            if (SerchForUserId != "")
                            {
                                dsUserDetails = MergeTwoResults(dsUserDetails, SerchForUserId, UserId);
                            }

                            SendUserDetailsForHomeInsurance(dsUserDetails);
                        }
                        else
                        {
                            dsUserDetails = BrokerWSUtility.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsUserDetails.Tables.Count > 0)
                            {
                                if (dsUserDetails.Tables[0].Rows.Count > 0)
                                {
                                    SendUserDetailsForHomeInsurance(dsUserDetails);
                                }
                                else
                                {
                                    dsUserDetails.Tables[0].TableName = "UserDetails";
                                    dsUserDetails.Tables[1].TableName = "ExperienceDetails";
                                    dsUserDetails.Tables[2].TableName = "EducationDetails";
                                    dsUserDetails.Tables[3].TableName = "BrokerContactList";
                                    dsUserDetails.AcceptChanges();

                                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsUserDetails, "DoSaveWorkersCompensationDetails", "true", "null"));
                                }
                            }
                            else
                            {
                                string Message = "";
                                DataSet dsDetails = new DataSet();
                                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveWorkersCompensationDetails", "false", "Error Occured"));
                            }
                        }
                    }
                    else
                    {
                        // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));
                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveLiabilityInsuranceDetails", "false", "Error Occured"));

                    }
                }
                else
                {
                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveAutoInsuranceDetails", "false", "Error Occured"));

                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSaveLiabilityInsuranceDetails", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveLiabilityInsuranceDetails()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static void DoSaveLiabilityInsuranceDetailsAPSP(string WSFlag)
        {
            string ZipCode = "", City = "", IsInsured = "", InsuranceCompany = "", SICCode = "",
                Revenue = "", CoverageExpires = "", Language = "", Notes = "", Longitude = "", Latitude = "",
                IndustryId = "", SubIndustryId = "", DeclarationDoc = "", DocName = "", DocPath = "";//29May18;

            int UserId;

            int User = 0;
            DataSet dsBrokerDetails = null;

            try
            {
                if (WSFlag == "Application")
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"].ToString());
                    ZipCode = HttpContext.Current.Request.Form["ZipCode"].ToString();
                    City = HttpContext.Current.Request.Form["City"].ToString();
                    IsInsured = HttpContext.Current.Request.Form["IsInsured"].ToString();
                    InsuranceCompany = HttpContext.Current.Request.Form["InsuranceCompany"].ToString();
                    SICCode = HttpContext.Current.Request.Form["SICCode"].ToString();
                    Revenue = HttpContext.Current.Request.Form["Revenue"].ToString();
                    CoverageExpires = HttpContext.Current.Request.Form["CoverageExpires"].ToString();
                    Language = HttpContext.Current.Request.Form["Language"].ToString();
                    Notes = HttpContext.Current.Request.Form["Notes"].ToString();

                    Longitude = HttpContext.Current.Request.Form["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.Form["Latitude"].ToString();

                    IndustryId = HttpContext.Current.Request.Form["IndustryId"].ToString();
                    SubIndustryId = HttpContext.Current.Request.Form["SubIndustryId"].ToString();

                    DeclarationDoc = HttpContext.Current.Request.Form["DeclarationDoc"].ToString();//29May18
                    DocName = HttpContext.Current.Request.Form["DocName"].ToString();//29May18

                }
                else
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"].ToString());
                    ZipCode = HttpContext.Current.Request.QueryString["ZipCode"].ToString();
                    City = HttpContext.Current.Request.QueryString["City"].ToString();
                    IsInsured = HttpContext.Current.Request.QueryString["IsInsured"].ToString();
                    InsuranceCompany = HttpContext.Current.Request.QueryString["InsuranceCompany"].ToString();
                    SICCode = HttpContext.Current.Request.QueryString["SICCode"].ToString();
                    Revenue = HttpContext.Current.Request.QueryString["Revenue"].ToString();
                    CoverageExpires = HttpContext.Current.Request.QueryString["CoverageExpires"].ToString();
                    Language = HttpContext.Current.Request.QueryString["Language"].ToString();
                    Notes = HttpContext.Current.Request.QueryString["Notes"].ToString();

                    Longitude = HttpContext.Current.Request.QueryString["Longitude"].ToString();
                    Latitude = HttpContext.Current.Request.QueryString["Latitude"].ToString();

                    IndustryId = HttpContext.Current.Request.QueryString["IndustryId"].ToString();
                    SubIndustryId = HttpContext.Current.Request.QueryString["SubIndustryId"].ToString();

                    DeclarationDoc = HttpContext.Current.Request.QueryString["DeclarationDoc"].ToString();//29May18
                    DocName = HttpContext.Current.Request.QueryString["DocName"].ToString();//29May18
                }

                //29May18
                if (DocName != "" && DeclarationDoc != "")
                {
                    DeclarationDoc = DeclarationDoc.Replace(" ", "+");
                    DocPath = BrokerUtility.SaveDeclarationDocument(DocName, DeclarationDoc, "LiabilityApp", UserId.ToString());
                }

                User = BrokerWSUtility.SaveLiabilityInsuranceDetailsNew(UserId, ZipCode, City, IsInsured, InsuranceCompany, SICCode, Revenue, CoverageExpires, Language, Notes, Longitude, Latitude, IndustryId, SubIndustryId, DocPath, DeclarationDoc);

                if (User != 0)
                {
                    //dsBrokerDetails = BrokerWSUtility.GetBrokersList(UserId, ZipCode, City, Language, "Liability", Longitude, Latitude, Revenue, IndustryId, SubIndustryId);
                    dsBrokerDetails = BrokerWSUtility.GetBrokersListForMeineke(UserId, ZipCode, City, "Liability", Longitude, Latitude, "0", "0");

                    if (dsBrokerDetails.Tables.Count > 0)
                    {
                        if (dsBrokerDetails.Tables[0].Rows.Count > 0)
                        {
                            string SerchForUserId = "";
                            if (strUsersToShowByDefaultInSearchList != "")
                            {
                                SerchForUserId = CheckUserIdInExistingResult(strUsersToShowByDefaultInSearchList, dsBrokerDetails);
                            }

                            if (SerchForUserId != "")
                            {
                                dsBrokerDetails = MergeTwoResults(dsBrokerDetails, SerchForUserId, UserId);
                            }

                            SendUserDetailsForHomeInsurance(dsBrokerDetails);
                        }
                        else
                        {
                            dsBrokerDetails = BrokerWSUtility.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsBrokerDetails.Tables.Count > 0)
                            {
                                if (dsBrokerDetails.Tables[0].Rows.Count > 0)
                                {
                                    SendUserDetailsForHomeInsurance(dsBrokerDetails);
                                }
                                else
                                {
                                    dsBrokerDetails.Tables[0].TableName = "UserDetails";
                                    dsBrokerDetails.Tables[1].TableName = "ExperienceDetails";
                                    dsBrokerDetails.Tables[2].TableName = "EducationDetails";
                                    dsBrokerDetails.Tables[3].TableName = "BrokerContactList";
                                    dsBrokerDetails.AcceptChanges();

                                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsBrokerDetails, "DoSavBusinessInsuranceDetails", "true", "null"));
                                }
                            }
                            else
                            {
                                string Message = "";
                                DataSet dsDetails = new DataSet();
                                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));
                            }
                        }
                    }
                    else
                    {
                        //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));

                    }
                }
                else
                {
                    // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveLiabilityInsuranceDetailsAPSP", "false", "Error Occured"));

                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSaveLiabilityInsuranceDetailsAPSP", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveLiabilityInsuranceDetailsAPSP()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveLiabilityInsuranceDetailsAPSP", "false", "Error occured, please try again."));
            }
        }


        public static void DoGetVideoList(string WSFlag)
        {
            int UserId;

            int User = 0;
            DataSet dsVideoDetails = null;

            try
            {
                if (WSFlag == "Application")
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"].ToString());
                }
                else
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"].ToString());
                }

                dsVideoDetails = BrokerWSUtility.GetVideoList(UserId);
                if (dsVideoDetails.Tables.Count > 0)
                {
                    if (dsVideoDetails.Tables[0].Rows.Count > 0)
                    {
                        dsVideoDetails.Tables[0].TableName = "VideoDetails";
                        dsVideoDetails.Tables[1].TableName = "UnWachedVideoDetails";
                        dsVideoDetails.AcceptChanges();

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsVideoDetails, "DoGetVideoList", "true", "null"));
                    }
                    else
                    {
                        //string VideoDetails = "";
                        //DataSet dsDetails = new DataSet();
                        //dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(VideoDetails);

                        //HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetVideoList", "true", "null"));
                        dsVideoDetails.Tables[0].TableName = "VideoDetails";
                        dsVideoDetails.Tables[1].TableName = "UnWachedVideoDetails";
                        dsVideoDetails.AcceptChanges();

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsVideoDetails, "DoGetVideoList", "true", "null"));
                    }
                }
                else
                {
                    string VideoDetails = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(VideoDetails);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetVideoList", "false", "null"));
                }

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoGetVideoList", Ex.Message.ToString(), "BrokerWSDB.cs_DoGetVideoList()", "");

                string VideoDetails = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(VideoDetails);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetVideoList", "false", "Error occured, please try again."));
            }
        }

        #endregion Meineke Insurance

        public static void DoGetUnWatchedVideoCount(string WSFlag)
        {
            int UserId;

            int User = 0;
            DataSet dsVideoDetails = null;

            try
            {
                if (WSFlag == "Application")
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"].ToString());
                }
                else
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"].ToString());
                }

                dsVideoDetails = BrokerWSUtility.GetUnWatchedVideoCount(UserId);
                if (dsVideoDetails.Tables.Count > 0)
                {
                    if (dsVideoDetails.Tables[0].Rows.Count > 0)
                    {
                        dsVideoDetails.Tables[0].TableName = "VideoDetails";

                        dsVideoDetails.AcceptChanges();

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsVideoDetails, "DoGetVideoList", "true", "null"));
                    }
                    else
                    {
                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetVideoList", "false", "null"));
                    }
                }
                else
                {
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetVideoList", "false", "null"));
                }

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoGetVideoList", Ex.Message.ToString(), "BrokerWSDB.cs_DoGetVideoList()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetVideoList", "false", "Error occured, please try again."));
            }
        }

        public static void DoSetVideoWatched(string WSFlag)
        {
            int UserId, VideoId;
            int Result = 0;
            DataSet dsVideoDetails = null;

            bool Res = false;
            try
            {
                if (WSFlag == "Application")
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"].ToString());
                    VideoId = Convert.ToInt32(HttpContext.Current.Request.Form["VideoId"].ToString());
                }
                else
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"].ToString());
                    VideoId = Convert.ToInt32(HttpContext.Current.Request.QueryString["VideoId"].ToString());
                }

                Result = BrokerWSUtility.SetVideoWatched(UserId, VideoId);

                if (Result != 0)
                {
                    Res = true;
                }
                else
                {
                    Res = false;
                }

                if (Res == true)
                {
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("IsWatched Flag Set to true");

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSetVideoWatched", "true", "null"));
                }
                else
                {
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("");

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSetVideoWatched", "false", "Error Occured"));
                }

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSetVideoWatched", Ex.Message.ToString(), "BrokerWSDB.cs_DoSetVideoWatched()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSetVideoWatched", "false", "Error occured, please try again."));
            }
        }

        public static void DoSetAllVideoWatchedForWeb(string WSFlag)
        {
            int UserId, VideoId;
            int Result = 0;
            DataSet dsVideoDetails = null;

            bool Res = false;
            try
            {
                if (WSFlag == "Application")
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.Form["UserId"].ToString());

                }
                else
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Request.QueryString["UserId"].ToString());

                }

                Result = BrokerWSUtility.SetAllVideoWatchedForWeb(UserId);

                if (Result != 0)
                {
                    Res = true;
                }
                else
                {
                    Res = false;
                }

                if (Res == true)
                {
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("IsWatched Flag Set to true");

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSetVideoWatched", "true", "null"));
                }
                else
                {
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage("");

                    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSetVideoWatched", "false", "Error Occured"));
                }

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSetVideoWatched", Ex.Message.ToString(), "BrokerWSDB.cs_DoSetVideoWatched()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSetVideoWatched", "false", "Error occured, please try again."));
            }
        }

        public static void DoSendNotification(string WSFlag)
        {
            string DeviceId = "", message = "", title = "", msgcnt = "", UserId = "", Notification = "";
            DataSet dsData = new DataSet();
            DataSet dsUnwatchedVideoCount = new DataSet();
            int badge = 0, UnWatchedVideoCnt = 0;

            DataSet dsDeviceId = null;

            try
            {
                if (WSFlag == "Application")
                {
                    message = HttpContext.Current.Request.Form["Message"];
                    title = HttpContext.Current.Request.Form["title"];
                    msgcnt = HttpContext.Current.Request.Form["msgcnt"];
                    UserId = HttpContext.Current.Request.Form["UserId"];
                }
                else
                {
                    message = HttpContext.Current.Request.QueryString["Message"];
                    title = HttpContext.Current.Request.QueryString["title"];
                    msgcnt = HttpContext.Current.Request.QueryString["msgcnt"];
                    UserId = HttpContext.Current.Request.QueryString["UserId"];
                }

                dsDeviceId = BrokerWSUtility.GetDeviceId(Convert.ToInt32(UserId));

                if (dsDeviceId.Tables.Count > 0)
                {
                    if (dsDeviceId.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsDeviceId.Tables[0].Rows.Count; i++)
                        {
                           
                            DeviceId = dsDeviceId.Tables[0].Rows[i]["DeviceId"].ToString();

                            BrokerWSUtility.SaveWebsiteLog(Convert.ToInt32(UserId), "DeviceId", DeviceId);

                            if (DeviceId.Contains("Android"))
                            {
                                DoPushNotification(DeviceId.Replace("Android", "").Replace("iOS", ""), message, title, msgcnt, Convert.ToInt32(UserId));
                                BrokerWSUtility.SaveWebsiteLog(Convert.ToInt32(UserId), "In DoPushNotification", DeviceId);

                            }
                            else if (DeviceId.Contains("iOS"))
                            {
                                DoPushNotificationForiOS(DeviceId.Replace("Android", "").Replace("iOS", ""), message, title, msgcnt, Convert.ToInt32(UserId));
                                BrokerWSUtility.SaveWebsiteLog(Convert.ToInt32(UserId), "In DoPushNotificationForiOS", DeviceId);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSendNotification", Ex.Message.ToString(), "BrokerWSDB.cs_DoSendNotification", "");
                BrokerWSUtility.SaveWebsiteLog(0, "ERROR", "Error in BrokerWSDB.cs_DoSendNotification()");
                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSendNotification", "false", "Error occured, please try again."));
            }
        }

        public static void DoGetAllChatMessages(string WSFlag)
        {
            DataSet dsChatMessages = null;

            dsChatMessages = BrokerWSUtility.GetAllChatMessages();

            if (dsChatMessages.Tables[0].Rows.Count > 0)
            {
                dsChatMessages.Tables[0].TableName = "Response";
                dsChatMessages.AcceptChanges();

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsChatMessages, "DoGetAllChatMessages", "true", "null"));
            }
            else
            {               
                string Message = "No Records Found";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetAllChatMessages", "true", "null"));
            }            
        }
    }
}