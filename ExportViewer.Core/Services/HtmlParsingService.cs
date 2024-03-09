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

            string source = await File.ReadAllTextAsync(filePath);
            var parser = new HtmlParser();
            var document = await parser.ParseDocumentAsync(source);
            var divs = document.QuerySelectorAll("div._a6-g, div._3-95._a6-g, div.pam._3-95._2pi0._2lej.uiBoxWhite.noborder");

            if (divs.Any())
            {
                Parallel.ForEach(divs , node =>
                {
                    var divImages = node.QuerySelectorAll("img._a6_o._3-96, img._2yuc._3-96");
                    var divVideos = node.QuerySelectorAll("video._a6_o._3-96, video._2yuc._3-96");
                    var dateElement = node.QuerySelector("div._3-94._2lem") ?? node.QuerySelector("div._3-94._a6-o")?.QuerySelector("div._a72d");

                    if ((divImages != null) && dateElement != null && !string.IsNullOrEmpty(dateElement.TextContent))
                    {
                        foreach (var divImage in divImages)
                        {
                            string href = divImage.GetAttribute("src");

                            if ((!href.StartsWith("http") || !href.StartsWith("https")) && (href.EndsWith(".jpg") || href.EndsWith(".png") || href.EndsWith(".gif")))
                            {
                                if (Thread.CurrentThread.CurrentCulture.IsReadOnly || locale.IsReadOnly)
                                {
                                    var clone = Thread.CurrentThread.CurrentCulture.Clone() as CultureInfo;
                                    clone!.DateTimeFormat.PMDesignator = "po południu";
                                    clone.DateTimeFormat.AMDesignator = "rano";
                                    Thread.CurrentThread.CurrentCulture = clone;
                                    Thread.CurrentThread.CurrentUICulture = clone;
                                    locale = clone;
                                }
                                else
                                {
                                    locale.DateTimeFormat.PMDesignator = "po południu";
                                    locale.DateTimeFormat.AMDesignator = "rano";
                                }
                                bool isValidDate = DateTime.TryParse(dateElement.TextContent , out var parsedDate);
                                if (!isValidDate)
                                {
                                    parsedDate = DateTime.ParseExact(dateElement.TextContent , "MMM dd, yyyy h:mm:sstt" , locale);
                                }

                                if (File.Exists(exportLocation + href))
                                {
                                    messages.Add(new Message { Link = href , Date = parsedDate });
                                }
                            }
                        }
                    }

                    if ((divVideos != null) && dateElement != null && !string.IsNullOrEmpty(dateElement.TextContent))
                    {
                        foreach (var divVideo in divVideos)
                        {
                            string href = divVideo.GetAttribute("src");

                            if ((!href.StartsWith("http") || !href.StartsWith("https")) && href.EndsWith(".mp4"))
                            {
                                if (Thread.CurrentThread.CurrentCulture.IsReadOnly || locale.IsReadOnly)
                                {
                                    var clone = Thread.CurrentThread.CurrentCulture.Clone() as CultureInfo;
                                    clone!.DateTimeFormat.PMDesignator = "po południu";
                                    clone.DateTimeFormat.AMDesignator = "rano";
                                    Thread.CurrentThread.CurrentCulture = clone;
                                    Thread.CurrentThread.CurrentUICulture = clone;
                                    locale = clone;
                                }
                                else
                                {
                                    locale.DateTimeFormat.PMDesignator = "po południu";
                                    locale.DateTimeFormat.AMDesignator = "rano";
                                }
                                bool isValidDate = DateTime.TryParse(dateElement.TextContent , out var parsedDate);
                                if (!isValidDate)
                                {
                                    parsedDate = DateTime.ParseExact(dateElement.TextContent , "MMM dd, yyyy h:mm:sstt" , locale);
                                }

                                if (File.Exists(exportLocation + href))
                                {
                                    messages.Add(new Message { Link = href , Date = parsedDate });
                                }
                            }
                        }
                    }
                });
            }
            return messages;
        }
    }
}
