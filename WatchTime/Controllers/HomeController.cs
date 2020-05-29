using CypherNet.Graph;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchTime.Core;
using WatchTime.Models;

namespace WatchTime.Controllers
{
    public class HomeController : Controller
    {
        private WatchTimeQueries WatchTimeQueries;

        public HomeController()
        {
            WatchTimeQueries = new WatchTimeQueries();
        }

        public ActionResult Index()
        {
            if (Session["email"] != null)
            {
                Response.Redirect("/Series");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string email, string password)
        {
            var user = WatchTimeQueries.Login(email, password);
            if (user != null)
            {
                Session["email"] = user.Email;
                Session["login"] = user.Login;
            }
            return View();
        }

    }
}