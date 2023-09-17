using System;
using System.Collections.Generic;
using System.Text;
using ExportViewer.Core.Models.Interfaces;
using Newtonsoft.Json;

namespace ExportViewer.Core.Models.JSON
{
    public class Gif : IMessage
    {
        [JsonProperty(PropertyName = "uri")]
        public string Link { get; set; }
        public DateTime Date { get; set; }

    }
}
