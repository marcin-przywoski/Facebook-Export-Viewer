using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ExportViewer.Core.Models.JSON
{
    public class Reaction
    {
        [JsonProperty(PropertyName = "reaction")]
        public string ReactionType { get; set; }

        [JsonProperty(PropertyName = "actor")]
        public string Actor { get; set; }
    }
}
