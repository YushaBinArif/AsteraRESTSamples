using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AsteraRESTSamples
{
    class Job
    {
        [JsonPropertyName("jobId")]
        public int JobID { get; set; }

        [JsonPropertyName("filePath")]
        public string FilePath { get; set; }
    }
}
