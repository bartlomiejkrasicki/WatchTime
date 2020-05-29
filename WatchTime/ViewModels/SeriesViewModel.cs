using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WatchTime.Models;

namespace WatchTime.ViewModels
{
    public class SeriesViewModel
    {
        public List<Series> Suggested { set; get; }
        public List<Series> Watched { set; get; }
        public List<Series> ToWatch { set; get; }
        public List<Series> Series { set; get; }
    }
}