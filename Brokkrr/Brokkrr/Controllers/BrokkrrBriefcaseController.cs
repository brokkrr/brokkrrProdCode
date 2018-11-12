using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrokerMVC.Models;
using System.Data.SqlTypes;
using System.Data;
using BrokerMVC.Models;
using System.Xml;
using Newtonsoft.Json;
using BrokerMVC.App_Code;
using System.Text.RegularExpressions;
using BrokerMVC.BrokerService;

namespace BrokerMVC.Controllers
{
    public class BrokkrrBriefcaseController : Controller
    {
        // GET: BrokkrrBriefcase
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                string Initials = "";

                ViewBag.UserName = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();
                ViewBag.User = "Authorize";
                ViewBag.Company = Session["Company"];

                if (Session["FirstName"].ToString() != "" && Session["LastName"].ToString() != "")
                {
                    Initials = Session["FirstName"].ToString()[0] + "" + Session["LastName"].ToString()[0];
                }
                else
                {
                    Initials = "N/A";
                }
                ViewBag.Initials = Initials.ToUpper();

                //if (Session["UsersForVideoEmail"] != null)
                //{
                //    Session["UsersForVideoEmail"] = "Yes";
                //}
                //else
                //{
                //    Session["UsersForVideoEmail"] = "No";
                //}


                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


        [Authorize]
        public JsonResult GetVideoData()
        {
            List<BrokkrrBriefcase> obj_list = null;
            if (Session["Company"] != null)
            {
                string company = Session["Company"].ToString();

                try
                {
                    obj_list = BrokerWebDB.BrokerWebDB.GetVideoList(Convert.ToInt32(Session["UserId"].ToString()));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return Json(new { result = obj_list }, JsonRequestBehavior.AllowGet);

        }


        [Authorize]
        [HttpPost]
        public ActionResult Index(BrokkrrBriefcase obj_briefcase, string[] chkCompany)
        {
            if (obj_briefcase.Actionsname == "Submit")
            {
                #region Save Video Details
                try
                {
                    //if (ModelState.IsValid)
                    //{
                    SqlDateTime? sqldatenull = null;
                    int uploadedby = 0;
                    string userid = "", url = "", title = "", description = "", assignedcompany = "",
                        fromdate = "", todate = "";
                    string[] strURL;
                    bool isdeleted = false;
                    ViewBag.UserId = Session["UserId"].ToString();
                    ViewBag.User = "Authorize";
                    userid = ViewBag.UserId;
                    if (userid != "")
                    {
                        if (obj_briefcase.Url != "")
                        {
                            url = obj_briefcase.Url;

                            if (!(url.Contains("https://youtube.com/embed")) && !(url.Contains("https://www.youtube.com/embed")))
                            {
                                strURL = url.Split('/');
                                if (strURL[3] != "")
                                {
                                    url = "https://youtube.com/embed/" + strURL[3];
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            url = "";
                        }
                        if (obj_briefcase.Title != "")
                        {
                            title = obj_briefcase.Title;
                        }
                        else
                        {
                            title = "";
                        }
                        if (obj_briefcase.Description != "" && obj_briefcase.Description != null)
                        {
                            description = obj_briefcase.Description;

                            Regex regex = new Regex(@"(\r\n|\r|\n)+");
                            description = regex.Replace(description, "<br />");
                        }
                        else
                        {
                            description = "";
                        }

                        if (chkCompany != null)
                        {
                            foreach (var Comp in chkCompany)
                            {
                                assignedcompany = assignedcompany + "," + Comp;
                            }
                            assignedcompany = assignedcompany.TrimStart(',');
                        }

                        if (obj_briefcase.DateFrom != null && obj_briefcase.DateFrom != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                        {
                            fromdate = Convert.ToDateTime(obj_briefcase.DateFrom).ToShortDateString();
                        }
                        else
                        {
                            fromdate = null;
                        }
                        if (obj_briefcase.DateTo != null && obj_briefcase.DateTo != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                        {
                            todate = Convert.ToDateTime(obj_briefcase.DateTo).ToShortDateString();
                        }
                        else
                        {
                            todate = null;
                        }
                        if (obj_briefcase.UploadedBy != null)
                        {
                            uploadedby = Convert.ToInt32(userid);
                        }
                    }

                    int val = 0;
                    val = BrokerWebDB.BrokerWebDB.saveVideoDetails(url, title, description, assignedcompany, fromdate, todate, uploadedby);

                    if (val > 0)
                    {
                        if (assignedcompany != "")
                        {
                            /*Insert data into table WatchedVideoHistory for selected company users and all brokers*/

                            BrokerWebDB.BrokerWebDB.SaveUserListInWatchedVideoHistory(assignedcompany, val);

                            DataSet ds = new DataSet();
                            ds = BrokerWebDB.BrokerWebDB.GetUserListForVideoNotification(assignedcompany);
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        string DeviceId = "", NewDevice = "", message = "", UId = "";

                                        DeviceId = ds.Tables[0].Rows[i]["DeviceId"].ToString();
                                        UId = ds.Tables[0].Rows[i]["UserId"].ToString();

                                        //BrokerUtility.ErrorLog(0, "1Index", "" + DeviceId + "," + UId, "BrokerBriefcaseController.cs_Index", "");

                                        int AdnroidDevice = DeviceId.IndexOf("Android");
                                        int IosDevice = DeviceId.IndexOf("iOS");

                                        if (AdnroidDevice >= 0)
                                        {
                                            NewDevice = DeviceId.Replace("Android", "");
                                            message = "New video - " + title + " added in your brokkrr briefcase";

                                            BrokerUtility.DoPushNotification(NewDevice, message, "", "1", UId);
                                        }
                                        else if (IosDevice >= 0)
                                        {
                                            NewDevice = DeviceId.Replace("iOS", "");
                                            message = "New video - " + title + " added in your brokkrr briefcase";

                                            BrokerUtility.DoPushNotificationForiOS(NewDevice, message, "", "1", UId);
                                        }
                                    }
                                }
                            }

                            DataSet dsUsers = new DataSet();
                            dsUsers = BrokerWebDB.BrokerWebDB.GetUserListForVideoEmail(assignedcompany, "Submit", 0);

                            DataTable dt = new DataTable();

                            dt.Columns.Add("Title");
                            dt.Columns.Add("Description");
                            dt.Columns.Add("Url");
                            //dt.Columns.Add("Description");

                            dt.Rows.Add(title, description, url);

                            dsUsers.Tables.Add(dt);
                            dsUsers.AcceptChanges();
                            Session["UsersForVideoEmail"] = dsUsers;

                        }
                    }

                    return RedirectToAction("Index", "BrokkrrBriefcase");

                    //}
                    //return RedirectToAction("Index", "BrokkrrBriefcase");
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                #endregion Save Video Details
            }

            if (obj_briefcase.Actionsname == "Update")
            {
                #region Update Video Details
                try
                {
                    BrokerDBEntities Db = new BrokerDBEntities();
                    //if (ModelState.IsValid)
                    //{

                    SqlDateTime? sqldatenull = null;
                    int uploadedby = 0;
                    int Id = 0;
                    string userid = "", url = "", title = "", description = "", assignedcompany = "", fromdate = "", todate = "", createdate = "", alreadyassignedcomp = "";
                    bool isdeleted = false;
                    ViewBag.UserId = Session["UserId"].ToString();
                    ViewBag.User = "Authorize";
                    userid = ViewBag.UserId;
                    if (userid != "")
                    {

                        if (obj_briefcase.Id.ToString() != "")
                        {
                            Id = obj_briefcase.Id;
                        }

                        if (obj_briefcase.Url != "")
                        {
                            url = obj_briefcase.Url;
                        }
                        else
                        {
                            url = "";
                        }
                        if (obj_briefcase.Title != "")
                        {
                            title = obj_briefcase.Title;
                        }
                        else
                        {
                            title = "";
                        }
                        if (obj_briefcase.Description != "" && obj_briefcase.Description != null)
                        {
                            description = obj_briefcase.Description;

                            Regex regex = new Regex(@"(\r\n|\r|\n)+");
                            description = regex.Replace(description, "<br />");

                        }
                        else
                        {
                            description = "";
                        }



                        if (chkCompany != null)
                        {
                            foreach (var Comp in chkCompany)
                            {
                                assignedcompany = assignedcompany + "," + Comp;
                            }
                            assignedcompany = assignedcompany.TrimStart(',');
                        }

                        if (obj_briefcase.DateFrom != null && obj_briefcase.DateFrom != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                        {
                            fromdate = Convert.ToDateTime(obj_briefcase.DateFrom).ToShortDateString();
                        }
                        else
                        {
                            fromdate = null;
                        }
                        if (obj_briefcase.DateTo != null && obj_briefcase.DateTo != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                        {
                            todate = Convert.ToDateTime(obj_briefcase.DateTo).ToShortDateString();
                        }
                        else
                        {
                            todate = null;
                        }
                        if (obj_briefcase.UploadedBy != null)
                        {
                            uploadedby = Convert.ToInt32(userid);
                        }
                    }

                    int val = 0;

                    var resultData = from data in Db.BrokerBriefcases
                                     where data.Id == Id
                                     select new { data.AssignedCompany };

                    foreach (var d in resultData)
                    {
                        alreadyassignedcomp = d.AssignedCompany;
                    }

                    val = BrokerWebDB.BrokerWebDB.UpdateVideoDetails(Id, url, title, description, assignedcompany, fromdate, todate, uploadedby);

                    if (val > 0)
                    {
                        string[] strsplit, strsplitAlreadyAssigned;
                        int round = 0;
                        if (assignedcompany != "")
                        {
                            /*Insert data into table WatchedVideoHistory for selected company users and all brokers*/

                            BrokerWebDB.BrokerWebDB.SaveUserListInWatchedVideoHistory(assignedcompany, Id);

                            if (alreadyassignedcomp != assignedcompany)
                            {
                                strsplit = assignedcompany.Split(',');

                                if (strsplit.Length > 0)
                                {
                                    for (int j = 0; j < strsplit.Length; j++)
                                    {
                                        if (!(alreadyassignedcomp.Contains(strsplit[j])))
                                        {
                                            string s = strsplit[j];

                                            DataSet ds = new DataSet();
                                            ds = BrokerWebDB.BrokerWebDB.GetUserListForVideoNotification(s);
                                            //if (round > 0)
                                            //{
                                            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                                            {
                                                DataRow dr = ds.Tables[0].Rows[k];
                                                string s1 = dr["UserType"].ToString();
                                                if (dr["UserType"].ToString() == "Broker")
                                                {
                                                    dr.Delete();
                                                    ds.AcceptChanges();
                                                    k--;
                                                }
                                            }

                                            //}

                                            if (ds.Tables.Count > 0)
                                            {
                                                if (ds.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                                    {
                                                        string DeviceId = "", NewDevice = "", message = "", UId = "";

                                                        DeviceId = ds.Tables[0].Rows[i]["DeviceId"].ToString();
                                                        UId = ds.Tables[0].Rows[i]["UserId"].ToString();

                                                        int AdnroidDevice = DeviceId.IndexOf("Android");
                                                        int IosDevice = DeviceId.IndexOf("iOS");

                                                        if (AdnroidDevice >= 0)
                                                        {
                                                            NewDevice = DeviceId.Replace("Android", "");
                                                            message = "New video - " + title + " added in your brokkrr briefcase";
                                                            BrokerUtility.DoPushNotification(NewDevice, message, "", "1", UId);
                                                        }
                                                        else if (IosDevice >= 0)
                                                        {
                                                            NewDevice = DeviceId.Replace("iOS", "");
                                                            message = "New video - " + title + " added in your brokkrr briefcase";
                                                            BrokerUtility.DoPushNotificationForiOS(NewDevice, message, "", "1", UId);
                                                        }
                                                    }
                                                }
                                            }

                                            DataSet dsUsers = new DataSet();
                                            dsUsers = BrokerWebDB.BrokerWebDB.GetUserListForVideoEmail(s, "Update", Id);

                                            DataTable dt = new DataTable();

                                            dt.Columns.Add("Title");
                                            dt.Columns.Add("Description");
                                            dt.Columns.Add("Url");
                                            //dt.Columns.Add("Description");

                                            dt.Rows.Add(title, description, url);

                                            dsUsers.Tables.Add(dt);
                                            dsUsers.AcceptChanges();
                                            Session["UsersForVideoEmail"] = dsUsers;
                                            //round++;
                                        }
                                        else
                                        {

                                        }
                                    }
                                }
                            }
                        }
                    }

                    return RedirectToAction("Index", "BrokkrrBriefcase");

                    //}
                    //return RedirectToAction("Index", "BrokkrrBriefcase");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                #endregion Update Video Details
            }

            return RedirectToAction("Index", "BrokkrrBriefcase");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Briefcase()
        {
            List<BrokkrrBriefcase> obj_list = null;
            if (Session["UserId"] != null)
            {
                string Initials = "";

                ViewBag.UserName = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();
                ViewBag.User = "Authorize";
                ViewBag.Company = Session["Company"];

                if (Session["FirstName"].ToString() != "" && Session["LastName"].ToString() != "")
                {
                    Initials = Session["FirstName"].ToString()[0] + "" + Session["LastName"].ToString()[0];
                }
                else
                {
                    Initials = "N/A";
                }
                ViewBag.Initials = Initials.ToUpper();

            }
            return View();


        }

        [Authorize]
        [HttpPost]
        public string DeleteVideo(Video objvideo)
        {

            try
            {

                if (Session["UserId"] != null)
                {
                    int vid_id = Convert.ToInt32(objvideo.VideoId.ToString());
                    if (vid_id != 0)
                    {
                        int val = 0;
                        val = BrokerWebDB.BrokerWebDB.DeleteVideoById(vid_id);

                    }
                    //return RedirectToAction("Index", "BrokkrrBriefcase");
                }
                //return RedirectToAction("Index", "BrokkrrBriefcase");
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return "success";
        }

        [Authorize]
        public JsonResult SendVideoEmail()
        {
            if (Session["UsersForVideoEmail"] != null)
            {
                try
                {
                    DataSet ds = (DataSet)Session["UsersForVideoEmail"];
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                bool flag = BrokerWSUtility.SendVideoLinkOnEmail(ds.Tables[0].Rows[i]["EmailId"].ToString(), ds.Tables[1].Rows[0]["Title"].ToString(), ds.Tables[1].Rows[0]["Url"].ToString(), ds.Tables[1].Rows[0]["Description"].ToString(), ds.Tables[0].Rows[i]["UserType"].ToString());

                                //For Testing
                                //if (ds.Tables[0].Rows[i]["UserType"].ToString() == "Broker")
                                //{
                                //    bool flag = BrokerWSUtility.SendVideoLinkOnEmail(ds.Tables[0].Rows[i]["EmailId"].ToString(), ds.Tables[1].Rows[0]["Title"].ToString(), ds.Tables[1].Rows[0]["Url"].ToString(), ds.Tables[1].Rows[0]["Description"].ToString(), ds.Tables[0].Rows[i]["UserType"].ToString());
                                //    break;
                                //}
                            }
                        }
                    }
                    Session["UsersForVideoEmail"] = null;
                }
                catch (Exception Ex)
                {
                    BrokerUtility.ErrorLog(0, "SendVideoEmail_WebSite", Ex.Message.ToString(), "BrokerBriefcaseController.cs_SendVideoEmail", "");
                }
            }
            return Json(new { result = "Success" }, JsonRequestBehavior.AllowGet);

        }

    }
}