using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;

namespace ExportViewer.Core.Models.JSON
{
    [ExcludeFromCodeCoverage]
    public class Participant
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
