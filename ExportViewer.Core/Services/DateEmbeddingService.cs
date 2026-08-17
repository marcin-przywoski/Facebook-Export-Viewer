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
           // Removes characters that are illegal in file names and collapses whitespace.
         private static string SanitizeFileName(string name)
           {
             if (string.IsNullOrEmpty(name))               {
                return string.Empty;
               }
            char[] invalid = Path.GetInvalidFileNameChars();
            StringBuilder sb = new StringBuilder(name.Length);
            foreach (char c in name)
             {
                if (Array.IndexOf(invalid, c) >= 0 || c == ' ')
                 {
                    sb.Append('_');
                 }
                else
                 {
                    sb.Append(c);
                 }
             }

             // Collapse runs of underscores to a single one.
            string result = Regex.Replace(sb.ToString(), "_+", "_").Trim('_');

             // Keep file names reasonably short.
            if (result.Length > 60)
               {
                result = result.Substring(0, 60).TrimEnd('_');
               }

            return result;
            }
