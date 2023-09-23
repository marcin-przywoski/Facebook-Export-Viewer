using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ExportViewer.Core.Models.JSON
{
    public class Reaction
    {
        [JsonPropertyName("reaction")]
        public string ReactionType { get; set; }

        [JsonPropertyName("actor")]
        public string Actor { get; set; }
    }
}
