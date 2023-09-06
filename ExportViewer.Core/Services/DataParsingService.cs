using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ExportViewer.Core.Enums;
using ExportViewer.Core.Models.Interfaces;
using ExportViewer.Core.Services.Interfaces;

namespace ExportViewer.Core.Services
{
    public class DataParsingService : IDataParsingService
    {
        public Task<IEnumerable<IMessage>> GetExportFileMessages (string exportLocation , string exportFileLocation , ExportType type , CultureInfo locale , IProgress<string> progress)
        public Task<IEnumerable<string>> GetExportFiles(string exportLocation, ExportType type, IProgress<string> progress)
        {
            var fileExtensions = new Dictionary<ExportType, string>
            {
                { ExportType.HTML, "*.html" },
                { ExportType.Json, "*.json" }
            };

            var exportFiles = new List<string>();

            foreach (var subDirectory in new[] { "archived_threads/", "filtered_threads/", "inbox/" })
            {
                var subDirectoryLocation = Path.Combine(exportLocation, "messages/", subDirectory);
                var fileExtension = fileExtensions[type];

                if (Directory.Exists(subDirectoryLocation))
                {
                    try
                    {
                exportFiles.AddRange(Directory.GetFiles(subDirectoryLocation, fileExtension, SearchOption.AllDirectories));
            }
                    catch (Exception ex)
                    {
                        // Handle the exception or log the error
                        Console.WriteLine($"Error getting files from {subDirectoryLocation}: {ex.Message}");
                    }
                }
                else
                {
                    // Handle the case when the directory does not exist
                    Console.WriteLine($"Directory {subDirectoryLocation} does not exist.");
                }
            }

            return Task.FromResult<IEnumerable<string>>(exportFiles);
        }


        public async Task<CultureInfo> GetExportLanguage(string exportLocation, ExportType exportType, IProgress<string> progress)
        {
            string preferencesLocation;
            string locale;

            var parser = new HtmlParser();

            if (exportType == ExportType.HTML)
            {
                preferencesLocation = Path.Combine(exportLocation, "about_you/preferences.html");
                if (File.Exists(preferencesLocation))
                {
                    string preferences = await File.ReadAllTextAsync(preferencesLocation);
                    var document = parser.ParseDocument(preferences);
                    locale = document.Body.SelectSingleNode("/html/body/div/div/div/div[2]/div[2]/div/div[3]/div/div[2]/div[1]/div[2]/div/div/div/div[1]/div[3]")?.TextContent?.Trim();
                    if (locale != null)
        {
                        progress.Report($"Export language: {locale}");
                        return new CultureInfo(locale, false);
                    }
        }

                preferencesLocation = Path.Combine(exportLocation, "preferences/language_and_locale.html");
                if (File.Exists(preferencesLocation))
                {
                    string preferences = await File.ReadAllTextAsync(preferencesLocation);
                    var document = parser.ParseDocument(preferences);
                    locale = document.Body.SelectSingleNode("/html/body/div/div/div/div[2]/div[2]/div/div[1]/div/div[2]/div[1]/div[2]/div/div/div/div[1]/div[3]")?.TextContent?.Trim();
                    if (locale != null)
                    {
                        progress.Report($"Export language: {locale}");
                        return new CultureInfo(locale, false);
                    }
                }
            }
            else if (exportType == ExportType.Json)
            {
                preferencesLocation = Path.Combine(exportLocation, "preferences/language_and_locale.json");
                if (File.Exists(preferencesLocation))
                {
                    string json = await File.ReadAllTextAsync(preferencesLocation);
                    JObject jsonObj = JObject.Parse(json);
                    locale = (string)jsonObj.SelectToken("language_and_locale_v2[0].children[0].entries[0].data.value");
                    if (locale != null)
        {
                        progress.Report($"Export language: {locale}");
                        return new CultureInfo(locale, false);
                    }
                }
            }

            return null;
        }



        public Task<ExportType> GetExportType(string exportLocation, IProgress<string> progress)
        {
            string jsonSearchPattern = "*.json";
            string htmlSearchPattern = "*.html";

            bool hasJsonFiles = Directory.EnumerateFiles(exportLocation, jsonSearchPattern, SearchOption.AllDirectories).Any();
            bool hasHtmlFiles = Directory.EnumerateFiles(exportLocation, htmlSearchPattern, SearchOption.AllDirectories).Any();

            if (hasJsonFiles)
            {
                progress.Report($"Export type: JSON");
                return Task.FromResult(ExportType.Json);
            }
            else if (hasHtmlFiles)
            {
                progress.Report($"Export type: HTML");
                return Task.FromResult(ExportType.HTML);
            }
            else
        {
                progress.Report($"Export type: HTML");
                return Task.FromResult(ExportType.NotApplicable);
            }
        }


    }
}
