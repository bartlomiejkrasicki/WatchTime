using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WatchTime.Models
{
    public class User
    {
        [JsonProperty("Login")]
        public string Login { get; set; }
        [JsonProperty("Password")]
        public string Password { get; set; }
        [JsonProperty("Email")]
        public string Email { get; set; }
        [JsonProperty("Role")]
        public string Role { get; set; }
        [JsonProperty("ToWatchTime")]
        public long ToWatchTime { get; set; }
        [JsonProperty("WatchedTime")]
        public long WatchedTime { get; set; }
    }
}