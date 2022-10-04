using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace _01_basic_ping_bot
{
    public class ConfigToken
    {
        [JsonProperty("auth_token")]
        public string Token { get; set; }
    }
}
