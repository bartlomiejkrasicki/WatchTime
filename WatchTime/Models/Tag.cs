using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WatchTime.Models
{
    public class Tag
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Category")]
        public string Category { get; set; }
    }
}