using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchTime.Core;

namespace WatchTime.Controllers
{
    public class TagController : Controller
    {

        public WatchTimeQueries WatchTimeQueries;

        public TagController()
        {
            WatchTimeQueries = new WatchTimeQueries();
        }

        // GET: Tag
        public ActionResult Index()
        {
            if (Session["email"] == null)
            {
                Response.Redirect("~/");
                return View();
            }
            else
            {
                var tags = WatchTimeQueries.GetTags();
                return View(tags);
            }
        }

        public ActionResult Details(int id)
        {
            var series = WatchTimeQueries.GetMostPopularSeriesByTag(Session["login"].ToString(), id, 5);
            return View(series);
        }
    }
}