
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace ExportViewer.Tests.HelperMethods
{
    public static class Json
    {
        public static string GenerateLanguageJson( string jsonPath)
        {
            var pathParts = jsonPath.Split('.')
    .SelectMany(p => p.Split('[').Select(s => s.TrimEnd(']')))
    .Where(p => !string.IsNullOrEmpty(p))
    .ToList();

            object rootObject = BuildJsonObject(pathParts);

            return JsonSerializer.Serialize(rootObject , new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        private static object BuildJsonObject (List<string> pathParts)
        {
            if (pathParts.Count == 0)
            {
                return "en_US";
            }

            var current = pathParts[0];
            var remaining = pathParts.Skip(1).ToList();

            if (int.TryParse(current , out _))
            {
                var array = new List<object>();
                array.Add(BuildJsonObject(remaining));
                return array;
            }
            else
            {
                var obj = new Dictionary<string , object>();
                obj[current] = BuildJsonObject(remaining);
                return obj;
            }
        }
    }
}