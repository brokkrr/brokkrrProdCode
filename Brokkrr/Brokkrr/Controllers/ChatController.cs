using BrokerMVC.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrokerMVC.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat

        [Authorize]
        [HttpGet]
        public ActionResult BrokerChat(string BrokerMessageId, string CustomerMessageId, string BrokerId, string CustomerId, string CustomerName)
        {
            DataSet dsMainMsgDetails = null;
            string MainMessage = "", IsRead = "", LocalDateTime = "";

            string this_BrokerMessageId = BrokerMessageId;
            string this_CustomerMessageId = CustomerMessageId;
            string this_BrokerId = BrokerId;
            string this_CustomerId = CustomerId;
            string this_CustomerName = CustomerName;

            this_BrokerMessageId = BrokerUtility.DecryptURL(this_BrokerMessageId);
            this_CustomerMessageId = BrokerUtility.DecryptURL(this_CustomerMessageId);
            this_BrokerId = BrokerUtility.DecryptURL(this_BrokerId);
            this_CustomerId = BrokerUtility.DecryptURL(this_CustomerId);
            this_CustomerName = BrokerUtility.DecryptURL(this_CustomerName);

            ViewBag.BrokerMessageId = this_BrokerMessageId;
            ViewBag.CustomerMessageId = this_CustomerMessageId;
            ViewBag.BrokerId = this_BrokerId;
            ViewBag.CustomerId = this_CustomerId;
            ViewBag.CustomerName = this_CustomerName;
            ViewBag.FirstName = Session["FirstName"].ToString();
            ViewBag.LastName = Session["LastName"].ToString();

            dsMainMsgDetails = BrokerWebDB.BrokerWebDB.GetMainMessage(this_BrokerId, this_CustomerId, this_BrokerMessageId, this_CustomerMessageId, "BrokerMessages");

            if (dsMainMsgDetails.Tables.Count > 0)
                {
                    if (dsMainMsgDetails.Tables[0].Rows.Count > 0)
                    {
                        MainMessage = dsMainMsgDetails.Tables[0].Rows[0][0].ToString();
                        IsRead = dsMainMsgDetails.Tables[0].Rows[0]["IsRead"].ToString();
                        LocalDateTime = dsMainMsgDetails.Tables[0].Rows[0]["LocalDateTime"].ToString();
                    }
                }

            ViewBag.MainMessage = MainMessage;
            ViewBag.IsRead = IsRead;
            ViewBag.LocalDateTime = LocalDateTime;

            if (Session["WebServiceURL"] != null)
            {
                ViewBag.WebServiceURL = Session["WebServiceURL"].ToString();
            }

            return View();
        }


        [Authorize]
        [HttpGet]
        public ActionResult CustomerChat(string BrokerMessageId, string CustomerMessageId, string BrokerId, string CustomerId, string CustomerName)
        {

            DataSet dsMainMsgDetails = null;
            string MainMessage = "", IsRead = "", LocalDateTime = "";

            string this_BrokerMessageId = BrokerMessageId;
            string this_CustomerMessageId = CustomerMessageId;
            string this_BrokerId = BrokerId;
            string this_CustomerId = CustomerId;
            string this_CustomerName = CustomerName;

            this_BrokerMessageId = BrokerUtility.DecryptURL(this_BrokerMessageId);
            this_CustomerMessageId = BrokerUtility.DecryptURL(this_CustomerMessageId);
            this_BrokerId = BrokerUtility.DecryptURL(this_BrokerId);
            this_CustomerId = BrokerUtility.DecryptURL(this_CustomerId);
            this_CustomerName = BrokerUtility.DecryptURL(this_CustomerName);

            ViewBag.BrokerMessageId = this_BrokerMessageId;
            ViewBag.CustomerMessageId = this_CustomerMessageId;
            ViewBag.BrokerId = this_BrokerId;
            ViewBag.CustomerId = this_CustomerId;
            ViewBag.BrokerName = this_CustomerName;

            ViewBag.FirstName = Session["FirstName"].ToString();
            ViewBag.LastName = Session["LastName"].ToString();

            dsMainMsgDetails = BrokerWebDB.BrokerWebDB.GetMainMessage(this_BrokerId, this_CustomerId, this_BrokerMessageId, this_CustomerMessageId, "CustomerMessages");

            if (dsMainMsgDetails.Tables.Count > 0)
            {
                if (dsMainMsgDetails.Tables[0].Rows.Count > 0)
                {
                    MainMessage = dsMainMsgDetails.Tables[0].Rows[0][0].ToString();
                    IsRead = dsMainMsgDetails.Tables[0].Rows[0]["IsRead"].ToString();
                    LocalDateTime = dsMainMsgDetails.Tables[0].Rows[0]["LocalDateTime"].ToString();
                }
            }

            ViewBag.MainMessage = MainMessage;
            ViewBag.IsRead = IsRead;
            ViewBag.LocalDateTime = LocalDateTime;

            if (Session["WebServiceURL"] != null)
            {
                ViewBag.WebServiceURL = Session["WebServiceURL"].ToString();
            }

            return View();
        }


        public JsonResult EncryptBrokerData( string BrokerMessageId,string CustomerMessageId,string BrokerId,string CustomerId,string CustomerName)
        {
            string this_BrokerMessageId = BrokerMessageId;
            string this_CustomerMessageId = CustomerMessageId;
            string this_BrokerId = BrokerId;
            string this_CustomerId = CustomerId;
            string this_CustomerName = CustomerName;

            this_BrokerMessageId = BrokerUtility.EncryptURL(this_BrokerMessageId);
            this_CustomerMessageId = BrokerUtility.EncryptURL(this_CustomerMessageId);
            this_BrokerId = BrokerUtility.EncryptURL(this_BrokerId);
            this_CustomerId = BrokerUtility.EncryptURL(this_CustomerId);
            this_CustomerName = BrokerUtility.EncryptURL(this_CustomerName);
            
            
            return Json(new { BrokerMessageId = this_BrokerMessageId, CustomerMessageId = this_CustomerMessageId, BrokerId = this_BrokerId, CustomerId = this_CustomerId, CustomerName = this_CustomerName }, JsonRequestBehavior.AllowGet);
        }
    }
}