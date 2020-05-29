using CypherNet.Graph;
using Neo4jClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchTime.Core;
using WatchTime.Models;
using WatchTime.ViewModels;

namespace WatchTime.Controllers
{
    public class SeriesController : Controller
    {
        private WatchTimeQueries WatchTimeQueries;

        public SeriesController()
        {
            WatchTimeQueries = new WatchTimeQueries();
        }

        public ActionResult Index()
        {
            if (Session["login"] == null)
            {
                Response.Redirect("~/");
                return View();
            }
            else
            { 
                var seriesViewModel = new SeriesViewModel
                {
                    Series = WatchTimeQueries.GetSeries(),
                    ToWatch = WatchTimeQueries.GetToWatchSeries(Session["login"].ToString()),
                    Watched = WatchTimeQueries.GetWatchedSeries(Session["login"].ToString()),
                    Suggested = WatchTimeQueries.GetSuggestedSeries(Session["login"].ToString(), 5)
                };
                return View(seriesViewModel);
            }
        }

        [HttpGet]
        public void UpdateWatchedSeries(int seriesId, bool createRelation)
        {
            WatchTimeQueries.AddWatchedSeries(seriesId, Session["login"].ToString(), createRelation);
            Response.Redirect("/Series");
        }

        [HttpGet]
        public void UpdateToWatchSeries(int seriesId, bool createRelation)
        {
            WatchTimeQueries.AddToWatchSeries(seriesId, Session["login"].ToString(), createRelation);
            Response.Redirect("/Series");
        }

    }
}