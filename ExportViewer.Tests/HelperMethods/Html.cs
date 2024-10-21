
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace ExportViewer.Tests.HelperMethods
{
    public static class Html
    {
        public static string GenerateLanguageHtml(string xPath)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument("<html><body></body></html>");
            string[] parts = xPath.Trim('/').Split('/');
            IElement currentElement = document.Body;
            IElement lastElement = null!;
            foreach (string part in parts)
            {
                var match = Regex.Match(part , @"^(\w+)(?:\[(\d+)\])?(?:#(\w+))?(?:\.(\w+))?$");
                if (!match.Success)
                {
                    continue;
                }

                string tagName = match.Groups[1].Value;
                int index = match.Groups[2].Success ? int.Parse(match.Groups[2].Value) - 1 : 0;
                string id = match.Groups[3].Value;
                string className = match.Groups[4].Value;

                var newElement = document.CreateElement(tagName);

                if (!string.IsNullOrEmpty(id))
                {
                    newElement.Id = id;
                }

                if (!string.IsNullOrEmpty(className))
                {
                    newElement.ClassList.Add(className);
                }

                if (index > 0)
                {
                    for (int i = 0; i < index; i++)
                    {
                        currentElement.AppendChild(document.CreateElement(tagName));
                    }
                }

                currentElement.AppendChild(newElement);
                currentElement = newElement;
                lastElement = newElement;
            }

            // Add "en_US" to the most nested element
            if (lastElement != null)
            {
                lastElement.TextContent = "en_US";
            }

            return document.DocumentElement.OuterHtml;
        }

        public static (string html, List<string> mediaFilePaths) GenerateMessagesHtml(string nodesXPath , string dateXpath = "" , string mediaXPath = "" , string dateTimeFormat = "", string locale = "en-US")
        {
            var mediaFilePaths = new List<string>();
            var random = new Random();
            var parser = new HtmlParser();
            var document = parser.ParseDocument("<html><body></body></html>");
            string[] nodesXPathParts = nodesXPath.Trim('/').Split('/');
            string[] dateXPathParts = dateXpath.Trim('/').Split('/');
            string[] mediaXPathParts = mediaXPath.Trim('/').Split('/');
            IElement rootElement = document.Body;

            for (int i = 0; i < random.Next(0 , 8); i++)
            {
                IElement currentElement = rootElement;
                IElement lastElement = null!;
                IElement divElement;
                IElement linkElement = null!;
                foreach (string part in nodesXPathParts)
                {
                    IElement newElement = GenerateHtmlElement(part , document);
                    if (newElement != null)
                    {
                        currentElement.AppendChild(newElement);
                        lastElement = newElement;
                        currentElement = newElement;

                    }
                }

                divElement = currentElement;

                // Iterate over the dateXpath parts and create a div for each
                foreach (string datePart in dateXPathParts)
                {
                    IElement dateElement = GenerateHtmlElement(datePart , document);
                    if (dateElement != null)
                    {
                        currentElement.AppendChild(dateElement);
                        lastElement = dateElement;
                        currentElement = dateElement;
                    }
                }

                CultureInfo.CurrentCulture = new CultureInfo(locale);
                // Clone the current CultureInfo
                if (CultureInfo.CurrentCulture.Name == "pl-PL")
                {
                    CultureInfo.CurrentCulture.DateTimeFormat.PMDesignator = "po południu";
                    CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator = "rano";
                }

                lastElement.TextContent = DateTime.Now.ToString(dateTimeFormat, CultureInfo.CurrentCulture);

                var fileName = $"messages/{Guid.NewGuid()}.jpg";
                mediaFilePaths.Add(fileName);
                foreach (string mediaPart in mediaXPathParts)
                {
                    IElement mediaElement = GenerateHtmlElement(mediaPart , document);
                    if (mediaElement != null)
                    {
                        divElement.AppendChild(mediaElement);
                        linkElement = mediaElement;
                        divElement = mediaElement;
                        //linkElement = mediaElement;

                    }
                }
                //linkElement = divElement;
                linkElement.SetAttribute("src" , fileName);
            }

            return (document.DocumentElement.OuterHtml, mediaFilePaths);
        }

        private static IElement GenerateHtmlElement(string xPath , IHtmlDocument document)
        {
            var match = Regex.Match(xPath , @"^(\w+)(?:\.([\w-]+(?:\.[\w-]+)*))?$");
            if (!match.Success)
            {
                return null;
            }

            string tagName = match.Groups[1].Value;
            string cssClasses = match.Groups[2].Value;

            var newElement = document.CreateElement(tagName);
            if (!string.IsNullOrEmpty(cssClasses))
            {
                foreach (string cssClass in cssClasses.Split('.'))
                {
                    newElement.ClassList.Add(cssClass);
                }
            }

            return newElement;
        }
    }
}