using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrokerMVC.Controllers
{
    public class MessageController : Controller
    {


        // GET: Message
        [Authorize]
        [HttpGet]
        public ActionResult BrokerMessage()
        {
            ViewBag.Company = Session["Company"].ToString();
            ViewBag.User = "Authorize";
            ViewBag.UserId = Session["UserId"].ToString();
            ViewBag.Initials = Session["Initials"];
            ViewBag.UserName = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();
            return View();
        }

        // GET: Message
        [Authorize]
        [HttpGet]
        public ActionResult CustomerMessage()
        {
            ViewBag.Company = Session["Company"].ToString();
            ViewBag.User = "Authorize";
            ViewBag.UserId = Session["UserId"].ToString();
            ViewBag.Initials = Session["Initials"];
            ViewBag.UserName = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();

            //if (Session["ProfilePic"] != null)
            //{
            //    ViewBag.ProfilePic = Session["ProfilePic"].ToString();
            //}

            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult CustomerMessageStatic()
        {
            ViewBag.Initials = Session["FirstName"].ToString()[0] + "" + Session["LastName"].ToString()[0];
            ViewBag.UserName = Session["FirstName"].ToString() + " " + Session["LastName"].ToString();

            //if (Session["ProfilePic"] != null)
            //{
            //    ViewBag.ProfilePic = Session["ProfilePic"].ToString();
            //}

            return View();
        }


       
    }
}