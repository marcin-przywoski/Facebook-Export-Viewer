using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExportViewer.Core.Models.JSON
{

    public class Conversation
    {
        [JsonPropertyName("participants")]
        public List<Participant> Participants { get; set; }

        [JsonPropertyName("messages")]
        public List<ConversationItem> Messages { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("is_still_participant")]
        public bool IsStillParticipant { get; set; }

        [JsonPropertyName("thread_type")]
        public string ThreadType { get; set; }

        [JsonPropertyName("thread_path")]
        public string ThreadPath { get; set; }

        [JsonPropertyName("magic_words")]
        public List<object> MagicWords { get; set; }

    }

    public class MilisecondEpochConverter : JsonConverter<DateTime>
    {
        private static readonly DateTime _epoch = new DateTime(1970 , 1 , 1 , 0 , 0 , 0 , DateTimeKind.Local);

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        long milliseconds = (long)(value - _epoch).TotalMilliseconds;
        writer.WriteNumberValue(milliseconds);
    }

        public override DateTime Read (ref Utf8JsonReader reader , Type typeToConvert , JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException($"Unexpected token type: {reader.TokenType}");
            }

            long milliseconds = reader.GetInt64();
            return _epoch.AddMilliseconds(milliseconds);
        }
    }
}
