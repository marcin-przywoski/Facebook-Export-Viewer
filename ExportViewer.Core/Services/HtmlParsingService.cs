using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.XPath;
using ExportViewer.Core.Models.Common;
using ExportViewer.Core.Models.Interfaces;
using ExportViewer.Core.Services.Interfaces;

namespace ExportViewer.Core.Services
{
    public class HtmlParsingService : IHtmlParsingService
    {
        public async Task<IEnumerable<Message>> GetMessages (string filePath , CultureInfo locale , string exportLocation)
        {
            ConcurrentBag<Message> messages = new ConcurrentBag<Message>();

            var effectiveLocale = (CultureInfo)locale.Clone();
            if (effectiveLocale.DisplayName == "pl_PL" || effectiveLocale.Name.StartsWith("pl", StringComparison.OrdinalIgnoreCase))
            {
                effectiveLocale.DateTimeFormat.PMDesignator = "po południu";
                effectiveLocale.DateTimeFormat.AMDesignator = "rano";
            }

            string[] dateFormats = new[]
            {
                "MMM dd, yyyy h:mm:ss tt",
                "MMM d, yyyy h:mm:ss tt",
                "MMM dd, yyyy h:mm:sstt",
                "MMM d, yyyy h:mm:sstt"
            };

            string source = await File.ReadAllTextAsync(filePath);
            var parser = new HtmlParser();
            var document = await parser.ParseDocumentAsync(source);
            var divs = document.QuerySelectorAll("div.pam._3-95._2pi0._2lej.uiBoxWhite.noborder");

            if (divs.Any())
            {
                Parallel.ForEach(divs , node =>
                {
                    var divImage = node.QuerySelector("img._2yuc._3-96");
                    var divVideo = node.QuerySelector("video._2yuc._3-96");
                    var divDate = node.QuerySelector("div._3-94._2lem");

                    if (((divImage != null && divDate != null) || (divVideo != null && divDate != null)) && !string.IsNullOrEmpty(divDate.TextContent))
                    {
                        string href = divImage != null ? divImage.GetAttribute("src") : divVideo.GetAttribute("src");
                        if ((!href.StartsWith("http") || !href.StartsWith("https")) && (href.EndsWith(".jpg") || href.EndsWith(".png") || href.EndsWith(".gif") || href.EndsWith(".mp4")))
                        {
                            DateTime parsedDate = Convert.ToDateTime(divDate.TextContent , effectiveLocale);

                            if (File.Exists(Path.Combine(exportLocation , href)))
                            {
                                messages.Add(new Message { Link = href , Date = parsedDate });
                            }
                        }
                    }
                });

            }
            else if (document.QuerySelectorAll("div._3-95._a6-g").Any())
            {
                divs = document.QuerySelectorAll("div._3-95._a6-g");

                Parallel.ForEach(divs , node =>
                {

                    var divImage = node.QuerySelector("img._a6_o._3-96");
                    var divVideo = node.QuerySelector("video._a6_o._3-96");
                    var divDate = node.QuerySelector("div._3-94._a6-o")?.QuerySelector("div._a72d");

                    if (((divImage != null && divDate != null) || (divVideo != null && divDate != null)) && !string.IsNullOrEmpty(divDate.TextContent))
                    {
                        string href = divImage != null ? divImage.GetAttribute("src") : divVideo.GetAttribute("src");
                        if ((!href.StartsWith("http") || !href.StartsWith("https")) && (href.EndsWith(".jpg") || href.EndsWith(".png") || href.EndsWith(".gif") || href.EndsWith(".mp4")))
                        {
                            DateTime parsedDate = DateTime.ParseExact(divDate.TextContent , dateFormats , effectiveLocale , DateTimeStyles.None);

                            if (File.Exists(Path.Combine(exportLocation , href)))
                            {
                                messages.Add(new Message { Link = href , Date = parsedDate });
                            }
                        }
                    }

                });

            }
            else if (document.QuerySelectorAll("._a6-g").Any())
            {
                divs = document.QuerySelectorAll("._a6-g");

                Parallel.ForEach(divs , node =>
                 {

                     var divImages = node.QuerySelectorAll("img._a6_o._3-96");
                     var divVideos = node.QuerySelectorAll("video._a6_o._3-96");
                     // Date is in <footer class="_3-94 _a6-o"><div class="_a72d">...</div></footer>
                     // in the new export format, so use tag-agnostic selectors.
                     var divDate = node.QuerySelector("._3-94._a6-o")?.QuerySelector("._a72d");

                     // Extract sender name from <h2 class="_2ph_ _a6-h"> or <h2 class="_2ph_ _a6-h _a6-i">
                     var senderElement = node.QuerySelector("h2._a6-h._a6-i") ?? node.QuerySelector("h2._a6-h");
                     string sender = senderElement?.TextContent?.Trim() ?? string.Empty;

                     if (((divImages != null && divDate != null) || (divVideos != null && divDate != null)) && !string.IsNullOrEmpty(divDate.TextContent))
                     {
                         Parallel.ForEach(divImages , divImage =>
                          {
                              string href = divImage.GetAttribute("src");
                              if ((!href.StartsWith("http") || !href.StartsWith("https")) && (href.EndsWith(".jpg") || href.EndsWith(".png") || href.EndsWith(".gif")))
                              {
                                  DateTime parsedDate = DateTime.ParseExact(divDate.TextContent , dateFormats , effectiveLocale , DateTimeStyles.None);

                                  if (File.Exists(Path.Combine(exportLocation , href)))
                                  {
                                      messages.Add(new Message { Link = href , Date = parsedDate , Sender = sender });
                                  }
                              }
                          });

                         Parallel.ForEach(divVideos , divVideo =>
                          {
                              string href = divVideo.GetAttribute("src");
                              if ((!href.StartsWith("http") || !href.StartsWith("https")) && href.EndsWith(".mp4"))
                              {
                                  DateTime parsedDate = DateTime.ParseExact(divDate.TextContent , dateFormats , effectiveLocale , DateTimeStyles.None);

                                  if (File.Exists(Path.Combine(exportLocation , href)))
                                  {
                                      messages.Add(new Message { Link = href , Date = parsedDate , Sender = sender });
                                  }
                              }
                          });
                     }

                 });
            }

            return messages.AsEnumerable();

        }
    }
}
