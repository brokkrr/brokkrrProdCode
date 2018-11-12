using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using BrokerMVC.Models;
using System.IO;
//using BrokerMVC.BrokerService;


namespace BrokerMVC.BrokerService
{
    /// <summary>
    /// Summary description for BrokerService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class BrokerService : System.Web.Services.WebService
    {
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod] //Web Method for Cross Platform Application.
        public void BrokerMainForAndroid()
        {
            //Flag for browser and application testing
            string WSFlag = ConfigurationManager.AppSettings["BrokerWSFlag"].ToString();
            string ActionName = "";

            //NSDictionary *json = [NSJSONSerialization JSONObjectWithData:responseObject options:0 error:nil];

            NameValueCollection form = new NameValueCollection();

            form = HttpContext.Current.Request.Form;

            if (WSFlag == "Application")
            {
                ActionName = HttpContext.Current.Request.Form["ActionName"].ToString();
                //ActionName = HttpContext.Current.Request.QueryString["ActionName"].ToString();
            }
            else
            {
                ActionName = HttpContext.Current.Request.QueryString["ActionName"].ToString();
            }

            switch (ActionName)
            {
                case "DoCheckUserExist":
                    BrokerWSDB.DoCheckUserExist(WSFlag);
                    break;

                case "DoGetUsersList":
                    BrokerWSDB.DoGetUsersList(WSFlag);
                    break;

                case "DoGetAllChatMessages":
                    BrokerWSDB.DoGetAllChatMessages(WSFlag);
                    break;

                case "DoLogin"://
                    BrokerWSDB.DoLogin(WSFlag);
                    break;
                case "DoExternalLogin"://
                    BrokerWSDB.DoExternalLogin(WSFlag);
                    break;
                case "DoRegularSignUp"://
                    BrokerWSDB.DoRegularSignUp(WSFlag);
                    break;
                case "DoVerifyEmailId":
                    BrokerWSDB.DoVerifyEmailID(WSFlag);
                    break;
                case "DoUpdateProfileCustomer"://For Cross Platform Application.
                    BrokerWSDB.DoUpdateProfileCustomerForAndroid(WSFlag);
                    break;
                case "DoUpdateProfileBroker"://For Cross Platform Application.
                    BrokerWSDB.DoUpdateProfileBrokerForAndroid(WSFlag);
                    break;
                case "DoViewProfileCustomer"://Currently not in use
                    BrokerWSDB.DoViewProfileCustomer(WSFlag);
                    break;
                case "DoViewProfileBroker"://Currently not in use
                    BrokerWSDB.DoViewProfileBroker(WSFlag);
                    break;
                case "DoSaveAutoInsuranceDetails":
                    BrokerWSDB.DoSaveAutoInsuranceDetails(WSFlag);
                    break;
                case "DoSaveBusinessInsuranceDetails":
                    BrokerWSDB.DoSaveBusinessInsuranceDetails(WSFlag);
                    break;
                case "DoSaveBenefitInsuranceDetails":
                    BrokerWSDB.DoSaveBenefitInsuranceDetails(WSFlag);
                    break;
                case "DoSaveLifeInsuranceDetails":
                    BrokerWSDB.DoSaveLifeInsuranceDetails(WSFlag);
                    break;
                case "DoSaveHomeInsuranceDetails":
                    BrokerWSDB.DoSaveHomeInsuranceDetails(WSFlag);
                    break;
                case "DoGetBrokerAvailabilityStatus":
                    BrokerWSDB.DoGetBrokerAvailabilityStatus(WSFlag);
                    break;
                case "DoSetBrokerAvailabilityStatus":
                    BrokerWSDB.DoSetBrokerAvailabilityStatus(WSFlag);
                    break;
                case "DoContactBroker":
                    BrokerWSDB.DoContactBroker(WSFlag);
                    break;
                case "DoGetMessages":
                    BrokerWSDB.DoGetMessages(WSFlag);
                    break;
                case "DoSaveBrokerChat":
                    BrokerWSDB.DoSaveBrokerChat(WSFlag);
                    break;
                case "DoSaveCustomerChat":
                    BrokerWSDB.DoSaveCustomerChat(WSFlag);
                    break;
                case "DoGetChatMessages":
                    BrokerWSDB.DoGetChatMessages(WSFlag);
                    break;
                case "DoForgetPassword":
                    BrokerWSDB.DoForgetPassword(WSFlag);
                    break;
                case "DoResetPassword":
                    BrokerWSDB.DoResetPassword(WSFlag);
                    break;
                case "DoSaveImage":
                    BrokerWSDB.DoSaveImage(WSFlag);
                    break;
                case "DoSetIsRead":
                    BrokerWSDB.DoSetIsRead(WSFlag);
                    break;
                case "DoGetCurrentTimeSpan":
                    BrokerWSDB.DoGetCurrentTimeSpan(WSFlag);
                    break;
                case "DoGetChatMessagesByMessageId":
                    BrokerWSDB.DoGetChatMessagesByMessageId(WSFlag);
                    break;

                case "DoGetUnreadChatMessages":
                    BrokerWSDB.DoGetUnreadChatMessages(WSFlag);
                    break;
                case "DoGetCompanyMaster":
                    BrokerWSDB.DoGetCompanyMaster(WSFlag);
                    break;

                case "DoDeleteMessage":
                    BrokerWSDB.DoDeleteMessage(WSFlag);
                    break;

                case "DoDeleteMultipleMessage":
                    BrokerWSDB.DoDeleteMultipleMessage(WSFlag);
                    break;

                case "DoSetBrokerAvailabilityWithZipCode"://not in use
                    BrokerWSDB.DoSetBrokerAvailabilityWithZipCode(WSFlag);
                    break;

                case "DoSetDeviceId":
                    BrokerWSDB.DoSetDeviceId(WSFlag);
                    break;

                case "DoGetDeviceId":
                    BrokerWSDB.DoGetDeviceId(WSFlag);
                    break;

                case "DoClearDeviceId":
                    BrokerWSDB.DoClearDeviceId(WSFlag);
                    break;

                case "DoSendNotification":
                    BrokerWSDB.DoSendNotification(WSFlag);
                    break;

                //case "DoPushNotification":
                //    BrokerWSDB.DoPushNotification(WSFlag);
                //    break;

                case "DoDeleteMultipleChatMessage":
                    BrokerWSDB.DoDeleteMultipleChatMessage(WSFlag);
                    break;

                //case "DoPushNotificationForiOS":
                //    BrokerWSDB.DoPushNotificationForiOS(WSFlag);
                //    break;

                case "DoGetUnreadMsgCount":
                    BrokerWSDB.DoGetUnreadMsgCount(WSFlag);
                    break;

                case "DoGetIndustryMaster":
                    BrokerWSDB.DoGetIndustryMaster(WSFlag);
                    break;

                case "DoGetSubIndustryMaster":
                    BrokerWSDB.DoGetSubIndustryMaster(WSFlag);
                    break;

                case "DoSampleAction":
                    BrokerWSDB.DoSampleAction(WSFlag);
                    break;

                case "DoGetMainMessage":
                    BrokerWSDB.DoGetMainMessageofCustomerForBroker(WSFlag);
                    break;

                //For Meineke insurance Company.

                case "DoSaveCommercialAutoInsuranceDetails":
                    BrokerWSDB.DoSaveCommercialAutoInsuranceDetails(WSFlag);
                    break;

                case "DoSaveWorkersCompensationDetails":
                    BrokerWSDB.DoSaveWorkersCompensationDetails(WSFlag);
                    break;

                case "DoSaveLiabilityInsuranceDetails":
                    BrokerWSDB.DoSaveLiabilityInsuranceDetails(WSFlag);//Check this action 17May18
                    break;

                case "DoSaveMeinekeBenefitInsuranceDetails":
                    BrokerWSDB.DoSaveMeinekeBenefitInsuranceDetails(WSFlag);
                    break;

                case "DoUpdateProfileCustomerForMeineke"://For Cross Platform Application.
                    BrokerWSDB.DoUpdateProfileCustomerForMeineke(WSFlag);
                    break;

                case "DoSave401kInsuranceDetails":
                    BrokerWSDB.DoSave401kInsuranceDetails(WSFlag);
                    break;

                case "DoSaveLiabilityInsuranceDetailsAPSP":
                    BrokerWSDB.DoSaveLiabilityInsuranceDetailsAPSP(WSFlag);
                    break;

                case "DoGetVideoList":
                    BrokerWSDB.DoGetVideoList(WSFlag);
                    break;

                case "DoGetUnWatchedVideoCount":
                    BrokerWSDB.DoGetUnWatchedVideoCount(WSFlag);
                    break;

                case "DoSetVideoWatched":
                    BrokerWSDB.DoSetVideoWatched(WSFlag);
                    break;

                case "DoSetAllVideoWatchedForWeb":
                    BrokerWSDB.DoSetAllVideoWatchedForWeb(WSFlag);
                    break;               
                    
                case "DoGetDocBase64FromMessageId":
                    BrokerWSDB.DoGetDocBase64FromMessageId(WSFlag);
                    break;

             
            }

        }

        [WebMethod]
        public void ImageConvert()
        {
            string strCountry = "US";
            //tsImage oImage = new tsImage();
            string baseImageLocation = "";
            string[] strvalue = new string[200];
            try
            {

                HttpFileCollection uploads = HttpContext.Current.Request.Files;
                strvalue = HttpContext.Current.Request.Params.GetValues("value1");
                strCountry = strvalue[0].ToString();
                //HttpContext.Current.Session["countryCode"] = strCountry;

                if (strCountry.ToUpper() == "IN")
                {
                    baseImageLocation = Server.MapPath("~/Team_Image_India/");
                }
                else
                {
                    baseImageLocation = Server.MapPath("~/Team_Image_US/");
                    strCountry = "US";
                }
                HttpPostedFile file = uploads["file"];

                string fileExt = Path.GetExtension(file.FileName).ToLower();
                string fileName = Path.GetFileName(file.FileName);

                //string[] arrFName = fileName.Split('_');
                //string fname = arrFName[arrFName.Length - 1].ToString();

                if (fileName != "")
                {
                    if (fileExt == ".jpg" || fileExt == ".jpeg" || fileExt == ".gif" || fileExt == ".png")
                        if (File.Exists(baseImageLocation + fileName))
                        {
                            if (file.ContentLength >= 1048576)
                            {
                                //oImage.res = "failed";
                                throw new Exception();

                            }
                            else
                            {
                                string strDate = System.DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss").Replace(":", "_").Replace(" ", "_");
                                //Thread.Sleep(2000);
                                File.Move(baseImageLocation + fileName, Server.MapPath("~/DeletedImages/") + strDate + "_" + fileName);

                                file.SaveAs(baseImageLocation + fileName);
                                //File.Replace(baseImageLocation + fileName, fileName, baseImageLocation + "bkp_" + fileName);
                                //byte[] a = File.ReadAllBytes(baseImageLocation+fileName);
                                //File.WriteAllBytes(baseImageLocation + fileName,a);
                                string[] arrFName = fileName.Split('.');
                                string id = arrFName[0].ToString();
                                //oImage.id = Convert.ToInt32(id);
                                //oImage.fname = fileName;
                                //bool res = tsData.saveImageinDB(oImage, strCountry);
                                //oImage.res = "Already exists";

                            }

                        }
                        else
                        {

                            if (file.ContentLength >= 1048576)
                            {
                                //oImage.res = "failed";
                                throw new Exception();

                            }
                            else
                            {
                                file.SaveAs(baseImageLocation + fileName);

                                File.SetAttributes(baseImageLocation + fileName, FileAttributes.Normal);
                                string strImagepath = baseImageLocation + fileName;

                                System.Drawing.Image oimage = System.Drawing.Image.FromFile(strImagepath);
                                string[] arrFName = fileName.Split('.');
                                string id = arrFName[0].ToString();
                                //oImage.id = Convert.ToInt32(id);
                                //oImage.fname = fileName;
                                //bool res = tsData.saveImageinDB(oImage, strCountry);
                                bool res = false;
                                if (res == true)
                                {
                                    // oImage.res = "Success";
                                }
                                else
                                {
                                    //oImage.res = "failed";
                                }

                            }




                        }

                }
                //Thread.Sleep(5000);
                GC.Collect();

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

    }
}
