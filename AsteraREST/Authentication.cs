using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AsteraRESTSamples
{
    public class Authentication
    {   
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
