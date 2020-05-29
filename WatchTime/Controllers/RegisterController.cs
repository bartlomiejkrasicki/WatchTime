using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchTime.Core;
using WatchTime.Models;

namespace WatchTime.Controllers
{
    public class RegisterController : Controller
    {

        private WatchTimeQueries WatchTimeQueries;

        public RegisterController()
        {
            WatchTimeQueries = new WatchTimeQueries();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string login, string email, string password)
        {
            if(!WatchTimeQueries.IsUserExists(login, email))
            {
                WatchTimeQueries.Register(login, email, password);
            }
            return View();
        }

    }
}