using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ExportViewer.Core.Models.JSON
{
    public class BumpedMessageMetadata
    {
        [JsonProperty(PropertyName = "bumped_message")]
        public string BumpedMessage { get; set; }

        [JsonProperty(PropertyName = "is_bumped")]
        public bool? IsBumped { get; set; }
    }
}
