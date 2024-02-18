using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportViewer.Core.Models.Interfaces;
using ExportViewer.Core.Models.Common;
using ExportViewer.Core.Models.JSON;
using ExportViewer.Core.Services.Interfaces;
using Microsoft.VisualBasic;
using System.Text.Json;

namespace ExportViewer.Core.Services
{
    public class JsonParsingService : IJsonParsingService
    {
        public async Task<IEnumerable<Message>> GetMessages(string filePath, CultureInfo locale, string exportLocation)
        {
            var conversation = JsonSerializer.Deserialize<Conversation>(await File.ReadAllTextAsync(filePath));

            conversation.Messages.RemoveAll(s => s.Photos is null && s.Gifs is null && s.Videos is null);

            var messages = new List<Message>();
            foreach ( var message in conversation.Messages )
            {
                if(message.Gifs != null)
                {
                    foreach ( var gif in message.Gifs )
                    {
                        if (File.Exists(exportLocation + gif.Link))
                        {
                            messages.Add(new Message { Date = message.Date, Link = gif.Link});
                        }


                    }
                }

                if (message.Photos != null)
                {
                    foreach (var photo in message.Photos)
                    {

                        if (File.Exists(exportLocation + photo.Link))
                        {
                            messages.Add(new Message { Date = message.Date , Link = photo.Link });
                        }
                    }
                }

                if (message.Videos != null)
                {
                    foreach (var video in message.Videos)
                    {
                        if (File.Exists(exportLocation + video.Link))
                        {
                            messages.Add(new Message { Date = message.Date, Link = video.Link });
                        }
                    }
                }
            }

            return messages.AsEnumerable();
        }
    }
}
