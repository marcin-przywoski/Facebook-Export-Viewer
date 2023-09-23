using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ExportViewer.Core.Models.JSON
{
    public class BumpedMessageMetadata
    {
        [JsonPropertyName("bumped_message")]
        public string BumpedMessage { get; set; }

        [JsonPropertyName("is_bumped")]
        public bool? IsBumped { get; set; }
    }
}
