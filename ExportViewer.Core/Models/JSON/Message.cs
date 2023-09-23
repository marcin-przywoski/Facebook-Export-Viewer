using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ExportViewer.Core.Models.JSON
{
    public class Message
    {
        [JsonConverter(typeof(MilisecondEpochConverter))]
        [JsonPropertyName("timestamp_ms")]
        public DateTime Date { get; set; }

        public string Link { get; set; }


        [JsonPropertyName("sender_name")]
        public string SenderName { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("is_unsent")]
        public bool IsUnsent { get; set; }

        [JsonPropertyName("is_taken_down")]
        public bool IsTakenDown { get; set; }

        [JsonPropertyName("bumped_message_metadata")]
        public BumpedMessageMetadata BumpedMessageMeta { get; set; }

        [JsonPropertyName("gifs")]
        public List<Gif> Gifs { get; set; }

        [JsonPropertyName("reactions")]
        public List<Reaction> Reactions { get; set; }

        [JsonPropertyName("photos")]
        public List<Photo> Photos { get; set; }

        [JsonPropertyName("videos")]
        public List<Video> Videos { get; set; }
    }
}
