using BrokerMVC.App_Code;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
//using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;
using BrokerMVC.Models;
using BrokerMVC.BrokerService;
using System.Net;
using System.Text;
using System.IO;
using PushSharp.Apple;
using Newtonsoft.Json.Linq;

namespace BrokerMVC.BrokerWebDB
{
    public class BrokerWebDB
    {
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
        public static string strEducationLogoFolder = ConfigurationManager.AppSettings["EducationLogo"].ToString();

        public static string strCompanyLogoFolder = ConfigurationManager.AppSettings["CompanyLogoFolder"].ToString();
        public static string strUsersToShowByDefaultInSearchList = ConfigurationManager.AppSettings["UsersToShowByDefaultInSearchList"].ToString();

        public static string strGoogleAppID = ConfigurationManager.AppSettings["GoogleAppID"].ToString();
        public static string strSENDER_ID = ConfigurationManager.AppSettings["SENDER_ID"].ToString();


        //public static string BrokerSearchWithinMiles = ConfigurationManager.AppSettings["BrokerSearchWithinMiles"].ToString();//san

        #region Common Methods

        public static List<spCheckUserLogin_Result> ValidateUserDetails(string UserName, string Password)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            List<spCheckUserLogin_Result> User = null;
            try
            {
                var @cmdText = "exec spCheckUserLogin @UserName, @Password";
                var @params = new[]{
                               new SqlParameter("UserName", UserName),
                               new SqlParameter("Password", Password)
                               };
                User = DB.Database.SqlQuery<spCheckUserLogin_Result>(@cmdText, @params).ToList<spCheckUserLogin_Result>();
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "ValidateUserDetails_Website", Ex.Message.ToString(), "BrokerWebDB.cs_ValidateUserDetails", "0");
            }
            return User;
        }

        public static List<spCheckUserExist_Result> GetCustomerDetails(string EmailId, string UserId)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            List<spCheckUserExist_Result> User = null;
            try
            {
                var @cmdText = "exec spCheckUserExist @UserName";
                var @params = new[]{
                               new SqlParameter("UserName", EmailId)
                               };
                User = DB.Database.SqlQuery<spCheckUserExist_Result>(@cmdText, @params).ToList<spCheckUserExist_Result>();
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "GetCustomerDetails_Website", Ex.Message.ToString(), "BrokerWebDB.cs_GetCustomerDetails", "0");
            }
            return User;
        }

        public static List<spUpdateCustomerForWeb_Result> SaveCustomerProfileDetails(string FirstName, string LastName, string Phone, string Address, string ZipCode, string HouseType, string IsCars, int NoOfCars, string Occupation, string CompanyName, string FileName2, string RenamedImageName, string IsProfileUpdated, int UserId, string NoofEmployee, string EstPremium, string Website)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            List<spUpdateCustomerForWeb_Result> User = null;
            try
            {
                var @cmdText = "exec spUpdateCustomerForWeb @UserId,@FirstName,@LastName,@Phone,@Address,@ZipCode,@HouseType,@IsHavingCars,@NoOfCars,@Occupation,@CompanyName,@ProfilePicture,@ProfilePictureImg,@IsProfileUpdated,@NoofEmployee,@EstPremium,@Website";
                var @params = new[]{
                               new SqlParameter("UserId", UserId),
                               new SqlParameter("FirstName", FirstName),
                               new SqlParameter("LastName", LastName),
                               new SqlParameter("Phone", Phone),
                               new SqlParameter("Address", Address),
                               new SqlParameter("ZipCode", ZipCode),
                               new SqlParameter("HouseType", HouseType),
                               new SqlParameter("IsHavingCars", IsCars),
                               new SqlParameter("NoOfCars", NoOfCars),
                               new SqlParameter("Occupation", Occupation),
                               new SqlParameter("CompanyName", CompanyName),
                               new SqlParameter("ProfilePicture", FileName2),
                               new SqlParameter("ProfilePictureImg", RenamedImageName),
                               new SqlParameter("IsProfileUpdated", IsProfileUpdated),
                               new SqlParameter("NoofEmployee", NoofEmployee),
                               new SqlParameter("EstPremium", EstPremium),
                               new SqlParameter("Website", Website),
                               };
                User = DB.Database.SqlQuery<spUpdateCustomerForWeb_Result>(@cmdText, @params).ToList<spUpdateCustomerForWeb_Result>();
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SaveCustomerProfileDetails_Website", Ex.Message.ToString(), "BrokerWebDBUtility.cs_SaveCustomerProfileDetails", "0");
            }
            return User;
        }

        public static DataSet CheckBrokerLogin(string UserName, string Password)
        {
            DataSet dsVerifyUser = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspCheckBrokerLogin");
                db.AddInParameter(dbCommand, "UserName", DbType.String, UserName);
                db.AddInParameter(dbCommand, "Password", DbType.String, Password);

                dsVerifyUser = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(0, "CheckBrokerLogin_Website", ex.Message.ToString(), "BrokerWebDB.cs_CheckBrokerLogin()", "0");
            }
            return dsVerifyUser;
        }

        public static DataSet GetBrokerDetails(string UserName)
        {
            DataSet dsVerifyUser = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetBrokerDetails");
                db.AddInParameter(dbCommand, "UserName", DbType.String, UserName);


                dsVerifyUser = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(0, "GetBrokerDetails_Website", ex.Message.ToString(), "BrokerWebDB.cs_GetBrokerDetails()", "0");
            }
            return dsVerifyUser;
        }


        /*Santosh */


        public static int SaveHomeInsuranceDetails(int UserId, string ZipCode, string City, string EstimatedValue, string IsInsured, string CompanyName, string CoverageExpires, string Language, string Notes, string Longitude, string Latitude, string DocPath, string Base64String)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;

            try
            {
                var @cmdText = "exec uspHomeInsuranceDetails @UserId,@ZipCode,@City,@EstimatedValue,@IsInsured,@CompanyName,@CoverageExpires,@Language,@Notes,@Longitude,@Latitude,@DocPath,@DeclarationDocBase64";
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
                               new SqlParameter("@DocPath", DocPath),
                               new SqlParameter("@DeclarationDocBase64", Base64String)
                                                 
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
                BrokerUtility.ErrorLog(0, "GetDeviceId_WebSite", Ex.Message.ToString(), "BrokerWebDB.cs_GetDeviceId", "0");
            }
            return dsDeviceId;
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

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspBrokerMessages");
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
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SendMessages", Ex.Message.ToString(), "BrokerWSUtility.cs_SendMessages()", BrokerUtility.GetIPAddress(UserId.ToString()));
            }

            return count;
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


        //Get Broker List San
        public static DataSet SearchBrokersList(int UserId, string ZipCode, string City, string Language, string Speciality, string Longitude, string Latitude, string SearchCriteria, string IndustryId, string SubIndustryId)
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


        //Save user details of Benefits Insurance.
        public static int SaveBenefitInsuranceDetails(int UserId, string ZipCode, string City, string IsInsured, string InsuranceCompany, string EmployeeStrength, string CoverageExpires, string Language, string Notes, string Longitude, string Latitude, string IndustryId, string SubIndustryId, string DocPath, string Base64String)
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
                               new SqlParameter("@DeclarationDocBase64",Base64String)
                              };
                User = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SaveBenefitInsuranceDetails", Ex.Message.ToString(), "BrokerWSUtility.cs_SaveBenefitInsuranceDetails", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return User;
        }


        //Rash
        public static int SaveLifeInsuranceDetails(int UserId, string ZipCode, string City, string IsInsured, string InsuranceCompany, string Language, string FaceValue, string CoverageExpires, string Notes, string Longitude, string Latitude)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;

            try
            {


                var @cmdText = "exec uspSaveLifeInsuranceDetails @UserId,@ZipCode,@City,@IsInsured,@InsuranceCompany,@Language,@FaceValue,@CoverageExpires,@Notes,@Longitude,@Latitude";
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
                               new SqlParameter("@Latitude", Latitude)
                    
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

        //yogita
        public static int SaveAutoInsuranceDetails(int UserId, string ZipCode, string City, string VehicleType, string IsInsured, string InsuranceCompany, string CoverageExpires, string Language, string Notes, string Longitude, string Latitude, string DocPath, string Base64String)
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
                               new SqlParameter("@DocPath",DocPath),
                               new SqlParameter("@DeclarationDocBase64",Base64String)
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

        public static DataSet SaveBusinessInsuranceDetails(BusinessInsurance business)
        {
            DataSet dsBrokerDetails = null;
            string ZipCode = "", City = "", IsInsured = "", InsuranceCompany = "", SICCode = "",
               Revenue = "", CoverageExpires = "", Language = "", Notes = "", Longitude = "", Latitude = "", IndustryId = "", SubIndustryId = "";

            int User, UserId = 0;
            try
            {
                if (HttpContext.Current.Session["UserId"] != null)
                {
                    UserId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                    ZipCode = business.ZipCode;
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
                        if (HttpContext.Current.Session["SubIndustryarray"] != null)
                        {
                            SubIndustryId = (HttpContext.Current.Session["SubIndustryarray"]).ToString();
                        }

                    }

                    if (business.Language == null) Language = ""; else Language = business.Language;
                    if (business.Notes == null) Notes = ""; else Notes = business.Notes;
                    if (business.Longitude == null) Longitude = ""; else Longitude = business.Longitude;
                    if (business.Latitude == null) Latitude = ""; else Latitude = business.Latitude;
                    User = BrokerWSUtility.SaveBusinessInsuranceDetails(UserId, ZipCode, City, IsInsured, InsuranceCompany, SICCode, Revenue, CoverageExpires, Language, Notes, Longitude, Latitude, IndustryId, SubIndustryId,"","");
                    if (User != 0)
                    {
                        dsBrokerDetails = BrokerWSUtility.GetBrokersList(UserId, ZipCode, City, Language, "Business", Longitude, Latitude, Revenue, IndustryId, SubIndustryId);

                        if (dsBrokerDetails.Tables.Count > 0)
                        {
                            if (dsBrokerDetails.Tables[0].Rows.Count > 0)
                            {
                                dsBrokerDetails = GetFinalData(dsBrokerDetails);
                            }
                            else
                            {
                                dsBrokerDetails = GetByDefaultBrokersList(UserId, strUsersToShowByDefaultInSearchList);

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
                }
                return dsBrokerDetails;
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "SaveBusinessInsuranceDetails_WebSite", Ex.Message.ToString(), "BrokerUtility.cs_SaveBusinessInsuranceDetails()", "");
                return dsBrokerDetails;
            }

        }

        //Shashikant
        public static List<uspSaveBrokerBasicDetails_Result> SaveBrokerBasicDetails(string FirstName, string LastName, string Phone, string Email, string Area, string ZipCode, string Title, string CompanyName, string Language, string Specialities, string EncryptPass, string Encryptrandom, string CompanyLogoPath, string ProfilePicPath, string HomeValue, string AutoType, string Revenue, string Employees, string CoverageAmt, string IndustryId, string SubIndustryId, string longitude, string latitude, string Bio)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            List<uspSaveBrokerBasicDetails_Result> User = null;
            try
            {
                var @cmdText = "exec uspSaveBrokerBasicDetails @FirstName,@LastName,@Phone,@Email,@Address,@ZipCode,@Title,@CompanyName,@Language,@Specialities,@Password,@RegistrationCode,@CompanyLogoPath,@ProfilePicPath,@HomeValue,@AutoType,@Revenue,@Employees,@CoverageAmt,@IndustryId,@SubIndustryId,@longitude,@latitude,@Bio";
                var @params = new[]{
                               new SqlParameter("@FirstName",FirstName),
                               new SqlParameter("@LastName",LastName),
                               new SqlParameter("@Phone",  Phone),
                               new SqlParameter("@Email", Email),
                               new SqlParameter("@Address", Area),
                               new SqlParameter("@ZipCode", ZipCode),
                               new SqlParameter("@Title", Title),
                               new SqlParameter("@CompanyName", CompanyName),
                               new SqlParameter("@Language", Language),
                               new SqlParameter("@Specialities",  Specialities),
                               new SqlParameter("@Password", EncryptPass),
                               new SqlParameter("@RegistrationCode", Encryptrandom),
                               new SqlParameter("@CompanyLogoPath", CompanyLogoPath),
                               new SqlParameter("@ProfilePicPath", ProfilePicPath),

                               new SqlParameter("@HomeValue", HomeValue),
                               new SqlParameter("@AutoType", AutoType),
                               new SqlParameter("@Revenue", Revenue),
                               new SqlParameter("@Employees", Employees),
                               new SqlParameter("@CoverageAmt", CoverageAmt),
                               new SqlParameter("@IndustryId", IndustryId),
                               new SqlParameter("@SubIndustryId", SubIndustryId),

                               new SqlParameter("@longitude", longitude),
                               new SqlParameter("@latitude", latitude),
                               new SqlParameter("@Bio", Bio)
                                             
                              };
                User = DB.Database.SqlQuery<uspSaveBrokerBasicDetails_Result>(@cmdText, @params).ToList<uspSaveBrokerBasicDetails_Result>();
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(0, "SaveBrokerBasicDetails_Website", ex.Message.ToString(), "BrokerWebDB.cs_SaveBrokerBasicDetails()", "0");
            }
            return User;
        }

        public static int SaveBrokerEducationDetails(int UserId, string School, string Degree, string Year, string EduLogoPath)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;
            try
            {
                var @cmdText = "exec uspSaveBrokerEducationDetailsNew @UserId,@School1,@Degree1,@Year1,@EduLogoPath1";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@School1", School),
                               new SqlParameter("@Degree1", Degree),
                               new SqlParameter("@Year1", Year),
                               new SqlParameter("@EduLogoPath1", EduLogoPath)
                              };
                user = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(0, "SaveBrokerEducationDetails_Website", ex.Message.ToString(), "BrokerWebDB.cs_SaveBrokerEducationDetails()", "0");
            }
            return user;
        }

        public static int SaveBrokerExperienceDetails(int UserId, string ExpComp, string ExpDesig, string ExpFromMonth, string ExpFromYear, string ExpToMonth, string ExpToYear, string ExpLogoPath)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;
            try
            {
                var @cmdText = "exec uspSaveBrokerCompanyDetails @UserId,@ExpComp,@ExpDesig,@ExpFromMonth,@ExpFromYear,@ExpToMonth,@ExpToYear,@ExpLogoPath";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@ExpComp", ExpComp),
                               new SqlParameter("@ExpDesig", ExpDesig),
                               new SqlParameter("@ExpFromMonth", ExpFromMonth),
                               new SqlParameter("@ExpFromYear", ExpFromYear),
                               new SqlParameter("@ExpToMonth", ExpToMonth),
                               new SqlParameter("@ExpToYear", ExpToYear),
                               new SqlParameter("@ExpLogoPath", ExpLogoPath)
                              };
                user = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(UserId, "SaveBrokerExperienceDetails_Website", ex.Message.ToString(), "BrokerWebDB.cs_SaveBrokerExperienceDetails()", "0");
            }
            return user;
        }

        public static int SaveBrokerIndustryDetails(int UserId, string IndustryId, string SubIndustryId)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;
            try
            {
                var @cmdText = "exec uspSaveBrokerIndustryDetails @UserId,@IndustryId,@SubIndustryId";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@IndustryId", IndustryId),
                               new SqlParameter("@SubIndustryId", SubIndustryId)
                              };
                user = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(UserId, "SaveBrokerIndustryDetails_Website", ex.Message.ToString(), "BrokerWebDB.cs_SaveBrokerIndustryDetails()", "0");
            }
            return user;
        }

        //Santosh

        //30Mar17
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
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "GetBrokerAvailabilityStatus_WebSite", Ex.Message.ToString(), "BrokerWebDB.cs_GetBrokerAvailabilityStatus()", BrokerUtility.GetIPAddress(UserId.ToString()));
            }

            return dsBrokerAvailability;
        }

        //30Mar17
        public static int SetBrokerAvailabilityStatus(int UserId, bool Availability)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;

            try
            {
                var @cmdText = "exec uspSetBrokersAvailabilty @UserId,@Availability";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@Availability", Availability)
                              
                              };

                user = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SetBrokerAvailabilityStatus_WebSite", Ex.Message.ToString(), "BrokerWSUtility.cs_SetBrokerAvailabilityStatus()", BrokerUtility.GetIPAddress(UserId.ToString()));
            }

            return user;
        }

        public static int UpdateBrokerBasicDetails(int UserId, string FirstName, string LastName, string Phone, string Area, string ZipCode, string Title, string CompanyName, string Language, string Specialities, string CompanyLogoPath, string ProfilePicPath, string HomeValue, string AutoType, string Revenue, string Employees, string CoverageAmt, string IndustryId, string SubIndustryId, string IsProfilePhotoChanged, string IsCompanyLogoChanged, string longitude, string latitude, string Bio)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            //List<uspSaveBrokerBasicDetails_Result> User = null;
            int User = 0;
            try
            {
                var @cmdText = "exec uspUpdateBrokerBasicDetails @UserId,@FirstName,@LastName,@Phone,@Area,@ZipCode,@Title,@CompanyName,@Language,@Specialities,@CompanyLogoPath,@ProfilePicPath,@HomeValue,@AutoType,@Revenue,@Employees,@CoverageAmt,@IndustryId,@SubIndustryId,@IsProfilePhotoChanged,@IsCompanyLogoChanged,@longitude,@latitude,@Bio";
                var @params = new[]{
                               new SqlParameter("@UserId",UserId),
                               new SqlParameter("@FirstName",FirstName),
                               new SqlParameter("@LastName",LastName),
                               new SqlParameter("@Phone",  Phone),
                               new SqlParameter("@Area", Area),
                               new SqlParameter("@ZipCode", ZipCode),
                               new SqlParameter("@Title", Title),
                               new SqlParameter("@CompanyName", CompanyName),
                               new SqlParameter("@Language", Language),
                               new SqlParameter("@Specialities",  Specialities),
                               new SqlParameter("@CompanyLogoPath", CompanyLogoPath),
                               new SqlParameter("@ProfilePicPath", ProfilePicPath),                              
                               new SqlParameter("@HomeValue", HomeValue),
                               new SqlParameter("@AutoType", AutoType),
                               new SqlParameter("@Revenue", Revenue),
                               new SqlParameter("@Employees", Employees),
                               new SqlParameter("@CoverageAmt", CoverageAmt),
                               new SqlParameter("@IndustryId", IndustryId),
                               new SqlParameter("@SubIndustryId", SubIndustryId),
                               new SqlParameter("@IsProfilePhotoChanged", IsProfilePhotoChanged),
                               new SqlParameter("@IsCompanyLogoChanged", IsCompanyLogoChanged),

                               new SqlParameter("@longitude", longitude),
                               new SqlParameter("@latitude", latitude),
                               new SqlParameter("@Bio",Bio)
                                             
                              };
                //User = DB.Database.SqlQuery<uspSaveBrokerBasicDetails_Result>(@cmdText, @params).ToList<uspSaveBrokerBasicDetails_Result>();
                User = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(0, "UpdateBrokerBasicDetails_Website", ex.Message.ToString(), "BrokerWebDB.cs_UpdateBrokerBasicDetails()", "0");
            }
            return User;
        }

        public static int DeleteBrokerSubDetailsWeb(int UserId, string TableName)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;

            try
            {
                var @cmdText = "exec uspDeleteBrokerSubDetailsWeb @UserId,@TableName";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@TableName", TableName)                    
                              };

                user = DB.Database.ExecuteSqlCommand(@cmdText, @params);

            }
            catch (Exception ex)
            {

                BrokerUtility.ErrorLog(UserId, "DeleteBrokerSubDetailsWeb_Website", ex.Message.ToString(), "BrokerWebDB.cs_DeleteBrokerSubDetailsWeb()", "0");
                throw;
            }

            return user;
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
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "DeleteZipCode", Ex.Message.ToString(), "BrokerWebDB.cs_DeleteZipCode()", "0");
            }
            return Result;
        }

        public static int InsertUserZipCode(int UserId, string ZipCode, string Long, string Lat)
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
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "InsertUserZipCode", Ex.Message.ToString(), "BrokerWebDB.cs_InsertUserZipCode()", "0");
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

        public static DataSet GetFinalData(DataSet dsUserDetails)
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
                        Logo = strDomainName + "" + strCompanyLogoFolder + "" + dsUserDetails.Tables["ExperienceDetails"].Rows[i]["Logo"].ToString(); ;

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

        public static DataSet GetMainMessage(string BrokerId, string CustomerId, string BrokerMessageId, string CustomerMessageId, string TableName)
        {
            DataSet dsMainMsgDetails = null;
            string MainMsg = "";
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetMainMessageofCustForBroker");

                db.AddInParameter(dbCommand, "BrokerId", DbType.Int32, BrokerId);
                db.AddInParameter(dbCommand, "CustomerId", DbType.Int32, CustomerId);
                db.AddInParameter(dbCommand, "BrokerMessageId", DbType.Int32, BrokerMessageId);
                db.AddInParameter(dbCommand, "CustomerMessageId", DbType.Int32, CustomerMessageId);
                db.AddInParameter(dbCommand, "TableName", DbType.String, TableName);

                dsMainMsgDetails = db.ExecuteDataSet(dbCommand);

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "GetMainMessageofCustForBroker_WebSite", Ex.Message.ToString(), "BrokerWebDB.cs_GetMainMessageofCustForBroker", "0");
            }
            return dsMainMsgDetails;
        }

        public static int UpdateCustomerProfilePhoto(int UserId, string ProfilePicture, string ProfilePictureImg)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;

            try
            {
                var @cmdText = "exec uspUpdateCustomerProfilePhoto @UserId,@ProfilePicture,@ProfilePictureImg";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@ProfilePicture", ProfilePicture),
                               new SqlParameter("@ProfilePictureImg", ProfilePictureImg)                              
                              };

                user = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "UpdateCustomerProfilePhoto_Website", Ex.Message.ToString(), "BrokerWebDB.cs_UpdateCustomerProfilePhoto()", BrokerUtility.GetIPAddress(UserId.ToString()));
            }

            return user;
        }

        //12Sept17
        public static int SaveUserSerchedZipCodes(int UserId, string ZipCode)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int Result = 0;
            try
            {
                var @cmdText = "exec uspSaveUserSerchedZipCodes @UserId,@ZipCode";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@ZipCode", ZipCode)
                              };
                Result = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SaveUserSerchedZipCodes", Ex.Message.ToString(), "BrokerWebDB.cs_SaveUserSerchedZipCodes()", "0");
            }
            return Result;
        }

        //22Sept17 Santosh //4Oct17 san
        public static DataSet GetUserStatus(string UserName, string FirstName, string LastName, string City, string PinCode, string MobNo, string RegistrationCode, string CompanyName)
        {
            DataSet dsuserstatus = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("spCheckUserStatus");
                db.AddInParameter(dbCommand, "UserName", DbType.String, UserName);
                db.AddInParameter(dbCommand, "FirstName", DbType.String, FirstName);
                db.AddInParameter(dbCommand, "LastName", DbType.String, LastName);
                db.AddInParameter(dbCommand, "City", DbType.String, City);
                db.AddInParameter(dbCommand, "PinCode", DbType.String, PinCode);
                db.AddInParameter(dbCommand, "MobNo", DbType.String, MobNo);
                db.AddInParameter(dbCommand, "RegistrationCode", DbType.String, RegistrationCode);
                db.AddInParameter(dbCommand, "CompanyName", DbType.String, CompanyName);

                dsuserstatus = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(0, "GetUserStatus_Website", ex.Message.ToString(), "BrokerWebDB.cs_GetUserStatus()", "0");
            }
            return dsuserstatus;
        }

        public static DataSet getCustomerMessageDetails(int CustMsgId)
        {

            DataSet dsCustMsg = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("spGetCustomerMessageDetails");
                db.AddInParameter(dbCommand, "CustMsgId", DbType.Int32, CustMsgId);

                dsCustMsg = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(0), "getCustomerMessageDetails_Website", Ex.Message.ToString(), "BrokerWebDBUtility.cs_getCustomerMessageDetails", "0");
            }
            return dsCustMsg;
        }


        public static DataSet CheckForValidUserDetails(string EmailId, string RegistrationCode)
        {
            int User = 0;
            DataSet ds = new DataSet();
            BrokerDBEntities DB = new BrokerDBEntities();

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspCheckForValidUserDetails");
                db.AddInParameter(dbCommand, "EmailId", DbType.String, EmailId);
                db.AddInParameter(dbCommand, "RegistrationCode", DbType.String, RegistrationCode);

                ds = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(0, "CheckForValidUserDetails", ex.Message.ToString(), "BrokerUtility.cs_CheckForValidUserDetails()", "0");
            }
            return ds;
        }

        public static void UpdateUserDetails(string Email, string registrationCode, string UserId, string FirstName, string LastName, string PhoneNo)
        {
            int User = 0;
            DataSet ds = new DataSet();
            BrokerDBEntities DB = new BrokerDBEntities();

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspUpdateCustomerVerificationDetails");
                db.AddInParameter(dbCommand, "EmailId", DbType.String, Email);
                db.AddInParameter(dbCommand, "RegistrationCode", DbType.String, registrationCode);
                db.AddInParameter(dbCommand, "UserId", DbType.Int32, UserId);
                db.AddInParameter(dbCommand, "FirstName", DbType.String, FirstName);
                db.AddInParameter(dbCommand, "LastName", DbType.String, LastName);
                db.AddInParameter(dbCommand, "PhoneNo", DbType.String, PhoneNo);

                ds = db.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(0, "UpdateUserDetails", ex.Message.ToString(), "BrokerWebDB.cs_UpdateUserDetails()", "0");
            }

        }

        #endregion Common Methods

        #region For Meineke Insurance Comp

        public static DataSet SearchBrokersListForMeineke(int UserId, string ZipCode, string City, string Speciality, string Longitude, string Latitude, string IndustryId, string SubIndustryId)
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
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SearchBrokersListForMeineke", Ex.Message.ToString(), "BrokerWSUtility.cs_GetBrokersList", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return dsBrokerList;
        }

        public static int MeinekeSendMessages(int UserId, int BrokerId, string InsuranceType, string Note, string LocalDateTime)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int count = 0;

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspBrokerMessagesForMeineke");
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
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SendMessages", Ex.Message.ToString(), "BrokerWSUtility.cs_SendMessages()", BrokerUtility.GetIPAddress(UserId.ToString()));
            }

            return count;
        }

        // For Commercial Auto Insurance

        public static int SaveCommercialAutoInsuranceDetails(int UserId, string ZipCode, string City, string Longitude, string Latitude, string NoOfStalls, string NoOfLocations, string GrossRevenue, string CurrentLimit, string DocPath, string Base64String)
        {
            int User = 0;
            BrokerDBEntities DB = new BrokerDBEntities();
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
                               new SqlParameter("@CurrentLimit",CurrentLimit),
                               new SqlParameter("@NoOfUnits",""),
                               new SqlParameter("@DeductibleIfAny",""),
                               new SqlParameter("@DocPath",DocPath),
                               new SqlParameter("@DeclarationDocBase64",Base64String)
                              };
                User = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SaveCommercialAutoInsuranceDetails", Ex.Message.ToString(), "BrokerWSUtility.cs_SaveBenefitInsuranceDetails", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return User;
        }

        public static int SaveWorkCompDetails(int UserId, string ZipCode, string City, string Longitude, string Latitude, string NoOfEmp, string GrossPayroll, string DocPath, string Base64String)
        {
            int User = 0;
            BrokerDBEntities DB = new BrokerDBEntities();
            try
            {
                var @cmdText = "exec uspSaveWorkCompDetails @UserId, @ZipCode, @City,@Longitude,@Latitude,@NoOfEmp,@GrossPayroll,@DocPath,@DeclarationDoc";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@ZipCode", ZipCode),
                               new SqlParameter("@City", City),
                               new SqlParameter("@Longitude", Longitude),
                               new SqlParameter("@Latitude", Latitude),
                               new SqlParameter("@NoOfEmp",NoOfEmp),
                               new SqlParameter("@GrossPayroll",GrossPayroll),
                               new SqlParameter("@DocPath",DocPath),
                               new SqlParameter("@DeclarationDoc",Base64String)
                              };
                User = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SaveWorkCompDetails_Website", Ex.Message.ToString(), "BrokerWebDB.cs_SaveWorkCompDetails", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return User;
        }

        public static int SaveLiabilityInsuranceDetails(int UserId, string ZipCode, string City, string Longitude, string Latitude, string DeductibleIfAny, string GrossSale, string IndustryId, string SubIndustryId, string DocPath, string Base64String)
        {
            int User = 0;
            BrokerDBEntities DB = new BrokerDBEntities();
            try
            {
                var @cmdText = "exec uspSaveLiabilityInsuranceDetails @UserId, @ZipCode, @City,@Longitude,@Latitude,@DeductibleIfAny,@GrossSale,@IndustryId,@SubIndustryId,@DocPath";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@ZipCode", ZipCode),
                               new SqlParameter("@City", City),
                               new SqlParameter("@Longitude", Longitude),
                               new SqlParameter("@Latitude", Latitude),
                               new SqlParameter("@DeductibleIfAny",DeductibleIfAny),
                               new SqlParameter("@GrossSale",GrossSale),
                               new SqlParameter("@IndustryId",Convert.ToInt32(IndustryId)),
                               new SqlParameter("@SubIndustryId",SubIndustryId),
                               new SqlParameter("@DocPath",DocPath)
                              };
                User = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SaveLiabilityInsuranceDetails_Website", Ex.Message.ToString(), "BrokerWSUtility.cs_SaveBenefitInsuranceDetails", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return User;
        }

        public static int SaveMeinekeBenefitInsuranceDetails(int UserId, string ZipCode, string City, string IsInsured, string InsuranceCompany, string EmployeeStrength, string CoverageExpires, string Language, string Notes, string Longitude, string Latitude, string IndustryId, string SubIndustryId, string DocPath, string Base64String)
        {
            int User = 0;
            BrokerDBEntities DB = new BrokerDBEntities();
            try
            {
                var @cmdText = "exec uspSaveMeinekeBenefitsInsuranceDetails @UserId, @ZipCode, @City, @IsInsured, @InsuranceCompany, @EmployeeStrength, @CoverageExpires, @Language, @Notes,@Longitude,@Latitude,@IndustryId,@SubIndustryId,@DocPath,@DeclarationDocBase64";
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
                               new SqlParameter("@DeclarationDocBase64",Base64String)
                              };
                User = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SaveBenefitInsuranceDetails", Ex.Message.ToString(), "BrokerWSUtility.cs_SaveBenefitInsuranceDetails", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return User;
        }

        public static int Save401kInsuranceDetails(int UserId, string ZipCode, string City, string Longitude, string Latitude, string CurrentPlan, string NoOfEmp, string PlanSize, string DocPath, string Base64String)
        {
            int User = 0;
            BrokerDBEntities DB = new BrokerDBEntities();
            try
            {
                var @cmdText = "exec uspSave401kDetails @UserId, @ZipCode, @City,@Longitude,@Latitude,@CurrentPlan,@NoOfEmp,@PlanSize,@DocPath,@DeclarationDocBase64";//30May18
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
                               new SqlParameter("@DeclarationDocBase64",Base64String)//30May18
                              };
                User = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "Save401kInsuranceDetails_Website", Ex.Message.ToString(), "BrokerWebDB.cs_Save401kInsuranceDetails", BrokerUtility.GetIPAddress(UserId.ToString()));
            }
            return User;
        }

        #endregion For Meineke Insurance Comp

        public static DataSet GetMessageDetails(string CustMsgId, string BrokerMsgId)
        {
            DataSet dsMessageDetails = null;


            int User, UserId = 0;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetBrokerMessageDetails");
                db.AddInParameter(dbCommand, "CustMsgId", DbType.Int32, CustMsgId);
                db.AddInParameter(dbCommand, "BrokerMsgId", DbType.Int32, BrokerMsgId);

                dsMessageDetails = db.ExecuteDataSet(dbCommand);

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "GetMessageDetails_WebSite", Ex.Message.ToString(), "BrokerWebDB_GetMessageDetails()", "");
                //dsBrokerDetails
            }
            return dsMessageDetails;
        }

        public static int saveVideoDetails(string url, string title, string description, string assignedcompany, string fromdate, string todate, int uploadedby)
        {
            int video = 0;
            BrokerDBEntities DB = new BrokerDBEntities();



            //var @cmdtxt = "exec uspSaveVideoDetails  @url ,@title ,@description ,@assignedcompany, @fromdate ,@todate ,@uploadedby";
            //var @para = new[] { 
            //                     new SqlParameter("@url", url),
            //                     new SqlParameter("@title", title),
            //                     new SqlParameter("@description", description),
            //                     new SqlParameter("@assignedcompany", assignedcompany),
            //                     new SqlParameter("@fromdate", fromdate),
            //                     new SqlParameter("@todate", todate),
            //                     new SqlParameter("@uploadedby", uploadedby)
            //           };

            //video = DB.Database.ExecuteSqlCommand(@cmdtxt, @para);     
            int count = 0;
            DataSet dsUserDetails = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspSaveVideoDetails");
                db.AddInParameter(dbCommand, "url", DbType.String, url);
                db.AddInParameter(dbCommand, "title", DbType.String, title);
                db.AddInParameter(dbCommand, "description", DbType.String, description);
                db.AddInParameter(dbCommand, "assignedcompany", DbType.String, assignedcompany);
                db.AddInParameter(dbCommand, "fromdate", DbType.DateTime, fromdate);
                db.AddInParameter(dbCommand, "todate", DbType.DateTime, todate);
                db.AddInParameter(dbCommand, "uploadedby", DbType.Int32, uploadedby);


                dsUserDetails = db.ExecuteDataSet(dbCommand);

                if (dsUserDetails.Tables.Count > 0)
                {
                    if (dsUserDetails.Tables[0].Rows.Count > 0)
                    {
                        count = Convert.ToInt32(dsUserDetails.Tables[0].Rows[0][0]);
                    }
                }

            }
            catch (Exception ex)
            {

                BrokerUtility.ErrorLog(0, "saveVideoDetails_Website", ex.Message.ToString(), "BrokerWebDB.cs_saveVideoDetails", "0");
            }

            return count;
        }

        public static int MeinekeSendMessagesPersonal(int UserId, int BrokerId, string InsuranceType, string Note, string LocalDateTime)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int count = 0;

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspBrokerMessagesForMeinekePersonal");
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
                BrokerUtility.ErrorLog(Convert.ToInt32(UserId), "SendMessages", Ex.Message.ToString(), "BrokerWSUtility.cs_SendMessages()", BrokerUtility.GetIPAddress(UserId.ToString()));
            }

            return count;
        }

        public static List<BrokkrrBriefcase> GetVideoList(int UserId)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            List<BrokkrrBriefcase> User = null;
            try
            {
                var @cmdText = "exec uspGetVideosList @UserId";
                var @params = new[]{
                               new SqlParameter("UserId", UserId)                              
                               };
                User = DB.Database.SqlQuery<BrokkrrBriefcase>(@cmdText, @params).ToList<BrokkrrBriefcase>();
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "GetVideoList_Website", Ex.Message.ToString(), "BrokerWebDB.cs_GetVideoList", "0");
            }
            return User;

        }

        public static DataSet GetUserListForVideoNotification(string AssignedCompany)
        {
            DataSet dsMessageDetails = null;


            int User, UserId = 0;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetUserListForVideoNotification");
                db.AddInParameter(dbCommand, "AssignedCompany", DbType.String, AssignedCompany);
                //db.AddInParameter(dbCommand, "UserType", DbType.String, UserType);

                dsMessageDetails = db.ExecuteDataSet(dbCommand);

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "GetUserListForVideoNotification_WebSite", Ex.Message.ToString(), "BrokerWebDB_GetUserListForVideoNotification()", "");
                //dsBrokerDetails
            }
            return dsMessageDetails;
        }

        public static void DoPushNotification(string deviceId, string message, string title, string msgcnt)
        {
            string Notification = message;
            DataSet dsData = new DataSet();
            int badge = 0;

            try
            {

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

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoPushNotification_WebSite", Ex.Message.ToString(), "BrokerWebDB.cs_DoPushNotification", "");


            }
        }


        public static void DoPushNotificationForiOS(string deviceId, string message, string title, string msgcnt)
        {
            string sound = "", deviceId1 = "";
            int badge = 0;
            DataSet dsData = null;

            sound = "Sound/pop.m4a";

            try
            {

                //deviceId1 = "iOS" + deviceId;



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


                apnsBroker.Start();

                //apnsBroker.QueueNotification(new ApnsNotification
                //{
                //    DeviceToken = deviceId,
                //    Payload = JObject.Parse("{\"aps\" : { \"alert\":\'" + message + "',\"badge\" : \'" + badge.ToString() + "',\"content-available\":\"1\",\"sound\" : \'" + sound + "' }}")
                //    //Payload = JObject.Parse("{\"aps\" : { \"alert\":\'" + message + "',\"badge\" : \"20\",\"content-available\":\"1\",\"sound\" : \'" + sound + "' }}")
                //});

                //Payload = JObject.Parse("{\"aps\" : { \"alert\":\'" + message + "',\"badge\" : \'" + badge.ToString() + "',\"content-available\":\"1\",\"sound\" : \'" + sound + "' }}")

                string appleJsonFormat = "{\"aps\" : { \"alert\":\'" + title + " - " + message + "',\"badge\" : \'" + badge.ToString() + "',\"content-available\":\"1\",\"sound\" : \'" + sound + "' }}";

                apnsBroker.QueueNotification(new ApnsNotification
                {
                    DeviceToken = deviceId,
                    Payload = JObject.Parse(appleJsonFormat)
                });

                apnsBroker.Stop();

                /************************************************************************/


            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "DoPushNotificationForiOS_WebSite", Ex.Message.ToString(), "BrokerWebDB.cs_DoPushNotificationForiOS", "");

            }
        }

        public static int DeleteVideoById(int id)
        {
            int video_id = 0;
            BrokerDBEntities DB = new BrokerDBEntities();
            try
            {
                var @cmdtxt = "exec uspDeleteVideoById  @Id";
                var @para = new[] { 
                                     new SqlParameter("@Id", id)                                    
                           };

                video_id = DB.Database.ExecuteSqlCommand(@cmdtxt, @para);
            }
            catch (Exception ex)
            {

                BrokerUtility.ErrorLog(0, "DeleteVideoById_Website", ex.Message.ToString(), "BrokerWebDB.cs_DeleteVideoById", "0");
            }
            return video_id;
        }


        public static void SaveUserListInWatchedVideoHistory(string AssignedCompany, int VideoId)
        {
            int Id = 0;
            BrokerDBEntities DB = new BrokerDBEntities();
            try
            {
                var @cmdtxt = "exec uspGetUserListofAssignedComp  @AssignedCompany,@VideoId";
                var @para = new[] { 
                                     new SqlParameter("@AssignedCompany", AssignedCompany),
                                     new SqlParameter("@VideoId", VideoId)
                           };

                Id = DB.Database.ExecuteSqlCommand(@cmdtxt, @para);
            }
            catch (Exception ex)
            {

                BrokerUtility.ErrorLog(0, "SaveUserListInWatchedVideoHistory_Website", ex.Message.ToString(), "BrokerWebDB.cs_SaveUserListInWatchedVideoHistory", "0");
            }
            //return video_id;
        }

        public static int UpdateVideoDetails(int vid_id, string url, string title, string description, string assignedcompany, string fromdate, string todate, int uploadedby)
        {
            int video = 0;
            BrokerDBEntities DB = new BrokerDBEntities();
            int count = 0;
            DataSet dsUserDetails = null;

            try
            {
                //var @cmdtxt = "exec uspUpdateVideoDetails  @Id,@url,@title,@description,@assignedcompany,@fromdate,@todate,@uploadedby";
                //var @para = new[] { 
                //                     new SqlParameter("@Id", vid_id),
                //                     new SqlParameter("@url", url),
                //                     new SqlParameter("@title", title),
                //                     new SqlParameter("@description", description),
                //                     new SqlParameter("@assignedcompany", assignedcompany),
                //                     new SqlParameter("@fromdate",(fromdate == null)?"NULL":fromdate),
                //                     new SqlParameter("@todate", (todate == null)?"NULL":todate),
                //                     new SqlParameter("@uploadedby",uploadedby)
                //           };

                //count = DB.Database.ExecuteSqlCommand(@cmdtxt, @para);

                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspUpdateVideoDetails");
                db.AddInParameter(dbCommand, "Id", DbType.Int32, vid_id);
                db.AddInParameter(dbCommand, "url", DbType.String, url);
                db.AddInParameter(dbCommand, "title", DbType.String, title);
                db.AddInParameter(dbCommand, "description", DbType.String, description);
                db.AddInParameter(dbCommand, "assignedcompany", DbType.String, assignedcompany);
                db.AddInParameter(dbCommand, "fromdate", DbType.DateTime, fromdate);
                db.AddInParameter(dbCommand, "todate", DbType.DateTime, todate);
                db.AddInParameter(dbCommand, "uploadedby", DbType.Int32, uploadedby);


                dsUserDetails = db.ExecuteDataSet(dbCommand);

                if (dsUserDetails.Tables.Count > 0)
                {
                    if (dsUserDetails.Tables[0].Rows.Count > 0)
                    {
                        count = Convert.ToInt32(dsUserDetails.Tables[0].Rows[0][0]);
                    }
                }
                count = 1;
            }
            catch (Exception ex)
            {
                count = 0;
                BrokerUtility.ErrorLog(0, "UpdateVideoDetails_Website", ex.Message.ToString(), "BrokerWebDB.cs_UpdateVideoDetails", "0");
            }

            return count;
        }

        public static void DeleteFromWatchedHistory(int VideoId)
        {

            BrokerDBEntities DB = new BrokerDBEntities();
            int count = 0;
            try
            {
                var @cmdtxt = "exec uspDeleteFromWatchedHistory  @VideoId";
                var @para = new[] { 
                                     new SqlParameter("@VideoId", VideoId)
                           };

                count = DB.Database.ExecuteSqlCommand(@cmdtxt, @para);


            }
            catch (Exception ex)
            {

                BrokerUtility.ErrorLog(0, "DeleteFromWatchedHistory_Website", ex.Message.ToString(), "BrokerWebDB.cs_DeleteFromWatchedHistory", "0");
            }

            //return count;
        }


        public static DataSet GetUserListForVideoEmail(string AssignedCompany, string Action, int VideoId)
        {
            DataSet dsMessageDetails = null;


            int User, UserId = 0;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbCommand = db.GetStoredProcCommand("uspGetUserListForVideoEmail");
                db.AddInParameter(dbCommand, "AssignedCompany", DbType.String, AssignedCompany);
                db.AddInParameter(dbCommand, "Action", DbType.String, Action);
                db.AddInParameter(dbCommand, "VideoId", DbType.Int32, VideoId);

                dsMessageDetails = db.ExecuteDataSet(dbCommand);

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "GetUserListForVideoEmail_WebSite", Ex.Message.ToString(), "BrokerWebDB_GetUserListForVideoEmail()", "");
                //dsBrokerDetails
            }
            return dsMessageDetails;
        }

        //30April18
        public static int UpdateBrokerExperienceDetails(int UserId, string ExpComp, string ExpDesig, string ExpFrom, string ExpTo, string ExpLogoPath)
        {
            BrokerDBEntities DB = new BrokerDBEntities();
            int user = 0;
            try
            {
                var @cmdText = "exec uspUpdateBrokerCompanyDetailsWeb @UserId,@ExpComp,@ExpDesig,@ExpFrom,@ExpTo,@ExpLogoPath";
                var @params = new[]{
                               new SqlParameter("@UserId", UserId),
                               new SqlParameter("@ExpComp", ExpComp),
                               new SqlParameter("@ExpDesig", ExpDesig),
                               new SqlParameter("@ExpFrom", ExpFrom),
                               new SqlParameter("@ExpTo", ExpTo),
                               new SqlParameter("@ExpLogoPath", ExpLogoPath)
                              };
                user = DB.Database.ExecuteSqlCommand(@cmdText, @params);
            }
            catch (Exception ex)
            {
                BrokerUtility.ErrorLog(UserId, "UpdateBrokerExperienceDetails_Website", ex.Message.ToString(), "BrokerWebDB.cs_UpdateBrokerExperienceDetails()", "0");
            }
            return user;
        }

        /****************************** 02Nov18 given by Umakant for Admin page **********************/

        public static List<UserList> validateAdminGetUsers(string username, string password)
        {
            List<UserList> dsUsers = null;
            try
            {
                BrokerDBEntities db = new BrokerDBEntities();
               
                var @cmdText = "exec uspValidateAdminGetUsers @Username,@Password";
                var @params = new[]{
                                       new SqlParameter("Username",username),
                                       new SqlParameter("Password",password)                                       
                                  };
                db.Database.CommandTimeout = 0;
                dsUsers = db.Database.SqlQuery<UserList>(@cmdText, @params).ToList<UserList>();

            }
            catch (Exception Ex)
            {
                 BrokerUtility.ErrorLog(0, "validateAdminGetUsers_Website", Ex.Message.ToString(), "BrokerWebDB_validateAdminGetUsers()", "");
                
            }
            return dsUsers;

        }

        public static List<UserList> getFilteredUsers(string searchBy, string searchText)
        {
            List<UserList> dsUsers = null;

            try
            {
                BrokerDBEntities db = new BrokerDBEntities();// DatabaseFactory.CreateDatabase();
                
                var @cmdText = "exec uspGetFilteredUsers @searchBy,@searchText";
                var @params = new[]{
                                       new SqlParameter("searchBy",searchBy),
                                       new SqlParameter("searchText",searchText)
                                       
                                  };
                db.Database.CommandTimeout = 0;
                dsUsers = db.Database.SqlQuery<UserList>(@cmdText, @params).ToList<UserList>();

            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "getFilteredUsers_Website", Ex.Message.ToString(), "BrokerWebDB_getFilteredUsers()", "");
                
            }
            return dsUsers;
        }

        public static User getAdminInfo(string username, string password)
        {
            BrokerDBEntities db = new BrokerDBEntities();
            return db.Users.SingleOrDefault((u => u.EmailId == username && u.Password == password));

        }

        public static List<UserList> getUserList()
        {
            List<UserList> dsUsers = null;

            try
            {
                BrokerDBEntities db = new BrokerDBEntities();
                var @cmdText = "exec uspGetUserList";
                db.Database.CommandTimeout = 0;
                dsUsers = db.Database.SqlQuery<UserList>(@cmdText).ToList<UserList>();
                
            }
            catch (Exception Ex)
            {
                BrokerUtility.ErrorLog(0, "getUserList_Website", Ex.Message.ToString(), "BrokerWebDB_getUserList()", "");
                
            }
            return dsUsers;

        }
    }
}