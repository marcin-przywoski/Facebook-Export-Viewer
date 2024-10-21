
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
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

        public static (string html, List<string> mediaFilePaths) GenerateMessagesHtml (string nodesXPath , string dateXpath)
        {
            var mediaFilePaths = new List<string>();
            var random = new Random();
            var parser = new HtmlParser();
            var document = parser.ParseDocument("<html><body></body></html>");
            string[] parts = nodesXPath.Trim('/').Split('/');
            IElement currentElement = document.Body;
            foreach (string part in parts)
            {
                var match = Regex.Match(part , @"^(\w+)(?:\.([\w-]+(?:\.[\w-]+)*))?$");
                if (!match.Success)
                {
                    continue;
                }

                string tagName = match.Groups[1].Value;
                string cssClasses = match.Groups[2].Value;

                for (int i = 0; i < random.Next(0 , 8); i++)
                {
                    var messageDiv = document.CreateElement(tagName);

                    if (!string.IsNullOrEmpty(cssClasses))
                    {
                        foreach (string cssClass in cssClasses.Split('.'))
                        {
                            messageDiv.ClassList.Add(cssClass);
                        }
                    }

                    var classes = dateXpath.Split('.');
                    // Create a div element to represent the date and add it to parent div
                    var dateElement = document.CreateElement("div");

                    foreach (string cssClass in classes)
                    {
                        dateElement.ClassList.Add(cssClass);
                    }

                    dateElement.TextContent = DateTime.Now.ToString("yyyy-MM-dd");
                    messageDiv.AppendChild(dateElement);

                    var fileName = $"messages/{Guid.NewGuid()}.jpg";
                    var hrefElement = document.CreateElement("a");
                    hrefElement.SetAttribute("href" , fileName);
                    mediaFilePaths.Add(fileName);
                    messageDiv.AppendChild(hrefElement);

                    currentElement.AppendChild(messageDiv);
                }
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