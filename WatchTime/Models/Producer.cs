using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WatchTime.Models
{
    public class Producer
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("YearOfEstablishment")]
        public int YearOfEstablishment { get; set; }
    }
}