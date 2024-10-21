
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using AngleSharp.Html.Parser;
using ExportViewer.Core.Services;
using Xunit;
using Xunit.Abstractions;

namespace ExportViewer.Tests
{
    public class HtmlParsingService_Tests
    {
        private readonly HtmlParsingService _htmlParsingService;

        private readonly ITestOutputHelper _output;

        private readonly IProgress<string> _progress = new Progress<string>(s => Console.WriteLine(s));

        public HtmlParsingService_Tests (ITestOutputHelper output)
        {
            _output = output;
            _htmlParsingService = new HtmlParsingService();
        }
        //TODO: Fix the method for parsing messages
        [Theory]
        [InlineData("div.pam._3-95._2pi0._2lej.uiBoxWhite.noborder" , "div._3-94._2lem", "div._3-96._2let/img._2yuc._3-96", "", "pl-PL")]
        [InlineData("div._3-95._a6-g" , "div._3-94._a6-o/div._a72d", "img._a6_o._3-96", "MMM dd, yyyy h:mm:sstt", "pl-PL")]
/*         [InlineData("div._a6-g" , "div._3-94._a6-o/div._a72d", "img._a6_o._3-96", "MMM dd, yyyy h:mm:sstt", "pl-PL")] */
        public async void HtmlParsingService_ParseMessages (string nodesXPath , string dateXpath, string mediaXPath, string dateTimeFormat, string locale)
        {
            (string htmlString, List<string> mediaFilePaths) = HelperMethods.Html.GenerateMessagesHtml(nodesXPath , dateXpath, mediaXPath, dateTimeFormat);

            string exportPath = Path.Combine(Path.GetTempPath() , Guid.NewGuid().ToString());
            string fullPath = Path.Combine(exportPath , "test.html");
            string? dir = Path.GetDirectoryName(fullPath);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            foreach (var path in mediaFilePaths)
            {
                //Create empty jpg files of the names created
                string filePath = Path.Combine(exportPath , path);
                dir = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                File.WriteAllText(filePath , string.Empty);
            }

            File.WriteAllText(fullPath , htmlString);

            var messages = (await _htmlParsingService.GetMessages(fullPath , new CultureInfo(locale) , exportPath)).ToList();


            Assert.NotEmpty(messages);
            _output.WriteLine($"Export language: {locale} \n Export HTML string: {htmlString}");
        }
    }
}