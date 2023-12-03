using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ExportViewer.Core.Models.Common;
using ExportViewer.Core.Models.Interfaces;
using ExportViewer.Core.Services.Interfaces;

namespace ExportViewer.Core.Services
{
    public class DateEmbeddingService : IDateEmbeddingService
    {
        public Task EmbeddDate(Message message, string exportLocation, string destinationPath, IProgress<string> progress)
        {
            progress.Report($"Processing {message.Link}");
            if(File.Exists(exportLocation + message.Link)) 
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath + message.Link));
                try
                {
                    File.Copy(exportLocation + message.Link, destinationPath + message.Link);
                }
                catch (Exception ex)
                {

                } 


                    File.SetCreationTime(destinationPath + message.Link, message.Date);
                    File.SetLastAccessTime(destinationPath + message.Link, message.Date);
                    File.SetLastWriteTime(destinationPath + message.Link, message.Date);

                    return Task.CompletedTask;


            }

            return Task.CompletedTask;
        }
    }
}
