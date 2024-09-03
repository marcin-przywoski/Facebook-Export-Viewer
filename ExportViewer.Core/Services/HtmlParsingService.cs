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
            var divs = document.QuerySelectorAll("div.pam._3-95._2pi0._2lej.uiBoxWhite.noborder");

            if (divs.Any())
            {
                Parallel.ForEach(divs , node =>
                {
                    if ((node.Descendents().OfType<IHtmlDivElement>().First(x => x.ClassList.Contains("_3-94") && x.ClassList.Contains("_2lem")).TextContent != "") && node.Descendents().OfType<IHtmlAnchorElement>().Any() && (node.Descendents().OfType<IHtmlAnchorElement>().First(x => x.HasAttribute("href"))).GetAttribute("href") != "")
                    {
                        string href = node.Descendents().OfType<IHtmlAnchorElement>().First(x => x.HasAttribute("href")).GetAttribute("href");

                        if ((!href.StartsWith("http") || !href.StartsWith("https")) && (href.EndsWith(".jpg") || href.EndsWith(".png") || href.EndsWith(".gif") || href.EndsWith(".mp4")))
                        {
                            string divDate = node.Descendents().OfType<IHtmlDivElement>().First(x => x.ClassList.Contains("_3-94") && x.ClassList.Contains("_2lem")).TextContent;

                            DateTime date = Convert.ToDateTime(divDate , locale);

                            if (File.Exists(Path.Combine(exportLocation, href)))
                            {
                                messages.Add(new Message { Link = href , Date = date });
                            }
                        }

                    }
                });

            }
            else if (document.QuerySelectorAll("div._3-95._a6-g").Any())
            {
                divs = document.QuerySelectorAll("div._3-95._a6-g");

                if (locale.DisplayName == "pl_PL")
                {
                    locale.DateTimeFormat.PMDesignator = "po południu";
                    locale.DateTimeFormat.AMDesignator = "rano";
                }

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

                            locale.DateTimeFormat.PMDesignator = "po południu";
                            locale.DateTimeFormat.AMDesignator = "rano";

                            DateTime parsedDate = DateTime.ParseExact(divDate.TextContent , "MMM dd, yyyy h:mm:sstt" , locale);

                            if (File.Exists(Path.Combine(exportLocation, href)))
                            {
                                messages.Add(new Message { Link = href , Date = parsedDate });
                            }
                        }
                    }

                });

            }
            else if (document.QuerySelectorAll("div._a6-g").Any())
            {
                divs = document.QuerySelectorAll("div._a6-g");

                if (locale.DisplayName == "pl_PL")
                {
                    locale.DateTimeFormat.PMDesignator = "po południu";
                    locale.DateTimeFormat.AMDesignator = "rano";
                }

                Parallel.ForEach(divs , node =>
                {

                    var divImages = node.QuerySelectorAll("img._a6_o._3-96");
                    var divVideos = node.QuerySelectorAll("video._a6_o._3-96");
                    var divDate = node.QuerySelector("div._3-94._a6-o")?.QuerySelector("div._a72d");

                    if (((divImages != null && divDate != null) || (divVideos != null && divDate != null)) && !string.IsNullOrEmpty(divDate.TextContent))
                    {
                        Parallel.ForEach(divImages , divImage =>
                        {
                            string href = divImage.GetAttribute("src");
                            if ((!href.StartsWith("http") || !href.StartsWith("https")) && (href.EndsWith(".jpg") || href.EndsWith(".png") || href.EndsWith(".gif")))
                            {
                                if (Thread.CurrentThread.CurrentCulture.IsReadOnly || locale.IsReadOnly)
                                {
                                    var clone = Thread.CurrentThread.CurrentCulture.Clone() as CultureInfo;
                                    clone.DateTimeFormat.PMDesignator = "po południu";
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


                                DateTime parsedDate = DateTime.ParseExact(divDate.TextContent , "MMM dd, yyyy h:mm:sstt" , locale);

                                if (File.Exists(Path.Combine(exportLocation, href)))
                                {
                                    messages.Add(new Message { Link = href , Date = parsedDate });
                                }
                            }
                        });

                        Parallel.ForEach(divVideos , divVideo =>
                        {
                            string href = divVideo.GetAttribute("src");
                            if ((!href.StartsWith("http") || !href.StartsWith("https")) && href.EndsWith(".mp4"))
                            {
                                if (Thread.CurrentThread.CurrentCulture.IsReadOnly || locale.IsReadOnly)
                                {
                                    var clone = Thread.CurrentThread.CurrentCulture.Clone() as CultureInfo;
                                    clone.DateTimeFormat.PMDesignator = "po południu";
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


                                DateTime parsedDate = DateTime.ParseExact(divDate.TextContent , "MMM dd, yyyy h:mm:sstt" , locale);

                                if (File.Exists(Path.Combine(exportLocation, href)))
                                {
                                    messages.Add(new Message { Link = href , Date = parsedDate });
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
