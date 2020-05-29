using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchTime.Core;
using WatchTime.Models;

namespace WatchTime.Controllers
{
    public class ProducerController : Controller
    {

        private WatchTimeQueries WatchTimeQueries;

        public ProducerController()
        {
            WatchTimeQueries = new WatchTimeQueries();
        }

        public ActionResult Index()
        {
            if (Session["email"] == null)
            {
                Response.Redirect("~/");
                return View();
            }
            else
            {
                var producers = WatchTimeQueries.GetProducers();
                return View(producers);
            }
        }

        public ActionResult Details(int id)
        {
            var series = WatchTimeQueries.GetToWatchSeriesByProducer(id, 3);
            return View(series);
        }

    }
}