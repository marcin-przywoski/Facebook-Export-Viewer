using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using ExportViewer.Core.Models.Interfaces;

namespace ExportViewer.Core.Models.JSON
{
    public class Video : IMessage
    {
        [JsonPropertyName("uri")]
        public string Link { get; set; }

        [JsonConverter(typeof(MilisecondEpochConverter))]
        [JsonPropertyName("creation_timestamp")]
        public DateTime Date { get; set; }
    }
}
