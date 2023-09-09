using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportViewer.Core.Models.Interfaces;
using ExportViewer.Core.Models.JSON;
using ExportViewer.Core.Services.Interfaces;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace ExportViewer.Core.Services
{
    public class JsonParsingService : IJsonParsingService
    {
        public async Task<IEnumerable<IMessage>> GetMessages(string filePath, CultureInfo locale, string exportLocation)
        {
            var conversation = JsonConvert.DeserializeObject<Conversation>(await File.ReadAllTextAsync(filePath));

            conversation.Messages.RemoveAll(s => s.Photos is null && s.Gifs is null && s.Videos is null);

            var messages = new List<IMessage>();
            foreach ( var message in conversation.Messages )
            {
                if(message.Gifs != null)
                {
                    foreach ( var gif in message.Gifs )
                    {
                        if (File.Exists(exportLocation + gif.Link))
                        {
                            gif.Date = message.Date;

                            messages.Add(gif);
                        }


                    }
                }

                if (message.Photos != null)
                {
                    foreach (var photo in message.Photos)
                    {

                        if (File.Exists(exportLocation + photo.Link))
                        {
                            messages.Add(photo);
                        }
                    }
                }

                if (message.Videos != null)
                {
                    foreach (var video in message.Videos)
                    {
                        if (File.Exists(exportLocation + video.Link))
                        {
                            messages.Add(video);
                        }
                    }
                }
            }

            return messages.AsEnumerable();
        }
    }
}
