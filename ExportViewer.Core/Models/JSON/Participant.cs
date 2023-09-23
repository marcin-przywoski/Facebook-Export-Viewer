using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ExportViewer.Core.Models.JSON
{
    public class Participant
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
