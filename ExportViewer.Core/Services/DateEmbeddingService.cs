using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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
                 // Embed the sender name into the output file name when available,
                 // so the user can tell at a glance who sent the message.
                string destinationLink = BuildDestinationLink(message);
                string sourceFullPath = exportLocation + message.Link;
                string destFullPath = destinationPath + destinationLink;

                string? directory = Path.GetDirectoryName(destFullPath);
                if (!string.IsNullOrEmpty(directory))
                 {
                    Directory.CreateDirectory(directory);
                 }

                try
                 {
                    File.Copy(sourceFullPath, destFullPath, overwrite: true);
                 }
                catch (Exception ex)
                 {
                    Console.WriteLine($"Error copying {message.Link}: {ex.Message}");
                 }

                File.SetCreationTime(destFullPath, message.Date);
                File.SetLastAccessTime(destFullPath, message.Date);
                File.SetLastWriteTime(destFullPath, message.Date);

                return Task.CompletedTask;
             }

            return Task.CompletedTask;
         }

         // Builds the destination relative path, inserting a sanitized sender name
         // before the file extension. Group messages ("Uczestnicy: ...") are treated
         // as a single generic "grupa" label to keep the file name short and clean.
        private static string BuildDestinationLink(Message message)
         {
            string sender = message.Sender?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(sender))
             {
                return message.Link;
             }

             // Group conversations use an "Uczestnicy:" header listing all participants;
             // collapse that to a single short label so the file name stays readable.
            if (sender.StartsWith("Uczestnicy:", StringComparison.OrdinalIgnoreCase))
             {
                sender = "grupa";
             }

            string sanitized = SanitizeFileName(sender);
            if (string.IsNullOrEmpty(sanitized))
             {
                return message.Link;
             }

              // Split the link into directory and file name using the last path separator.
            string link = message.Link;
            int lastSlash = Math.Max(link.LastIndexOf('/'), link.LastIndexOf('\\'));
            string dir = lastSlash >= 0 ? link.Substring(0, lastSlash) : string.Empty;
            string fileName = lastSlash >= 0 ? link.Substring(lastSlash + 1) : link;

              // Split the file name into base name and extension using the last dot.
            string baseName = fileName;
            string extension = string.Empty;
            int lastDot = fileName.LastIndexOf('.');
            if (lastDot > 0)
              {
                baseName = fileName.Substring(0, lastDot);
                extension = fileName.Substring(lastDot);
              }

              // Insert the sender name before the base name: "sender baseName.ext".
            string newFileName = $"{sanitized} {baseName}{extension}";
            if (string.IsNullOrEmpty(dir))
              {
                return newFileName;
              }

              // Preserve the original separator style.
            string sep = link.Contains('/') ? "/" : "\\";
            return dir + sep + newFileName;
           }

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
        }
    }
