using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AsteraRESTSamples
{
    public class Token
    {   
        [JsonPropertyName("access_token")]
        public string BearerToken { get; set; }
    }
}
