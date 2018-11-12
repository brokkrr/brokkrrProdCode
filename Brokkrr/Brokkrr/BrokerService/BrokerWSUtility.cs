using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;
using System.Data;
using System.Web.Script.Serialization;
using System.Text;
using BrokerMVC.App_Code;
using System.Data.SqlClient;
using System.Net.Mail;
using BrokerMVC.Models;
using System.Configuration;
using System.Net.Mime;
using System.Net;

namespace BrokerMVC.BrokerService
{
    public class BrokerWSUtility
    {
        public static string strActivateUserPageLinkForApp = ConfigurationManager.AppSettings["ActivateUserPageLinkForApp"].ToString();
        public static string strMailTestingFlag = ConfigurationManager.AppSettings["EmailFlag"].ToString();
        public static string strActivateUserPageLink = ConfigurationManager.AppSettings["ActivateUserPageLink"].ToString();
        public static string strServerLink = ConfigurationManager.AppSettings["ServerLink"].ToString();
        public static string strFromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
        public static string BrokerSearchWithinMiles = ConfigurationManager.AppSettings["BrokerSearchWithinMiles"].ToString();
        public static string strResumeForlderName = ConfigurationManager.AppSettings["ResumeForlderName"].ToString();

        public static string strResetPasswordPageLink = ConfigurationManager.AppSettings["ResetPasswordPageLink"].ToString();

        public static string strDomainName = ConfigurationManager.AppSettings["DomainName"].ToString();
        public static string strProfilePicForlderName = ConfigurationManager.AppSettings["ProfilePicForlderName"].ToString();

        public static string strProfilePicImageFolder = ConfigurationManager.AppSettings["ProfilePicImageFolder"].ToString();
        public static string strResumeImageFolder = ConfigurationManager.AppSettings["ResumeImageFolder"].ToString();
        public static string strUploadedCompLogoFolder = ConfigurationManager.AppSettings["UploadedCompLogoFolder"].ToString();
        public static string strExperienceCompLogoFolder = ConfigurationManager.AppSettings["ExperienceCompLogoFolder"].ToString();
        public static string strEducationLogoFolder = ConfigurationManager.AppSettings["EducationLogo"].ToString();

        public static string strCompanyLogoFolder = ConfigurationManager.AppSettings["CompanyLogoFolder"].ToString();

        #region Common Methods

        //Get Users List from DB
        public static DataSet GetUsersList()
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            DataSet dt = null;
            int user = 0;

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetUsersList");

                dt = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "GetUsersList", Ex.Message.ToString(), "BrokerWSUtility.cs_GetUsersList()", "");
            }

            return dt;
        }

        //Check user exist in DB
        public static DataSet CheckUserExistInDB(int UserId)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            DataSet dt = null;
            int user = 0;

            try
            {
                //var @cmdText = "exec uspCheckUserExistInDB @UserId";
                //var @params = new[]{
                //               new SqlParameter("@UserId", UserId)
                //              };

                //user = DB.Database.ExecuteSqlCommand(@cmdText, @params);

                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspCheckUserExistInDB");
                db.AddInParameter(dbCommand, "UserId", DbType.Int32, UserId);


                dt = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "CheckUserExistInDB", Ex.Message.ToString(), "BrokerWSUtility.cs_CheckUserExistInDB()", BrokerUtility.GetIPAddress(UserId.ToString()));
            }

            return dt;
        }


        //Check User regualr login
        public static string CheckLogin(string UserName, string Password, string UserType)
        {
            string LoginJson = "";
            BrokerDBEntities DB = new BrokerDBEntities();
            DataSet dsCheckLogin = null;
            string TempPass = BrokerUtility.EncryptURL(Password);
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("spCheckUserLogin");
                db.AddInParameter(dbCommand, "UserName", DbType.String, UserName);
                db.AddInParameter(dbCommand, "Password", DbType.String, TempPass);
                //db.AddInParameter(dbCommand, "UserType", DbType.String, UserType);

                dsCheckLogin = db.ExecuteDataSet(dbCommand);

                if (dsCheckLogin.Tables.Count > 0)
                {
                    if (dsCheckLogin.Tables[0].Rows[0]["UserType"].ToString() == UserType)
                    {
                        if (dsCheckLogin.Tables[0].Rows[0]["UserType"].ToString() == "Broker")
                        {
                            dsCheckLogin.Tables[0].TableName = "UserDetails";
                            dsCheckLogin.Tables[1].TableName = "ExperienceDetails";
                            dsCheckLogin.Tables[2].TableName = "EducationDetails";
                            dsCheckLogin.AcceptChanges();

                            string binData = dsCheckLogin.Tables[0].Rows[0]["ProfilePicture"].ToString();
                            if (binData != "")
                            {
                                binData = strDomainName + "" + strProfilePicForlderName + "" + dsCheckLogin.Tables[0].Rows[0]["ProfilePicture"].ToString();

                                dsCheckLogin.Tables[0].Rows[0]["ProfilePicture"] = binData;
                            }

                            string ResumeData = dsCheckLogin.Tables[0].Rows[0]["Resume"].ToString();
                            if (ResumeData != "")
                            {
                                ResumeData = strDomainName + "" + strResumeForlderName + "" + dsCheckLogin.Tables[0].Rows[0]["Resume"].ToString();

                                dsCheckLogin.Tables[0].Rows[0]["Resume"] = ResumeData;
                            }

                            ////////////////////////////////////////////////////////////
                            string ProfilePicImg = dsCheckLogin.Tables[0].Rows[0]["ProfilePictureImg"].ToString();
                            if (ProfilePicImg != "")
                            {
                                ProfilePicImg = strDomainName + "" + strProfilePicImageFolder + "" + dsCheckLogin.Tables[0].Rows[0]["ProfilePictureImg"].ToString();

                                dsCheckLogin.Tables[0].Rows[0]["ProfilePictureImg"] = ProfilePicImg;
                            }

                            string ResumeImg = dsCheckLogin.Tables[0].Rows[0]["ResumeDoc"].ToString();
                            if (ResumeImg != "")
                            {
                                ResumeImg = strDomainName + "" + strResumeImageFolder + "" + dsCheckLogin.Tables[0].Rows[0]["ResumeDoc"].ToString(); ;

                                dsCheckLogin.Tables[0].Rows[0]["ResumeDoc"] = ResumeImg;
                            }

                            string CompanyLogo = dsCheckLogin.Tables[0].Rows[0]["CompanyLogo"].ToString();
                            if (CompanyLogo != "")
                            {
                                CompanyLogo = strDomainName + "" + strUploadedCompLogoFolder + "" + dsCheckLogin.Tables[0].Rows[0]["CompanyLogo"].ToString();

                                dsCheckLogin.Tables[0].Rows[0]["CompanyLogo"] = CompanyLogo;
                            }

                            if (dsCheckLogin.Tables["ExperienceDetails"].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsCheckLogin.Tables["ExperienceDetails"].Rows.Count; i++)
                                {
                                    string Logo = dsCheckLogin.Tables["ExperienceDetails"].Rows[i]["Logo"].ToString();
                                    if (Logo != "")
                                    {
                                        Logo = strDomainName + "" + strExperienceCompLogoFolder + "" + dsCheckLogin.Tables["ExperienceDetails"].Rows[i]["Logo"].ToString(); ;

                                        dsCheckLogin.Tables["ExperienceDetails"].Rows[i]["Logo"] = Logo;
                                    }
                                }
                            }

                            /******************************Add Server path to School logo ********************************/
                            if (dsCheckLogin.Tables["EducationDetails"].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsCheckLogin.Tables["EducationDetails"].Rows.Count; i++)
                                {
                                    string Logo = dsCheckLogin.Tables["EducationDetails"].Rows[i]["EducationLogo"].ToString();
                                    if (Logo != "")
                                    {
                                        Logo = strDomainName + "" + strEducationLogoFolder + "" + dsCheckLogin.Tables["EducationDetails"].Rows[i]["EducationLogo"].ToString(); ;

                                        dsCheckLogin.Tables["EducationDetails"].Rows[i]["EducationLogo"] = Logo;
                                    }
                                }
                            }
                            /******************************End of Add Server path to School logo ********************************/

                        }
                        else
                        {
                            dsCheckLogin.Tables[0].TableName = "UserDetails";
                            dsCheckLogin.AcceptChanges();

                            string binData = dsCheckLogin.Tables[0].Rows[0]["ProfilePicture"].ToString();
                            if (binData != "")
                            {
                                binData = strDomainName + "" + strProfilePicForlderName + "" + dsCheckLogin.Tables[0].Rows[0]["ProfilePicture"].ToString();

                                dsCheckLogin.Tables[0].Rows[0]["ProfilePicture"] = binData;
                            }

                            ///////////////////////////////////////////////////

                            string ProfilePicImg = dsCheckLogin.Tables[0].Rows[0]["ProfilePictureImg"].ToString();
                            if (ProfilePicImg != "")
                            {
                                ProfilePicImg = strDomainName + "" + strProfilePicImageFolder + "" + dsCheckLogin.Tables[0].Rows[0]["ProfilePictureImg"].ToString(); ;

                                dsCheckLogin.Tables[0].Rows[0]["ProfilePictureImg"] = ProfilePicImg;
                            }

                        }
                        LoginJson = CreateJsonFromDataset(dsCheckLogin, "DoLogin", "true", "null");
                    }
                    else
                    {
                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        LoginJson = CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Invalid User.");
                    }
                }
                else
                {
                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    LoginJson = CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Wrong Credentials/User is not active.");
                }
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(0, "CheckLogin", ex.Message.ToString(), "BrokerWSUtility.cs_CheckLogin()", "");
            }
            return LoginJson;
        }

        //Verify emailid from activation link which is sent after successfull regular signup.
        public static DataSet VerifyUserDetails(string EmailId, string RegistrationCode)
        {
            DataSet dsVerifyUser = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspVerifyEmailId");
                db.AddInParameter(dbCommand, "EmailId", DbType.String, EmailId);
                db.AddInParameter(dbCommand, "RegistrationCode", DbType.String, RegistrationCode);

                dsVerifyUser = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(0, "VerifyUserDetails", ex.Message.ToString(), "BrokerWSUtility.cs_VerifyUserDetails()", "0");
            }
            return dsVerifyUser;
        }

        //convert class object data to JSON formate
        public static string CreateJSONOutput(Object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        //Create json result for Error
        public static string createjsonForError(string strstatus, string responseData)
        {
            StringBuilder JsonString = new StringBuilder();
            JsonString.Append("{");
            JsonString.Append("\"ErrorMessage\":");
            JsonString.Append("\"" + strstatus + "\",");
            JsonString.Append("\"ResponseData\":");
            JsonString.Append("\"" + responseData + "\"");
            JsonString.Append("}");
            return JsonString.ToString();

        }
        //Create json result for Success
        public static string createjsonForSuccess(string strstatus, string responseData)
        {
            StringBuilder JsonString = new StringBuilder();
            JsonString.Append("{");
            JsonString.Append("\"SuccessMessage\":");
            JsonString.Append("\"" + strstatus + "\",");
            JsonString.Append("\"ResponseData\":");
            JsonString.Append("\"" + responseData + "\"");
            JsonString.Append("}");
            return JsonString.ToString();

        }

        //Create json result for Successfully regular Signup
        public static string createjsonForSignupSuccess(string strstatus, string responseData, bool EmailFlag)
        {
            StringBuilder JsonString = new StringBuilder();
            JsonString.Append("{");
            JsonString.Append("\"SuccessMessage\":");
            JsonString.Append("\"" + strstatus + "\",");
            JsonString.Append("\"ResponseData\":");
            JsonString.Append("\"" + responseData + "\",");
            JsonString.Append("\"EmailSuccess\":");
            JsonString.Append("\"" + EmailFlag + "\"");
            JsonString.Append("}");
            return JsonString.ToString();

        }

        //Create json result for Success
        public static string CreateJsonResultForSuccess(DataSet ds, string Actionname, string IsSuccess, string ErrorMessage)
        {
            HttpContext Context = HttpContext.Current;
            StringBuilder JsonString = new StringBuilder();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    JsonString.Append("{\"IsSuccess\": " + IsSuccess + ",");
                    JsonString.Append("\"ErrorMessage\": " + ErrorMessage + ",");
                    for (int a = 0; a < ds.Tables.Count; a++)
                    {
                        JsonString.Append("\"" + ds.Tables[a].TableName + "\":[ ");
                        foreach (DataRow dr in ds.Tables[a].Rows)
                        {
                            JsonString.Append("{ ");
                            foreach (DataColumn col in ds.Tables[a].Columns)
                            {
                                JsonString.Append("\"" + col.ColumnName.Trim() + "\" : \"" + dr[col] + "\" ");

                                if (ds.Tables[a].Columns.IndexOf(col) == ds.Tables[a].Columns.Count - 1)
                                {
                                    JsonString.Append("");
                                    // this is the last item
                                }
                                else
                                {
                                    JsonString.Append(",");
                                }
                            }
                            if (ds.Tables[a].Rows.IndexOf(dr) == ds.Tables[a].Rows.Count - 1)
                            {
                                JsonString.Append("} ");
                                // this is the last item
                            }
                            else
                            {
                                JsonString.Append("}, ");
                            }
                        }
                        if (a == ds.Tables.Count - 1)
                        {
                            JsonString.Append("] ");
                        }
                        else
                        {
                            JsonString.Append("], ");
                        }

                    }
                    JsonString.Append("} ");
                }

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "CreateJsonResultForSuccess", Ex.Message.ToString(), "BrokerWSUtility.cs_CreateJsonResultForSuccess()", "");
            }
            return JsonString.ToString();
        }


        //Create json result for Dataset to JSON
        public static string CreateJsonFromDataset(DataSet ds, string Actionname, string IsSuccess, string ErrorMessage)
        {
            HttpContext Context = HttpContext.Current;
            StringBuilder JsonString = new StringBuilder();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    JsonString.Append("{\"IsSuccess\": " + IsSuccess + ",");
                    if (IsSuccess == "true")
                    {
                        JsonString.Append("\"ErrorMessage\": " + ErrorMessage + ",");
                    }
                    else
                    {
                        JsonString.Append("\"ErrorMessage\": \"" + ErrorMessage + "\",");
                    }
                    for (int a = 0; a < ds.Tables.Count; a++)
                    {
                        JsonString.Append("\"" + ds.Tables[a].TableName + "\":[ ");
                        foreach (DataRow dr in ds.Tables[a].Rows)
                        {
                            JsonString.Append("{ ");
                            foreach (DataColumn col in ds.Tables[a].Columns)
                            {
                                JsonString.Append("\"" + col.ColumnName.Trim() + "\" : \"" + dr[col] + "\" ");

                                if (ds.Tables[a].Columns.IndexOf(col) == ds.Tables[a].Columns.Count - 1)
                                {
                                    JsonString.Append("");
                                    // this is the last item
                                }
                                else
                                {
                                    JsonString.Append(",");
                                }
                            }
                            if (ds.Tables[a].Rows.IndexOf(dr) == ds.Tables[a].Rows.Count - 1)
                            {
                                JsonString.Append("} ");
                                // this is the last item
                            }
                            else
                            {
                                JsonString.Append("}, ");
                            }
                        }
                        if (a == ds.Tables.Count - 1)
                        {
                            JsonString.Append("] ");
                        }
                        else
                        {
                            JsonString.Append("], ");
                        }

                    }
                    JsonString.Append("} ");
                }

            }
            catch (Exception Ex)
            {
                // throw ex;
                //strRetVal = mAPDB.createjsonforFail(Actionname);
                //Context.Response.Write(strRetVal);
                BrokerUtility.ErrorLog(0, "CreateJsonFromDataset", Ex.Message.ToString(), "BrokerWSUtility.cs_CreateJsonFromDataset()", "");
            }
            return JsonString.ToString();
        }


        //Create json result for Dataset to JSON
        public static string CreateJsonFromDatasetNew(DataSet ds, string Actionname, string IsSuccess, string ErrorMessage, string IsNewRegister)
        {
            HttpContext Context = HttpContext.Current;
            StringBuilder JsonString = new StringBuilder();
            try
            {
                if (ds.Tables.Count > 0)
                {
                    JsonString.Append("{\"IsSuccess\": " + IsSuccess + ",");
                    if (IsSuccess == "true")
                    {
                        JsonString.Append("\"ErrorMessage\": " + ErrorMessage + ",");
                    }
                    else
                    {
                        JsonString.Append("\"ErrorMessage\": \"" + ErrorMessage + "\",");
                    }
                    JsonString.Append("\"IsNewRegister\": " + IsNewRegister + ",");
                    for (int a = 0; a < ds.Tables.Count; a++)
                    {
                        JsonString.Append("\"" + ds.Tables[a].TableName + "\":[ ");
                        foreach (DataRow dr in ds.Tables[a].Rows)
                        {
                            JsonString.Append("{ ");
                            foreach (DataColumn col in ds.Tables[a].Columns)
                            {
                                JsonString.Append("\"" + col.ColumnName.Trim() + "\" : \"" + dr[col] + "\" ");

                                if (ds.Tables[a].Columns.IndexOf(col) == ds.Tables[a].Columns.Count - 1)
                                {
                                    JsonString.Append("");
                                    // this is the last item
                                }
                                else
                                {
                                    JsonString.Append(",");
                                }
                            }
                            if (ds.Tables[a].Rows.IndexOf(dr) == ds.Tables[a].Rows.Count - 1)
                            {
                                JsonString.Append("} ");
                                // this is the last item
                            }
                            else
                            {
                                JsonString.Append("}, ");
                            }
                        }
                        if (a == ds.Tables.Count - 1)
                        {
                            JsonString.Append("] ");
                        }
                        else
                        {
                            JsonString.Append("], ");
                        }

                    }
                    JsonString.Append("} ");
                }

            }
            catch (Exception ex)
            {
                // throw ex;
                //strRetVal = mAPDB.createjsonforFail(Actionname);
                //Context.Response.Write(strRetVal);
            }
            return JsonString.ToString();
        }


        //Create Randome number for Registration Code.
        public static string GetRandomNumber()
        {
            Random r = new Random();
            int rInt = r.Next(0, 100); //for ints
            int range = 100;
            int rDouble = r.Next() * range; //for doubles
            return rDouble.ToString();
        }

        //Send Successfull Registration Email for Regular Signup
        public static bool SendRegistrationEmail(string EmailId, string RegistrationCode, string UserId, string UserType)
        {
            bool SuccessFlag = false;
            EmailInfoModel oEmail = new EmailInfoModel();
            StringBuilder Obody = new StringBuilder();
            try
            {
                Obody.Append("<html><head></head><body>Hi ,<br/><br/>");
                Obody.Append("Greetings from Team brokkrr! <br/><br/>Thank you for registering  with our app as " + UserType + ".  For your security, we need you to verify your email id.<br/> ");
                //Obody.Append("Please <a href='" + strServerLink + strActivateUserPageLink + "?EmailId=" + EmailId + "&RegistrationCode=" + RegistrationCode + "'>click here</a> to varify your email id.");
                Obody.Append("Please <a href='" + strServerLink + strActivateUserPageLinkForApp + "?EmailId=" + BrokerUtility.EncryptURL(EmailId) + "&RegistrationCode=" + RegistrationCode + "'>click here</a> to verify your email address.<br/><br/><br/>");
                Obody.Append("Thank you,<br/>");
                Obody.Append("Anton and Curtis,<br/>");
                Obody.Append("brokkrr App Team.</body></html>");


                oEmail.Message = Obody.ToString();
                oEmail.Subject = "Welcome to brokkrr App.";
                oEmail.ToEmailid = EmailId;
                oEmail.FromEmailid = strFromEmail;
                oEmail.MailType = "NewSignUp";
                oEmail.UserId = UserId;
                SuccessFlag = sendmail(oEmail);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SendRegistrationEmail_website", ex.Message.ToString(), "BrokerWSUtility.cs_SendRegistrationEmail()", "");
            }
            return SuccessFlag;

        }

        //Send Email For Forget Password.
        public static bool SendForgetPasswordEmail(string EmailId, string RandomNumber)
        {
            bool SuccessFlag = false;
            EmailInfoModel oEmail = new EmailInfoModel();
            StringBuilder Obody = new StringBuilder();
            try
            {
                Obody.Append("<html><head></head><body>Hi ,<br/><br/>");
                //Obody.Append("Click on the link .<br/>");
                //Obody.Append("Please <a href='" + strServerLink + strActivateUserPageLink + "?EmailId=" + EmailId + "&RegistrationCode=" + RegistrationCode + "'>click here</a> to varify your email id.");
                Obody.Append("Please <a href='" + strServerLink + strResetPasswordPageLink + "?EmailId=" + BrokerUtility.EncryptURL(EmailId) + "&Code=" + RandomNumber + "'>click here</a> to reset your password.<br/><br/><br/>");
                //Obody.Append("Please <a href='" + strServerLink + strResetPasswordPageLink + "/" + EmailId + "'>click here</a> to reset your password.<br/><br/><br/>");
                Obody.Append("Thank You,<br/>");
                Obody.Append("Anton and Curtis,<br/>");
                Obody.Append("brokkrr App Team.</body></html>");


                oEmail.Message = Obody.ToString();
                oEmail.Subject = "brokkrr App - Reset Password";
                oEmail.ToEmailid = EmailId;
                oEmail.FromEmailid = strFromEmail;
                oEmail.MailType = "ResetPassword";
                oEmail.UserId = "0";
                SuccessFlag = sendmail(oEmail);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(0, "SendForgetPasswordEmail_Website", ex.Message.ToString(), "BrokerWSUtility.cs_SendForgetPasswordEmail()", "0");
            }
            return SuccessFlag;

        }

        //Send Successfull Registration Email for Regular Signup
        public static bool SendRegistrationEmailForExternal(string FirstName, string LastName, string EmailId, string UserId, string Provider, string UserType)
        {
            bool SuccessFlag = false;
            EmailInfoModel oEmail = new EmailInfoModel();
            StringBuilder Obody = new StringBuilder();
            try
            {
                Obody.Append("<html><head></head><body>Hi " + FirstName + " " + LastName + ",<br/><br/>");
                Obody.Append("Greetings from Team brokkrr!<br/><br/> You have registered successfully as " + UserType + " using " + Provider + ".<br/>Please log in to continue. <br/><br/>");
                Obody.Append("Thank you,<br/>");
                Obody.Append("Anton and Curtis,<br/>");
                Obody.Append("brokkrr App Team.</body></html>");
                oEmail.Message = Obody.ToString();
                oEmail.Subject = "Welcome to brokkrr App.";
                oEmail.ToEmailid = EmailId;
                oEmail.FromEmailid = strFromEmail;
                oEmail.MailType = "NewSignUp";
                oEmail.UserId = UserId;
                SuccessFlag = sendmail(oEmail);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SendRegistrationEmailForExternal", ex.Message.ToString(), "BrokerWSUtility.cs_SendRegistrationEmailForExternal()", "");
            }
            return SuccessFlag;

        }

        // Main send email function
        public static bool sendmail(EmailInfoModel oEmail)
        {
            bool mailFlag = false;
            SmtpClient smtpclient = new SmtpClient();
            MailMessage Mailmsg = null;

            try
            {
                ////for testing purpose
                if (strMailTestingFlag == "false")
                {
                    oEmail.FromEmailid = ConfigurationManager.AppSettings["FromEmailForTest"].ToString();
                    oEmail.ToEmailid = ConfigurationManager.AppSettings["ToEmailForTest"].ToString();
                }

                //Start For Regular mail sending code(Used on Server)

                MailAddress FromUser = new MailAddress(oEmail.FromEmailid);
                AlternateView av = AlternateView.CreateAlternateViewFromString(oEmail.Message, null, MediaTypeNames.Text.Html);
                Mailmsg = new MailMessage(oEmail.FromEmailid, oEmail.ToEmailid);
                Mailmsg.From = FromUser;
                string[] Toemailid = oEmail.ToEmailid.Split(',');
                for (int i = 0; i < Toemailid.Length; i++)
                {
                    Mailmsg.Bcc.Add(Toemailid[i].ToString());

                }

                Mailmsg.Subject = oEmail.Subject;
                Mailmsg.AlternateViews.Add(av);
                Mailmsg.IsBodyHtml = true;
                Mailmsg.Body = oEmail.Message;

                smtpclient.Send(Mailmsg); //For Production
                //sendMailUsingGmail(oEmail);   //For Testing Purpose at local

                mailFlag = true;

            }
            catch (SmtpException ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(oEmail.UserId), "sendmail", ex.Message.ToString(), "BrokerWSUtility.cs_sendmail()", "");
            }
            finally
            {
                oEmail = null;
            }
            return mailFlag;
        }

        public static void sendMailUsingGmail(EmailInfoModel oEmail)
        {
            try
            {
                string strPassword = ConfigurationManager.AppSettings["FromEmailPass"].ToString();
                string strHost = ConfigurationManager.AppSettings["Host"].ToString();
                string strPort = ConfigurationManager.AppSettings["Port"].ToString();

                ////Start For Gmail mail sending Code

                var fromAddress = new MailAddress(oEmail.FromEmailid, "");
                var toAddress = new MailAddress(oEmail.ToEmailid, "");
                AlternateView av = AlternateView.CreateAlternateViewFromString(oEmail.Message, null, MediaTypeNames.Text.Html);
                var smtp = new SmtpClient
                {
                    Host = strHost,
                    Port = Convert.ToInt32(strPort),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, strPassword)
                };
                var message = new MailMessage(fromAddress, toAddress);
                message.Subject = oEmail.Subject;
                message.Body = oEmail.Message;
                message.AlternateViews.Add(av);
                smtp.Send(message);

               ///End of Gmail mail sending Code
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //Update Customer Profile Data for Android.
        public static DataSet UpdateCustomerForAndroid(string UserId, string FirstName, string LastName, string Address, string City, string State, string Country, string PinCode, string MobNo, string HouseType, string IsHavingCar, string NoOfCars, string TypeOfEmployment, string CompanyName, string DOB, string ProfilePicture, string PhoneNo, string AddressOfHouse, string ProfilePicImagePath, string IsProfilePicUpdated, string NoOfEmp, string Website, string EstPremium)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            DataSet dsUserDetails = null;

            int user = 0;
            try
            {

                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspUpdateCustomerForAndroid");
                db.AddInParameter(dbCommand, "UserId", DbType.String, UserId);
                db.AddInParameter(dbCommand, "FirstName", DbType.String, FirstName);
                db.AddInParameter(dbCommand, "LastName", DbType.String, LastName);
                db.AddInParameter(dbCommand, "Address", DbType.String, Address);
                db.AddInParameter(dbCommand, "City", DbType.String, City);
                db.AddInParameter(dbCommand, "State", DbType.String, State);
                db.AddInParameter(dbCommand, "Country", DbType.String, Country);
                db.AddInParameter(dbCommand, "PinCode", DbType.String, PinCode);
                db.AddInParameter(dbCommand, "MobNo", DbType.String, MobNo);
                db.AddInParameter(dbCommand, "HouseType", DbType.String, HouseType);
                db.AddInParameter(dbCommand, "AddressOfHouse", DbType.String, AddressOfHouse);
                db.AddInParameter(dbCommand, "IsHavingCar", DbType.String, IsHavingCar);
                db.AddInParameter(dbCommand, "NoOfCars", DbType.String, NoOfCars);
                db.AddInParameter(dbCommand, "TypeOfEmployment", DbType.String, TypeOfEmployment);
                db.AddInParameter(dbCommand, "CompanyName", DbType.String, CompanyName);

                db.AddInParameter(dbCommand, "ProfilePicture", DbType.String, ProfilePicture);
                db.AddInParameter(dbCommand, "PhoneNo", DbType.String, PhoneNo);
                db.AddInParameter(dbCommand, "DOB", DbType.String, DOB);

                db.AddInParameter(dbCommand, "ProfilePicImagePath", DbType.String, ProfilePicImagePath);
                db.AddInParameter(dbCommand, "IsProfilePicUpdated", DbType.String, IsProfilePicUpdated);

                db.AddInParameter(dbCommand, "NoOfEmp", DbType.String, NoOfEmp);
                db.AddInParameter(dbCommand, "Website", DbType.String, Website);
                db.AddInParameter(dbCommand, "EstPremium", DbType.String, EstPremium);

                dsUserDetails = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "UpdateCustomerForAndroid", ex.Message.ToString(), "BrokerWSUtility.cs_UpdateCustomerForAndroid", BrokerUtility.GetIPAddress(UserId));
            }
            return dsUserDetails;
        }

        //Update Customer Profile Data for Android.
        public static DataSet UpdateCustomer(string UserId, string FirstName, string LastName, string Address, string City, string State, string Country, string PinCode, string MobNo, string HouseType, string IsHavingCar, string NoOfCars, string TypeOfEmployment, string CompanyName, string DOB, string ProfilePicture, string PhoneNo, string AddressOfHouse, string ProfilePicImagePath)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            DataSet dsUserDetails = null;

            int user = 0;
            try
            {

                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("spUpdateCustomer");
                db.AddInParameter(dbCommand, "UserId", DbType.String, UserId);
                db.AddInParameter(dbCommand, "FirstName", DbType.String, FirstName);
                db.AddInParameter(dbCommand, "LastName", DbType.String, LastName);
                db.AddInParameter(dbCommand, "Address", DbType.String, Address);
                db.AddInParameter(dbCommand, "City", DbType.String, City);
                db.AddInParameter(dbCommand, "State", DbType.String, State);
                db.AddInParameter(dbCommand, "Country", DbType.String, Country);
                db.AddInParameter(dbCommand, "PinCode", DbType.String, PinCode);
                db.AddInParameter(dbCommand, "MobNo", DbType.String, MobNo);
                db.AddInParameter(dbCommand, "HouseType", DbType.String, HouseType);
                db.AddInParameter(dbCommand, "AddressOfHouse", DbType.String, AddressOfHouse);
                db.AddInParameter(dbCommand, "IsHavingCar", DbType.String, IsHavingCar);
                db.AddInParameter(dbCommand, "NoOfCars", DbType.String, NoOfCars);
                db.AddInParameter(dbCommand, "TypeOfEmployment", DbType.String, TypeOfEmployment);
                db.AddInParameter(dbCommand, "CompanyName", DbType.String, CompanyName);

                db.AddInParameter(dbCommand, "ProfilePicture", DbType.String, ProfilePicture);
                db.AddInParameter(dbCommand, "PhoneNo", DbType.String, PhoneNo);
                db.AddInParameter(dbCommand, "DOB", DbType.String, DOB);

                db.AddInParameter(dbCommand, "ProfilePicImagePath", DbType.String, ProfilePicImagePath);
                //db.AddInParameter(dbCommand, "IsProfilePicUpdated", DbType.String, IsProfilePicUpdated);

                dsUserDetails = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "UpdateCustomer", ex.Message.ToString(), "BrokerWSUtility.cs_UpdateCustomer", BrokerUtility.GetIPAddress(UserId));
            }
            return dsUserDetails;
        }

        //Get Customer Details after profile updation.
        public static DataSet GetCustomerDetails(string EmailId)
        {
            DataSet dsUserDetails = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("spCheckUserExist");
                db.AddInParameter(dbCommand, "UserName", DbType.String, EmailId);

                dsUserDetails = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "GetCustomerDetails", Ex.Message.ToString(), "BrokerWSUtility.cs_GetCustomerDetails()", "0");
            }
            return dsUserDetails;
        }

        //Update Broker Profile data.
        public static DataSet UpdateBroker(string UserId, string FirstName, string LastName, string Address, string City, string State, string Country, string PinCode, string MobNo, string CompanyName, string Title, string Designation, string Languages, string Specialities, string Exp_Designation, string Exp_CompanyName, string Exp_DurationFrom, string Exp_DurationTo, string Edu_UniversityName, string Edu_CourseName, string Edu_DurationFrom, string Edu_DurationTo, string DOB, string ProfilePicture, string PhoneNo, string UpdateTable, string Awards, string Skills, string Recomendations, string License, string ExpiryDate, string Resume, string ProfilePicImagePath, string ResumeImagePath)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;
            DataSet dsUserDetails = null;

            try
            {

                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("spUpdateBroker");
                db.AddInParameter(dbCommand, "UserId", DbType.String, UserId);
                db.AddInParameter(dbCommand, "FirstName", DbType.String, FirstName);
                db.AddInParameter(dbCommand, "LastName", DbType.String, LastName);
                db.AddInParameter(dbCommand, "Address", DbType.String, Address);
                db.AddInParameter(dbCommand, "City", DbType.String, City);
                db.AddInParameter(dbCommand, "State", DbType.String, State);
                db.AddInParameter(dbCommand, "Country", DbType.String, Country);
                db.AddInParameter(dbCommand, "PinCode", DbType.String, PinCode);
                db.AddInParameter(dbCommand, "MobNo", DbType.String, MobNo);
                db.AddInParameter(dbCommand, "Title", DbType.String, Title);
                db.AddInParameter(dbCommand, "CompanyName", DbType.String, CompanyName);
                db.AddInParameter(dbCommand, "Designation", DbType.String, Designation);
                db.AddInParameter(dbCommand, "Languages", DbType.String, Languages);
                db.AddInParameter(dbCommand, "Specialities", DbType.String, Specialities);

                db.AddInParameter(dbCommand, "ProfilePicture", DbType.String, ProfilePicture);
                db.AddInParameter(dbCommand, "Resume", DbType.String, Resume);
                db.AddInParameter(dbCommand, "PhoneNo", DbType.String, PhoneNo);
                db.AddInParameter(dbCommand, "DOB", DbType.String, DOB);

                db.AddInParameter(dbCommand, "Awards", DbType.String, Awards);
                db.AddInParameter(dbCommand, "Skills", DbType.String, Skills);
                db.AddInParameter(dbCommand, "Recomendations", DbType.String, Recomendations);
                db.AddInParameter(dbCommand, "License", DbType.String, License);
                db.AddInParameter(dbCommand, "ExpiryDate", DbType.String, ExpiryDate);

                db.AddInParameter(dbCommand, "Exp_Designation", DbType.String, Exp_Designation);
                db.AddInParameter(dbCommand, "Exp_CompanyName", DbType.String, Exp_CompanyName);
                db.AddInParameter(dbCommand, "Exp_DurationFrom", DbType.String, Exp_DurationFrom);
                db.AddInParameter(dbCommand, "Exp_DurationTo", DbType.String, Exp_DurationTo);
                db.AddInParameter(dbCommand, "Edu_UniversityName", DbType.String, Edu_UniversityName);
                db.AddInParameter(dbCommand, "Edu_CourseName", DbType.String, Edu_CourseName);
                db.AddInParameter(dbCommand, "Edu_DurationFrom", DbType.String, Edu_DurationFrom);
                db.AddInParameter(dbCommand, "Edu_DurationTo", DbType.String, Edu_DurationTo);
                db.AddInParameter(dbCommand, "UpdateTable", DbType.String, UpdateTable);

                db.AddInParameter(dbCommand, "ProfilePicImagePath", DbType.String, ProfilePicImagePath);
                db.AddInParameter(dbCommand, "ResumeImagePath", DbType.String, ResumeImagePath);

                dsUserDetails = db.ExecuteDataSet(dbCommand);

            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "UpdateBroker", ex.Message.ToString(), "BrokerWSUtility.cs_UpdateBroker()", BrokerUtility.GetIPAddress(UserId));
            }
            return dsUserDetails;
        }

        //Update Broker Profile data.
        public static DataSet UpdateBrokerForAndroid(string UserId, string FirstName, string LastName, string Address, string City, string State, string Country, string PinCode, string MobNo, string CompanyName, string Title, string Designation, string Languages, string Specialities, string Exp_Designation, string Exp_CompanyName, string Exp_DurationFrom, string Exp_DurationTo, string Edu_UniversityName, string Edu_CourseName, string Edu_DurationFrom, string Edu_DurationTo, string DOB, string ProfilePicture, string PhoneNo, string UpdateTable, string Awards, string Skills, string Recomendations, string License, string ExpiryDate, string Resume, string ProfilePicImagePath, string ResumeImagePath, string IsProfilePicUpdated, string longitude, string latitude, string HomeValue, string AutoType, string Revenue, string Employees, string CoverageAmt, string IsCompanyLogoUpdated, string CompanyLogoPath, string Edu_Logo, string IndustryId, string SubIndustryId, string Exp_Bio, string UserBio, string Exp_Logo)// 09Oct17
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;
            DataSet dsUserDetails = null;

            try
            {

                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspUpdateBrokerForAndroid");
                db.AddInParameter(dbCommand, "UserId", DbType.String, UserId);
                db.AddInParameter(dbCommand, "FirstName", DbType.String, FirstName);
                db.AddInParameter(dbCommand, "LastName", DbType.String, LastName);
                db.AddInParameter(dbCommand, "Address", DbType.String, Address);
                db.AddInParameter(dbCommand, "City", DbType.String, City);
                db.AddInParameter(dbCommand, "State", DbType.String, State);
                db.AddInParameter(dbCommand, "Country", DbType.String, Country);
                db.AddInParameter(dbCommand, "PinCode", DbType.String, PinCode);
                db.AddInParameter(dbCommand, "MobNo", DbType.String, MobNo);
                db.AddInParameter(dbCommand, "Title", DbType.String, Title);
                db.AddInParameter(dbCommand, "CompanyName", DbType.String, CompanyName);
                db.AddInParameter(dbCommand, "Designation", DbType.String, Designation);
                db.AddInParameter(dbCommand, "Languages", DbType.String, Languages);
                db.AddInParameter(dbCommand, "Specialities", DbType.String, Specialities);

                db.AddInParameter(dbCommand, "ProfilePicture", DbType.String, ProfilePicture);
                db.AddInParameter(dbCommand, "Resume", DbType.String, Resume);
                db.AddInParameter(dbCommand, "PhoneNo", DbType.String, PhoneNo);
                db.AddInParameter(dbCommand, "DOB", DbType.String, DOB);

                db.AddInParameter(dbCommand, "Awards", DbType.String, Awards);
                db.AddInParameter(dbCommand, "Skills", DbType.String, Skills);
                db.AddInParameter(dbCommand, "Recomendations", DbType.String, Recomendations);
                db.AddInParameter(dbCommand, "License", DbType.String, License);
                db.AddInParameter(dbCommand, "ExpiryDate", DbType.String, ExpiryDate);

                db.AddInParameter(dbCommand, "Exp_Designation", DbType.String, Exp_Designation);
                db.AddInParameter(dbCommand, "Exp_CompanyName", DbType.String, Exp_CompanyName);
                db.AddInParameter(dbCommand, "Exp_DurationFrom", DbType.String, Exp_DurationFrom);
                db.AddInParameter(dbCommand, "Exp_DurationTo", DbType.String, Exp_DurationTo);
                db.AddInParameter(dbCommand, "Exp_Bio", DbType.String, Exp_Bio);

                db.AddInParameter(dbCommand, "Edu_UniversityName", DbType.String, Edu_UniversityName);
                db.AddInParameter(dbCommand, "Edu_CourseName", DbType.String, Edu_CourseName);
                db.AddInParameter(dbCommand, "Edu_DurationFrom", DbType.String, Edu_DurationFrom);
                db.AddInParameter(dbCommand, "Edu_DurationTo", DbType.String, Edu_DurationTo);
                db.AddInParameter(dbCommand, "UpdateTable", DbType.String, UpdateTable);

                db.AddInParameter(dbCommand, "ProfilePicImagePath", DbType.String, ProfilePicImagePath);
                db.AddInParameter(dbCommand, "ResumeImagePath", DbType.String, ResumeImagePath);
                db.AddInParameter(dbCommand, "IsProfilePicUpdated", DbType.String, IsProfilePicUpdated);

                db.AddInParameter(dbCommand, "longitude", DbType.String, longitude);
                db.AddInParameter(dbCommand, "latitude", DbType.String, latitude);

                db.AddInParameter(dbCommand, "HomeValue", DbType.String, HomeValue);
                db.AddInParameter(dbCommand, "AutoType", DbType.String, AutoType);
                db.AddInParameter(dbCommand, "Revenue", DbType.String, Revenue);
                db.AddInParameter(dbCommand, "Employees", DbType.String, Employees);
                db.AddInParameter(dbCommand, "CoverageAmt", DbType.String, CoverageAmt);

                db.AddInParameter(dbCommand, "IsCompanyLogoUpdated", DbType.String, IsCompanyLogoUpdated);
                db.AddInParameter(dbCommand, "CompanyLogoPath", DbType.String, CompanyLogoPath);

                db.AddInParameter(dbCommand, "Edu_Logo", DbType.String, Edu_Logo);

                db.AddInParameter(dbCommand, "IndustryId", DbType.String, IndustryId);
                db.AddInParameter(dbCommand, "SubIndustryId", DbType.String, SubIndustryId);

                db.AddInParameter(dbCommand, "UserBio", DbType.String, UserBio);
                db.AddInParameter(dbCommand, "Exp_Logo", DbType.String, Exp_Logo);

                dsUserDetails = db.ExecuteDataSet(dbCommand);

            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "UpdateBrokerForAndroid", ex.Message.ToString(), "BrokerWSUtility.cs_UpdateBrokerForAndroid()", BrokerUtility.GetIPAddress(UserId));
            }
            return dsUserDetails;
        }

        //Ger Broker Details after profile updation.
        public static DataSet GetBrokerDetails(string EmailId)
        {
            DataSet dsUserDetails = null;
            try
            {

                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("spCheckUserExist");
                db.AddInParameter(dbCommand, "UserName", DbType.String, EmailId);

                dsUserDetails = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "GetBrokerDetails", Ex.Message.ToString(), "BrokerWSUtility.cs_GetBrokerDetails()", "0");
            }
            return dsUserDetails;
        }

        //Get details of User from UserID
        public static DataSet CheckValidUser(string UserId)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            DataSet dsUserDetails = null;

            int user = 0;
            try
            {

                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspViewUserProfile");
                db.AddInParameter(dbCommand, "UserId", DbType.Int32, UserId);

                dsUserDetails = db.ExecuteDataSet(dbCommand);


            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "CheckValidUser", ex.Message.ToString(), "BrokerWSUtility.cs_CheckValidUser", BrokerUtility.GetIPAddress(UserId));
            }
            return dsUserDetails;
        }

        //Save user details of Business Insurance.
        public static int SaveBusinessInsuranceDetails(int UserId, string ZipCode, string City, string IsInsured, string InsuranceCompany, string SICCode, string Revenue, string CoverageExpires, string Language, string Notes, string Longitude, string Latitude, string IndustryId, string SubIndustryId, string DocPath, string DeclarationDoc)
        {
            int User = 0;
            BrokerDBEntities DB = new BrokerDBEntities();
            try
            {
                var @cmdText = "exec uspSaveBusinessInsuranceDetails @UserId, @ZipCode, @City, @IsInsured, @InsuranceCompany, @SICCode, @Revenue, @CoverageExpires, @Language, @Notes,@Longitude,@Latitude,@IndustryId,@SubIndustryId,@DocPath,@DeclarationDocBase64";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@ZipCode", ZipCode),
                               new SqlParameter("@City", City),
                               new SqlParameter("@IsInsured", IsInsured),
                               new SqlParameter("@InsuranceCompany",InsuranceCompany),
                               new SqlParameter("@SICCode", SICCode),
                               new SqlParameter("@Revenue", Revenue),
                               new SqlParameter("@CoverageExpires", CoverageExpires),
                               new SqlParameter("@Language", Language),
                               new SqlParameter("@Notes", Notes),
                               new SqlParameter("@Longitude", Longitude),
                               new SqlParameter("@Latitude", Latitude),
                               new SqlParameter("@IndustryId", Convert.ToInt32(IndustryId)),
                               new SqlParameter("@SubIndustryId", SubIndustryId),
                               new SqlParameter("@DocPath",DocPath),
                               new SqlParameter("@DeclarationDocBase64",DeclarationDoc)
                    
                              };
                User = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SaveBusinessInsuranceDetails", Ex.Message.ToString(), "BrokerWSUtility.cs_SaveBusinessInsuranceDetails", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return User;
        }

        //Save user details of Benefits Insurance.
        public static int SaveBenefitInsuranceDetails(int UserId, string ZipCode, string City, string IsInsured, string InsuranceCompany, string EmployeeStrength, string CoverageExpires, string Language, string Notes, string Longitude, string Latitude, string IndustryId, string SubIndustryId,string DocPath,string DeclarationDoc)
        {
            int User = 0;
            BrokerDBEntities DB = new BrokerDBEntities();
            try
            {
                var @cmdText = "exec uspSaveBenefitsInsuranceDetails @UserId, @ZipCode, @City, @IsInsured, @InsuranceCompany, @EmployeeStrength, @CoverageExpires, @Language, @Notes,@Longitude,@Latitude,@IndustryId,@SubIndustryId,@DocPath,@DeclarationDocBase64";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@ZipCode", ZipCode),
                               new SqlParameter("@City", City),
                               new SqlParameter("@IsInsured", IsInsured),
                               new SqlParameter("@InsuranceCompany",InsuranceCompany),
                               new SqlParameter("@EmployeeStrength", EmployeeStrength),
                               new SqlParameter("@CoverageExpires", CoverageExpires),
                               new SqlParameter("@Language", Language),
                               new SqlParameter("@Notes", Notes),
                               new SqlParameter("@Longitude", Longitude),
                               new SqlParameter("@Latitude", Latitude),
                               new SqlParameter("@IndustryId",Convert.ToInt32(IndustryId)),
                               new SqlParameter("@SubIndustryId",SubIndustryId),
                               new SqlParameter("@DocPath",DocPath),
                               new SqlParameter("@DeclarationDocBase64",DeclarationDoc)
                               
                              };
                User = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SaveBenefitInsuranceDetails", Ex.Message.ToString(), "BrokerWSUtility.cs_SaveBenefitInsuranceDetails", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return User;
        }

        public static int SaveHomeInsuranceDetails(int UserId, string ZipCode, string City, string EstimatedValue, string IsInsured, string CompanyName, string CoverageExpires, string Language, string Notes, string Longitude, string Latitude,string DocPath,string DeclarationDoc)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;

            try
            {
                var @cmdText = "exec uspHomeInsuranceDetails @UserId,@ZipCode,@City,@EstimatedValue,@IsInsured,@CompanyName,@CoverageExpires,@Language,@Notes,@Longitude,@Latitude,@Path,@DeclarationDocBase64";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@ZipCode", ZipCode),
                               new SqlParameter("@City", City),
                               new SqlParameter("@EstimatedValue",EstimatedValue),
                               new SqlParameter("@IsInsured", IsInsured),// DBNull.Value
                               new SqlParameter("@CompanyName", CompanyName),// DBNull.Value
                               new SqlParameter("@CoverageExpires", CoverageExpires),
                               new SqlParameter("@Language", Language),                              
                               new SqlParameter("@Notes", Notes),
                               new SqlParameter("@Longitude", Longitude),
                               new SqlParameter("@Latitude", Latitude),
                               new SqlParameter("@Path", DocPath),
                               new SqlParameter("@DeclarationDocBase64", DeclarationDoc)
                                                 
                              };

                user = DB.Database.ExecuteSqlCommand(@cmdText, @params);

            }

            catch (Exception ex)
            {

                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SaveHomeInsuranceDetails", ex.Message.ToString(), "BrokerWSUtility.cs_SaveHomeInsuranceDetails()", BrokerUtility.GetIPAddress(UserId.ToString()));
                throw;
            }

            return user;
        }



        //Get Broker List
        public static DataSet GetBrokersList(int UserId, string ZipCode, string City, string Language, string Speciality, string Longitude, string Latitude, string SearchCriteria, string IndustryId, string SubIndustryId)
        {
            DataSet dsBrokerList = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetBrokersList");

                db.AddInParameter(dbCommand, "UserId", DbType.Int32, UserId);
                db.AddInParameter(dbCommand, "ZipCode", DbType.String, ZipCode);
                db.AddInParameter(dbCommand, "City", DbType.String, City);
                db.AddInParameter(dbCommand, "Language", DbType.String, Language);
                db.AddInParameter(dbCommand, "Speciality", DbType.String, Speciality);
                db.AddInParameter(dbCommand, "Longitude", DbType.String, Longitude);
                db.AddInParameter(dbCommand, "Latitude", DbType.String, Latitude);
                db.AddInParameter(dbCommand, "Withinmiles", DbType.String, BrokerSearchWithinMiles);
                db.AddInParameter(dbCommand, "SearchCriteria", DbType.String, SearchCriteria);
                db.AddInParameter(dbCommand, "IndustryId", DbType.Int32, IndustryId);
                db.AddInParameter(dbCommand, "SubIndustryId", DbType.String, SubIndustryId);

                dsBrokerList = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "GetBrokersList", Ex.Message.ToString(), "BrokerWSUtility.cs_GetBrokersList", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return dsBrokerList;
        }
        public static int SaveAutoInsuranceDetails(int UserId, string ZipCode, string City, string VehicleType, string IsInsured, string InsuranceCompany, string CoverageExpires, string Language, string Notes, string Longitude, string Latitude,string DocPath,string DeclarationDoc)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;

            try
            {
                var @cmdText = "exec uspSaveAutoInsuranceDetails @UserId,@ZipCode,@City,@VehicleType,@IsInsured,@InsuranceCompany,@CoverageExpires,@Language,@Notes,@Longitude,@Latitude,@DocPath,@DeclarationDocBase64";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@ZipCode", ZipCode),
                               new SqlParameter("@City", City),
                               new SqlParameter("@VehicleType", VehicleType),
                               new SqlParameter("@IsInsured", IsInsured),// DBNull.Value
                               new SqlParameter("@InsuranceCompany", InsuranceCompany),// DBNull.Value
                               new SqlParameter("@CoverageExpires", CoverageExpires),
                               new SqlParameter("@Language", Language),
                               new SqlParameter("@Notes", Notes),
                               new SqlParameter("@Longitude", Longitude),
                               new SqlParameter("@Latitude", Latitude),
                               new SqlParameter("@DocPath", DocPath),
                               new SqlParameter("@DeclarationDocBase64", DeclarationDoc),
                              };

                user = DB.Database.ExecuteSqlCommand(@cmdText, @params);

            }
            catch (Exception ex)
            {

                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SaveAutoInsuranceDetails", ex.Message.ToString(), "BrokerWSUtility.cs_SaveAutoInsuranceDetails()", BrokerUtility.GetIPAddress(UserId.ToString()));
                throw;
            }

            return user;
        }


        public static int SaveLifeInsuranceDetails(int UserId, string ZipCode, string City, string IsInsured, string InsuranceCompany, string Language, string FaceValue, string CoverageExpires, string Notes, string Longitude, string Latitude, string DocPath, string DeclarationDoc)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;

            try
            {
                var @cmdText = "exec uspSaveLifeInsuranceDetails @UserId,@ZipCode,@City,@IsInsured,@InsuranceCompany,@Language,@FaceValue,@CoverageExpires,@Notes,@Longitude,@Latitude,@DocPath,@DeclarationDocBase64 ";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@ZipCode", ZipCode),
                               new SqlParameter("@City", City),
                               new SqlParameter("@IsInsured", IsInsured),// DBNull.Value
                               new SqlParameter("@InsuranceCompany", InsuranceCompany),// DBNull.Value
                               new SqlParameter("@Language", Language),
                               new SqlParameter("@FaceValue", FaceValue),
                               new SqlParameter("@CoverageExpires", CoverageExpires),
                               new SqlParameter("@Notes", Notes),
                               new SqlParameter("@Longitude", Longitude),
                               new SqlParameter("@Latitude", Latitude),
                               new SqlParameter("@DocPath",DocPath),
                               new SqlParameter("@DeclarationDocBase64 ",DeclarationDoc)                    
                              };

                user = DB.Database.ExecuteSqlCommand(@cmdText, @params);

            }
            catch (Exception ex)
            {

                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SaveLifeInsuranceDetails", ex.Message.ToString(), "BrokerWSUtility.cs_SaveLifeInsuranceDetails()", BrokerUtility.GetIPAddress(UserId.ToString()));
                throw;
            }

            return user;
        }

        public static DataSet GetBrokerAvailabilityStatus(int UserId)
        {
            DataSet dsBrokerAvailability = null;

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetBrokersAvailabilty");

                db.AddInParameter(dbCommand, "UserId", DbType.String, UserId);

                dsBrokerAvailability = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "GetBrokerAvailabilityStatus", Ex.Message.ToString(), "BrokerWSUtility.cs_GetBrokerAvailabilityStatus()", BrokerUtility.GetIPAddress(UserId.ToString()));
            }

            return dsBrokerAvailability;
        }

        public static int SetBrokerAvailabilityStatus(int UserId, bool Availability, string longitude, string latitude)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;

            try
            {
                var @cmdText = "exec uspSetBrokersAvailabilty @UserId,@Availability";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@Availability", Availability)
                               //new SqlParameter("@longitude", longitude),
                               //new SqlParameter("@latitude", latitude)
                              };

                user = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SetBrokerAvailabilityStatus", Ex.Message.ToString(), "BrokerWSUtility.cs_SetBrokerAvailabilityStatus()", BrokerUtility.GetIPAddress(UserId.ToString()));
            }

            return user;
        }

        public static DataSet SetBrokerAvailabilityWithZipCode(int UserId, bool Availability, string longitude, string latitude, string ZipCode)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;
            string EmailId = "";

            DataSet dsUserDetails = null;

            try
            {
                //var @cmdText = "exec uspSetBrokerAvailabilityWithZipCode @UserId,@Availability,@longitude,@latitude,@ZipCode";
                //var @params = new[]{
                //               new SqlParameter("@UserId", UserId),
                //               new SqlParameter("@Availability", Availability),
                //               new SqlParameter("@longitude", longitude),
                //               new SqlParameter("@latitude", latitude),
                //               new SqlParameter("@ZipCode", ZipCode)
                //              };

                //user = DB.Database.ExecuteSqlCommand(@cmdText, @params);


                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspSetBrokerAvailabilityWithZipCode");
                db.AddInParameter(dbCommand, "UserId", DbType.String, UserId);
                db.AddInParameter(dbCommand, "Availability", DbType.String, Availability);
                //db.AddInParameter(dbCommand, "longitude", DbType.String, longitude);
                //db.AddInParameter(dbCommand, "latitude", DbType.String, latitude);
                //db.AddInParameter(dbCommand, "ZipCode", DbType.String, ZipCode);

                dsUserDetails = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SetBrokerAvailabilityWithZipCode", Ex.Message.ToString(), "BrokerWSUtility.cs_SetBrokerAvailabilityWithZipCode()", BrokerUtility.GetIPAddress(UserId.ToString()));
            }

            return dsUserDetails;
        }

        public static int ContactBroker(int UserId, int BrokerId, string InsuranceType)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;

            try
            {
                var @cmdText = "exec uspContactBroker @UserId,@BrokerId,@InsuranceType";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@BrokerId", BrokerId),
                               new SqlParameter("@InsuranceType",InsuranceType)
                              };


                user = DB.Database.ExecuteSqlCommand(@cmdText, @params);

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "ContactBroker", Ex.Message.ToString(), "BrokerWSUtility.cs_ContactBroker()", BrokerUtility.GetIPAddress(UserId.ToString()));
            }

            return user;
        }

        public static int SendMessages(int UserId, int BrokerId, string InsuranceType, string Note, string LocalDateTime)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int count = 0;
            DataSet dsUserDetails = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspBrokerMessages");
                db.AddInParameter(dbCommand, "UserId", DbType.Int32, UserId);
                db.AddInParameter(dbCommand, "BrokerId", DbType.Int32, BrokerId);
                db.AddInParameter(dbCommand, "InsuranceType", DbType.String, InsuranceType);
                db.AddInParameter(dbCommand, "Note", DbType.String, Note);
                db.AddInParameter(dbCommand, "LocalDateTime", DbType.String, LocalDateTime);

                //dsUserDetails = db.ExecuteDataSet(dbCommand);

                //var @cmdText = "exec uspBrokerMessages @UserId,@BrokerId,@InsuranceType,@Note,@LocalDateTime";
                //var @params = new[]{
                //               new SqlParameter("@UserId", UserId),
                //               new SqlParameter("@BrokerId", BrokerId),
                //               new SqlParameter("@InsuranceType", InsuranceType),
                //               new SqlParameter("@Note", Note),
                //               new SqlParameter("@LocalDateTime", LocalDateTime)
                //              };

                //count = DB.Database.ExecuteSqlCommand(@cmdText, @params);

                dsUserDetails = db.ExecuteDataSet(dbCommand);

                if (dsUserDetails.Tables.Count > 0)
                {
                    if (dsUserDetails.Tables[0].Rows.Count > 0)
                    {
                        count = Convert.ToInt32(dsUserDetails.Tables[0].Rows[0]["CustMsgId"]);
                    }
                }

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SendMessages", Ex.Message.ToString(), "BrokerWSUtility.cs_SendMessages()", BrokerUtility.GetIPAddress(UserId.ToString()));
            }

            return count;
        }

        //Get Contacted Broker List

        public static DataSet GetContactedBrokerList(int UserId)
        {
            DataSet dsBrokerList = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetContactedBrokersList");

                db.AddInParameter(dbCommand, "UserId", DbType.Int32, UserId);

                dsBrokerList = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "GetContactedBrokerList", Ex.Message.ToString(), "BrokerWSUtility.cs_GetContactedBrokerList", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return dsBrokerList;
        }

        public static DataSet GetContactedList(int UserId, string TimeStamp)
        {
            DataSet dsBrokerList = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetContactedList");

                db.AddInParameter(dbCommand, "UserId", DbType.Int32, UserId);
                db.AddInParameter(dbCommand, "TimeStamp", DbType.String, TimeStamp);

                dsBrokerList = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "GetContactedList", Ex.Message.ToString(), "BrokerWSUtility.cs_GetContactedList", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return dsBrokerList;
        }

        public static List<uspSaveCustomerChat_Result> SaveCustomerChat(int CustMsgId, int CustomerId, int BrokerId, string CustomerMessage, int BrokerMsgId, string LocalDateTime)
        {
            List<uspSaveCustomerChat_Result> User = null;
            BrokerDBEntities DB = new BrokerDBEntities();
            int count = 0;

            try
            {
                var @cmdText = "exec uspSaveCustomerChat @CustMsgId,@CustomerId,@BrokerId,@CustomerMessage,@BrokerMsgId,@LocalDateTime";
                var @params = new[]{
                               new SqlParameter("@CustMsgId", CustMsgId),
                               new SqlParameter("@CustomerId", CustomerId),
                               new SqlParameter("@BrokerId", BrokerId),
                               new SqlParameter("@CustomerMessage", CustomerMessage),
                               new SqlParameter("@BrokerMsgId", BrokerMsgId),
                               new SqlParameter("@LocalDateTime", LocalDateTime)
                              };

                User = DB.Database.SqlQuery<uspSaveCustomerChat_Result>(@cmdText, @params).ToList<uspSaveCustomerChat_Result>();

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(CustomerId), "SaveCustomerChat", Ex.Message.ToString(), "BrokerWSUtility.cs_SaveCustomerChat()", BrokerUtility.GetIPAddress(CustomerId.ToString()));
            }

            return User;
        }

        public static List<uspSaveBrokerChat_Result> SaveBrokerChat(int BrokerMsgId, int BrokerId, int CustomerId, string BrokerMessage, int CustomerMsgId, string LocalDateTime)
        {
            //int user = 0;

            BrokerDBEntities DB = new BrokerDBEntities();
            List<uspSaveBrokerChat_Result> user = null;

            try
            {
                var @cmdText = "exec uspSaveBrokerChat @BrokerMsgId,@BrokerId,@CustomerId,@BrokerMessage,@CustomerMsgId,@LocalDateTime";
                var @params = new[]{
                               new SqlParameter("@BrokerMsgId", BrokerMsgId),
                               new SqlParameter("@BrokerId", BrokerId ),
                               new SqlParameter("@CustomerId", CustomerId),
                               new SqlParameter("@BrokerMessage", BrokerMessage),
                                new SqlParameter("@CustomerMsgId", CustomerMsgId),
                                new SqlParameter("@LocalDateTime", LocalDateTime)
                              };

                // count = DB.Database.ExecuteSqlCommand(@cmdText, @params);
                user = DB.Database.SqlQuery<uspSaveBrokerChat_Result>(@cmdText, @params).ToList<uspSaveBrokerChat_Result>();

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(CustomerId), "SaveBrokerChat", Ex.Message.ToString(), "BrokerWSUtility.cs_SaveBrokerChat()", BrokerUtility.GetIPAddress(CustomerId.ToString()));
            }

            return user;
        }

        public static DataSet GetChatMessages(int UserId, string TimeSpan)
        {
            DataSet dsChatDetails = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetChatMessages");

                //db.AddInParameter(dbCommand, "MessageId", DbType.Int32, MessageId);
                db.AddInParameter(dbCommand, "UserId", DbType.Int32, UserId);
                db.AddInParameter(dbCommand, "TimeSpan", DbType.String, TimeSpan);

                dsChatDetails = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "GetChatMessages", Ex.Message.ToString(), "BrokerWSUtility.cs_GetChatMessages", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return dsChatDetails;
        }

        //Check whether Email Id is valid or not which come for resetting pasword.
        public static DataSet CheckForValidEmailId(string EmailId)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int count = 0;
            DataSet dsValidEmailId = null;

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspCheckForValidEmailId");

                db.AddInParameter(dbCommand, "EmailId", DbType.String, EmailId);

                dsValidEmailId = db.ExecuteDataSet(dbCommand);

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "CheckForValidEmailId", Ex.Message.ToString(), "BrokerWSUtility.cs_CheckForValidEmailId()", "0");
            }

            return dsValidEmailId;
        }

        //Check whether Email Id is active and is verified
        public static DataSet CheckForActiveEmailId(string EmailId)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int count = 0;
            DataSet dsValidEmailId = null;

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspCheckForActiveEmailId");

                db.AddInParameter(dbCommand, "EmailId", DbType.String, EmailId);

                dsValidEmailId = db.ExecuteDataSet(dbCommand);

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "CheckForActiveEmailId", Ex.Message.ToString(), "BrokerWSUtility.cs_CheckForActiveEmailId()", "0");
            }

            return dsValidEmailId;
        }

        //Resetting the Password.
        public static int ResetPassword(string EmailId, string Password)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int count = 0;

            try
            {
                var @cmdText = "exec uspResetPassword @EmailId,@Password";
                var @params = new[]{
                               new SqlParameter("@EmailId", EmailId),
                               new SqlParameter("@Password", Password ),
                              
                              };

                count = DB.Database.ExecuteSqlCommand(@cmdText, @params);

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "ResetPassword", Ex.Message.ToString(), "BrokerWSUtility.cs_ResetPassword()", "0");
            }

            return count;
        }



        public static DataSet CreateDataSetForErrorMessage(string Message)
        {
            DataTable dtDetails = new DataTable();
            DataSet dsDetails = new DataSet();

            dtDetails.Columns.Add("Message");

            dtDetails.Rows.Add(Message);

            dsDetails.Tables.Add(dtDetails);
            dsDetails.Tables[0].TableName = "Response";
            dsDetails.AcceptChanges();

            return dsDetails;
        }

        public static int SetIsRead(string MessageId, int UserId, string MainMessageId)
        {
            string JSONString = "";
            DataSet dsSetIsRead = null;
            BrokerDBEntities DB = new BrokerDBEntities();
            int Result = 0;
            try
            {
                var @cmdText = "exec uspSetIsRead @MessageId,@UserId,@MainMessageId";
                var @params = new[]{
                               new SqlParameter("@MessageId", MessageId),
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@MainMessageId", MainMessageId)
                    
                              };
                Result = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "SetIsRead", Ex.Message.ToString(), "BrokerWSUtility.cs_SetIsRead()", "0");
            }
            return Result;
        }

        public static int DeleteBrokerDetails(string EmailId, string TableName, string ArrayList)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;

            try
            {
                var @cmdText = "exec uspDeleteBrokerDetails @EmailId,@TableName,@ArrayList";
                var @params = new[]{
                               new SqlParameter("@EmailId", EmailId),
                               new SqlParameter("@TableName", TableName),
                               new SqlParameter("@ArrayList", ArrayList)
                    
                              };

                user = DB.Database.ExecuteSqlCommand(@cmdText, @params);

            }
            catch (Exception ex)
            {

                BrokerUtility.ErrorLog(0, "DeleteBrokerDetails", ex.Message.ToString(), "BrokerWSUtility.cs_DeleteBrokerDetails()", "0");
                throw;
            }

            return user;
        }

        //Update User Table to add ForgetPasswordRanNum to emailId

        public static int ForgetPasswordRanNum(string EmailId, string RandomNumber)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int count = 0;

            try
            {
                var @cmdText = "exec uspForgetPasswordRanNum @EmailId,@ForgetPasswordRanNum";
                var @params = new[]{
                               new SqlParameter("@EmailId", EmailId),
                               new SqlParameter("@ForgetPasswordRanNum", RandomNumber ),
                              
                              };

                count = DB.Database.ExecuteSqlCommand(@cmdText, @params);

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "ForgetPasswordRanNum", Ex.Message.ToString(), "BrokerWSUtility.cs_ForgetPasswordRanNum()", "0");
            }

            return count;
        }

        //public static string GetCurrentTimeSpan()
        //{
        //    BrokerDBEntities DB = new BrokerDBEntities();
        //    string Time="";
        //    try
        //    {
        //        var @cmdText = "exec uspGetCurrentTimeSpan";
        //        //var @params = new[]{
        //        //               new SqlParameter("@EmailId", EmailId),
        //        //               new SqlParameter("@ForgetPasswordRanNum", RandomNumber ),

        //        //              };

        //        //Time = DB.Database.ExecuteSqlCommand(@cmdText, @params);

        //    }
        //    catch (Exception Ex)
        //    {

        //    }
        //    return Time;
        //}

        public static List<uspGetCurrentTimeSpan_Result> GetCurrentTimeSpan()
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            List<uspGetCurrentTimeSpan_Result> user = null;

            try
            {
                var @cmdText = "exec uspGetCurrentTimeSpan";

                user = DB.Database.SqlQuery<uspGetCurrentTimeSpan_Result>(@cmdText).ToList<uspGetCurrentTimeSpan_Result>();

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "GetCurrentTimeSpan", Ex.Message.ToString(), "BrokerWSUtility.cs_GetCurrentTimeSpan()", "0");
            }

            return user;
        }

        public static DataSet GetChatMessagesByMessageId(int UserId, int MessageId, string TimeStamp)
        {
            DataSet dsChatDetails = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetChatMessagesByMessageId");

                db.AddInParameter(dbCommand, "UserId", DbType.Int32, UserId);
                db.AddInParameter(dbCommand, "MessageId", DbType.String, MessageId);
                db.AddInParameter(dbCommand, "TimeStamp", DbType.String, TimeStamp);

                dsChatDetails = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "GetChatMessagesByMessageId", Ex.Message.ToString(), "BrokerWSUtility.cs_GetChatMessagesByMessageId", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return dsChatDetails;
        }

        public static DataSet GetUnreadChatMessages(int UserId, int MessageId)
        {
            DataSet dsChatDetails = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetUnreadChatMessages");

                db.AddInParameter(dbCommand, "UserId", DbType.Int32, UserId);
                db.AddInParameter(dbCommand, "MessageId", DbType.String, MessageId);

                dsChatDetails = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "GetUnreadChatMessages", Ex.Message.ToString(), "BrokerWSUtility.cs_GetUnreadChatMessages", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return dsChatDetails;
        }

        public static DataSet GetCompanyMaster()
        {
            DataSet dsCompanyMaster = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetCompanyMaster");

                dsCompanyMaster = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "GetCompanyMaster", Ex.Message.ToString(), "BrokerWSUtility.cs_GetCompanyMaster", "0");
            }
            return dsCompanyMaster;
        }

        public static int DeleteMessage(int UserId, int MessageId)
        {
            string JSONString = "";
            DataSet dsSetIsRead = null;
            BrokerDBEntities DB = new BrokerDBEntities();
            int Result = 0;
            try
            {
                var @cmdText = "exec uspDeleteMessage @UserId,@MessageId";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@MessageId", MessageId)
                    
                              };
                Result = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DeleteMessage", Ex.Message.ToString(), "BrokerWSUtility.cs_DeleteMessage()", "0");
            }
            return Result;
        }

        public static int DeleteMultipleMessage(int UserId, string MessageId)
        {
            string JSONString = "";
            DataSet dsSetIsRead = null;
            BrokerDBEntities DB = new BrokerDBEntities();
            int Result = 0;
            try
            {
                var @cmdText = "exec uspDeleteMultipleMessages @UserId,@MessageId";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@MessageId", MessageId)
                    
                              };
                Result = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DeleteMultipleMessage", Ex.Message.ToString(), "BrokerWSUtility.cs_DeleteMultipleMessage()", "0");
            }
            return Result;
        }

        public static int DoSetDeviceId(int UserId, string DeviceId)
        {
            DataSet dsSetDeviceId = null;
            BrokerDBEntities DB = new BrokerDBEntities();
            int Result = 0;
            try
            {
                var @cmdText = "exec uspSetDeviceId @UserId,@DeviceId";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@DeviceId", DeviceId)
                    
                              };
                Result = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSetDeviceId", Ex.Message.ToString(), "BrokerWSUtility.cs_DoSetDeviceId()", "0");
            }
            return Result;
        }

        public static int DoClearDeviceId(int UserId, string DeviceId)
        {
            DataSet dsSetDeviceId = null;
            BrokerDBEntities DB = new BrokerDBEntities();
            int Result = 0;
            try
            {
                var @cmdText = "exec uspClearDeviceId @UserId,@DeviceId";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@DeviceId", DeviceId)
                    
                              };
                Result = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoClearDeviceId", Ex.Message.ToString(), "BrokerWSUtility.cs_DoClearDeviceId()", "0");
            }
            return Result;
        }

        public static DataSet GetDeviceId(int UserId)
        {
            DataSet dsDeviceId = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetDeviceId");

                db.AddInParameter(dbCommand, "UserId", DbType.Int32, UserId);

                dsDeviceId = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "GetDeviceId", Ex.Message.ToString(), "BrokerWSUtility.cs_GetDeviceId", "0");
            }
            return dsDeviceId;
        }

        public static int DeleteMultipleChatMessage(int UserId, string MessageId)
        {
            string JSONString = "";
            DataSet dsSetIsRead = null;
            BrokerDBEntities DB = new BrokerDBEntities();
            int Result = 0;
            try
            {
                var @cmdText = "exec uspDeleteMultipleChatMessages @UserId,@MessageId";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@MessageId", MessageId)
                    
                              };
                Result = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DeleteMultipleChatMessage", Ex.Message.ToString(), "BrokerWSUtility.cs_DeleteMultipleChatMessage()", "0");
            }
            return Result;
        }

        public static DataSet UpdateEducationDetails(string EduId, string UserId, string Edu_UniversityName, string Edu_CourseName, string Edu_DurationFrom, string Edu_DurationTo, string Edu_Logo, string IsUpdated)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;
            DataSet dsUserDetails = null;

            try
            {

                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspUpdateEducationDetails");
                db.AddInParameter(dbCommand, "EduId", DbType.String, EduId);
                db.AddInParameter(dbCommand, "UserId", DbType.String, UserId);
                db.AddInParameter(dbCommand, "Edu_UniversityName", DbType.String, Edu_UniversityName);
                db.AddInParameter(dbCommand, "Edu_CourseName", DbType.String, Edu_CourseName);
                db.AddInParameter(dbCommand, "Edu_DurationFrom", DbType.String, Edu_DurationFrom);
                db.AddInParameter(dbCommand, "Edu_DurationTo", DbType.String, Edu_DurationTo);
                db.AddInParameter(dbCommand, "Edu_Logo", DbType.String, Edu_Logo);
                db.AddInParameter(dbCommand, "IsUpdated", DbType.String, IsUpdated);

                dsUserDetails = db.ExecuteDataSet(dbCommand);

            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "UpdateEducationDetails", ex.Message.ToString(), "BrokerWSUtility.cs_UpdateEducationDetails()", BrokerUtility.GetIPAddress(UserId));
            }
            return dsUserDetails;
        }

        public static DataSet GetUnreadMsgCount(int UserId)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;
            DataSet dsUserDetails = null;

            try
            {

                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetUnreadMsgCount");
                db.AddInParameter(dbCommand, "UserId", DbType.Int32, UserId);

                dsUserDetails = db.ExecuteDataSet(dbCommand);

            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "GetUnreadMsgCount", ex.Message.ToString(), "BrokerWSUtility.cs_GetUnreadMsgCount()", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return dsUserDetails;
        }

        public static DataSet GetIndustryMaster(int CompanyId)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;
            DataSet dsIndustryMaster = null;

            try
            {

                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetIndustryMaster");
                db.AddInParameter(dbCommand, "CompanyId", DbType.Int32, CompanyId);

                dsIndustryMaster = db.ExecuteDataSet(dbCommand);

            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(0, "GetIndustryMaster", ex.Message.ToString(), "BrokerWSUtility.cs_GetIndustryMaster()", "");
            }
            return dsIndustryMaster;
        }

        public static DataSet GetSubIndustryMaster(int IndustryId)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;
            DataSet dsSubIndustryMaster = null;

            try
            {

                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetSubIndustryMaster");
                db.AddInParameter(dbCommand, "IndustryId", DbType.Int32, IndustryId);
                //db.AddInParameter(dbCommand, "CompanyId", DbType.Int32, CompanyId);

                dsSubIndustryMaster = db.ExecuteDataSet(dbCommand);

            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(0, "GetSubIndustryMaster", ex.Message.ToString(), "BrokerWSUtility.cs_GetSubIndustryMaster()", "");
            }
            return dsSubIndustryMaster;
        }

        public static int InsertIndustryId(string UserId, string IndustryId, string SubIndustryId)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int Result = 0;
            try
            {
                var @cmdText = "exec uspInsertIndustryId @UserId,@IndustryId,@SubIndustryId";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@IndustryId", IndustryId),
                               new SqlParameter("@SubIndustryId", SubIndustryId)
                              };
                Result = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "InsertIndustryId", Ex.Message.ToString(), "BrokerWSUtility.cs_InsertIndustryId()", "0");
            }
            return Result;
        }

        public static int DeleteIndustryId(string UserId)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int Result = 0;
            try
            {
                var @cmdText = "exec uspDeleteIndustryId @UserId";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId)
                              };
                Result = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "DeleteIndustryId", Ex.Message.ToString(), "BrokerWSUtility.cs_DeleteIndustryId()", "0");
            }
            return Result;
        }

        public static DataSet getUnreadMsgCountByDeviceid(string UserId)
        {
            DataSet dsResult = new DataSet();

            BrokerDBEntities DB = new BrokerDBEntities();
            int Result = 0;
            try
            {

                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspgetUnreadMsgCountByDeviceid");
                db.AddInParameter(dbCommand, "UserId", DbType.Int32, UserId);

                dsResult = db.ExecuteDataSet(dbCommand);

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "getUnreadMsgCountByDeviceid", Ex.Message.ToString(), "BrokerWSUtility.cs_getUnreadMsgCountByDeviceid()", "0");
            }
            return dsResult;
        }

        public static int DeleteZipCode(string UserId)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int Result = 0;
            try
            {
                var @cmdText = "exec uspDeleteZipCodes @UserId";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId)
                              };
                Result = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "DeleteZipCode", Ex.Message.ToString(), "BrokerWSUtility.cs_DeleteZipCode()", "0");
            }
            return Result;
        }

        public static int InsertUserZipCode(string UserId, string ZipCode, string Long, string Lat)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int Result = 0;
            try
            {
                var @cmdText = "exec uspInsertUserZipCode @UserId,@ZipCode,@Longitude,@Latitude";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@ZipCode", ZipCode),
                               new SqlParameter("@Longitude", Long),
                                new SqlParameter("@Latitude", Lat)
                              };
                Result = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "InsertUserZipCode", Ex.Message.ToString(), "BrokerWSUtility.cs_InsertUserZipCode()", "0");
            }
            return Result;
        }

        public static DataSet GetByDefaultBrokersList(int UserId, string SearchUserList)//change 26Jun17
        {
            DataSet dsBrokerList = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetByDefaultBrokersList");

                db.AddInParameter(dbCommand, "UserId", DbType.Int32, UserId);
                db.AddInParameter(dbCommand, "SearchUserList", DbType.String, SearchUserList);

                dsBrokerList = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "GetByDefaultBrokersList", Ex.Message.ToString(), "BrokerWSUtility.cs_GetByDefaultBrokersList", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return dsBrokerList;
        }


        //22Sept17 Santosh

        // 27Sep17 santosh //Send Successfull Registration Email from website
        public static bool SendRegistrationEmailFromWebSite(string EmailId, string RegistrationCode, string UserId, string UserType)
        {
            bool SuccessFlag = false;
            EmailInfoModel oEmail = new EmailInfoModel();
            StringBuilder Obody = new StringBuilder();
            try
            {
                Obody.Append("<html><head></head><body>Hi ,<br/><br/>");
                Obody.Append("Greetings from Team brokkrr! <br/><br/>Thank you for registering  with brokkrr app as " + UserType + ".  For your security, we need you to verify your email id.<br/> ");
                //Obody.Append("Please <a href='" + strServerLink + strActivateUserPageLink + "?EmailId=" + EmailId + "&RegistrationCode=" + RegistrationCode + "'>click here</a> to varify your email id.");
                Obody.Append("Please <a href='" + strServerLink + strActivateUserPageLink + "?EmailId=" + BrokerUtility.EncryptURL(EmailId) + "&RegistrationCode=" + RegistrationCode + "'>click here</a> to verify your email address.<br/><br/><br/>");
                Obody.Append("Thank you,<br/>");
                Obody.Append("Anton and Curtis,<br/>");
                Obody.Append("brokkrr Website Team.</body></html>");


                oEmail.Message = Obody.ToString();
                oEmail.Subject = "Welcome to brokkrr.";
                oEmail.ToEmailid = EmailId;
                oEmail.FromEmailid = strFromEmail;
                oEmail.MailType = "NewSignUp";
                oEmail.UserId = UserId;
                SuccessFlag = sendmail(oEmail);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SendRegistrationEmailFromWebSite_website", ex.Message.ToString(), "BrokerWSUtility.cs_SendRegistrationEmailFromWebSite()", "");
            }
            return SuccessFlag;

        }

        // 27Sep17 santosh send email verificaton link
        public static bool SendMailVerificationEmail(string EmailId, string RegistrationCode, string UserId, string UserType)
        {
            bool SuccessFlag = false;
            EmailInfoModel oEmail = new EmailInfoModel();
            StringBuilder Obody = new StringBuilder();
            try
            {
                Obody.Append("<html><head></head><body>Hi ,<br/><br/>");
                Obody.Append("Greetings from Team brokkrr! <br/><br/>Thank you for using  brokkrr app.  For your security, we need you to verify your email id.<br/> ");
                //Obody.Append("Please <a href='" + strServerLink + strActivateUserPageLink + "?EmailId=" + EmailId + "&RegistrationCode=" + RegistrationCode + "'>click here</a> to varify your email id.");
                Obody.Append("Please <a href='" + strServerLink + strActivateUserPageLink + "?EmailId=" + BrokerUtility.EncryptURL(EmailId) + "&RegistrationCode=" + RegistrationCode + "'>click here</a> to verify your email address.<br/><br/><br/>");
                Obody.Append("Thank you,<br/>");
                Obody.Append("Anton and Curtis,<br/>");
                Obody.Append("brokkrr.</body></html>");


                oEmail.Message = Obody.ToString();
                oEmail.Subject = "brokkrr- Verify your email Id.";
                oEmail.ToEmailid = EmailId;
                oEmail.FromEmailid = strFromEmail;
                oEmail.MailType = "SendMailVerificationEmail";
                oEmail.UserId = UserId;
                SuccessFlag = sendmail(oEmail);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SendMailVerificationEmail_website", ex.Message.ToString(), "BrokerWSUtility.cs_SendMailVerificationEmail()", "");
            }
            return SuccessFlag;

        }


        //03Oct17  Fro creating new password for customer
        public static int CreateNewPassword(string EmailId, string TempPass)
        {
            int User = 0;
            BrokerDBEntities DB = new BrokerDBEntities();

            try
            {
                var @cmdText = "exec uspCreateNewPassword @EmailId,@Password";
                var @params = new[]{
                                    new SqlParameter("@EmailId", EmailId),
                                    new SqlParameter("@Password", TempPass)
                              };
                User = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(0, "CreateNewPassword", ex.Message.ToString(), "BrokerUtility.cs_CreateNewPassword()", "0");
            }
            return User;
        }

        public static DataSet UpdateExperienceDetails(string ExpId, string UserId, string Exp_Designation, string Exp_CompanyName, string Exp_DurationFrom, string Exp_DurationTo, string Exp_Logo, string IsUpdated)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;
            DataSet dsUserDetails = null;

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspUpdateExperienceDetails");
                db.AddInParameter(dbCommand, "ExpId", DbType.String, ExpId);
                db.AddInParameter(dbCommand, "UserId", DbType.String, UserId);
                db.AddInParameter(dbCommand, "Exp_Designation", DbType.String, Exp_Designation);
                db.AddInParameter(dbCommand, "Exp_CompanyName", DbType.String, Exp_CompanyName);
                db.AddInParameter(dbCommand, "Exp_DurationFrom", DbType.String, Exp_DurationFrom);
                db.AddInParameter(dbCommand, "Exp_DurationTo", DbType.String, Exp_DurationTo);
                db.AddInParameter(dbCommand, "Exp_Logo", DbType.String, Exp_Logo);
                db.AddInParameter(dbCommand, "IsUpdated", DbType.String, IsUpdated);

                dsUserDetails = db.ExecuteDataSet(dbCommand);

            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "UpdateExperienceDetails", ex.Message.ToString(), "BrokerWSUtility.cs_UpdateExperienceDetails()", BrokerUtility.GetIPAddress(UserId));
            }
            return dsUserDetails;
        }

        #endregion Common Methods

        //For Meineke company

        #region For Meineke Company
        public static DataSet GetBrokersListForMeineke(int UserId, string ZipCode, string City, string Speciality, string Longitude, string Latitude, string IndustryId, string SubIndustryId)
        {
            DataSet dsBrokerList = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetBrokersListForMeineke");

                db.AddInParameter(dbCommand, "UserId", DbType.Int32, UserId);
                db.AddInParameter(dbCommand, "ZipCode", DbType.String, ZipCode);
                db.AddInParameter(dbCommand, "City", DbType.String, City);
                db.AddInParameter(dbCommand, "Speciality", DbType.String, Speciality);
                db.AddInParameter(dbCommand, "Longitude", DbType.String, Longitude);
                db.AddInParameter(dbCommand, "Latitude", DbType.String, Latitude);
                db.AddInParameter(dbCommand, "Withinmiles", DbType.String, BrokerSearchWithinMiles);
                db.AddInParameter(dbCommand, "IndustryId", DbType.Int32, IndustryId);
                db.AddInParameter(dbCommand, "SubIndustryId", DbType.String, SubIndustryId);


                dsBrokerList = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "GetBrokersListForMeineke", Ex.Message.ToString(), "BrokerWSUtility.cs_GetBrokersListForMeineke", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return dsBrokerList;
        }


        public static int SaveCommercialAutoInsuranceDetails(int UserId, string ZipCode, string City, string NoOfStalls, string NoOfLocations, string GrossRevenue, string CurrentLimit, string Longitude, string Latitude,string DocPath,string DeclarationDoc)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;

            try
            {
                var @cmdText = "exec uspSaveCommercialAutoInsuranceDetails @UserId, @ZipCode, @City,@Longitude,@Latitude,@NoOfStalls,@NoOfLocations,@GrossRevenue,@CurrentLimit,@NoOfUnits,@DeductibleIfAny,@DocPath,@DeclarationDocBase64";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@ZipCode", ZipCode),
                               new SqlParameter("@City", City),
                               new SqlParameter("@Longitude", Longitude),
                               new SqlParameter("@Latitude", Latitude),                              
                               new SqlParameter("@NoOfStalls",NoOfStalls),
                               new SqlParameter("@NoOfLocations",NoOfLocations),
                               new SqlParameter("@GrossRevenue",GrossRevenue),
                               new SqlParameter("@CurrentLimit", CurrentLimit),
                               new SqlParameter("@NoOfUnits", ""),
                               new SqlParameter("@DeductibleIfAny", ""),
                               new SqlParameter("@DocPath", DocPath),
                               new SqlParameter("@DeclarationDocBase64", DeclarationDoc)
                    
                              };

                user = DB.Database.ExecuteSqlCommand(@cmdText, @params);

            }
            catch (Exception ex)
            {

                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SaveCommercialAutoInsuranceDetails", ex.Message.ToString(), "BrokerWSUtility.cs_SaveCommercialAutoInsuranceDetails()", BrokerUtility.GetIPAddress(UserId.ToString()));
                throw;
            }

            return user;
        }

        //public static int SaveCommercialAutoInsuranceDetails(int UserId, string ZipCode, string City, string NoOfUnits, string DeductibleIfAny, string CurrentLimit, string Longitude, string Latitude)
        //{
        //    BrokerDBEntities DB = new BrokerDBEntities();
        //    int user = 0;

        //    try
        //    {
        //        var @cmdText = "exec uspSaveCommercialAutoInsuranceDetails @UserId, @ZipCode, @City,@Longitude,@Latitude,@NoOfStalls,@NoOfLocations,@GrossRevenue,@CurrentLimit,@NoOfUnits,@DeductibleIfAny";
        //        var @params = new[]{
        //                       new SqlParameter("@UserId", UserId),
        //                       new SqlParameter("@ZipCode", ZipCode),
        //                       new SqlParameter("@City", City),
        //                       new SqlParameter("@Longitude", Longitude),
        //                       new SqlParameter("@Latitude", Latitude),                              
        //                       new SqlParameter("@NoOfStalls",""),
        //                       new SqlParameter("@NoOfLocations",""),
        //                       new SqlParameter("@GrossRevenue",""),
        //                       new SqlParameter("@CurrentLimit", CurrentLimit),
        //                       new SqlParameter("@NoOfUnits", NoOfUnits),
        //                       new SqlParameter("@DeductibleIfAny", DeductibleIfAny)

        //                      };

        //        user = DB.Database.ExecuteSqlCommand(@cmdText, @params);

        //    }
        //    catch (Exception ex)
        //    {

        //        BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SaveCommercialAutoInsuranceDetails", ex.Message.ToString(), "BrokerWSUtility.cs_SaveCommercialAutoInsuranceDetails()", BrokerUtility.GetIPAddress(UserId.ToString()));
        //        throw;
        //    }

        //    return user;
        //}

        public static int SaveWorkersCompensationDetails(int UserId, string ZipCode, string City, string Longitude, string Latitude, string NoOfEmp, string GrossPayroll, string DocPath, string DeclarationDoc)
        {
            int User = 0;
            BrokerDBEntities DB = new BrokerDBEntities();
            try
            {
                var @cmdText = "exec uspSaveWorkCompDetails @UserId, @ZipCode, @City,@Longitude,@Latitude,@NoOfEmp,@GrossPayroll,@DocPath,@DeclarationDocBase64";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@ZipCode", ZipCode),
                               new SqlParameter("@City", City),
                               new SqlParameter("@Longitude", Longitude),
                               new SqlParameter("@Latitude", Latitude),
                               new SqlParameter("@NoOfEmp",NoOfEmp),
                               new SqlParameter("@GrossPayroll",GrossPayroll),
                               new SqlParameter("@DocPath",DocPath),
                               new SqlParameter("@DeclarationDocBase64",DeclarationDoc)
                              };
                User = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SaveWorkersCompensationDetails", Ex.Message.ToString(), "BrokerWSUtility.cs_SaveWorkersCompensationDetails", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return User;
        }

        public static int SaveLiabilityInsuranceDetails(int UserId, string ZipCode, string City, string Longitude, string Latitude, string DeductibleIfAny, string GrossSale, string IndustryId, string SubIndustryId)
        {
            int User = 0;
            BrokerDBEntities DB = new BrokerDBEntities();
            try
            {
                var @cmdText = "exec uspSaveLiabilityInsuranceDetails @UserId, @ZipCode, @City,@Longitude,@Latitude,@DeductibleIfAny,@GrossSale,@IndustryId,@SubIndustryId";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@ZipCode", ZipCode),
                               new SqlParameter("@City", City),
                               new SqlParameter("@Longitude", Longitude),
                               new SqlParameter("@Latitude", Latitude),
                               new SqlParameter("@DeductibleIfAny",DeductibleIfAny),
                               new SqlParameter("@GrossSale",GrossSale),
                               new SqlParameter("@IndustryId",Convert.ToInt32(IndustryId)),
                               new SqlParameter("@SubIndustryId",SubIndustryId)
                              };
                User = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SaveLiabilityInsuranceDetails", Ex.Message.ToString(), "BrokerWSUtility.cs_SaveLiabilityInsuranceDetails", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return User;
        }

        public static int SaveMeinekeBenefitInsuranceDetails(int UserId, string ZipCode, string City, string IsInsured, string InsuranceCompany, string EmployeeStrength, string CoverageExpires, string Language, string Notes, string Longitude, string Latitude, string IndustryId, string SubIndustryId, string DocPath, string DeclarationDoc)
        {
            int User = 0;
            BrokerDBEntities DB = new BrokerDBEntities();
            try
            {
                var @cmdText = "exec uspSaveMeinekeBenefitsInsuranceDetails @UserId, @ZipCode, @City, @IsInsured, @InsuranceCompany, @EmployeeStrength, @CoverageExpires, @Language, @Notes,@Longitude,@Latitude,@IndustryId,@SubIndustryId,@DocPath,@DeclarationDocBase64 ";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@ZipCode", ZipCode),
                               new SqlParameter("@City", City),
                               new SqlParameter("@IsInsured", IsInsured),
                               new SqlParameter("@InsuranceCompany",InsuranceCompany),
                               new SqlParameter("@EmployeeStrength", EmployeeStrength),
                               new SqlParameter("@CoverageExpires", CoverageExpires),
                               new SqlParameter("@Language", Language),
                               new SqlParameter("@Notes", Notes),
                               new SqlParameter("@Longitude", Longitude),
                               new SqlParameter("@Latitude", Latitude),
                               new SqlParameter("@IndustryId",Convert.ToInt32(IndustryId)),
                               new SqlParameter("@SubIndustryId",SubIndustryId),
                               new SqlParameter("@DocPath",DocPath),
                               new SqlParameter("@DeclarationDocBase64",DeclarationDoc) 
                              };
                User = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SaveBenefitInsuranceDetails", Ex.Message.ToString(), "BrokerWSUtility.cs_SaveBenefitInsuranceDetails", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return User;
        }

        public static int SendMessagesForMeineke(int UserId, int BrokerId, string InsuranceType, string Note, string LocalDateTime)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int count = 0;

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspBrokerMessagesForMeinekeApp");
                db.AddInParameter(dbCommand, "UserId", DbType.String, UserId);
                db.AddInParameter(dbCommand, "BrokerId", DbType.String, BrokerId);
                db.AddInParameter(dbCommand, "InsuranceType", DbType.String, InsuranceType);
                db.AddInParameter(dbCommand, "Note", DbType.String, Note);
                db.AddInParameter(dbCommand, "LocalDateTime", DbType.String, LocalDateTime);
                DataSet dsCustMsgId = db.ExecuteDataSet(dbCommand);
                if (dsCustMsgId.Tables.Count > 0)
                {
                    if (dsCustMsgId.Tables[0].Rows.Count > 0)
                    {
                        count = Convert.ToInt32(dsCustMsgId.Tables[0].Rows[0]["CustMsgId"]);
                    }
                }

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SendMessagesForMeineke", Ex.Message.ToString(), "BrokerWSUtility.cs_SendMessagesForMeineke()", BrokerUtility.GetIPAddress(UserId.ToString()));
            }

            return count;
        }

        public static int Save401kInsuranceDetails(int UserId, string ZipCode, string City, string Longitude, string Latitude, string CurrentPlan, string NoOfEmp, string PlanSize, string DocPath, string DeclarationDocBase64) //29May18
        {
            int User = 0;
            BrokerDBEntities DB = new BrokerDBEntities();
            try
            {
                var @cmdText = "exec uspSave401kDetails @UserId, @ZipCode, @City,@Longitude,@Latitude,@CurrentPlan,@NoOfEmp,@PlanSize,@DocPath,@DeclarationDocBase64";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@ZipCode", ZipCode),
                               new SqlParameter("@City", City),
                               new SqlParameter("@Longitude", Longitude),
                               new SqlParameter("@Latitude", Latitude),
                               new SqlParameter("@CurrentPlan",CurrentPlan),
                               new SqlParameter("@NoOfEmp",NoOfEmp),
                               new SqlParameter("@PlanSize",PlanSize),
                               new SqlParameter("@DocPath",DocPath),
                               new SqlParameter("@DeclarationDocBase64",DeclarationDocBase64)
                              };
                User = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "Save401kInsuranceDetails_Website", Ex.Message.ToString(), "BrokerWebDB.cs_Save401kInsuranceDetails", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return User;
        }

        public static int SaveLiabilityInsuranceDetailsNew(int UserId, string ZipCode, string City, string IsInsured, string InsuranceCompany, string SICCode, string Revenue, string CoverageExpires, string Language, string Notes, string Longitude, string Latitude, string IndustryId, string SubIndustryId, string DocPath, string DeclarationDoc)
        {
            int User = 0;
            BrokerDBEntities DB = new BrokerDBEntities();
            try
            {
                var @cmdText = "exec uspSaveLiabilityInsuranceDetailsNew @UserId, @ZipCode, @City, @IsInsured, @InsuranceCompany, @SICCode, @Revenue, @CoverageExpires, @Language, @Notes,@Longitude,@Latitude,@IndustryId,@SubIndustryId,@DocPath,@DeclarationDocBase64";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@ZipCode", ZipCode),
                               new SqlParameter("@City", City),
                               new SqlParameter("@IsInsured", IsInsured),
                               new SqlParameter("@InsuranceCompany",InsuranceCompany),
                               new SqlParameter("@SICCode", SICCode),
                               new SqlParameter("@Revenue", Revenue),
                               new SqlParameter("@CoverageExpires", CoverageExpires),
                               new SqlParameter("@Language", Language),
                               new SqlParameter("@Notes", Notes),
                               new SqlParameter("@Longitude", Longitude),
                               new SqlParameter("@Latitude", Latitude),
                               new SqlParameter("@IndustryId", Convert.ToInt32(IndustryId)),
                               new SqlParameter("@SubIndustryId", SubIndustryId),
                               new SqlParameter("@DocPath",DocPath),
                               new SqlParameter("@DeclarationDocBase64",DeclarationDoc)
                    
                              };
                User = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SaveLiabilityInsuranceDetailsNew", Ex.Message.ToString(), "BrokerWSUtility.cs_SaveLiabilityInsuranceDetailsNew", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return User;
        }

        #endregion For Meineke Company


        public static bool SendContactMessageToBrokerOnEmail(int BrokerId, string InsuranceType, string BrokerName, string EmailId, string BrokerMessage)
        {
            bool SuccessFlag = false;
            EmailInfoModel oEmail = new EmailInfoModel();
            StringBuilder Obody = new StringBuilder();
            try
            {
                Obody.Append("<html><head></head><body><br/>");
                Obody.Append("" + BrokerMessage + "<br/><br> ");
                Obody.Append("Thank you,<br/>");
                Obody.Append("Anton and Curtis,<br/>");
                Obody.Append("brokkrr Website Team.</body></html>");


                oEmail.Message = Obody.ToString();
                oEmail.Subject = "New message on brokkrr app.";
                oEmail.ToEmailid = EmailId;
                oEmail.FromEmailid = strFromEmail;
                oEmail.MailType = "ContactMessage";
                oEmail.UserId = "0";
                SuccessFlag = sendmail(oEmail);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32("0"), "SendContactMessageToBrokerOnEmail_website", ex.Message.ToString(), "BrokerWSUtility.cs_SendContactMessageToBrokerOnEmail()", "");
            }
            return SuccessFlag;

        }

        public static bool SendChatMessageOnEmail(string CustomerName, string EmailId, string Message)
        {
            bool SuccessFlag = false;
            EmailInfoModel oEmail = new EmailInfoModel();
            StringBuilder Obody = new StringBuilder();
            try
            {
                Obody.Append("<html><head></head><body><br/>");
                Obody.Append("" + CustomerName + " sent a message to you - ");
                Obody.Append("" + Message + "<br/><br> ");
                Obody.Append("Thank you,<br/>");
                Obody.Append("Anton and Curtis,<br/>");
                Obody.Append("brokkrr Website Team.</body></html>");


                oEmail.Message = Obody.ToString();
                oEmail.Subject = "New message on brokkrr app.";
                oEmail.ToEmailid = EmailId;
                oEmail.FromEmailid = strFromEmail;
                oEmail.MailType = "ChatMessage";
                oEmail.UserId = "0";
                SuccessFlag = sendmail(oEmail);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32("0"), "SendContactMessageToBrokerOnEmail_website", ex.Message.ToString(), "BrokerWSUtility.cs_SendContactMessageToBrokerOnEmail()", "");
            }
            return SuccessFlag;

        }

        public static int SendMessagesForMeinekePersonal(int UserId, int BrokerId, string InsuranceType, string Note, string LocalDateTime)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int count = 0;
            DataSet dsUserDetails = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspBrokerMessagesForMeinekePersonal");
                db.AddInParameter(dbCommand, "UserId", DbType.Int32, UserId);
                db.AddInParameter(dbCommand, "BrokerId", DbType.Int32, BrokerId);
                db.AddInParameter(dbCommand, "InsuranceType", DbType.String, InsuranceType);
                db.AddInParameter(dbCommand, "Note", DbType.String, Note);
                db.AddInParameter(dbCommand, "LocalDateTime", DbType.String, LocalDateTime);

                dsUserDetails = db.ExecuteDataSet(dbCommand);

                if (dsUserDetails.Tables.Count > 0)
                {
                    if (dsUserDetails.Tables[0].Rows.Count > 0)
                    {
                        count = Convert.ToInt32(dsUserDetails.Tables[0].Rows[0]["CustMsgId"]);
                    }
                }

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SendMessages", Ex.Message.ToString(), "BrokerWSUtility.cs_SendMessages()", BrokerUtility.GetIPAddress(UserId.ToString()));
            }

            return count;
        }

        public static DataSet GetVideoList(int UserId)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;
            DataSet dsUserDetails = null;

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetvideoListForApp");
                db.AddInParameter(dbCommand, "UserId", DbType.Int32, UserId);

                dsUserDetails = db.ExecuteDataSet(dbCommand);

            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "GetVideoList", ex.Message.ToString(), "BrokerWSUtility.cs_GetVideoList()", "");
            }
            return dsUserDetails;
        }

        public static DataSet GetUnWatchedVideoCount(int UserId)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;
            DataSet dsUserDetails = null;

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetUnWatchedVideoCount");
                db.AddInParameter(dbCommand, "UserId", DbType.Int32, UserId);

                dsUserDetails = db.ExecuteDataSet(dbCommand);

            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "GetUnWatchedVideoCount", ex.Message.ToString(), "BrokerWSUtility.cs_GetUnWatchedVideoCount()", "");
            }
            return dsUserDetails;
        }

        public static int SetVideoWatched(int UserId, int VideoId)
        {
            string JSONString = "";
            DataSet dsSetIsRead = null;
            BrokerDBEntities DB = new BrokerDBEntities();
            int Result = 0;
            try
            {
                var @cmdText = "exec uspSetVideoWatched @UserId,@VideoId";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@VideoId", VideoId)
                    
                              };
                Result = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "SetVideoWatched", Ex.Message.ToString(), "BrokerWSUtility.cs_SetVideoWatched()", "0");
            }
            return Result;
        }

        public static int SetAllVideoWatchedForWeb(int UserId)
        {
            string JSONString = "";
            DataSet dsSetIsRead = null;
            BrokerDBEntities DB = new BrokerDBEntities();
            int Result = 0;
            try
            {
                var @cmdText = "exec uspSetAllVideoWatchedForWeb @UserId";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId)
                                                  
                              };
                Result = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "SetAllVideoWatchedForWeb", Ex.Message.ToString(), "BrokerWSUtility.cs_SetAllVideoWatchedForWeb()", "0");
            }
            return Result;
        }

        public static bool SendVideoLinkOnEmail(string EmailId, string Title, string Url, string Description, string UserType)
        {
            bool SuccessFlag = false;
            EmailInfoModel oEmail = new EmailInfoModel();
            StringBuilder Obody = new StringBuilder();
            try
            {
                Obody.Append("<html><head></head><body>Hi ,<br/>");
                Obody.Append("New video added in your brokkrr briefcase -<br><br> ");
                Obody.Append("Title - <b>" + Title + "</b><br> ");
                if (UserType == "Customer")
                {
                    string UrlLink = strDomainName + "Login/CustomerLogin?ReturnUrl=BrokkrrBriefcase/Briefcase";

                    Obody.Append("Url - <a href=" + UrlLink + ">" + UrlLink + "</a>" + "<br><br/><br/>");
                }
                else if (UserType == "Broker")
                {
                    string UrlLink = strDomainName + "Login/BrokerLogin?ReturnUrl=BrokkrrBriefcase";

                    Obody.Append("Url - <a href=" + UrlLink + ">" + UrlLink + "</a>" + "<br><br/><br/>");

                    //Obody.Append("Url - <a href=" + strDomainName + "Login/BrokerLogin?ReturnUrl=BrokkrrBriefcase></a>" + "<br><br/><br/>");
                }
                Obody.Append("Thank you,<br/>");
                Obody.Append("Anton and Curtis,<br/>");
                Obody.Append("brokkrr Website Team.</body></html>");


                oEmail.Message = Obody.ToString();
                oEmail.Subject = "New video added in your brokkrr briefcase";
                oEmail.ToEmailid = EmailId;
                oEmail.FromEmailid = strFromEmail;
                oEmail.MailType = "VideoEmail";
                oEmail.UserId = "0";
                SuccessFlag = sendmail(oEmail);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32("0"), "SendContactMessageToBrokerOnEmail_website", ex.Message.ToString(), "BrokerWSUtility.cs_SendContactMessageToBrokerOnEmail()", "");
            }
            return SuccessFlag;

        }

        public static DataSet GetDocBase64String(int MainMessageId)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;
            DataSet dsUserDetails = null;

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetDocBase64String");
                db.AddInParameter(dbCommand, "MainMessageId", DbType.Int32, MainMessageId);

                dsUserDetails = db.ExecuteDataSet(dbCommand);

            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(0, "GetDocBase64String", ex.Message.ToString(), "BrokerWSUtility.cs_GetDocBase64String()", "");
            }
            return dsUserDetails;
        }

        public static DataSet GetAllChatMessages()
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;
            DataSet dsChatMessages = null;

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetAllChatMessages");

                dsChatMessages = db.ExecuteDataSet(dbCommand);

            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(0, "GetAllChatMessages", ex.Message.ToString(), "BrokerWSUtility.cs_GetAllChatMessages()", "");
            }
            return dsChatMessages;
        }

        public static int SaveWebsiteLog(int UserId, string LogType,string LogMessage)
        {
            
            BrokerDBEntities DB = new BrokerDBEntities();
            int Result = 0;
            try
            {
                var @cmdText = "exec spWebsiteLog @UserId,@Type,@LogMessage";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@Type", LogType),
                                new SqlParameter("@LogMessage", LogMessage)
                    
                              };
                Result = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "SaveWebsiteLog", Ex.Message.ToString(), "BrokerWSUtility.cs_SaveWebsiteLog()", "0");
            }
            return Result;
        }
    }
}