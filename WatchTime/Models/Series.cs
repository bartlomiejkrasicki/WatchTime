using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WatchTime.Models
{
    public class Series
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("SeasonsNumber")]
        public int SeasonsNumber { get; set; }
        [JsonProperty("YearOfProduction")]
        public int YearOfProduction { get; set; }
        [JsonProperty("Time")]
        public TimeSpan Time { get; set; }
    }
}