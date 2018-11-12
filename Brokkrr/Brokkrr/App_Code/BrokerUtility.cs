using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using BrokerMVC.BrokerService;
using System.IO;
using BrokerMVC.Models;
using System.Net.Mail;
using System.Configuration;
using System.Net.Mime;


using BrokerMVC.BrokerWebDB;
using BrokerMVC.Models;
using System.Globalization;
using PushSharp.Apple;
using Newtonsoft.Json.Linq;
using Microsoft.Owin.Infrastructure;
using System.Drawing;

namespace BrokerMVC.App_Code
{
    public class BrokerUtility
    {

        public static string strDomainName = ConfigurationManager.AppSettings["DomainName"].ToString();
        public static string strProfilePicForlderName = ConfigurationManager.AppSettings["ProfilePicForlderName"].ToString();
        public static string strResumeForlderName = ConfigurationManager.AppSettings["ResumeForlderName"].ToString();

        public static string strProfilePicImageFolder = ConfigurationManager.AppSettings["ProfilePicImageFolder"].ToString();
        public static string strResumeImageFolder = ConfigurationManager.AppSettings["ResumeImageFolder"].ToString();
        public static string strUploadedCompLogoFolder = ConfigurationManager.AppSettings["UploadedCompLogoFolder"].ToString();
        public static string strEducationLogoFolder = ConfigurationManager.AppSettings["EducationLogo"].ToString();
        public static string strExperienceCompLogoFolder = ConfigurationManager.AppSettings["ExperienceCompLogoFolder"].ToString();

        public static string strCompanyLogoFolder = ConfigurationManager.AppSettings["CompanyLogoFolder"].ToString();

        public static string strGoogleAppID = ConfigurationManager.AppSettings["GoogleAppID"].ToString();
        public static string strSENDER_ID = ConfigurationManager.AppSettings["SENDER_ID"].ToString();

        public static string strUsersToShowByDefaultInSearchList = ConfigurationManager.AppSettings["UsersToShowByDefaultInSearchList"].ToString();
        public static string strDeclarationDoc = ConfigurationManager.AppSettings["DeclarationDocumentFolder"].ToString();

        BrokerDBEntities DB = new BrokerDBEntities();

        public static string strMailTestingFlag = ConfigurationManager.AppSettings["EmailFlag"].ToString();

        public static void ErrorLog(int UserId, string ErrorType, string ErrorMessage, string PageName, string IPAddress)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            var @cmdText = "exec spErrorLog @UserId,@ErrorType,@ErrorMessage,@PageName,@IPAddress";
            var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@ErrorType",ErrorType),
                               new SqlParameter("ErrorMessage",ErrorMessage),
                               new SqlParameter("@PageName",PageName),
                               new SqlParameter("@IPAddress",IPAddress),
                                              
                              };
            DB.Database.ExecuteSqlCommand(@cmdText, @params);
        }

        public static DataSet CheckUSerExist(string UserName)
        {

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("spCheckUserExist");
            db.AddInParameter(dbCommand, "UserName", DbType.String, UserName);

            DataSet oDataSet = db.ExecuteDataSet(dbCommand);

            //BrokerDBEntities DB = new BrokerDBEntities();
            //var @cmdText = "exec spCheckUserExist @UserName";
            //var @params = new[]{
            //                   new SqlParameter("UserName", UserName )
            //                  };
            //List<spCheckUserExist_Result> user = DB.Database.SqlQuery<spCheckUserExist_Result>(@cmdText, @params).ToList<spCheckUserExist_Result>();

            return oDataSet;
        }

        public static DataSet LoginExternal(string Provider, string FirstName, string LastName, string EmailID, string UserType, string UserId, string RegisteredFor)//22Feb18
        {
            string IsNewRegister = "false";

            DataSet dsUSerExist = null;

            dsUSerExist = BrokerUtility.CheckUSerExist(EmailID);


            if (dsUSerExist.Tables.Count <= 0)
            {
                dsUSerExist = RegisterUser(FirstName, LastName, EmailID, "", "", "", null, null, "", "", "1", UserType, Provider, "0", UserId, "0", RegisteredFor);//22Feb18
                if (dsUSerExist.Tables.Count > 0)
                {
                    IsNewRegister = "true";
                    // write email for registration
                    BrokerWSUtility.SendRegistrationEmailForExternal(FirstName, LastName, EmailID, dsUSerExist.Tables[0].Rows[0]["UserId"].ToString(), Provider, UserType);

                }
            }
            if (dsUSerExist.Tables.Count > 0)
            {
                if (dsUSerExist.Tables[0].Rows[0]["UserType"].ToString() == UserType)
                {
                    if (UserType == "Broker")
                    {
                        dsUSerExist.Tables[0].TableName = "UserDetails";
                        dsUSerExist.Tables[1].TableName = "ExperienceDetails";
                        dsUSerExist.Tables[2].TableName = "EducationDetails";
                        dsUSerExist.AcceptChanges();
                    }
                    else
                    {
                        dsUSerExist.Tables[0].TableName = "UserDetails";
                        dsUSerExist.AcceptChanges();
                    }
                }
                else
                {
                    //dsUSerExist.Tables.Clear();
                    DataSet ds = new DataSet();
                    DataTable dtMessage = new DataTable();
                    dtMessage.Columns.Add("Message");

                    dtMessage.Rows.Add(dsUSerExist.Tables[0].Rows[0]["EmailId"].ToString() + " has already register with User Type - " + dsUSerExist.Tables[0].Rows[0]["UserType"].ToString());

                    ds.Tables.Add(dtMessage);
                    ds.Tables[0].TableName = "Response";
                    ds.AcceptChanges();

                    return ds;

                }
            }

            if (dsUSerExist.Tables.Count > 0)
            {
                dsUSerExist.Tables.Add("RegisterFlag");
                dsUSerExist.Tables["RegisterFlag"].Columns.Add("IsNewRegister");
                dsUSerExist.Tables["RegisterFlag"].Rows.Add(IsNewRegister);
            }
            return dsUSerExist;

        }

        public static DataSet RegisterUser(string FirstName, string LastName, string EmailId, string Password, string Address, string City, string StateId, string Country, string PinCode, string MobNo, string IsActive, string UserType, string RegisterdType, string IsUpdateProfile, string UserId, string RegistrationCode, string RegisteredFor)//22Feb18
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            DataSet oDataSet = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("spRegisterUser");
                db.AddInParameter(dbCommand, "FirstName", DbType.String, FirstName);
                db.AddInParameter(dbCommand, "LastName", DbType.String, LastName);
                db.AddInParameter(dbCommand, "EmailId", DbType.String, EmailId);
                db.AddInParameter(dbCommand, "Password", DbType.String, Password);
                db.AddInParameter(dbCommand, "Address", DbType.String, Address);
                db.AddInParameter(dbCommand, "City", DbType.String, City);
                db.AddInParameter(dbCommand, "State", DbType.String, StateId);
                db.AddInParameter(dbCommand, "Country", DbType.String, Country);
                db.AddInParameter(dbCommand, "PinCode", DbType.String, PinCode);
                db.AddInParameter(dbCommand, "MobNo", DbType.String, MobNo);
                db.AddInParameter(dbCommand, "IsActive", DbType.String, IsActive);
                db.AddInParameter(dbCommand, "UserType", DbType.String, UserType);
                db.AddInParameter(dbCommand, "RegisterdType", DbType.String, RegisterdType);
                db.AddInParameter(dbCommand, "IsUpdateProfile", DbType.String, IsUpdateProfile);
                db.AddInParameter(dbCommand, "RegistrationCode", DbType.String, RegistrationCode);
                db.AddInParameter(dbCommand, "RegisteredFor", DbType.String, RegisteredFor);//22Feb18
                oDataSet = db.ExecuteDataSet(dbCommand);
                return oDataSet;
            }
            catch (Exception ex)
            {
                ErrorLog(Convert.ToInt32(UserId), "RegisterUser", ex.Message.ToString(), "LoginController.cs_RegisterUser()", BrokerUtility.GetIPAddress(UserId));
            }
            return oDataSet;
        }


        public static List<spGetUserDetails_Result> GetUserDetails(int iUserId)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            var @cmdText = "exec spGetUserDetails @UserId";
            var @params = new[]{
                               new SqlParameter("UserId", iUserId )
                              };
            List<spGetUserDetails_Result> user = DB.Database.SqlQuery<spGetUserDetails_Result>(@cmdText, @params).ToList<spGetUserDetails_Result>();
            return user;
        }

        public static List<uspGetPaymentAmount_Result> GetPaymentDetails()
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            var @cmdText = "exec uspGetPaymentAmount";

            List<uspGetPaymentAmount_Result> user = DB.Database.SqlQuery<uspGetPaymentAmount_Result>(@cmdText).ToList<uspGetPaymentAmount_Result>();
            return user;
        }


        public static int UpdateUser(string UserId, string Password, string Address, string City, int? StateId, int? CountryId, string PinCode, string MobNo, string IsActive, string UserType, string IsUpdateProfile)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;
            try
            {
                var @cmdText = "exec spUpdateUser @UserId,@Password,@Address,@City,@StateId,@CountryId,@PinCode,@MobNo,@IsActive,@UserType,@IsUpdateProfile";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@Password", Password),
                               new SqlParameter("@Address", (Address==null?"":Address)),
                               new SqlParameter("@City", (City==null?"":City)),
                               new SqlParameter("@StateId", (StateId==null?(object)DBNull.Value:StateId)),// DBNull.Value
                               new SqlParameter("@CountryId", (CountryId==null?(object)DBNull.Value:CountryId)),// DBNull.Value
                               new SqlParameter("@PinCode", PinCode),
                               new SqlParameter("@MobNo", (MobNo==null?"":MobNo)),
                               new SqlParameter("@IsActive", IsActive),
                               new SqlParameter("@UserType", UserType),
                               new SqlParameter("@IsUpdateProfile",IsUpdateProfile)
                    
                              };
                user = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception ex)
            {
                ErrorLog(Convert.ToInt32(UserId), "UpdateUser", ex.Message.ToString(), "HomeController.cs_UpdateProfileView()", BrokerUtility.GetIPAddress(UserId));
            }
            return user;
        }


        public static string EncryptURL(string url)
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(url);
            string encodedText = Convert.ToBase64String(bytesToEncode);
            return encodedText;

        }

        public static string DecryptURL(string EncodedUrl)
        {
            byte[] decodedBytes = Convert.FromBase64String(EncodedUrl);
            string decodedText = Encoding.UTF8.GetString(decodedBytes);
            return decodedText;
        }

        internal static string GetIPAddress(string UserId)
        {
            string ipAddress = "";
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddress = ip.ToString();
                    }
                }
                return ipAddress;
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "GetIPAddress", ex.Message.ToString(), "BrokerUtility.cs_GetIPAddress()", "");
                return ipAddress;
            }
        }

        public static List<uspInsertPaymentDetails_Result> InsertPaymentDetails(string UserId, string tx, string ItemName, string Description, string amt, bool status, string PaymentMode)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            List<uspInsertPaymentDetails_Result> user = null;
            try
            {


                var @cmdText = "exec uspInsertPaymentDetails @UserId,@PaymentId,@ItemName,@Description,@Amount,@IsPaid,@PaymentMode";
                var @params = new[]{
                               new SqlParameter("@UserId", Convert.ToInt32(UserId)),
                               new SqlParameter("@PaymentId",  tx),
                               new SqlParameter("@ItemName", ItemName),
                               new SqlParameter("@Description", Description),
                               new SqlParameter("@Amount", amt),
                               new SqlParameter("@IsPaid", status),// DBNull.Value
                               new SqlParameter("@PaymentMode", PaymentMode),// DBNull.Value
                                                 
                              };
                user = DB.Database.SqlQuery<uspInsertPaymentDetails_Result>(@cmdText, @params).ToList<uspInsertPaymentDetails_Result>();
            }
            catch (Exception ex)
            {
                ErrorLog(Convert.ToInt32(UserId), "InsertPaymentDetails", ex.Message.ToString(), "BrokerUtility.cs_InsertPaymentDetails()", BrokerUtility.GetIPAddress(UserId));
            }
            return user;
        }

        public static int UpdateNewPaymentEntry(int Amount, DateTime DateOfEffect, string UserId)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;
            try
            {
                var @cmdText = "exec uspInsertNewPayment @Amount,@DateOfEffect";
                var @params = new[]{
                               new SqlParameter("@Amount", Amount),
                               new SqlParameter("@DateOfEffect", DateOfEffect)                    
                              };
                user = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception ex)
            {
                ErrorLog(Convert.ToInt32(UserId), "UpdateNewPaymentEntry", ex.Message.ToString(), "UpdatePaymentController.cs_IndexView()", BrokerUtility.GetIPAddress(UserId));
            }
            return user;
        }

        //public static List<uspGetPaymentMasterData_Result> GetPaymentMasterData()
        //{
        //    BrokerDBEntities DB = new BrokerDBEntities();
        //    var @cmdText = "exec uspGetPaymentMasterData";
        //    //var @params = new[]{
        //    //                   new SqlParameter("UserId", iUserId )
        //    //                  };
        //    List<uspGetPaymentMasterData_Result> user = DB.Database.SqlQuery<uspGetPaymentMasterData_Result>(@cmdText).ToList<uspGetPaymentMasterData_Result>();
        //    return user;
        //}

        /////////////////////////////////////////////////////////////////////////////////////////

        //Create a text file for storing base64 string on server.

        public static bool WriteFile(string FileName, string base64String)
        {
            using (FileStream fs = File.Create(FileName))
            {
                // Add some text to file
                Byte[] title = new UTF8Encoding(true).GetBytes(base64String);
                fs.Write(title, 0, title.Length);
                return true;
            };
        }

        public static bool CheckFile(string FileName1)
        {
            if (File.Exists(FileName1))
            {
                File.Delete(FileName1);
            }
            return true;
        }

        public static List<uspSaveBrokerBasicDetails_Result> SaveBrokerBasicDetails(string FirstName, string LastName, string Phone, string Email, string Address, string ZipCode, string Title, string Awards, string Language, string Specialities, string Skills, string Recommendations, string License, string ExpiryDate, string Password, string RegistrationCode)
        {
            //int User = 0;
            BrokerDBEntities DB = new BrokerDBEntities();
            List<uspSaveBrokerBasicDetails_Result> User = null;
            try
            {
                var @cmdText = "exec uspSaveBrokerBasicDetails @FirstName,@LastName,@Phone,@Email,@Address,@ZipCode,@Title,@Awards,@Language,@Specialities,@Skills,@Recommendations,@License,@ExpiryDate,@Password,@RegistrationCode";
                var @params = new[]{
                               new SqlParameter("@FirstName",FirstName),
                               new SqlParameter("@LastName",LastName),
                               new SqlParameter("@Phone",  Phone),
                               new SqlParameter("@Email", Email),
                               new SqlParameter("@Address", Address),
                               new SqlParameter("@ZipCode", ZipCode),
                               new SqlParameter("@Title", Title),
                               new SqlParameter("@Awards", Awards),
                               new SqlParameter("@Language", Language),
                               new SqlParameter("@Specialities",  Specialities),
                               new SqlParameter("@Skills", Skills),
                               new SqlParameter("@Recommendations", Recommendations),
                               new SqlParameter("@License", License),
                               new SqlParameter("@ExpiryDate", ExpiryDate),
                               new SqlParameter("@Password", Password),
                               new SqlParameter("@RegistrationCode", RegistrationCode),                  
                              };
                User = DB.Database.SqlQuery<uspSaveBrokerBasicDetails_Result>(@cmdText, @params).ToList<uspSaveBrokerBasicDetails_Result>();
            }
            catch (Exception Ex)
            {
                ErrorLog(0, "SaveBrokerBasicDetails", Ex.Message.ToString(), "BrokerUtility.cs_SaveBrokerBasicDetails", "0");
            }
            return User;
        }

        public static int SaveBrokerEducationDetails(int UserId, string Education)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;
            try
            {
                var @cmdText = "exec uspSaveBrokerEducationDetails @UserId,@Education";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@Education", Education)                    
                              };
                user = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception ex)
            {
                ErrorLog(0, "SaveBrokerEducationDetails", ex.Message.ToString(), "BrokerUtility.cs_SaveBrokerEducationDetails()", "0");
            }
            return user;
        }

        public static int SaveBrokerCompanyDetails(int UserId, string CompanyName)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;
            try
            {
                var @cmdText = "exec uspSaveBrokerCompanyDetails @UserId,@CompanyName";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@CompanyName", CompanyName)                    
                              };
                user = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception ex)
            {
                ErrorLog(0, "SaveBrokerCompanyDetails", ex.Message.ToString(), "BrokerUtility.cs_SaveBrokerCompanyDetails()", "0");
            }
            return user;
        }

        public static int SaveBrokerFiles(string FileName, int UserId, string FieldName, string FieldName1, string RenamedImageName)
        {
            int User = 0;
            BrokerDBEntities DB = new BrokerDBEntities();

            try
            {
                var @cmdText = "exec uspSaveBrokerFiles @FileName,@UserId,@FieldName,@FieldName1,@RenamedImageName";
                var @params = new[]{
                               new SqlParameter("@FileName", FileName),
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@FieldName", FieldName),
                               new SqlParameter("@FieldName1", FieldName1),
                               new SqlParameter("@RenamedImageName", RenamedImageName) 
                              };
                User = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception ex)
            {
                ErrorLog(UserId, "SaveBrokerFiles", ex.Message.ToString(), "BrokerUtility.cs_SaveBrokerFiles()", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return User;
        }

        public static List<uspSaveCustomerBasicDetails_Result> SaveCustomerBasicDetails(string FirstName, string LastName, string Phone, string Email, string Address, string ZipCode, string TempPass, string random, string HouseType, string IsHavingCars, int NoOfCars, string Occupation, string CompanyName, string NoofEmployee, string EstPremium, string Website, string RegisteredFor)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            List<uspSaveCustomerBasicDetails_Result> User = null;
            try
            {
                var @cmdText = "exec uspSaveCustomerBasicDetails @FirstName,@LastName,@Phone,@Email,@Address,@ZipCode,@Password,@RegistrationCode,@HouseType,@IsHavingCars,@NoOfCars,@Occupation,@CompanyName,@NoofEmployee, @EstPremium, @Website, @RegisteredFor";
                var @params = new[]{
                               new SqlParameter("@FirstName",FirstName),
                               new SqlParameter("@LastName",LastName),
                               new SqlParameter("@Phone",  Phone),
                               new SqlParameter("@Email", Email),
                               new SqlParameter("@Address", Address),
                               new SqlParameter("@ZipCode", ZipCode),
                               new SqlParameter("@Password", TempPass),
                               new SqlParameter("@RegistrationCode", random),
                               new SqlParameter("@HouseType", HouseType),
                               new SqlParameter("@IsHavingCars",  IsHavingCars),
                               new SqlParameter("@NoOfCars", NoOfCars),
                               new SqlParameter("@Occupation", Occupation),
                               new SqlParameter("@CompanyName", CompanyName),
                               new SqlParameter("@NoofEmployee", NoofEmployee),
                               new SqlParameter("@EstPremium", EstPremium),
                               new SqlParameter("@Website", Website),
                               new SqlParameter("@RegisteredFor", RegisteredFor) 
                              };
                User = DB.Database.SqlQuery<uspSaveCustomerBasicDetails_Result>(@cmdText, @params).ToList<uspSaveCustomerBasicDetails_Result>();
            }
            catch (Exception Ex)
            {
                ErrorLog(0, "SaveCustomerBasicDetails", Ex.Message.ToString(), "BrokerUtility.cs_SaveCustomerBasicDetails", "0");
            }
            return User;
        }

        public static int UpdateBrokerPaymentDoneFlag(string UserId)
        {
            int User = 0;
            BrokerDBEntities DB = new BrokerDBEntities();

            try
            {
                var @cmdText = "exec uspUpdateBrokerPaymentDoneFlag @UserId";
                var @params = new[]{
                                    new SqlParameter("@UserId", UserId)
                              };
                User = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception ex)
            {
                ErrorLog(Convert.ToInt32(UserId), "UpdateBrokerPaymentDoneFlag", ex.Message.ToString(), "BrokerUtility.cs_UpdateBrokerPaymentDoneFlag()", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return User;
        }

        public static int ResetPassWord(string EmailId, string TempPass)
        {
            int User = 0;
            BrokerDBEntities DB = new BrokerDBEntities();

            try
            {
                var @cmdText = "exec uspResetPassWord @EmailId,@Password";
                var @params = new[]{
                                    new SqlParameter("@EmailId", EmailId),
                                    new SqlParameter("@Password", TempPass)
                              };
                User = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception ex)
            {
                ErrorLog(0, "ResetPassWord", ex.Message.ToString(), "BrokerUtility.cs_ResetPassWord()", "0");
            }
            return User;
        }


        //Send Contact Us Mail from user
        public static bool SendContactUsMail(string Name, string Email, string Subject, string Message)
        {
            bool SuccessFlag = false;
            EmailInfoModel oEmail = new EmailInfoModel();
            StringBuilder Obody = new StringBuilder();
            try
            {
                Obody.Append("<html><head></head><body>" + Message + "");
                //Obody.Append("Greetings from Team Brokkrr! <br/><br/>Thank you for registering  with our app as " + UserType + ".  For your security, we need you to verify your email id.<br/> ");
                Obody.Append("</body></html>");


                oEmail.Message = Obody.ToString();
                oEmail.Subject = Subject;
                oEmail.ToEmailid = "";
                oEmail.FromEmailid = Email;
                oEmail.MailType = "ContactUs";
                oEmail.UserId = "0";
                SuccessFlag = sendmail(oEmail);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(0, "SendContactUsMail", ex.Message.ToString(), "BrokerUtility.cs_SendContactUsMail", "");
            }
            return SuccessFlag;

        }

        // send email function
        public static bool sendmail(EmailInfoModel oEmail)
        {
            bool mailFlag = false;
            SmtpClient smtpclient = new SmtpClient();
            MailMessage Mailmsg = null;


            //For Gmail mail sending Code

            //string strPassword = ConfigurationManager.AppSettings["FromEmailPass"].ToString();
            //string strHost = ConfigurationManager.AppSettings["Host"].ToString();
            //string strPort = ConfigurationManager.AppSettings["Port"].ToString();

            try
            {
                ////for testing purpose
                if (strMailTestingFlag == "false")
                {
                    oEmail.ToEmailid = ConfigurationManager.AppSettings["ToContactUsEmailIdTest"].ToString();
                }

                oEmail.ToEmailid = ConfigurationManager.AppSettings["ToContactUsEmailIdTest"].ToString();

                //For Regular mail sending code(Used on Server)

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
                smtpclient.Send(Mailmsg);


                ////For Gmail mail sending Code

                //var fromAddress = new MailAddress(oEmail.FromEmailid, "");
                //var toAddress = new MailAddress(oEmail.ToEmailid, "");
                //AlternateView av = AlternateView.CreateAlternateViewFromString(oEmail.Message, null, MediaTypeNames.Text.Html);
                //var smtp = new SmtpClient
                //{
                //    Host = strHost,
                //    Port = Convert.ToInt32(strPort),
                //    EnableSsl = true,
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    UseDefaultCredentials = false,
                //    Credentials = new NetworkCredential(fromAddress.Address, strPassword)
                //};
                //var message = new MailMessage(fromAddress, toAddress);
                //message.Subject = oEmail.Subject;
                //message.Body = oEmail.Message;
                //message.AlternateViews.Add(av);
                //smtp.Send(message);

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





        /***************santosh ******************  */

        /*santosh 22March17*/
        public static DataSet SaveHomeInsuranceDetails(HomeInsurance home)
        {
            DataSet dsUserDetails = null;


            //DataSet dsUserDetailsMain = null;

            int User, UserId = 0;
            string ZipCode = "", City = "", EstimatedValue = "", IsInsured = "false", CompanyName = "",
                CoverageExpires = "", Language = "", Notes = "", Longitude = "", Latitude = "";


            try
            {
                if (HttpContext.Current.Session["UserId"] != null)
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                    ZipCode = home.ZipCode;
                    City = home.City;
                    EstimatedValue = home.EstimatedValue;

                    if (home.IsInsured == "Currently insured")
                        IsInsured = "true";

                    if (IsInsured == "true")
                        CompanyName = home.InsuranceCompany;
                    else
                        CompanyName = "";

                    CoverageExpires = home.CoverageExpires;
                    Language = home.Language;

                    if (home.Notes == null) Notes = ""; else Notes = home.Notes;

                    if (home.Longitude == null) Longitude = ""; else Longitude = home.Longitude;
                    if (home.Latitude == null) Latitude = ""; else Latitude = home.Latitude;


                    User = BrokerWebDB.BrokerWebDB.SaveHomeInsuranceDetails(UserId, ZipCode, City, EstimatedValue, IsInsured, CompanyName, CoverageExpires, Language, Notes, Longitude, Latitude, "", "");

                    if (User != 0)
                    {
                        dsUserDetails = BrokerWebDB.BrokerWebDB.SearchBrokersList(UserId, ZipCode, City, Language, "Home", Longitude, Latitude, EstimatedValue, "0", "0");

                        if (dsUserDetails.Tables.Count > 0)
                        {
                            if (dsUserDetails.Tables[0].Rows.Count > 0)
                            {


                                dsUserDetails = GetFinalData(dsUserDetails);

                            }
                            else
                            {
                                dsUserDetails = BrokerWebDB.BrokerWebDB.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                                if (dsUserDetails.Tables.Count > 0)
                                {
                                    if (dsUserDetails.Tables[0].Rows.Count > 0)
                                    {
                                        dsUserDetails = GetFinalData(dsUserDetails);
                                    }
                                }
                            }

                            return dsUserDetails;
                        }
                        return dsUserDetails;
                    }
                    else
                    {
                        return dsUserDetails;
                    }

                }
                return dsUserDetails;
            }
            catch (Exception Ex)
            {

                BrokerUtility.ErrorLog(0, "SaveHomeInsuranceDetails_WebSite", Ex.Message.ToString(), "BrokerUtility.cs_SaveHomeInsuranceDetails()", "");
                return dsUserDetails;
            }
        }

        public static DataSet GetFinalData(DataSet dsUserDetails)
        {
            dsUserDetails.Tables[0].TableName = "UserDetails";
            dsUserDetails.Tables[1].TableName = "ExperienceDetails";
            dsUserDetails.Tables[2].TableName = "EducationDetails";
            dsUserDetails.Tables[3].TableName = "BrokerContactList";
            dsUserDetails.Tables[4].TableName = "UserRating";
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
            return dsUserDetails;
        }



        public static string GetIndustryMaster()
        {
            DataSet dsIndustryMaster = null;
            string jsonstring = "";
            try
            {

                dsIndustryMaster = BrokerWSUtility.GetIndustryMaster(0);

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

                        jsonstring = BrokerWSUtility.CreateJsonFromDataset(dsIndustryMaster, "GetIndustryMaster", "true", "null");
                        return jsonstring;
                        // HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsIndustryMaster, "DoGetIndustryMaster", "true", "null"));
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


                        jsonstring = BrokerWSUtility.CreateJsonFromDataset(dsMessages, "GetIndustryMaster", "true", "null");
                        return jsonstring;


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

                    jsonstring = BrokerWSUtility.CreateJsonFromDataset(dsMessages, "GetIndustryMaster", "true", "null");
                    return jsonstring;
                    // HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsMessages, "DoGetIndustryMaster", "true", "null"));
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "GetIndustryMaster_WebSite", Ex.Message.ToString(), "BrokerUtility.cs_GetIndustryMaster", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                jsonstring = BrokerWSUtility.CreateJsonFromDataset(dsDetails, "GetIndustryMaster", "false", "Error occured, please try again.");
                return jsonstring;
            }
        }


        public static string DoGetCompanyMaster()
        {
            DataSet dsCompanyMaster = null;
            string jsonstring = "";
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

                        jsonstring = BrokerWSUtility.CreateJsonFromDataset(dsCompanyMaster, "DoGetCompanyMaster", "true", "null");
                        return jsonstring;
                        // HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsCompanyMaster, "DoGetCompanyMaster", "true", "null"));
                    }
                    else
                    {
                        dsCompanyMaster.Tables[0].TableName = "CompanyList";
                        dsCompanyMaster.AcceptChanges();

                        jsonstring = BrokerWSUtility.CreateJsonFromDataset(dsCompanyMaster, "DoGetCompanyMaster", "true", "null");
                        return jsonstring;
                        //HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsCompanyMaster, "DoGetCompanyMaster", "true", "null"));
                    }
                }

                else
                {
                    DataTable dtCompanyMaster = new DataTable();
                    dsCompanyMaster.Tables.Add(dtCompanyMaster);
                    dsCompanyMaster.Tables[0].TableName = "CompanyList";
                    dsCompanyMaster.AcceptChanges();

                    jsonstring = BrokerWSUtility.CreateJsonFromDataset(dsCompanyMaster, "DoGetCompanyMaster", "true", "null");
                    return jsonstring;
                    // HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsCompanyMaster, "DoGetCompanyMaster", "true", "null"));

                    //HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsChatDetails, "DoGetChatMessages", "true", "null"));
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoGetCompanyMaster_WebSite", Ex.Message.ToString(), "BrokerUtility.cs_DoGetCompanyMaster()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                jsonstring = BrokerWSUtility.CreateJsonFromDataset(dsCompanyMaster, "DoGetCompanyMaster", "true", "null");
                return jsonstring;
                //HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoGetCompanyMaster", "false", "Error occured, please try again."));
            }
        }

        //public static int DoContactBroker(int BrokerId, string InsuranceType, string BrokerName, string DeviceID, string LocalDateTime)
        //{

        //    int UserId, User, count = 0;
        //    string Note = "";
        //    DataSet dsBrokerList = null;


        //    try
        //    {
        //        if (HttpContext.Current.Session["UserId"] != null)
        //        {

        //            UserId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
        //            Note = "";
        //            //LocalDateTime = DateTime.Now.ToLongDateString();
        //            //LocalDateTime = DateTime.Now.ToString(); // shashi

        //            //Insert Data in UserBroker table for storing which customer contacted which broker for
        //            //what type of Insurance
        //            User = BrokerWebDB.BrokerWebDB.ContactBroker(UserId, BrokerId, InsuranceType);


        //            if (User != 0)
        //            {
        //                //Send contact messages to both Customer and Broker.
        //                //Enter message details in BrokerMessages and CustomerMessages tables

        //                count = BrokerWebDB.BrokerWebDB.SendMessages(UserId, BrokerId, InsuranceType, Note, LocalDateTime);

        //                if (count != 0)
        //                {

        //                    string title = "", msgcnt = "", message = "", NewDevice = "";

        //                    title = BrokerName;
        //                    msgcnt = "1";

        //                    int AdnroidDevice = DeviceID.IndexOf("Android");
        //                    int IosDevice = DeviceID.IndexOf("iOS");

        //                    if (AdnroidDevice >= 0)
        //                    {

        //                        NewDevice = DeviceID.Replace("Android", "");
        //                        message = "Wants to get " + InsuranceType + " insurance";
        //                        DoPushNotification(NewDevice, message, title, msgcnt, Convert.ToString(UserId));
        //                    }
        //                    else if (IosDevice >= 0)
        //                    {
        //                        NewDevice = DeviceID.Replace("iOS", "");
        //                        message = title + " Wants to get " + InsuranceType + " insurance";
        //                        DoPushNotificationForiOS(NewDevice, message, title, msgcnt, Convert.ToString(UserId));
        //                    }


        //                    ////Get already contacted broker list.
        //                    //dsBrokerList = BrokerWSUtility.GetContactedBrokerList(UserId);

        //                    //if (dsBrokerList.Tables.Count > 0)
        //                    //{
        //                    //    dsBrokerList.Tables[0].TableName = "BrokerContactList";
        //                    //    dsBrokerList.AcceptChanges();

        //                    //    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsBrokerList, "DoContactBroker", "true", "null"));
        //                    //}
        //                    //else
        //                    //{
        //                    //    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

        //                    //    string Message = "";
        //                    //    DataSet dsDetails = new DataSet();
        //                    //    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

        //                    //    HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoContactBroker", "false", "Error Occured"));

        //                    //}




        //                    //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForSuccess("true", "Contacted"));
        //                    return count;
        //                }
        //                else
        //                {

        //                    return count;
        //                }
        //            }
        //        }
        //        return count;
        //    }
        //    catch (Exception Ex)
        //    {
        //        return count;
        //        BrokerUtility.ErrorLog(0, "DoContactBroker_WebSite", Ex.Message.ToString(), "BrokerWSDB.cs_DoContactBroker()", "");


        //    }

        //}


        public static DataSet DoContactBroker(int BrokerId, string InsuranceType, string BrokerName, string DeviceID, string LocalDateTime)
        {

            int UserId, User, count = 0;
            string Note = "";
            // DataSet dsBrokerList = null;
            DataSet dsMsgDetails = null;

            try
            {
                if (HttpContext.Current.Session["UserIdToContactBroker"] != null)
                {

                    UserId = Convert.ToInt32(HttpContext.Current.Session["UserIdToContactBroker"]);
                    Note = "";
                    //LocalDateTime = DateTime.Now.ToLongDateString();
                    //LocalDateTime = DateTime.Now.ToString(); // shashi

                    //Insert Data in UserBroker table for storing which customer contacted which broker for
                    //what type of Insurance
                    User = BrokerWebDB.BrokerWebDB.ContactBroker(UserId, BrokerId, InsuranceType);


                    if (User != 0)
                    {
                        //Send contact messages to both Customer and Broker.
                        //Enter message details in BrokerMessages and CustomerMessages tables

                        if (HttpContext.Current.Session["Company"] == "Brokkrr")
                        {
                            string LineType = HttpContext.Current.Session["LineType"].ToString();
                            if (LineType == "401k")
                            {
                                count = BrokerWebDB.BrokerWebDB.MeinekeSendMessages(UserId, BrokerId, "401k", Note, LocalDateTime);
                            }
                            else if (LineType == "Commercial" && InsuranceType == "Workers compensation")
                            {
                                count = BrokerWebDB.BrokerWebDB.MeinekeSendMessages(UserId, BrokerId, InsuranceType, Note, LocalDateTime);
                            }
                            else
                            {
                                count = BrokerWebDB.BrokerWebDB.SendMessages(UserId, BrokerId, InsuranceType, Note, LocalDateTime);
                            }
                            //dsMsgDetails = BrokerWebDB.BrokerWebDB.getCustomerMessageDetails(count);
                        }
                        else if (HttpContext.Current.Session["Company"] == "Meineke")
                        {

                            string LineType = HttpContext.Current.Session["LineType"].ToString();
                            if (LineType == "Personal")
                            {
                                count = BrokerWebDB.BrokerWebDB.MeinekeSendMessagesPersonal(UserId, BrokerId, InsuranceType, Note, LocalDateTime);
                            }
                            else if (LineType == "Commercial")
                            {
                                count = BrokerWebDB.BrokerWebDB.MeinekeSendMessages(UserId, BrokerId, InsuranceType, Note, LocalDateTime);
                            }
                            else if (LineType == "401k")
                            {
                                count = BrokerWebDB.BrokerWebDB.MeinekeSendMessages(UserId, BrokerId, "401k", Note, LocalDateTime);
                            }
                            //dsMsgDetails = BrokerWebDB.BrokerWebDB.getCustomerMessageDetails(count);
                        }
                        else if (HttpContext.Current.Session["Company"] == "APSP")
                        {

                            string LineType = HttpContext.Current.Session["LineType"].ToString();
                            if (LineType == "Personal")
                            {
                                count = BrokerWebDB.BrokerWebDB.MeinekeSendMessagesPersonal(UserId, BrokerId, InsuranceType, Note, LocalDateTime);
                            }
                            else if (LineType == "Commercial")
                            {
                                count = BrokerWebDB.BrokerWebDB.MeinekeSendMessages(UserId, BrokerId, InsuranceType, Note, LocalDateTime);
                            }
                            else if (LineType == "401k")
                            {
                                count = BrokerWebDB.BrokerWebDB.MeinekeSendMessages(UserId, BrokerId, "401k", Note, LocalDateTime);
                            }
                            //dsMsgDetails = BrokerWebDB.BrokerWebDB.getCustomerMessageDetails(count);
                        }

                        dsMsgDetails = BrokerWebDB.BrokerWebDB.getCustomerMessageDetails(count);
                        if (count != 0)
                        {

                            string title = "", msgcnt = "", message = "", NewDevice = "";

                            //title = BrokerName;
                            title = HttpContext.Current.Session["FirstName"].ToString() + " " + HttpContext.Current.Session["LastName"].ToString();
                            msgcnt = "1";

                            DataSet dsDeviceId = BrokerWSUtility.GetDeviceId(BrokerId);

                            if (dsDeviceId.Tables.Count > 0)
                            {
                                if (dsDeviceId.Tables[0].Rows.Count > 0)
                                {
                                    for (int k = 0; k < dsDeviceId.Tables[0].Rows.Count; k++)
                                    {
                                        string DeviceId = dsDeviceId.Tables[0].Rows[k]["DeviceId"].ToString();

                                        int AdnroidDevice = DeviceId.IndexOf("Android");
                                        int IosDevice = DeviceId.IndexOf("iOS");

                                        if (HttpContext.Current.Session["LineType"].ToString() == "401k")
                                        {
                                            InsuranceType = "401k";
                                        }

                                        if (AdnroidDevice >= 0)
                                        {

                                            NewDevice = DeviceId.Replace("Android", "");
                                            message = title + " wants to get " + InsuranceType + " insurance";
                                            DoPushNotification(NewDevice, message, title, msgcnt, Convert.ToString(BrokerId));
                                        }
                                        else if (IosDevice >= 0)
                                        {
                                            NewDevice = DeviceId.Replace("iOS", "");
                                            message = title + " wants to get " + InsuranceType + " insurance";
                                            DoPushNotificationForiOS(NewDevice, message, title, msgcnt, Convert.ToString(BrokerId));
                                        }
                                    }
                                }
                            }

                            //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForSuccess("true", "Contacted"));
                            return dsMsgDetails;
                        }
                        else
                        {

                            return dsMsgDetails;
                        }
                    }
                }
                return dsMsgDetails;
            }
            catch (Exception Ex)
            {
                return dsMsgDetails;
                BrokerUtility.ErrorLog(0, "DoContactBroker_WebSite", Ex.Message.ToString(), "BrokerWSDB.cs_DoContactBroker()", "");


            }

        }

        public static void DoPushNotification(string deviceId, string message, string title, string msgcnt, string UserId)
        {
            string Notification = "";
            DataSet dsData = new DataSet();
            DataSet dsUnwatchedVideoCount = new DataSet();
            int badge = 0, UnWatchedVideoCnt = 0;

            try
            {
                dsData = BrokerWebDB.BrokerWebDB.getUnreadMsgCountByDeviceid(UserId);

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

                //if (title == "")
                //{
                Notification = message;
                //}

                badge = badge + UnWatchedVideoCnt;

                WebRequest tRequest;
                tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", strGoogleAppID));

                tRequest.Headers.Add(string.Format("Sender: id={0}", strSENDER_ID));

                // string postData = "{ 'registration_id': [ '" + regId + "' ], 'data': {'message': '" + txtMsg.Text + "'}}";
                string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + Notification + "&data.title=Brokkrr&data.msgcnt=" + badge + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + deviceId + "";
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
                //return sResponseFromServer;


            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoPushNotification_WebSite", Ex.Message.ToString(), "BrokerUtility.cs_DoSetDeviceId", "");
            }
        }


        public static void DoPushNotificationForiOS(string deviceId, string message, string title, string msgcnt, string UserId)
        {
            //BrokerUtility.ErrorLog(0, "1-DoPushNotificationForiOS_WebSite", "" + deviceId + "," + message + "," + title + "," + msgcnt + "," + UserId, "BrokerUtility.cs_DoPushNotificationForiOS", "");

            string sound = "", deviceId1 = "";
            int badge = 0, UnWatchedVideoCnt = 0;
            DataSet dsData = null;
            DataSet dsUnwatchedVideoCount = null;

            sound = "Sound/pop.m4a";
            try
            {
                //deviceId1 = "iOS" + deviceId;

                dsData = BrokerWebDB.BrokerWebDB.getUnreadMsgCountByDeviceid(UserId);

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

                //BrokerUtility.ErrorLog(0, "2-DoPushNotificationForiOS_WebSite", "" + deviceId + "," + message + "," + title + "," + msgcnt + "," + UserId, "BrokerUtility.cs_DoPushNotificationForiOS", "");

                dsUnwatchedVideoCount = BrokerWSUtility.GetUnWatchedVideoCount(Convert.ToInt32(UserId));

                if (dsUnwatchedVideoCount.Tables.Count > 0)
                {
                    if (dsUnwatchedVideoCount.Tables[0].Rows.Count > 0)
                    {
                        UnWatchedVideoCnt = Convert.ToInt32(dsUnwatchedVideoCount.Tables[0].Rows[0][0].ToString());
                    }
                }

                badge = badge + UnWatchedVideoCnt;

                //BrokerUtility.ErrorLog(0, "3-DoPushNotificationForiOS_WebSite", "" + deviceId + "," + message + "," + title + "," + msgcnt + "," + UserId, "BrokerUtility.cs_DoPushNotificationForiOS", "");

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
                //BrokerUtility.ErrorLog(0, "4-DoPushNotificationForiOS_WebSite", "" + deviceId + "," + message + "," + title + "," + msgcnt + "," + UserId, "BrokerUtility.cs_DoPushNotificationForiOS", "");
                apnsBroker.OnNotificationSucceeded += (notification) =>
                {
                    //jsonString.stringValue = "Apple Notification Sent!";
                    Console.WriteLine("Apple Notification Sent!");
                };


                apnsBroker.Start();

                //apnsBroker.QueueNotification(new ApnsNotification
                //{
                //    DeviceToken = deviceId,
                //    Payload = JObject.Parse("{\"aps\" : { \"alert\":\'" + message + "',\"badge\" : \'" + badge.ToString() + "',\"content-available\":\"1\",\"sound\" : \'" + sound + "' }}")
                //    //Payload = JObject.Parse("{\"aps\" : { \"alert\":\'" + message + "',\"badge\" : \"20\",\"content-available\":\"1\",\"sound\" : \'" + sound + "' }}")
                //});

                //Payload = JObject.Parse("{\"aps\" : { \"alert\":\'" + message + "',\"badge\" : \'" + badge.ToString() + "',\"content-available\":\"1\",\"sound\" : \'" + sound + "' }}")
                string appleJsonFormat = "";

                //if (title != "")
                //{
                //    appleJsonFormat = "{\"aps\" : { \"alert\":\'" + title + " - " + message + "',\"badge\" : \'" + badge.ToString() + "',\"content-available\":\"1\",\"sound\" : \'" + sound + "' }}";
                //}
                //else
                //{
                appleJsonFormat = "{\"aps\" : { \"alert\":\'" + message + "',\"badge\" : \'" + badge.ToString() + "',\"content-available\":\"1\",\"sound\" : \'" + sound + "' }}";
                //}              

                //BrokerUtility.ErrorLog(0, "5-DoPushNotificationForiOS_WebSite", "" + deviceId + "," + message + "," + title + "," + msgcnt + "," + UserId, "BrokerUtility.cs_DoPushNotificationForiOS", "");
                apnsBroker.QueueNotification(new ApnsNotification
                {
                    DeviceToken = deviceId,
                    Payload = JObject.Parse(appleJsonFormat)
                });

                apnsBroker.Stop();

                //BrokerUtility.ErrorLog(0, "6-DoPushNotificationForiOS_WebSite", "" + deviceId + "," + message + "," + title + "," + msgcnt + "," + UserId, "BrokerUtility.cs_DoPushNotificationForiOS", "");
                /************************************************************************/


            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoPushNotificationForiOS_WebSite", Ex.Message.ToString(), "BrokerUtility.cs_DoPushNotificationForiOS", "");

            }
        }



        public static DataSet DoSaveBenefitInsuranceDetails(BenefitsInsurance model)
        {
            string ZipCode = "", City = "", IsInsured = "", InsuranceCompany = "", EmployeeStrength = "",
                 CoverageExpires = "", Language = "", Notes = "", Longitude = "", Latitude = "", IndustryId = "", SubIndustryId = "";

            int UserId;

            int User = 0;
            DataSet dsBrokerDetails = null;

            try
            {

                if (HttpContext.Current.Session["UserId"] != null)
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);

                    ZipCode = model.ZipCode;
                    City = model.City;

                    if (model.IsInsured == "Currently insured")
                        IsInsured = "true";

                    if (IsInsured == "true")
                        InsuranceCompany = model.InsuranceCompany;
                    else
                        InsuranceCompany = "";

                    EmployeeStrength = model.EmployeeStrength;
                    //Revenue = HttpContext.Current.Request.Form["Revenue"].ToString();
                    CoverageExpires = model.CoverageExpires;
                    Language = model.Language;

                    if (model.Notes == null) Notes = ""; else Notes = model.Notes;

                    Longitude = model.Longitude;
                    Latitude = model.Latitude;

                    //IndustryId = model.IndustryId;
                    //SubIndustryId = model.SubIndustrySICCode;

                    if (model.IndustryId == null || model.IndustryId == "")
                    {
                        IndustryId = "0";
                        SubIndustryId = "";
                    }
                    else
                    {
                        IndustryId = model.IndustryId;
                        if (HttpContext.Current.Session["SubIndustryarray"] != null)
                        {
                            SubIndustryId = (HttpContext.Current.Session["SubIndustryarray"]).ToString();
                        }
                        // SubIndustryId = business.SubIndustrySICCode;
                    }

                    User = BrokerWebDB.BrokerWebDB.SaveBenefitInsuranceDetails(UserId, ZipCode, City, IsInsured, InsuranceCompany, EmployeeStrength, CoverageExpires, Language, Notes, Longitude, Latitude, IndustryId, SubIndustryId, "", "");

                    if (User != 0)
                    {
                        dsBrokerDetails = BrokerWebDB.BrokerWebDB.SearchBrokersList(UserId, ZipCode, City, Language, "Benefit", Longitude, Latitude, EmployeeStrength, IndustryId, SubIndustryId);

                        if (dsBrokerDetails.Tables.Count > 0)
                        {
                            if (dsBrokerDetails.Tables[0].Rows.Count > 0)
                            {
                                dsBrokerDetails = GetFinalData(dsBrokerDetails);
                            }
                            else
                            {
                                dsBrokerDetails = BrokerWebDB.BrokerWebDB.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                                if (dsBrokerDetails.Tables.Count > 0)
                                {
                                    if (dsBrokerDetails.Tables[0].Rows.Count > 0)
                                    {
                                        dsBrokerDetails = GetFinalData(dsBrokerDetails);
                                    }
                                }
                            }

                            return dsBrokerDetails;
                        }
                        else
                        {
                            // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                            string Message = "";
                            DataSet dsDetails = new DataSet();
                            dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                            return dsBrokerDetails;
                            // HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));

                        }
                    }
                    else
                    {
                        // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        return dsBrokerDetails;
                        // HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));

                    }
                }
                return dsBrokerDetails;
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSaveBenefitInsuranceDetails_WebSite", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveBenefitInsuranceDetails()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                return dsBrokerDetails;
                //  HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }



        /***************santosh ******************  */


        /***************Rashmikant ******************  */


        public static DataSet DoSaveLifeInsuranceDetails(LifeandDisabilityInsurance life)
        {
            DataSet dsUserDetails = null;
            int User, UserId = 0;
            string ZipCode = "", City = "", IsInsured = "", InsuranceCompany = "",
                 Language = "", FaceValue = "", CoverageExpires = "", Notes = "", Longitude = "", Latitude = "";

            try
            {
                if (HttpContext.Current.Session["UserId"] != null)
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                    ZipCode = life.ZipCode;
                    City = life.City;
                    FaceValue = life.FaceValue;
                    //  IsInsured = life.IsInsured;
                    Language = life.Language;

                    if (life.Notes == null) Notes = ""; else Notes = life.Notes;
                    if (life.Longitude == null) Longitude = ""; else Longitude = life.Longitude;
                    if (life.Latitude == null) Latitude = ""; else Latitude = life.Latitude;

                    User = BrokerWSUtility.SaveLifeInsuranceDetails(UserId, ZipCode, City, IsInsured, InsuranceCompany, Language, FaceValue, CoverageExpires, Notes, Longitude, Latitude, "", "");

                    if (User != 0)
                    {
                        dsUserDetails = BrokerWebDB.BrokerWebDB.SearchBrokersList(UserId, ZipCode, City, Language, "Life", Longitude, Latitude, FaceValue, "0", "0");

                        if (dsUserDetails.Tables.Count > 0)
                        {
                            if (dsUserDetails.Tables[0].Rows.Count > 0)
                            {
                                dsUserDetails = GetFinalData(dsUserDetails);
                            }
                            else
                            {
                                dsUserDetails = BrokerWebDB.BrokerWebDB.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                                if (dsUserDetails.Tables.Count > 0)
                                {
                                    if (dsUserDetails.Tables[0].Rows.Count > 0)
                                    {
                                        dsUserDetails = GetFinalData(dsUserDetails);
                                    }
                                }
                            }

                            return dsUserDetails;

                            return dsUserDetails;
                        }
                        return dsUserDetails;
                    }
                    else
                    {
                        return dsUserDetails;

                    }
                }
                return dsUserDetails;
            }
            catch (Exception Ex)
            {



                BrokerUtility.ErrorLog(0, "DoSaveLifeInsuranceDetails_WebSite", Ex.Message.ToString(), "BrokerUtility.cs_DoSaveLifeInsuranceDetails()", "");
                return dsUserDetails;


                //BrokerUtility.ErrorLog(0, "DoSaveLifeInsuranceDetails", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveLifeInsuranceDetails()", "");

                //string Message = "";
                //DataSet dsDetails = new DataSet();
                //dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                //HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }




        /*Yogita*/

        public static DataSet SaveAutoInsuranceDetails(AutoInsurance auto)
        {
            DataSet dsUserDetails = null;
            int User, UserId = 0;
            string ZipCode = "", City = "", IsInsured = "", CompanyName = "", VehicleType = "", InsuranceCompany = "",
                CoverageExpires = "", Language = "", Notes = "", Longitude = "", Latitude = "";
            try
            {
                if (HttpContext.Current.Session["UserId"] != null)
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                    ZipCode = (HttpContext.Current.Session["zip"]).ToString();
                    if (City == null) City = ""; else City = (HttpContext.Current.Session["city"]).ToString();
                    if (VehicleType == null) { VehicleType = ""; } else { VehicleType = (HttpContext.Current.Session["VType"]).ToString(); }
                    if (auto.IsInsured == null) IsInsured = "false"; else IsInsured = "true";
                    if (auto.InsuranceCompany == null) CompanyName = ""; else CompanyName = auto.InsuranceCompany;
                    if (auto.CoverageExpires == null) CoverageExpires = ""; else CoverageExpires = auto.CoverageExpires;
                    if (auto.Language == null) Language = ""; else Language = auto.Language;
                    if (auto.Notes == null) Notes = ""; else Notes = auto.Notes;

                    if (Longitude == null) Longitude = ""; else Longitude = (HttpContext.Current.Session["lng"]).ToString();
                    if (Latitude == null) Latitude = ""; else Latitude = (HttpContext.Current.Session["lat"]).ToString();

                    // User = BrokerWSUtility.SaveAutoInsuranceDetails(UserId, ZipCode, City, VehicleType, IsInsured, InsuranceCompany, CoverageExpires, Language, Notes, Longitude, Latitude);
                    User = BrokerWebDB.BrokerWebDB.SaveAutoInsuranceDetails(UserId, ZipCode, City, VehicleType, IsInsured, CompanyName, CoverageExpires, Language, Notes, Longitude, Latitude, "", "");

                    if (User != 0)
                    {
                        dsUserDetails = BrokerWebDB.BrokerWebDB.SearchBrokersList(UserId, ZipCode, City, Language, "Auto", Longitude, Latitude, VehicleType, "0", "0");

                        if (dsUserDetails.Tables.Count > 0)
                        {
                            if (dsUserDetails.Tables[0].Rows.Count > 0)
                            {
                                dsUserDetails = GetFinalData(dsUserDetails);
                            }
                            else
                            {
                                dsUserDetails = BrokerWebDB.BrokerWebDB.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                                if (dsUserDetails.Tables.Count > 0)
                                {
                                    if (dsUserDetails.Tables[0].Rows.Count > 0)
                                    {
                                        dsUserDetails = GetFinalData(dsUserDetails);
                                    }
                                }
                            }

                            return dsUserDetails;
                        }
                        else
                        {
                            // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));
                            string Message = "";
                            DataSet dsDetails = new DataSet();
                            dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                            //HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveAutoInsuranceDetails", "false", "Error Occured"));
                            return dsUserDetails;
                        }
                        return dsUserDetails;
                    }

                    else
                    {
                        return dsUserDetails;
                    }
                }
                return dsUserDetails;
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "SaveAutoInsuranceDetails_WebSite", Ex.Message.ToString(), "BrokerUtility.cs_SaveAutoInsuranceDetails()", "");
                return dsUserDetails;
            }


        }

        public static DataTable ForgetPassword(string forgetEmail)
        {
            string EmailId = "";
            bool EmailFlag = false;
            int Count;
            DataSet dsValidEmail = null;
            DataTable dtDetails = new DataTable();
            dtDetails.Columns.Add("Message");

            try
            {

                EmailId = forgetEmail;

                dsValidEmail = BrokerWSUtility.CheckForActiveEmailId(EmailId);

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
                                dtDetails.Rows.Add("Success");
                                return dtDetails;

                            }
                            else
                            {
                                dtDetails.Rows.Add("Fail");
                                return dtDetails;
                            }

                        }
                        else
                        {
                            dtDetails.Rows.Add("Fail");
                            return dtDetails;

                        }
                    }
                    dtDetails.Rows.Add("Invalid");
                    return dtDetails;
                }
                else
                {
                    dtDetails.Rows.Add("Invalid");
                    return dtDetails;
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "ForgetPassword_website", Ex.Message.ToString(), "BrokerUtility.cs_ForgetPassword()", "");

                dtDetails.Rows.Add("Fail");
                return dtDetails;
            }
        }

        //30Mar17
        public static bool DoGetBrokerAvailabilityStatus(int UserID)
        {

            DataSet dsBrokerAvialability = null;
            bool IsAvailable = false;
            try
            {

                dsBrokerAvialability = BrokerWebDB.BrokerWebDB.GetBrokerAvailabilityStatus(UserID);

                if (dsBrokerAvialability.Tables.Count > 0)
                {
                    if (dsBrokerAvialability.Tables[0].Rows.Count > 0)
                    {

                        dsBrokerAvialability.Tables[0].TableName = "Response";
                        dsBrokerAvialability.AcceptChanges();
                        IsAvailable = Convert.ToBoolean(dsBrokerAvialability.Tables[0].Rows[0]["IsAvailable"].ToString());
                        return IsAvailable;
                        //HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsBrokerAvialability, "DoGetBrokerAvailabilityStatus", "true", "null"));
                    }
                    return IsAvailable;
                }
                return IsAvailable;

            }
            catch (Exception Ex)
            {
                return IsAvailable;
                BrokerUtility.ErrorLog(0, "DoGetBrokerAvailabilityStatus", Ex.Message.ToString(), "BrokerWSDB.cs_DoGetBrokerAvailabilityStatus()", "");

            }
        }

        public static int DoSetBrokerAvailabilityStatus(int UserId, bool Availability)
        {
            //int UserId;
            //   bool Availability = false;

            int Status = 0;
            try
            {

                Status = BrokerWebDB.BrokerWebDB.SetBrokerAvailabilityStatus(UserId, Availability);
                return Status;

            }
            catch (Exception Ex)
            {
                return Status;
                BrokerUtility.ErrorLog(0, "DoSetBrokerAvailabilityStatus_WebSite", Ex.Message.ToString(), "BrokerUitlity.cs_DoSetBrokerAvailabilityStatus()", "");

            }
        }



        //21Sep17 san
        public static DataSet SaveAutoInsuranceDetailsNew(AutoInsurance auto, string Base64String)
        {
            DataSet dsUserDetails = null;
            int User, UserId = 0;
            string ZipCode = "", City = "", IsInsured = "false", CompanyName = "", VehicleType = "", InsuranceCompany = "",
                CoverageExpires = "", Language = "", Notes = "", Longitude = "", Latitude = "", DocPath = "";
            try
            {
                UserId = auto.UserId;

                if (auto.ZipCode == null) ZipCode = ""; else ZipCode = auto.ZipCode;
                if (auto.City == null) City = ""; else City = auto.City;
                if (VehicleType == null) { VehicleType = ""; } else { VehicleType = auto.VehicleType; }

                if (auto.IsInsured != null)
                {
                    if (auto.IsInsured == "Currently insured")
                        IsInsured = "true";
                }

                if (IsInsured == "true")
                    if (auto.InsuranceCompany != null)
                        InsuranceCompany = auto.InsuranceCompany;
                    else
                        InsuranceCompany = "";


                if (auto.CoverageExpires == null) CoverageExpires = ""; else CoverageExpires = auto.CoverageExpires;
                if (auto.Language == null) Language = ""; else Language = auto.Language;
                if (auto.Notes == null) Notes = ""; else Notes = auto.Notes;

                if (Longitude == null) Longitude = ""; else Longitude = auto.Longitude;
                if (Latitude == null) Latitude = ""; else Latitude = auto.Latitude;
                if (auto.DocPath == null) DocPath = ""; else DocPath = auto.DocPath;

                User = BrokerWebDB.BrokerWebDB.SaveAutoInsuranceDetails(UserId, ZipCode, City, VehicleType, IsInsured, InsuranceCompany, CoverageExpires, Language, Notes, Longitude, Latitude, DocPath, Base64String);

                if (User != 0)
                {
                    dsUserDetails = BrokerWebDB.BrokerWebDB.SearchBrokersList(UserId, ZipCode, City, Language, "Auto", Longitude, Latitude, VehicleType, "0", "0");

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

                            dsUserDetails = GetFinalData(dsUserDetails);
                        }
                        else
                        {
                            dsUserDetails = BrokerWebDB.BrokerWebDB.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsUserDetails.Tables.Count > 0)
                            {
                                if (dsUserDetails.Tables[0].Rows.Count > 0)
                                {
                                    dsUserDetails = GetFinalData(dsUserDetails);
                                }
                            }
                        }

                        return dsUserDetails;
                    }
                    else
                    {
                        // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));
                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        //HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveAutoInsuranceDetails", "false", "Error Occured"));
                        return dsUserDetails;
                    }

                }

                else
                {
                    return dsUserDetails;
                }

                return dsUserDetails;
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "SaveAutoInsuranceDetails_WebSite", Ex.Message.ToString(), "BrokerUtility.cs_SaveAutoInsuranceDetails()", "");
                return dsUserDetails;
            }


        }


        public static DataSet SaveBusinessInsuranceDetailsNew(BusinessInsurance business, string Base64String)
        {
            DataSet dsBrokerDetails = null;
            string ZipCode = "", City = "", IsInsured = "false", InsuranceCompany = "", SICCode = "",
               Revenue = "", CoverageExpires = "", Language = "", Notes = "", Longitude = "", Latitude = "", IndustryId = "", SubIndustryId = "",
               DocPath = "";

            int User, UserId = 0;
            try
            {

                UserId = business.Id;
                if (business.ZipCode == null) ZipCode = ""; else ZipCode = business.ZipCode;
                if (business.City == null) City = ""; else City = business.City;
                if (business.Revenue == null) { Revenue = ""; } else { Revenue = business.Revenue; }
                if (business.IsInsured != null || business.IsInsured != "")
                {
                    if (business.IsInsured == "Currently insured")
                    {
                        IsInsured = "true";
                    }
                    else
                        IsInsured = "false";

                }


                if (business.InsuranceCompany == null) InsuranceCompany = ""; else InsuranceCompany = business.InsuranceCompany;
                if (business.CoverageExpires == null) CoverageExpires = ""; else CoverageExpires = business.CoverageExpires;
                if (business.IndustryId == null || business.IndustryId == "")
                {
                    IndustryId = "0";
                    SubIndustryId = "";
                }
                else
                {
                    IndustryId = business.IndustryId;
                    if (business.SubIndustryId != null)
                        SubIndustryId = business.SubIndustryId;

                }

                if (business.Language == null) Language = ""; else Language = business.Language;
                if (business.Notes == null) Notes = ""; else Notes = business.Notes;
                if (business.Longitude == null) Longitude = ""; else Longitude = business.Longitude;
                if (business.Latitude == null) Latitude = ""; else Latitude = business.Latitude;

                if (business.DocPath == null) DocPath = ""; else DocPath = business.DocPath;

                User = BrokerWSUtility.SaveBusinessInsuranceDetails(UserId, ZipCode, City, IsInsured, InsuranceCompany, SICCode, Revenue, CoverageExpires, Language, Notes, Longitude, Latitude, IndustryId, SubIndustryId, DocPath, Base64String);
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

                            dsBrokerDetails = GetFinalData(dsBrokerDetails);
                        }
                        else
                        {
                            dsBrokerDetails = BrokerWebDB.BrokerWebDB.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsBrokerDetails.Tables.Count > 0)
                            {
                                if (dsBrokerDetails.Tables[0].Rows.Count > 0)
                                {
                                    dsBrokerDetails = GetFinalData(dsBrokerDetails);
                                }
                            }
                        }

                        return dsBrokerDetails;
                    }
                    else
                    {
                        //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);
                        return dsBrokerDetails;
                        //  HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));

                    }

                }
                return dsBrokerDetails;

                return dsBrokerDetails;
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "SaveBusinessInsuranceDetails_WebSite", Ex.Message.ToString(), "BrokerUtility.cs_SaveBusinessInsuranceDetails()", "");
                return dsBrokerDetails;
            }

        }

        public static DataSet GeneralLiabilityDetails(BusinessInsurance business, string Base64String)
        {
            DataSet dsBrokerDetails = null;
            string ZipCode = "", City = "", IsInsured = "false", InsuranceCompany = "", SICCode = "",
               Revenue = "", CoverageExpires = "", Language = "", Notes = "", Longitude = "", Latitude = "", IndustryId = "", SubIndustryId = "",
               DocPath = "";

            int User, UserId = 0;
            try
            {

                UserId = business.Id;
                if (business.ZipCode == null) ZipCode = ""; else ZipCode = business.ZipCode;
                if (business.City == null) City = ""; else City = business.City;
                if (business.Revenue == null) { Revenue = ""; } else { Revenue = business.Revenue; }
                if (business.IsInsured != null || business.IsInsured != "")
                {
                    if (business.IsInsured == "Currently insured")
                    {
                        IsInsured = "true";
                    }
                    else
                        IsInsured = "false";

                }


                if (business.InsuranceCompany == null) InsuranceCompany = ""; else InsuranceCompany = business.InsuranceCompany;
                if (business.CoverageExpires == null) CoverageExpires = ""; else CoverageExpires = business.CoverageExpires;
                if (business.IndustryId == null || business.IndustryId == "")
                {
                    IndustryId = "0";
                    SubIndustryId = "";
                }
                else
                {
                    IndustryId = business.IndustryId;
                    if (business.SubIndustryId != null)
                        SubIndustryId = business.SubIndustryId;

                }

                if (business.Language == null) Language = ""; else Language = business.Language;
                if (business.Notes == null) Notes = ""; else Notes = business.Notes;
                if (business.Longitude == null) Longitude = ""; else Longitude = business.Longitude;
                if (business.Latitude == null) Latitude = ""; else Latitude = business.Latitude;

                if (business.DocPath == null) DocPath = ""; else DocPath = business.DocPath;

                User = BrokerWSUtility.SaveBusinessInsuranceDetails(UserId, ZipCode, City, IsInsured, InsuranceCompany, SICCode, Revenue, CoverageExpires, Language, Notes, Longitude, Latitude, IndustryId, SubIndustryId, DocPath, Base64String);
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

                            dsBrokerDetails = GetFinalData(dsBrokerDetails);
                        }
                        else
                        {
                            dsBrokerDetails = BrokerWebDB.BrokerWebDB.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsBrokerDetails.Tables.Count > 0)
                            {
                                if (dsBrokerDetails.Tables[0].Rows.Count > 0)
                                {
                                    dsBrokerDetails = GetFinalData(dsBrokerDetails);
                                }
                            }
                        }

                        return dsBrokerDetails;
                    }
                    else
                    {
                        //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);
                        return dsBrokerDetails;
                        //  HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));

                    }

                }
                return dsBrokerDetails;

                //return dsBrokerDetails;
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "SaveBusinessInsuranceDetails_WebSite", Ex.Message.ToString(), "BrokerUtility.cs_SaveBusinessInsuranceDetails()", "");
                return dsBrokerDetails;
            }

        }


        public static DataSet DoSaveBenefitInsuranceDetailsNew(BenefitsInsurance model, string Base64String)
        {
            string ZipCode = "", City = "", IsInsured = "false", InsuranceCompany = "", EmployeeStrength = "",
                 CoverageExpires = "", Language = "", Notes = "", Longitude = "", Latitude = "", IndustryId = "", SubIndustryId = "",
                 DocPath = "";

            int UserId;

            int User = 0;
            DataSet dsBrokerDetails = null;

            try
            {

                UserId = model.Id;
                if (model.ZipCode == null) ZipCode = ""; else ZipCode = model.ZipCode;

                if (model.City == null) City = ""; else City = model.City;

                if (model.IsInsured != null)
                {
                    if (model.IsInsured == "Currently insured")
                        IsInsured = "true";
                }

                if (IsInsured == "true")
                    if (model.InsuranceCompany != null)
                        InsuranceCompany = model.InsuranceCompany;
                    else
                        InsuranceCompany = "";

                if (model.EmployeeStrength == null) EmployeeStrength = ""; else EmployeeStrength = model.EmployeeStrength;

                //Revenue = HttpContext.Current.Request.Form["Revenue"].ToString();
                if (model.CoverageExpires != null)
                {
                    CoverageExpires = model.CoverageExpires;
                }
                if (model.Language != null)
                {
                    Language = model.Language;
                }
                if (model.Notes == null) Notes = ""; else Notes = model.Notes;

                if (model.Longitude == null) Longitude = ""; else Longitude = model.Longitude;

                if (model.Latitude == null) Latitude = ""; else Latitude = model.Latitude;


                //IndustryId = model.IndustryId;
                //SubIndustryId = model.SubIndustrySICCode;

                if (model.IndustryId == null || model.IndustryId == "")
                {
                    IndustryId = "0";
                    SubIndustryId = "";
                }
                else
                {
                    IndustryId = model.IndustryId;
                    if (model.SubIndustryId != null)
                        SubIndustryId = model.SubIndustryId;
                }

                if (model.DocPath == null) DocPath = ""; else DocPath = model.DocPath;

                User = BrokerWebDB.BrokerWebDB.SaveBenefitInsuranceDetails(UserId, ZipCode, City, IsInsured, InsuranceCompany, EmployeeStrength, CoverageExpires, Language, Notes, Longitude, Latitude, IndustryId, SubIndustryId, DocPath, Base64String);

                if (User != 0)
                {
                    dsBrokerDetails = BrokerWebDB.BrokerWebDB.SearchBrokersList(UserId, ZipCode, City, Language, "Benefit", Longitude, Latitude, EmployeeStrength, IndustryId, SubIndustryId);

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

                            dsBrokerDetails = GetFinalData(dsBrokerDetails);
                        }
                        else
                        {
                            dsBrokerDetails = BrokerWebDB.BrokerWebDB.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsBrokerDetails.Tables.Count > 0)
                            {
                                if (dsBrokerDetails.Tables[0].Rows.Count > 0)
                                {
                                    dsBrokerDetails = GetFinalData(dsBrokerDetails);
                                }
                            }
                        }

                        return dsBrokerDetails;
                    }
                    else
                    {
                        // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        return dsBrokerDetails;
                        // HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));

                    }
                }
                else
                {
                    // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    return dsBrokerDetails;
                    // HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));

                }

                return dsBrokerDetails;
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSaveBenefitInsuranceDetails_WebSite", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveBenefitInsuranceDetails()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                return dsBrokerDetails;
                //  HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static DataSet SaveHomeInsuranceDetailsNew(HomeInsurance home, string Base64String)
        {
            DataSet dsUserDetails = null;
            //DataSet dsUserDetailsMain = null;

            int User, UserId = 0;
            string ZipCode = "", City = "", EstimatedValue = "", IsInsured = "false", CompanyName = "",
                CoverageExpires = "", Language = "", Notes = "", Longitude = "", Latitude = "", DocPath = "";


            try
            {

                UserId = home.Id;
                if (home.ZipCode == null) ZipCode = ""; else ZipCode = home.ZipCode;

                if (home.City == null) City = ""; else City = home.City;

                if (home.EstimatedValue == null) EstimatedValue = ""; else EstimatedValue = home.EstimatedValue;

                if (home.IsInsured != null)
                {
                    if (home.IsInsured == "Currently insured")
                        IsInsured = "true";
                }
                if (IsInsured == "true")
                    if (home.InsuranceCompany != null)
                        CompanyName = home.InsuranceCompany;
                    else
                        CompanyName = "";

                if (home.CoverageExpires == null) CoverageExpires = ""; else CoverageExpires = home.CoverageExpires;
                if (home.Language == null) Language = ""; else Language = home.Language;


                if (home.Notes == null) Notes = ""; else Notes = home.Notes;

                if (home.Longitude == null) Longitude = ""; else Longitude = home.Longitude;
                if (home.Latitude == null) Latitude = ""; else Latitude = home.Latitude;

                if (home.DocPath == null) DocPath = ""; else DocPath = home.DocPath;

                User = BrokerWebDB.BrokerWebDB.SaveHomeInsuranceDetails(UserId, ZipCode, City, EstimatedValue, IsInsured, CompanyName, CoverageExpires, Language, Notes, Longitude, Latitude, DocPath, Base64String);

                if (User != 0)
                {
                    dsUserDetails = BrokerWebDB.BrokerWebDB.SearchBrokersList(UserId, ZipCode, City, Language, "Home", Longitude, Latitude, EstimatedValue, "0", "0");

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

                            dsUserDetails = GetFinalData(dsUserDetails);
                        }
                        else
                        {
                            dsUserDetails = BrokerWebDB.BrokerWebDB.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsUserDetails.Tables.Count > 0)
                            {
                                if (dsUserDetails.Tables[0].Rows.Count > 0)
                                {
                                    dsUserDetails = GetFinalData(dsUserDetails);
                                }
                            }
                        }

                        return dsUserDetails;
                    }
                    return dsUserDetails;
                }
                else
                {
                    return dsUserDetails;
                }


                return dsUserDetails;
            }
            catch (Exception Ex)
            {

                BrokerUtility.ErrorLog(0, "SaveHomeInsuranceDetails_WebSite", Ex.Message.ToString(), "BrokerUtility.cs_SaveHomeInsuranceDetails()", "");
                return dsUserDetails;
            }
        }


        public static DataSet DoSaveLifeInsuranceDetailsNew(LifeandDisabilityInsurance life, string Base64String)
        {
            DataSet dsUserDetails = null;
            int User, UserId = 0;
            string ZipCode = "", City = "", IsInsured = "false", InsuranceCompany = "",
                 Language = "", FaceValue = "", CoverageExpires = "", Notes = "", Longitude = "", Latitude = "", DocPath = "";

            try
            {
                UserId = life.Id;

                if (life.ZipCode == null) ZipCode = ""; else ZipCode = life.ZipCode;
                if (life.City == null) City = ""; else City = life.City;
                if (life.FaceValue == null) FaceValue = ""; else FaceValue = life.FaceValue;
                if (life.Language == null) Language = ""; else Language = life.Language;
                if (life.Notes == null) Notes = ""; else Notes = life.Notes;
                if (life.Longitude == null) Longitude = ""; else Longitude = life.Longitude;
                if (life.Latitude == null) Latitude = ""; else Latitude = life.Latitude;
                if (life.CoverageExpires == null) CoverageExpires = ""; else CoverageExpires = life.CoverageExpires;//29Sep17

                if (life.DocPath == null) DocPath = ""; else DocPath = life.DocPath;

                User = BrokerWSUtility.SaveLifeInsuranceDetails(UserId, ZipCode, City, IsInsured, InsuranceCompany, Language, FaceValue, CoverageExpires, Notes, Longitude, Latitude, DocPath, Base64String);

                if (User != 0)
                {
                    dsUserDetails = BrokerWebDB.BrokerWebDB.SearchBrokersList(UserId, ZipCode, City, Language, "Life", Longitude, Latitude, FaceValue, "0", "0");

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

                            dsUserDetails = GetFinalData(dsUserDetails);
                        }
                        else
                        {
                            dsUserDetails = BrokerWebDB.BrokerWebDB.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsUserDetails.Tables.Count > 0)
                            {
                                if (dsUserDetails.Tables[0].Rows.Count > 0)
                                {
                                    dsUserDetails = GetFinalData(dsUserDetails);
                                }
                            }
                        }

                        return dsUserDetails;

                        return dsUserDetails;
                    }
                    return dsUserDetails;
                }
                else
                {
                    return dsUserDetails;

                }

                return dsUserDetails;
            }
            catch (Exception Ex)
            {



                BrokerUtility.ErrorLog(0, "DoSaveLifeInsuranceDetails_WebSite", Ex.Message.ToString(), "BrokerUtility.cs_DoSaveLifeInsuranceDetails()", "");
                return dsUserDetails;


                //BrokerUtility.ErrorLog(0, "DoSaveLifeInsuranceDetails", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveLifeInsuranceDetails()", "");

                //string Message = "";
                //DataSet dsDetails = new DataSet();
                //dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                //HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        //For Meineke Insurance Company

        #region Meineke Insurance

        public static DataSet DoSaveCommercialAutoInsuranceDetails(CommercialAutoInsurance model, string Base64String)
        {
            string ZipCode = "", City = "", Longitude = "", Latitude = "", NoOfUnits = "", DeductibleIfAny = "",
                CurrentLimit = "", NoOfStalls = "", NoOfLocations = "", GrossRevenue = "", DocPath = "";

            int UserId;

            int User = 0;
            DataSet dsBrokerDetails = null;

            try
            {

                UserId = model.UserId;
                if (model.ZipCode == null) ZipCode = ""; else ZipCode = model.ZipCode;

                if (model.City == null) City = ""; else City = model.City;

                if (model.Longitude == null) Longitude = ""; else Longitude = model.Longitude;

                if (model.Latitude == null) Latitude = ""; else Latitude = model.Latitude;

                //if (model.NoOfUnits == null) NoOfUnits = ""; else NoOfUnits = model.NoOfUnits;

                //if (model.DeductibleIfAny == null) DeductibleIfAny = ""; else DeductibleIfAny = model.DeductibleIfAny;

                if (model.NoOfStalls == null) NoOfStalls = ""; else NoOfStalls = model.NoOfStalls;

                if (model.NoOfLocations == null) NoOfLocations = ""; else NoOfLocations = model.NoOfLocations;

                if (model.grossrevenueforgarage == null) GrossRevenue = ""; else GrossRevenue = model.grossrevenueforgarage;

                if (model.CurrentLimit == null) CurrentLimit = ""; else CurrentLimit = model.CurrentLimit;

                if (model.DocPath == null) DocPath = ""; else DocPath = model.DocPath;

                User = BrokerWebDB.BrokerWebDB.SaveCommercialAutoInsuranceDetails(UserId, ZipCode, City, Longitude, Latitude, NoOfStalls, NoOfLocations, GrossRevenue, CurrentLimit, DocPath, Base64String);

                if (User != 0)
                {
                    dsBrokerDetails = BrokerWebDB.BrokerWebDB.SearchBrokersListForMeineke(UserId, ZipCode, City, "CommercialAuto", Longitude, Latitude, "0", "0");

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

                            dsBrokerDetails = GetFinalData(dsBrokerDetails);
                        }
                        else
                        {
                            dsBrokerDetails = BrokerWebDB.BrokerWebDB.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsBrokerDetails.Tables.Count > 0)
                            {
                                if (dsBrokerDetails.Tables[0].Rows.Count > 0)
                                {
                                    dsBrokerDetails = GetFinalData(dsBrokerDetails);
                                }
                            }
                        }

                        return dsBrokerDetails;
                    }
                    else
                    {
                        // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        return dsBrokerDetails;
                        // HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));

                    }
                }
                else
                {
                    // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    return dsBrokerDetails;
                    // HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));

                }

                return dsBrokerDetails;
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSaveCommercialAutoInsuranceDetails_WebSite", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveBenefitInsuranceDetails()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                return dsBrokerDetails;
                //  HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static DataSet DoSaveWorkersCompensationDetails(WorkersCompensation model, string Base64String)
        {
            string ZipCode = "", City = "", Longitude = "", Latitude = "", NoOfEmp = "", GrossPayroll = "", DocPath = "";

            int UserId;

            int User = 0;
            DataSet dsBrokerDetails = null;          

            try
            {

                UserId = model.UserId;
                if (model.ZipCode == null) ZipCode = ""; else ZipCode = model.ZipCode;

                if (model.City == null) City = ""; else City = model.City;

                if (model.Longitude == null) Longitude = ""; else Longitude = model.Longitude;

                if (model.Latitude == null) Latitude = ""; else Latitude = model.Latitude;

                if (model.NoOfEmployees == null) NoOfEmp = ""; else NoOfEmp = model.NoOfEmployees;

                if (model.GrossPayroll == null) GrossPayroll = ""; else GrossPayroll = model.GrossPayroll;

                if (model.DocPath == null) DocPath = ""; else DocPath = model.DocPath;

                User = BrokerWebDB.BrokerWebDB.SaveWorkCompDetails(UserId, ZipCode, City, Longitude, Latitude, NoOfEmp, GrossPayroll, DocPath, Base64String);

                if (User != 0)
                {
                    dsBrokerDetails = BrokerWebDB.BrokerWebDB.SearchBrokersListForMeineke(UserId, ZipCode, City, "WorkComp", Longitude, Latitude, "0", "0");

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

                            dsBrokerDetails = GetFinalData(dsBrokerDetails);
                        }
                        else
                        {
                            dsBrokerDetails = BrokerWebDB.BrokerWebDB.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsBrokerDetails.Tables.Count > 0)
                            {
                                if (dsBrokerDetails.Tables[0].Rows.Count > 0)
                                {
                                    dsBrokerDetails = GetFinalData(dsBrokerDetails);
                                }
                            }
                        }

                        return dsBrokerDetails;
                    }
                    else
                    {
                        // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        return dsBrokerDetails;
                        // HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));

                    }
                }
                else
                {
                    // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    return dsBrokerDetails;
                    // HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));

                }

                return dsBrokerDetails;
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSaveWorkersCompensationDetails_WebSite", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveBenefitInsuranceDetails()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                return dsBrokerDetails;
                //  HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
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

        public static DataSet DoSaveLiabilityInsuranceDetails(LiabilityInsurance model, string Base64String)
        {
            string ZipCode = "", City = "", Longitude = "", Latitude = "", DeductibleIfAny = "", GrossSale = "",
                IndustryId = "", SubIndustryId = "", DocPath = "";

            int UserId;

            int User = 0;
            DataSet dsBrokerDetails = null;

            try
            {

                UserId = model.UserId;
                if (model.ZipCode == null) ZipCode = ""; else ZipCode = model.ZipCode;

                if (model.City == null) City = ""; else City = model.City;

                if (model.Longitude == null) Longitude = ""; else Longitude = model.Longitude;

                if (model.Latitude == null) Latitude = ""; else Latitude = model.Latitude;

                if (model.DeductibleIfAny == null) DeductibleIfAny = ""; else DeductibleIfAny = model.DeductibleIfAny;

                if (model.GrossSale == null) GrossSale = ""; else GrossSale = model.GrossSale;

                if (model.IndustryId == null || model.IndustryId == "")
                {
                    IndustryId = "0";
                    SubIndustryId = "";
                }
                else
                {
                    IndustryId = model.IndustryId;
                    if (model.SubIndustryId != null)
                        SubIndustryId = model.SubIndustryId;
                }

                if (model.DocPath == null) DocPath = ""; else DocPath = model.DocPath;

                User = BrokerWebDB.BrokerWebDB.SaveLiabilityInsuranceDetails(UserId, ZipCode, City, Longitude, Latitude, DeductibleIfAny, GrossSale, IndustryId, SubIndustryId, DocPath, Base64String);

                if (User != 0)
                {
                    dsBrokerDetails = BrokerWebDB.BrokerWebDB.SearchBrokersListForMeineke(UserId, ZipCode, City, "Liability", Longitude, Latitude, IndustryId, SubIndustryId);

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

                            dsBrokerDetails = GetFinalData(dsBrokerDetails);
                        }
                        else
                        {
                            dsBrokerDetails = BrokerWebDB.BrokerWebDB.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsBrokerDetails.Tables.Count > 0)
                            {
                                if (dsBrokerDetails.Tables[0].Rows.Count > 0)
                                {
                                    dsBrokerDetails = GetFinalData(dsBrokerDetails);
                                }
                            }
                        }

                        return dsBrokerDetails;
                    }
                    else
                    {
                        // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        return dsBrokerDetails;
                        // HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));

                    }
                }
                else
                {
                    // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    return dsBrokerDetails;
                    // HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));

                }

                return dsBrokerDetails;
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSaveWorkersCompensationDetails_WebSite", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveBenefitInsuranceDetails()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                return dsBrokerDetails;
                //  HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static DataSet DoSaveMeinekeBenefitInsuranceDetails(BenefitsInsurance model, string Base64String)
        {
            string ZipCode = "", City = "", IsInsured = "false", InsuranceCompany = "", EmployeeStrength = "",
                 CoverageExpires = "", Language = "", Notes = "", Longitude = "", Latitude = "", IndustryId = "", SubIndustryId = "",
                 DocPath = "";

            int UserId;

            int User = 0;
            DataSet dsBrokerDetails = null;

            try
            {

                UserId = model.Id;
                if (model.ZipCode == null) ZipCode = ""; else ZipCode = model.ZipCode;

                if (model.City == null) City = ""; else City = model.City;

                if (model.IsInsured != null)
                {
                    if (model.IsInsured == "Currently insured")
                        IsInsured = "true";
                }

                if (IsInsured == "true")
                    if (model.InsuranceCompany != null)
                        InsuranceCompany = model.InsuranceCompany;
                    else
                        InsuranceCompany = "";

                if (model.EmployeeStrength == null) EmployeeStrength = ""; else EmployeeStrength = model.EmployeeStrength;

                //Revenue = HttpContext.Current.Request.Form["Revenue"].ToString();
                if (model.CoverageExpires != null)
                {
                    CoverageExpires = model.CoverageExpires;
                }
                if (model.Language != null)
                {
                    Language = model.Language;
                }
                if (model.Notes == null) Notes = ""; else Notes = model.Notes;

                if (model.Longitude == null) Longitude = ""; else Longitude = model.Longitude;

                if (model.Latitude == null) Latitude = ""; else Latitude = model.Latitude;


                //IndustryId = model.IndustryId;
                //SubIndustryId = model.SubIndustrySICCode;

                if (model.IndustryId == null || model.IndustryId == "")
                {
                    IndustryId = "0";
                    SubIndustryId = "";
                }
                else
                {
                    IndustryId = model.IndustryId;
                    if (model.SubIndustryId != null)
                        SubIndustryId = model.SubIndustryId;
                }

                if (model.DocPath == null) DocPath = ""; else DocPath = model.DocPath;

                User = BrokerWebDB.BrokerWebDB.SaveMeinekeBenefitInsuranceDetails(UserId, ZipCode, City, IsInsured, InsuranceCompany, EmployeeStrength, CoverageExpires, Language, Notes, Longitude, Latitude, IndustryId, SubIndustryId, DocPath, Base64String);

                if (User != 0)
                {
                    dsBrokerDetails = BrokerWebDB.BrokerWebDB.SearchBrokersListForMeineke(UserId, ZipCode, City, "MeinekeBenefit", Longitude, Latitude, IndustryId, SubIndustryId);

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

                            dsBrokerDetails = GetFinalData(dsBrokerDetails);
                        }
                        else
                        {
                            dsBrokerDetails = BrokerWebDB.BrokerWebDB.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsBrokerDetails.Tables.Count > 0)
                            {
                                if (dsBrokerDetails.Tables[0].Rows.Count > 0)
                                {
                                    dsBrokerDetails = GetFinalData(dsBrokerDetails);
                                }
                            }
                        }

                        return dsBrokerDetails;
                    }
                    else
                    {
                        // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        return dsBrokerDetails;
                        // HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));

                    }
                }
                else
                {
                    // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    return dsBrokerDetails;
                    // HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));

                }

                return dsBrokerDetails;
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSaveBenefitInsuranceDetails_WebSite", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveBenefitInsuranceDetails()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                return dsBrokerDetails;
                //  HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static DataSet DoSave401kInsuranceDetails(_401kInsurance model, string Base64String)
        {
            string ZipCode = "", City = "", Longitude = "", Latitude = "", CurrentPlan = "", NumberOfEmp = "",
                PlanSize = "", DocPath = "";

            int UserId;

            int User = 0;
            DataSet dsBrokerDetails = null;

            try
            {

                UserId = model.UserId;
                if (model.ZipCode == null) ZipCode = ""; else ZipCode = model.ZipCode;

                if (model.City == null) City = ""; else City = model.City;

                if (model.Longitude == null) Longitude = ""; else Longitude = model.Longitude;

                if (model.Latitude == null) Latitude = ""; else Latitude = model.Latitude;

                if (model.CurrentPlan == null) CurrentPlan = ""; else CurrentPlan = model.CurrentPlan;

                if (model.NumberOfEmp == null) NumberOfEmp = ""; else NumberOfEmp = model.NumberOfEmp;

                if (model.PlanSize == null) PlanSize = ""; else PlanSize = model.PlanSize;

                if (model.DocPath == null) DocPath = ""; else DocPath = model.DocPath;

                User = BrokerWebDB.BrokerWebDB.Save401kInsuranceDetails(UserId, ZipCode, City, Longitude, Latitude, CurrentPlan, NumberOfEmp, PlanSize, DocPath, Base64String);

                if (User != 0)
                {
                    dsBrokerDetails = BrokerWebDB.BrokerWebDB.SearchBrokersListForMeineke(UserId, ZipCode, City, "_401k", Longitude, Latitude, "0", "0");

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

                            dsBrokerDetails = GetFinalData(dsBrokerDetails);
                        }
                        else
                        {
                            dsBrokerDetails = BrokerWebDB.BrokerWebDB.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsBrokerDetails.Tables.Count > 0)
                            {
                                if (dsBrokerDetails.Tables[0].Rows.Count > 0)
                                {
                                    dsBrokerDetails = GetFinalData(dsBrokerDetails);
                                }
                            }
                        }

                        return dsBrokerDetails;
                    }
                    else
                    {
                        // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                        return dsBrokerDetails;
                        // HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));

                    }
                }
                else
                {
                    // HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                    string Message = "";
                    DataSet dsDetails = new DataSet();
                    dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                    return dsBrokerDetails;
                    // HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));

                }

                return dsBrokerDetails;
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoSaveWorkersCompensationDetails_WebSite", Ex.Message.ToString(), "BrokerWSDB.cs_DoSaveBenefitInsuranceDetails()", "");

                string Message = "";
                DataSet dsDetails = new DataSet();
                dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);

                return dsBrokerDetails;
                //  HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoLogin", "false", "Error occured, please try again."));
            }
        }

        public static DataSet SaveLiabilityInsuranceDetailsNew(BusinessInsurance business, string Base64String)
        {
            DataSet dsBrokerDetails = null;
            string ZipCode = "", City = "", IsInsured = "false", InsuranceCompany = "", SICCode = "",
               Revenue = "", CoverageExpires = "", Language = "", Notes = "", Longitude = "", Latitude = "", IndustryId = "", SubIndustryId = "",
               DocPath = "";

            int User, UserId = 0;
            try
            {

                UserId = business.Id;
                if (business.ZipCode == null) ZipCode = ""; else ZipCode = business.ZipCode;
                if (business.City == null) City = ""; else City = business.City;
                if (business.Revenue == null) { Revenue = ""; } else { Revenue = business.Revenue; }
                if (business.IsInsured != null || business.IsInsured != "")
                {
                    if (business.IsInsured == "Currently insured")
                    {
                        IsInsured = "true";
                    }
                    else
                        IsInsured = "false";

                }


                if (business.InsuranceCompany == null) InsuranceCompany = ""; else InsuranceCompany = business.InsuranceCompany;
                if (business.CoverageExpires == null) CoverageExpires = ""; else CoverageExpires = business.CoverageExpires;
                if (business.IndustryId == null || business.IndustryId == "")
                {
                    IndustryId = "0";
                    SubIndustryId = "";
                }
                else
                {
                    IndustryId = business.IndustryId;
                    if (business.SubIndustryId != null)
                        SubIndustryId = business.SubIndustryId;

                }

                if (business.Language == null) Language = ""; else Language = business.Language;
                if (business.Notes == null) Notes = ""; else Notes = business.Notes;
                if (business.Longitude == null) Longitude = ""; else Longitude = business.Longitude;
                if (business.Latitude == null) Latitude = ""; else Latitude = business.Latitude;
                if (business.DocPath == null) DocPath = ""; else DocPath = business.DocPath;

                User = BrokerWSUtility.SaveLiabilityInsuranceDetailsNew(UserId, ZipCode, City, IsInsured, InsuranceCompany, SICCode, Revenue, CoverageExpires, Language, Notes, Longitude, Latitude, IndustryId, SubIndustryId, DocPath, Base64String);
                if (User != 0)
                {
                    dsBrokerDetails = BrokerWebDB.BrokerWebDB.SearchBrokersListForMeineke(UserId, ZipCode, City, "APSPLiability", Longitude, Latitude, IndustryId, SubIndustryId);

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

                            dsBrokerDetails = GetFinalData(dsBrokerDetails);
                        }
                        else
                        {
                            dsBrokerDetails = BrokerWebDB.BrokerWebDB.GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

                            if (dsBrokerDetails.Tables.Count > 0)
                            {
                                if (dsBrokerDetails.Tables[0].Rows.Count > 0)
                                {
                                    dsBrokerDetails = GetFinalData(dsBrokerDetails);
                                }
                            }
                        }

                        return dsBrokerDetails;
                    }
                    else
                    {
                        //HttpContext.Current.Response.Write(BrokerWSUtility.createjsonForError("Fail", ""));

                        string Message = "";
                        DataSet dsDetails = new DataSet();
                        dsDetails = BrokerWSUtility.CreateDataSetForErrorMessage(Message);
                        return dsBrokerDetails;
                        //  HttpContext.Current.Response.Write(BrokerWSUtility.CreateJsonFromDataset(dsDetails, "DoSaveBusinessInsuranceDetails", "false", "Error Occured"));

                    }

                }
                return dsBrokerDetails;

                return dsBrokerDetails;
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "SSaveLiabilityInsuranceDetailsNew_WebSite", Ex.Message.ToString(), "BrokerUtility.cs_SaveLiabilityInsuranceDetailsNew()", "");
                return dsBrokerDetails;
            }

        }

        #endregion


        public static string SaveDeclarationDocument(string FileName, string base64String, string InsuranceType, string UserId)
        {
            string SaveName = "", FullPath = "", FileExt = "";
            try
            {

                FileExt = FileName.Substring(FileName.LastIndexOf('.') + 1);

                if (FileExt.ToLower() == "pdf" || FileExt.ToLower() == "doc" || FileExt.ToLower() == "docx")
                {
                    SaveName = (InsuranceType + UserId + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + FileExt);
                    //FullPath = strDomainName + strDeclarationDoc + SaveName;
                    FullPath = SaveName;

                    string FileName1 = System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/DeclarationDocument/" + SaveName);

                    byte[] pdfBytes = Convert.FromBase64String(base64String);

                    using (FileStream Writer = new System.IO.FileStream(FileName1, FileMode.Create, FileAccess.Write))
                    {

                        Writer.Write(pdfBytes, 0, pdfBytes.Length);
                    }
                }
                else if (FileExt.ToLower() == "jpg" || FileExt.ToLower() == "jpe" || FileExt.ToLower() == "jpeg" || FileExt.ToLower() == "png")
                {
                    SaveName = (InsuranceType + UserId + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png");
                    //FullPath = strDomainName + strDeclarationDoc + SaveName;
                    FullPath = SaveName;

                    byte[] imageBytes1 = Convert.FromBase64String(base64String);

                    MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);
                    ms1.Write(imageBytes1, 0, imageBytes1.Length);
                    System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms1, true);

                    Size thumbnailSize = GetThumbnailSize(image1);
                    System.Drawing.Image thumbnail = image1.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                    thumbnail.Save(System.Web.HttpContext.Current.Server.MapPath("~/UploadedImages/DeclarationDocument/" + SaveName), System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "SaveDeclarationDocument_WebSite", Ex.Message.ToString(), "BrokerUtility.cs_SaveDeclarationDocument()", "");
            }
            return FullPath;
        }

        public static Size GetThumbnailSize(System.Drawing.Image original)
        {
            // Width and height.
            int originalWidth = original.Width;
            int originalHeight = original.Height;


            // Return thumbnail size.
            return new Size((int)(originalWidth), (int)(originalHeight));
        }

        public static bool ThumbnailCallback()
        {
            return false;
        }
    }
}