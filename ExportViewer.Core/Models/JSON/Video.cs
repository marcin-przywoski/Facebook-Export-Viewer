using System;
using System.Collections.Generic;
using System.Text;
using ExportViewer.Core.Models.Interfaces;
using Newtonsoft.Json;

namespace ExportViewer.Core.Models.JSON
{
    public class Video : IMessage
    {
        [JsonProperty(PropertyName = "uri")]
        public string Link { get; set; }

        [JsonConverter(typeof(MilisecondEpochConverter))]
        [JsonProperty(PropertyName = "creation_timestamp")]
        public DateTime Date { get; set; }
    }
}
