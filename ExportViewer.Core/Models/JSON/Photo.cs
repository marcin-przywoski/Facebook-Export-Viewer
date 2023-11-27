using System;
using System.Collections.Generic;
using System.Text;
using ExportViewer.Core.Models.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExportViewer.Core.Models.JSON
{
    public class Photo : IMessage
    {
        [JsonPropertyName("uri")]
        public string Link { get; set; }

        [JsonConverter(typeof(MilisecondEpochConverter))]
        [JsonPropertyName("creation_timestamp")]
        public DateTime Date { get; set; }
    }
}
