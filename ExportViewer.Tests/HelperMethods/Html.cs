
using System.IO;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace ExportViewer.Tests.HelperMethods
{
    public static class Html
    {
        public static string GenerateLanguageHtml (string xPath)
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
    }
}