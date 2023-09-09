using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ExportViewer.Core.Models.JSON
{
    public class Participant
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
